using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCTextDisplay : MonoBehaviour
{
    private DialogueHandler dialogueHandler;

    [SerializeField] private TextMeshProUGUI NPCNameText;
    [SerializeField] private TextMeshProUGUI NPCSpeechText;
    //[SerializeField] private Image NPCImage;

    //npc text colours
    [SerializeField] private Color32 HunterColour = new Color32(109, 234, 214, 255);
    [SerializeField] private Color32 WatchColour = new Color32(57, 230, 78, 255);
    [SerializeField] private Color32 PriestColour = new Color32(236, 39, 63, 255);
    [SerializeField] private Color DefaultColour = new Color32(255, 255, 255, 255);

    private void Awake()
    {
        dialogueHandler = GetComponentInParent<DialogueHandler>();
    }

    private void Update()
    {
        NPCNameText.text = dialogueHandler.currentData.dialogueItem?.NameTextRO;
        NPCSpeechText.text = dialogueHandler.currentData.dialogueItem?.DialogueText;
        //NPCImage.sprite = dialogueHandler.currentData.dialogueItem?.IconRO;

        //change colour of npc text based on who is speaking
        switch (NPCNameText.text.ToLower())
        {
            case "hunter":
                NPCNameText.color = HunterColour;
                break;
            case "watch":
                NPCNameText.color = WatchColour;
                break;
            case "priest":
                NPCNameText.color = PriestColour;
                break;
            default:
                NPCNameText.color = DefaultColour;
                break;
        }
    }
}
