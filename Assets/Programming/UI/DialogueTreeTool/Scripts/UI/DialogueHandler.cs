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

    //for waiting between dialogue so it doesn't cut to new dialogue too fast
    private bool waitingBetweenDialogue;

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;

        waitingBetweenDialogue = false;

        GetTreeData();
    }

    void Start()
    {
        if (currentData.dialogueItem == null) // ! domibron ~ added check to fix a bug
        {
            CloseDialogue();
        }
    }

    public void CloseDialogue()
    {
        ClearPlayerDialogueOptions();


        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        //set player camera controller to focus on player camera holder
        playerObject.GetComponentInChildren<CameraController>().ChangeCameraFocus
            (playerObject.GetComponentInChildren<PlayerCameraObject>().gameObject);

        //reenable input if not in game menu rn
        if (!UIManager.Instance.inGameMenu)
        {
            playerObject.GetComponent<PlayerInput>().enabled = true;
            UIManager.Instance.inDialogueMenu = false;
        }

        //set this to false
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (!waitingBetweenDialogue)
            {
                StartCoroutine("ProgressDialogueAfterXSeconds", 1);
            }
        }
    }

    private IEnumerator ProgressDialogueAfterXSeconds(float seconds)
    {
        waitingBetweenDialogue = true;
        yield return new WaitForSecondsRealtime(seconds);
        GetNextDialogueData(currentData);
        waitingBetweenDialogue = false;
    }

    private void GetTreeData()
    {
        if (string.IsNullOrEmpty(ConversationAsset)) return;

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
        ClearPlayerDialogueOptions();

        foreach (DialogueSaveData dialogueData in dialogueTreeData.dialogueData)
        {
            if (dialogueData.previousguids.Contains(playerOption.guid) && !dialogueData.dialogueItem.IsPlayerTextOptionRO)
            {
                //set new npc dialogue data
                currentData = dialogueData;

                //stop anything currently playing
                //then play the npc dialogue sound for the new data
                audioSource.Stop();
                audioSource.clip = currentData.dialogueItem.SoundToPlay;
                audioSource.Play();
            }
        }

        //disable this if no more data
        if (currentData == null)
        {
            CloseDialogue();
        }

        //update player options
        SetPlayerDialogueOptions();
    }

    private void ClearPlayerDialogueOptions()
    {
        foreach (Transform child in PlayerTextContainer.transform) { Destroy(child.gameObject); } //remove all children within player text
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
