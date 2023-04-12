using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item object,", menuName = "Inventory system/WeaponScripts/BurstRifle")]
public class BurstRifle : WeaponBase
{
    public override void FireWeapon(Vector3 firePoint, Vector3 dir)
    {
        Vector3 dirRight = Vector3.Cross(dir, Vector3.up);
        float newAngle = Random.Range(-aimDistortion, aimDistortion);
        Vector3 shootVector = Vector3.RotateTowards(dir, dirRight, DegreesToRadians(newAngle), 0f);
        
        Debug.DrawRay(firePoint, shootVector * 10, Color.green, 1f);
    }

    public override IEnumerator FireWeaponCoroutine(Vector3 firePoint, Vector3 dir)
    {
        int shots = projectileAmount;
        while (shots > 1)
        {
            FireWeapon(firePoint, dir);
            shots--;
            yield return new WaitForSeconds(burstFireRate);
        }
    }
}
