using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// domibron modified.

public class CameraController : MonoBehaviour
{
    // TODO: camera lerps back to normal weirdly, need to fix.

    public float cameraMoveSpeed = 0.05f;


    private bool cameraMoving = false;
    private float localTimer = 0f;

    private Transform cameraCurrentTransform;

    [HideInInspector] public GameObject objectToFollow;

    private void Update()
    {
        CameraFollowObject();
    }

    private void CameraFollowObject()
    {
        if (objectToFollow != null)
        {
            if (cameraMoving == true) localTimer += Time.unscaledDeltaTime * cameraMoveSpeed;

            if (localTimer >= 1) cameraMoving = false;


            transform.position = Vector3.Lerp(cameraCurrentTransform.position, objectToFollow.transform.position, localTimer);
            transform.rotation = Quaternion.Lerp(cameraCurrentTransform.rotation, objectToFollow.transform.rotation, localTimer);
        }
    }

    public void ChangeCameraFocus(GameObject obj)
    {
        cameraCurrentTransform = transform;
        cameraMoving = true;
        localTimer = 0f;
        print(obj.name);

        objectToFollow = obj;
    }
}
