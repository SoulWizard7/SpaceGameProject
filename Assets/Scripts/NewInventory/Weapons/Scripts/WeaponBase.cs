using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireType //for input
{
    single,
    auto,
    burst
}

[System.Serializable]
public class WeaponBase : ScriptableObject
{
    [TextArea(15, 10)] 
    public string description;
    
    public static float TAU = 6.28f;
    public static Vector2 AngToDir(float angleRad) => new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    public static float DegreesToRadians(float angleInDegrees)
    {
        return angleInDegrees * (TAU / 360);
    }
    

    [SerializeField]protected FireType fireType;
    [SerializeField]protected int projectileAmount = 1;
    [Tooltip("Degrees, plus and minus from forward angle")]
    [SerializeField]protected float aimDistortion = 0;
    [Tooltip("Degrees added for every projectile amount")]
    [SerializeField]protected float spreadAngle = 0f;
    [SerializeField]protected float firePointDist = 0f;
    [SerializeField]protected float fireRate = 1f;
    [SerializeField]protected float burstFireRate = 1f;

    public FireType GetFireType() => fireType;
    public float GetFireRate() => fireRate;
    public float GetBurstFireRate() => burstFireRate * projectileAmount + fireRate;

    public virtual void FireWeapon(Vector3 firePoint, Vector3 dir) {}

    public virtual IEnumerator FireWeaponCoroutine(Vector3 firePoint, Vector3 dir) {yield return null;}


    public float GetFirePointDist() => firePointDist;
}
