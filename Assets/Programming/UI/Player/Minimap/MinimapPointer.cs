using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPointer : MonoBehaviour
{
    [Tooltip("Leave blank to reset")]
    public Transform objectToPointTo;

    public void PointToLocation()
    {
        if (MinimapController.instance == null) return;

        MinimapController.instance.GetComponent<MinimapController>().targetPoint = objectToPointTo.position;
    }
}
