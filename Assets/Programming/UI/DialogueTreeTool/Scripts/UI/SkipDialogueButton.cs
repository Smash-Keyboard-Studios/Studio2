using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkipDialogueButton : MonoBehaviour
{
    public void SkipDialogue()
    {
        GetComponent<DialogueHandler>().CloseDialogue();
    }
}
