using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// DOMIBRON OWNS this now :3 basically re did most / all of this.
public class CameraController : MonoBehaviour
{
    const float cameraMoveSpeed = 0.05f;


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
            // //extra checks so camera doesnt nudge when moving
            // if (Vector3.Distance(transform.position, objectToFollow.transform.position) < 0.1f)
            // {
            //     lerpSpeed = fastLerpSpeed;
            // }

            // redid most of this because it was in FUCKING FRAME TIME!!!!!!! 

            if (cameraMoving == true) localTimer += Time.deltaTime * cameraMoveSpeed;

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
