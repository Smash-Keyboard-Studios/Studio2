using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TriggerText : MonoBehaviour
{
    //UI text object that dialogue is outputted to
    [SerializeField] private TextMeshProUGUI dialogueText;

    //class that contains dialogue name and dialogue text in it
    [System.Serializable] public class Dialogue
    {
        public string dialogueName;
        public string dialogueText;
    }

    //instantiation of that class
    [SerializeField] private Dialogue[] dialogue;

    //whether currently in dialogue or not
    private bool inDialogue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!inDialogue)
            {
                StartCoroutine("ShowDialogue");
                inDialogue = true;
            }
        }
    }

    private IEnumerator ShowDialogue()
    {
        foreach (Dialogue item in dialogue)
        {
            string itemName = item.dialogueName;
            string itemText = item.dialogueText;

            dialogueText.text = itemName + "\n" + itemText;
            yield return new WaitForSeconds(itemText.Length/5); //just used a temp value for the delay

            ClearText();
            yield return new WaitForSeconds(0.5f);
        }
        Destroy(gameObject);

        ClearText();
    }

    private void ClearText()
    {
        dialogueText.text = string.Empty;
    }
}
