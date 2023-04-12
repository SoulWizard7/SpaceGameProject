using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item object,", menuName = "Inventory system/WeaponScripts/AutoRifle")]
public class AutoRifle : WeaponBase
{
    public override void FireWeapon(Vector3 firePoint, Vector3 dir)
    {
        Vector3 dirRight = Vector3.Cross(dir, Vector3.up);
        float newAngle = Random.Range(-aimDistortion, aimDistortion);
        Vector3 shootVector = Vector3.RotateTowards(dir, dirRight, DegreesToRadians(newAngle), 0f);
        
        Debug.DrawRay(firePoint, shootVector * 10, Color.green, 1f);
    }
}
