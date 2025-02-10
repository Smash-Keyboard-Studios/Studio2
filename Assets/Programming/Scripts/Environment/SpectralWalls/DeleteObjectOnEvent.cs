using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectOnEvent : MonoBehaviour
{
    // Removes the object this script is attached to when called.
    // Void will make this method return nothing
    public void RemoveObject()
    {
        Destroy(gameObject);
    }
}


