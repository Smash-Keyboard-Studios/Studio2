using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraAngle : MonoBehaviour
{
    private GameObject PlayerCameraLocation;

    [Header("New gameobject for the camera controller to move to")]
    public GameObject NewCameraLocation;

    [Header("Time that the camera will be in the new location for")]
    public float cameraMoveTime = 1f;

    private bool inCameraMovement;

    private void Awake()
    {
        inCameraMovement = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCameraLocation = other.GetComponentInChildren<PlayerCameraObject>().gameObject; //set player camera

            if (!inCameraMovement) { StartCoroutine("ChangeTheCameraAngle", other.gameObject); }
        }
    }

    IEnumerator ChangeTheCameraAngle(GameObject playerObject)
    {
        playerObject.GetComponentInChildren<CameraController>().ChangeCameraFocus(NewCameraLocation);

        yield return new WaitForSeconds(cameraMoveTime);

        playerObject.GetComponentInChildren<CameraController>().ChangeCameraFocus(PlayerCameraLocation);
    }
}
