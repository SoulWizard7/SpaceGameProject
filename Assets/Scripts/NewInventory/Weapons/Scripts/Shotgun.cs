using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New item object,", menuName = "Inventory system/WeaponScripts/Shotgun")]
public class Shotgun : WeaponBase
{
    public override void FireWeapon(Vector3 firePoint, Vector3 dir)
    {
        float angle = 0f;
        Vector3 dirRight = Vector3.Cross(dir, Vector3.up);
        for (int i = 0; i < projectileAmount; i++)
        {
            float newAngle = Random.Range(-aimDistortion, aimDistortion);

            if (i % 2 == 0)
            {
                newAngle += angle;
                newAngle = -newAngle;
                
                Vector3 shootVector = Vector3.RotateTowards(dir, dirRight, DegreesToRadians(newAngle), 0f);
                Debug.DrawRay(firePoint, shootVector * 10, Color.green, 1f);
            }
            else
            {
                angle += spreadAngle;
                
                newAngle += angle;
                
                Vector3 shootVector = Vector3.RotateTowards(dir, dirRight, DegreesToRadians(newAngle), 0f);
                Debug.DrawRay(firePoint, shootVector * 10, Color.red, 1f);
            }
        }
    }
}
