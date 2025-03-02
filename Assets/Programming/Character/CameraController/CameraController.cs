using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    const float fastLerpSpeed = 1f;
    const float slowLerpSpeed = 0.05f;

    private float lerpSpeed = slowLerpSpeed;

    [HideInInspector] public GameObject objectToFollow;

    private void Update()
    {
        CameraFollowObject();
    }
    private void CameraFollowObject()
    {
        if (objectToFollow!=null)
        {
            //extra checks so camera doesnt nudge when moving
            if (Vector3.Distance(transform.position, objectToFollow.transform.position) < 0.1f)
            {
                lerpSpeed = fastLerpSpeed;
            }

            transform.position = Vector3.Lerp(transform.position, objectToFollow.transform.position, lerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, objectToFollow.transform.rotation, lerpSpeed);
        }
    }

    public void ChangeCameraFocus(GameObject obj)
    {
        objectToFollow = obj;
        lerpSpeed = slowLerpSpeed;
    }
}
