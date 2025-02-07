using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

public class DialogueNode : Node
{
    //unique identifier thing
    internal GUID guid { get; private set; }

    //internal is only accessible within the assembly/DLL file
    internal DialogueItem dialogueItem;
    Image icon;
    TextField speechText;

    public DialogueNode(Vector2 position)
    {
        guid = GUID.Generate();
        SetPosition(new Rect(position, Vector2.zero));
        UpdateDialogueNode();
    }

    public DialogueNode(GUID guidParam, Vector2 position, DialogueItem dialogueItemParam)
    {
        guid = guidParam;
        SetPosition(new Rect(position, Vector2.zero));
        dialogueItem = dialogueItemParam;
        UpdateDialogueNode();
    }

    public void UpdateDialogueNode()
    {
        //update title container to be the person speaking field
        UpdateTitle();

        //add input/output ports
        Port inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, null);
        inputPort.portName = "Input";
        inputContainer.Add(inputPort);

        Port outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, null);
        outputPort.portName = "Output";
        outputContainer.Add(outputPort);

        //create custom container for other elements
        VisualElement speechContainer = new VisualElement();

        //DialogueItem/scriptable object field
        ObjectField SOField = new ObjectField()
        {
            objectType = typeof(DialogueItem),
            value = dialogueItem ? dialogueItem : null,
        };
        speechContainer.Add(SOField);
        SOField.RegisterValueChangedCallback(evt => ChangeDialogueItem(evt));

        //create and update icon and speech text before drawing them
        icon = new Image();
        speechText = new TextField();
        UpdateIconAndSpeech();

        //icon image
        speechContainer.Add(icon);
        speechText.RegisterValueChangedCallback(evt => ChangeSpeech(evt));

        //speech text in foldout
        Foldout textFoldout = new Foldout()
        {
            text = "Speech \n(Modifying this will also change the Dialogue Item)",
        };
        textFoldout.Add(speechText);
        speechContainer.Add(textFoldout);

        //add speech container to the bottom of the node
        extensionContainer.Add(speechContainer);

        //make custom elements visible after creating
        RefreshExpandedState();
    }

    private void UpdateTitle()
    {
        titleContainer.Clear();
        //create new textfields depending on if there is a dialogue item

        TextField whetherPlayerText = new TextField()
        {
            value = dialogueItem != null ?
            (dialogueItem.IsPlayerTextOptionRO ? "Player Text\n" : "NPC Text\n")
            : string.Empty,
            isReadOnly = true,
        };
        //set title container to the text field we made
        titleContainer.Insert(0, whetherPlayerText);

        TextField nodeName = new TextField()
        {
            value = dialogueItem != null ? dialogueItem.NameTextRO : "Empty",
            isReadOnly = true,
        };
        //set title container to the text field we made
        titleContainer.Insert(0, nodeName);
    }

    private void UpdateIconAndSpeech()
    {
        //icon update
        const float iconWidthHeight = 16f;

        if(dialogueItem != null)
        {
            icon.sprite = dialogueItem.IconRO;
            icon.style.width = iconWidthHeight;
            icon.style.height = iconWidthHeight;
        }
        else
        {
            icon.sprite = null;
        }

        //speech update
        speechText.value = dialogueItem != null ? dialogueItem.DialogueText : "Empty";
    }

    private void ChangeSpeech(ChangeEvent<string> evt)
    {
        if(dialogueItem != null) { dialogueItem.DialogueText = speechText.value; }
    }

    private void ChangeDialogueItem(ChangeEvent<UnityEngine.Object> evt)
    {
        dialogueItem = evt.newValue as DialogueItem;
        UpdateTitle();
        UpdateIconAndSpeech();
    }
}
