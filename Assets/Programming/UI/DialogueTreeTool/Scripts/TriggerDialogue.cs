using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerDialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueHandler;
    [SerializeField] private string conversationAsset;
    [SerializeField] private bool pausesPlayer;

    [Header("Changes Camera Angle?")]
    public bool HasCameraChange;

    [Header("If Changes Camera Angle:\nNew gameobject for Camera Controller to move to")]
    public GameObject NewCameraLocation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (pausesPlayer)
            {
                //disable input
                other.GetComponent<PlayerInput>().enabled = false;
                UIManager.Instance.inDialogueMenu = true;
            }

            //if camerachange, change camera to new location
            if (HasCameraChange) { other.GetComponentInChildren<CameraController>().ChangeCameraFocus(NewCameraLocation); }

            //open dialogue
            dialogueHandler.SetActive(false); //set to false so can be reopened and have conversaionasset update
            dialogueHandler.GetComponent<DialogueHandler>().ConversationAsset = conversationAsset;
            dialogueHandler.SetActive(true);
            Destroy(gameObject);
        }
    }
}
