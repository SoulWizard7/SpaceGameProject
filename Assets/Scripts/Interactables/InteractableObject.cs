
using System;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] public float radius = 3f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public abstract void Interact(InteractionController interactionController);
}
