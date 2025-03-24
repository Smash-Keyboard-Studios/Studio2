using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogEventTrigger : MonoBehaviour
{
    [SerializeField] private GameObject dialogueHandler;
    [SerializeField] private string conversationAsset;

    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
    }

    public void TriggerDialog()
    {

        playerInput.enabled = false;

        UIManager.Instance.inDialogueMenu = true;

        dialogueHandler.GetComponent<DialogueHandler>().ConversationAsset = conversationAsset;
        dialogueHandler.SetActive(true);
        Destroy(gameObject);

    }
}
