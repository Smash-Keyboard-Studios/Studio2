using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPointer : MonoBehaviour
{
    [Tooltip("Leave blank to reset")]
    public Transform objectToPointTo;

    private GameObject playerCanvasObject;

    void Start()
    {
        playerCanvasObject = PlayerCanvasReference.instance.GetPlayerCanvasReference();
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     if (!other.gameObject.CompareTag("Player")) return;

    //     playerCanvasObject.GetComponent<MinimapController>().targetPoint = objectToPointTo.position;
    // }


    public void PointToLocation()
    {
        playerCanvasObject.GetComponent<MinimapController>().targetPoint = objectToPointTo.position;
    }
}
