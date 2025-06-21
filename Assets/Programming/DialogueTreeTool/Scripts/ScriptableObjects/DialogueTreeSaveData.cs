using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DialogueSaveData
{
    [SerializeField] public string guid;
    [SerializeField] public DialogueItem dialogueItem;
    [SerializeField] public Vector2 position;
    [SerializeField] public List<string> previousguids;

    public override string ToString()
    {
        return dialogueItem == null ? "No Dialogue Item" : dialogueItem.NameTextRO;
    }
}

public class DialogueTreeSaveData : ScriptableObject
{
    public List<DialogueSaveData> dialogueData = new List<DialogueSaveData>();
}
