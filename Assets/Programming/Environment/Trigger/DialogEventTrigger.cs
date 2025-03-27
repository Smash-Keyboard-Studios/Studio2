using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogEventTrigger : MonoBehaviour
{
    [SerializeField] private GameObject dialogueHandler;
    [SerializeField] private string conversationAsset;

    [Header("Camera Angle"), SerializeField]
    private bool hasTargetCameraLocation = false;

    [SerializeField]
    private GameObject targetCameraLocation;

    private PlayerInput playerInput;
    private CameraController cameraController;

    public void TriggerDialog()
    {
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        cameraController = GameObject.Find("Player").GetComponentInChildren<CameraController>();


        // disable input
        playerInput.enabled = false;
        UIManager.Instance.inDialogueMenu = true;

        //camera angle
        if (hasTargetCameraLocation && targetCameraLocation != null) cameraController.ChangeCameraFocus(targetCameraLocation);

        // Open dialog
        dialogueHandler.GetComponent<DialogueHandler>().ConversationAsset = conversationAsset;
        dialogueHandler.SetActive(true);
        Destroy(gameObject);

    }
}