using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform gunPosition;
    public int currentWeaponIndex = 0;
    
    private FireType _fireType;
    private GameObject _currentWeaponGameObject;
    private WeaponBase _currentWeapon;

    private InteractionController _controller;
    private TopDownMovement _topDownMovement;
    private PlayerStats _stats;
    
    // variables for shooting
    private Transform _firePointLocal;
    private int _currentWeaponId = -1;
    private float _firePointDist;
    private bool _canFire;
    private float _fireRate;
    private float _fireTimer;
    private Vector3 GetFirePoint() => gunPosition.position + transform.forward * _currentWeapon.GetFirePointDist(); //TODO make better system for this piece of shit
    private Vector3 GetFirePointDir()
    {
        if (InputHandler.mouseRightHold)
        {
            Ray ray = _topDownMovement._camera.ScreenPointToRay(InputHandler.MousePosition);
            Vector3 worldPos = Vector3.zero;

            if (_topDownMovement.plane.Raycast(ray, out float distance))
            {
                worldPos = ray.GetPoint(distance);
            }

            worldPos.y = gunPosition.position.y;

            return (worldPos - GetFirePoint()).normalized;
        }
        else
        {
            return transform.forward;
        }
    }

    private void Start()
    {
        _topDownMovement = GetComponent<TopDownMovement>();
        _stats = GetComponent<PlayerStats>();
        _controller = GetComponent<InteractionController>();
        
        _currentWeaponId = -1;

        InputHandler.leftMouseButtonDown += FireWeapon;
    }

    private void Update()
    {
        if(_canFire) return;
        
        if (_fireTimer < _fireRate)
        {
            _fireTimer += Time.deltaTime;
            return;
        }
        _canFire = true;
        if (Input.GetKeyDown(KeyCode.O))
        {
            CheckWeapon();
        }
    }

    public void FireWeapon()
    {
        if (_topDownMovement.bIsDodging || !_currentWeapon || MouseData.tempItemBeingDragged != null) return;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            switch (_fireType)
            {
                case FireType.single : ShootSingle();
                    break;
                case FireType.burst : ShootBurst();
                    break;
                case FireType.auto : ShootAuto();
                    break;
            }
        }
    }

    private void ShootSingle()
    {
        if (_canFire)
        {
            _currentWeapon.FireWeapon(GetFirePoint(), GetFirePointDir());
            _fireTimer -= _currentWeapon.GetFireRate();
            _canFire = false;
        }
        

        // if (Input.GetMouseButtonDown(0))
        // {
        //     currentWeapon.FireWeapon(GetFirePoint(), GetFirePointDir());
        //     _fireTimer -= currentWeapon.GetFireRate();
        // }
    }

    private void ShootBurst()
    {
        if (_canFire)
        {
            _currentWeapon.FireWeapon(GetFirePoint(), GetFirePointDir());
            StartCoroutine(_currentWeapon.FireWeaponCoroutine(GetFirePoint(), GetFirePointDir()));
            _fireTimer -= _currentWeapon.GetBurstFireRate();
            _canFire = false;
        }
        
        
        // if (Input.GetMouseButtonDown(0))
        // {
        //     currentWeapon.FireWeapon(GetFirePoint(), GetFirePointDir());
        //     StartCoroutine(currentWeapon.FireWeaponCoroutine(GetFirePoint(), GetFirePointDir()));
        //     _fireTimer -= currentWeapon.GetBurstFireRate();
        // }
    }

    private void ShootAuto()
    {
        // if (input.mouseLeftHold)
        // {
        //     currentWeapon.FireWeapon(GetFirePoint(), GetFirePointDir());
        //     _fireTimer -= currentWeapon.GetFireRate();;
        // }

        if (_canFire)
        {
            StartCoroutine(ShootAutoCoroutine());
            _canFire = false;
        }
    }

    private IEnumerator ShootAutoCoroutine()
    {
        while (InputHandler.mouseLeftHold)
        {
            _fireTimer = 0f;
            _currentWeapon.FireWeapon(GetFirePoint(), GetFirePointDir());
            yield return new WaitForSeconds(_currentWeapon.GetFireRate());
        }

        _canFire = true;
    }
    

    public void CheckWeapon()
    {
        _controller.uiController.OpenWeaponsScreen();
        //_currentWeaponId = _controller.weapons.GetSlots[currentWeaponIndex].ItemObject.data.Id;
        
        if (_currentWeaponId == -1) // has no weapon
        {
            if (_controller.weapons.GetSlots[currentWeaponIndex].item.Id >= 0) // slot is not empty
            {
                CreateCurrentWeapon(_controller.weapons.GetSlots[currentWeaponIndex].ItemObject.model);
                SetUpGunStats(_controller.weapons.GetSlots[currentWeaponIndex].ItemObject.weaponScript);
            }
            else // slot is empty
            {
                RemoveCurrentWeapon();
            }
        }
        else // has weapon
        {
            if (_controller.weapons.GetSlots[currentWeaponIndex].item.Id >= 0) // slot is not empty
            {
                if (_currentWeaponId != _controller.weapons.GetSlots[currentWeaponIndex].item.Id) // id´s do not match
                {
                    RemoveCurrentWeapon();
                    CreateCurrentWeapon(_controller.weapons.GetSlots[currentWeaponIndex].ItemObject.model);
                    SetUpGunStats(_controller.weapons.GetSlots[currentWeaponIndex].ItemObject.weaponScript);
                }
            }
            else //slot is empty
            {
                RemoveCurrentWeapon();
            }
        }
        _controller.uiController.uiHoverItemDisplayController.UpdateCompareWeaponDisplay();
    }

    private void OnDrawGizmos()
    {
        if(_currentWeapon == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetFirePoint(), .1f);
    }

    private void SetUpGunStats(WeaponBase weapon)
    {
        _currentWeapon = weapon;
        _fireType = _currentWeapon.GetFireType();
        _fireTimer = _fireRate = _currentWeapon.GetFireRate();
    }

    private void RemoveCurrentWeapon()
    {
        _currentWeapon = null;
        _currentWeaponId = -1;
        if(_currentWeaponGameObject) Destroy(_currentWeaponGameObject);
    }

    private void CreateCurrentWeapon(GameObject gunModel)
    {
        _currentWeaponId = _controller.weapons.GetSlots[currentWeaponIndex].ItemObject.data.Id;
        _currentWeaponGameObject = Instantiate(gunModel, gunPosition.position, transform.rotation, transform);
        _currentWeaponGameObject.transform.Rotate(Vector3.up, 90f); // Models should be correct rotation but are not
        //print("instantiated new weapon");
    }
    
    
}
