using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|
// © 2025 Dominic McNeill dommcneill@outlook.com


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
