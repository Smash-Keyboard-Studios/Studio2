using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerDialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueHandler;
    [SerializeField] private string conversationAsset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerInput>().enabled = false;

            UIManager.Instance.inDialogueMenu = true;

            dialogueHandler.GetComponent<DialogueHandler>().ConversationAsset = conversationAsset;
            dialogueHandler.SetActive(true);
            Destroy(gameObject);
        }
    }
}
