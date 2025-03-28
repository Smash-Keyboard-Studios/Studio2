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


    //sorry odin but this very confusing code and it doesn't work because u are playing audiosources without having any reference to an audiosource or any audioclip to play
    //let me know if you would like me to fix the code or explain to u how

    ////audio
    //public AudioSource audio1; 
    //public AudioSource audio2;
    //private bool isAudio1Playing = false; // if audio 1 is playing 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!inDialogue)
            {
                //audio1.Play(); // audio 1 plays
                //isAudio1Playing = true; // audio 1 is now playing
                StartCoroutine("ShowDialogue");
                inDialogue = true;
            }
        }
    }

    //private void Update()
    //{
    //    if (isAudio1Playing && !audio1.isPlaying) //checking audio1 and if its playing and then will play audio2 after 1 second
    //    {
    //        StartCoroutine("PlayAudio2");
    //        isAudio1Playing = false;
    //    }
    //}


    //private IEnumerator PlayAudio2()
    //{
    //    yield return new WaitForSeconds(1f);
    //    if (audio2 != null)
    //    {
    //        audio2.Play();
    //    }
    //}

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
