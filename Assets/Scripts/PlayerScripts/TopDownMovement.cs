using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [Header("Debug stuff")]
    [SerializeField] private Material bodyMaterial;
    [SerializeField] private Material dodgeMaterial;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color dodgeColor;
    
    [SerializeField] private MeshRenderer body;
    private MaterialPropertyBlock mbp;

    public MaterialPropertyBlock Mbp
    {
        get
        {
            if (mbp == null) mbp = new MaterialPropertyBlock();
            return mbp;
        }
    }

    [Header("Player Variables")]
    [SerializeField] private float movementSpeed;
    //[SerializeField] private float jumpHeight;
    [SerializeField] private AnimationCurve dodgeCurve;
    [SerializeField] private float dodgeLengthTime = 1.2f;
    [SerializeField] private float dodgeSpeed;
    
    private Vector3 _move;
    private Vector3 _gravityVelocity;
    public Transform camPos;
    public Camera _camera;
    private CharacterController _controller;
    
    private float _turnSmoothVelocity = 0f;
    private float _turnSmoothTime = 0.05f;
    private bool _bIsGrounded;
    public bool bIsDodging { get; private set; }
    private float gravity = -9.81f;
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    
    // [NonSerialized] public Vector3 _getSmashedVelocity;
    // [NonSerialized] public Vector3 _jumpSmashVelocity;

    public Plane plane = new Plane(Vector3.up, -1.5f); //-1.5 helps with aiming for some reason
    private static readonly int Color1 = Shader.PropertyToID("_BaseColor");


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        InputHandler.dodgeButton += Dodge;
        ApplyColor(normalColor);
    }

    private void OnValidate()
    {
        ApplyColor(normalColor);
    }

    void ApplyColor(Color color)
    {
        Mbp.SetColor(Color1, color);
        body.SetPropertyBlock(Mbp);
    }

    public void SetCamera(GameObject cameraGameObject)
    {
       _camera = cameraGameObject.GetComponent<Camera>();
    }

    void Update()
    {
        if (bIsDodging) return;

        _bIsGrounded = Physics.CheckSphere(groundCheck.position, 0.5f, groundMask);
        if(_bIsGrounded && _gravityVelocity.y < 0)
        {
            _gravityVelocity.y = -2f;
            //_jumpSmashVelocity = Vector3.zero;
        }
        
        float x = InputHandler.WASD.x;
        float z = InputHandler.WASD.y;

        _move = camPos.right * x + camPos.forward * z;
        _move = _move.normalized;
        _move.y = -0.1f;
        
        _controller.Move(_move * (movementSpeed * Time.deltaTime));

        if (InputHandler.mouseRightHold)
        {
            Ray ray = _camera.ScreenPointToRay(InputHandler.MousePosition);
            Vector3 worldPos = Vector3.zero;

            if (plane.Raycast(ray, out float distance))
            {
                worldPos = ray.GetPoint(distance);
            }
            
            Vector3 lookDir = (worldPos - transform.position).normalized;
            float targetAngle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        else if (_move.magnitude >= 0.3f)
        {
            float targetAngle = Mathf.Atan2(_move.x, _move.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        
        //Gravity
        _gravityVelocity.y += gravity * Time.deltaTime;
        _controller.Move(_gravityVelocity * Time.deltaTime);
        
        // Enemy bumping
        //_controller.Move(_getSmashedVelocity * Time.deltaTime);
        //_controller.Move(_jumpSmashVelocity * Time.deltaTime);

    }

    public void Dodge()
    {
        if (_move.magnitude >= 0.3f && !bIsDodging)
        {
            bIsDodging = true;
            StartCoroutine(DodgeRoll(_move));
        }
    }

    IEnumerator DodgeRoll(Vector3 dir)
    {
        //body.material = dodgeMaterial;
        ApplyColor(dodgeColor);
        Debug.Log("startDodge");
        float timer = 0;
        while (timer < dodgeLengthTime)
        {
            float eval = Mathf.Lerp(0f, 1f, timer/dodgeLengthTime);
            float speed = dodgeCurve.Evaluate(eval);
            speed *= dodgeSpeed;
            _controller.Move(dir * (speed * Time.deltaTime));
            timer += Time.deltaTime;
            yield return null;
        }
        Debug.Log("endDodge");
        bIsDodging = false;
        //body.material = bodyMaterial;
        ApplyColor(normalColor);
    }
}
