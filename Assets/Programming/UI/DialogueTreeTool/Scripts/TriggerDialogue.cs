using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueHandler;
    [SerializeField] private string conversationAsset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;

            dialogueHandler.GetComponent<DialogueHandler>().ConversationAsset = conversationAsset;
            dialogueHandler.SetActive(true);
            Destroy(gameObject);
        }
    }
}
