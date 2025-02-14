using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOptionDisplay : MonoBehaviour
{
    public DialogueSaveData PlayerOption;
    [SerializeField] private DialogueHandler dialogueHandler;

    private TextMeshProUGUI PlayerSpeechText;
    [SerializeField] private Image PlayerImage;

    private void Awake()
    {
        dialogueHandler = GetComponentInParent<DialogueHandler>();

        PlayerSpeechText = GetComponentInChildren<TextMeshProUGUI>();

        PlayerImage = GameObject.FindGameObjectWithTag("DialoguePlayerImage").GetComponent<Image>();
    }

    private void Update()
    {
        PlayerSpeechText.text = PlayerOption.dialogueItem.DialogueText;
        PlayerImage.sprite = PlayerOption.dialogueItem.IconRO;
    }

    public void onPress()
    {
        dialogueHandler.GetNextDialogueData(PlayerOption);
    }
}
