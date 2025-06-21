using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json.Bson;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Codice.Client.Common.Connection.AskCredentialsToUser;

public static class DialogueSaveUtility
{
    public static void Save(string filename, TheGraphView graphView)
    {
        if (graphView == null) return;

        string savePath = EditorUtility.SaveFilePanelInProject("Save Dialogue Tree", filename, "asset", string.Empty);
        if (savePath == string.Empty) return;

        DialogueTreeSaveData saveData = ScriptableObject.CreateInstance("DialogueTreeSaveData") as DialogueTreeSaveData;

        graphView.graphElements.ForEach(element =>
        {
            if (element is DialogueNode node)
            {
                //create and initialise save data for each node
                DialogueSaveData dialogue = new DialogueSaveData();
                DialogueSaveDataInit(dialogue, node.guid, node.dialogueItem, node.GetPosition().position);

                List<string> previousNodes = GetPreviousNode(node);
                if (previousNodes != null)
                {
                    foreach (var previousNode in previousNodes)
                    {
                        SetPreviousNode(dialogue, StringToGUID(previousNode));
                    }
                }

                saveData.dialogueData.Add(dialogue);
                return;
            }
        });

        //save asset in specified path in asset database
        AssetDatabase.CreateAsset(saveData, savePath);
        AssetDatabase.Refresh();
    }

    public static void Load(TheGraphView graphView)
    {
        string loadPath = EditorUtility.OpenFilePanel("Load Dialogue Tree", "Assets", "asset");
        if(loadPath == string.Empty) return;

        loadPath = SetPathAsRelative(loadPath);
        DialogueTreeSaveData saveData = AssetDatabase.LoadAssetAtPath<DialogueTreeSaveData>(loadPath);

        Dictionary<GUID, DialogueNode> nodes = new Dictionary<GUID, DialogueNode>();

        foreach(DialogueSaveData dialogueData in saveData.dialogueData)
        {
            GUID dialogueguid = StringToGUID(dialogueData.guid);

            DialogueNode node = graphView.CreateDialogueNode(dialogueguid, dialogueData.position, dialogueData.dialogueItem);
            graphView.AddElement(node);
            nodes.Add(dialogueguid, node);
        }

        foreach(var node in nodes)
        {
            Port inputPort = node.Value.inputContainer.ElementAt(0) as Port;

            List<string> previousnodesguids = LinearSearchForGUID(saveData.dialogueData, node.Key).previousguids;
            List<DialogueNode> previousnodes = new List<DialogueNode>();

            if (previousnodesguids != null)
            {
                foreach (var prevnode in previousnodesguids)
                {
                    previousnodes.Add(nodes[StringToGUID(prevnode)]);
                }

                //previousnodes now contains the previous nodes in scene reference
                Debug.Log(previousnodesguids.Count);
                Debug.Log(previousnodes.Count);

                foreach (var prevnode in previousnodes)
                {
                    Port outputPort = prevnode.outputContainer.ElementAt(0) as Port;
                    Edge newEdge = inputPort.ConnectTo(outputPort);
                    graphView.Add(newEdge);
                }
            }
        }
    }

    public static List<string> GetPreviousNode(DialogueNode node)
    {
        List<string> previousnodes = new List<string>();

        Port inputPort = node.inputContainer.ElementAt(0) as Port;//get inputport which will be first input container

        foreach (Edge edge in inputPort.connections)
        {
            DialogueNode prevnode = edge.output.node as DialogueNode;
            Debug.Log(prevnode.guid.ToString());
            previousnodes.Add(prevnode.guid.ToString());
        }
        return previousnodes;
    }

    public static string SetPathAsRelative(string absolutePath)
    {
        if (absolutePath.StartsWith(Application.dataPath))
        {
            return "Assets" + absolutePath.Substring(Application.dataPath.Length);
        }
        else
        {
            throw new System.ArgumentException("Full path does not contain the current project's Assets folder", absolutePath);
        }
    }

    public static GUID StringToGUID(string str)
    {
        GUID guid;
        if (!GUID.TryParse(str, out guid))
        {
            throw new ArgumentException("Invalid GUID: cannot convert from string to GUID");
        }
        return guid;
    }


    //for dialogue tree save adta

    public static void DialogueSaveDataInit(DialogueSaveData dialogueSaveData, GUID guidParam, DialogueItem dialogueItemParam, Vector2 positionParam)
    {
        dialogueSaveData.guid = guidParam.ToString();
        dialogueSaveData.dialogueItem = dialogueItemParam;
        dialogueSaveData.position = positionParam;
        dialogueSaveData.previousguids = new List<string>();
    }

    public static void SetPreviousNode(DialogueSaveData dialogueSaveData, GUID previousguidParam)
    {
        dialogueSaveData.previousguids.Add(previousguidParam.ToString());
    }

    public static DialogueSaveData LinearSearchForGUID(List<DialogueSaveData> dialogueData, GUID guidParam)
    {
        foreach (DialogueSaveData dialogue in dialogueData)
        {
            if (dialogue.guid == guidParam.ToString()) { return dialogue; }
        }
        return null;
    }
}
