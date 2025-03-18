using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueHandler : MonoBehaviour
{
    private DialogueTreeSaveData dialogueTreeData;

    public DialogueSaveData currentData = null;

    [SerializeField] private GameObject PlayerTextPrefab;
    [SerializeField] private GameObject PlayerTextContainer;
    public string ConversationAsset;

    //for playing audio
    private AudioSource audioSource;

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();

        GetTreeData();
    }

    private void GetTreeData()
    {
        dialogueTreeData = Resources.Load(ConversationAsset) as DialogueTreeSaveData; //loads the example conversation from resources

        //first foreach loop to find what the root dialogue is
        foreach (DialogueSaveData dialogueData in dialogueTreeData.dialogueData)
        {
            if (dialogueData.previousguids.Count == 0) { currentData = dialogueData; }
        }

        //update player options
        SetPlayerDialogueOptions();
    }

    public void GetNextDialogueData(DialogueSaveData playerOption)
    {
        currentData = null; //set currentdata to empty
        foreach(Transform child in PlayerTextContainer.transform) { Destroy(child.gameObject); } //remove all children within player text

        foreach (DialogueSaveData dialogueData in dialogueTreeData.dialogueData)
        {
            if (dialogueData.previousguids.Contains(playerOption.guid) && !dialogueData.dialogueItem.IsPlayerTextOptionRO)
            {
                //set new npc dialogue data
                currentData = dialogueData;

                //play the npc dialogue sound for the new data
                audioSource.PlayOneShot(currentData.dialogueItem.SoundToPlay);
            }
        }

        //disable this if no more data
        if(currentData == null) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>().enabled = true;

            UIManager.Instance.inDialogueMenu = false;

            this.gameObject.SetActive(false); 
        }
        
        //update player options
        SetPlayerDialogueOptions();
    }

    private void SetPlayerDialogueOptions()
    {
        //if current item isnt null
        if (currentData != null)
        {
            //foreach loop to find which playeroptions are children of the current item
            foreach (DialogueSaveData dialogueData in dialogueTreeData.dialogueData)
            {
                if (dialogueData.previousguids.Contains(currentData.guid) && dialogueData.dialogueItem.IsPlayerTextOptionRO)
                {
                    GameObject playeroption = Instantiate(PlayerTextPrefab, PlayerTextContainer.transform);
                    playeroption.GetComponent<PlayerOptionDisplay>().PlayerOption = dialogueData;
                }
            }
        }
    }
}
