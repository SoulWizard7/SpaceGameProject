
using System.Collections.Generic;
using UnityEngine;

public class GroundItemManager : MonoBehaviour
{
    public static List<GroundItem> GroundItems = new List<GroundItem>();

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < GroundItems.Count; i++)
        {
            Gizmos.DrawLine(transform.position, GroundItems[i].transform.position);
        }
    }
}
