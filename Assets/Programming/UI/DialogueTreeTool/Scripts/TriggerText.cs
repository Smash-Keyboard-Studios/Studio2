using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TriggerText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private string[] dialogue;

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
        foreach (string item in dialogue)
        {
            dialogueText.text = item;
            yield return new WaitForSeconds(item.Length/5); //just used a temp value for the delay

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
