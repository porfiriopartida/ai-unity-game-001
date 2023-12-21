using System;
using Unity.VisualScripting;
using UnityEngine;

public class HangableObject : MonoBehaviour
{
    // Define any properties specific to the hangable object here,
    // such as force required to hang, durability, etc. if needed
    
    public Transform hangingPoint;

    //
    // private void OnTriggerEnter(Collider other)
    // {
    //     foreach (ContactPoint contact in collision.contacts)
    //     {
    //         Debug.DrawRay(contact.point, contact.normal, Color.white);
    //     }
    // }
}