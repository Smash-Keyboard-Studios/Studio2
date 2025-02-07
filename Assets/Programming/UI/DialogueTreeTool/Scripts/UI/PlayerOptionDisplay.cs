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

    private void Update()
    {
        dialogueHandler = GetComponentInParent<DialogueHandler>();

        PlayerSpeechText = GetComponentInChildren<TextMeshProUGUI>();

        PlayerSpeechText.text = PlayerOption.dialogueItem.DialogueText;
        PlayerImage.sprite = PlayerOption.dialogueItem.IconRO;
    }

    public void onPress()
    {
        dialogueHandler.GetNextDialogueData(PlayerOption);
    }
}
