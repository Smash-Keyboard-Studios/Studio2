using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class TheGraphView : GraphView
{
    public TheGraphView()
    {
        SetStyleSheet();
        CreateGridBackground();
        AddManipulators();
    }

    private void SetStyleSheet()
    {
        StyleSheet styleSheet = EditorGUIUtility.Load("DialogueTreeStyleSheet.uss") as StyleSheet;
        styleSheets.Add(styleSheet);
    }

    private void CreateGridBackground()
    {
        GridBackground gridBackground = new GridBackground();
        gridBackground.StretchToParentSize();
        Insert(0, gridBackground);
    }

    private void AddManipulators()
    {
        // panning
        this.AddManipulator(new ContentDragger());
        // soom specifically for graph views
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        // move nodes
        this.AddManipulator(new SelectionDragger());
        // rectangle node selection
        this.AddManipulator(new RectangleSelector());
        // add option to right click context menu
        this.AddManipulator(ContextMenuNode());
    }

    private IManipulator ContextMenuNode()
    {
        //create manipulator which makes the option in the context menu
        ContextualMenuManipulator manipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction("Add Dialogue Node", 
                actionEvent => AddElement(
                    CreateDialogueNode(actionEvent.eventInfo.localMousePosition)
                )
            )
        );
        return manipulator;
    }

    public DialogueNode CreateDialogueNode(Vector2 position)
    {
        return new DialogueNode(position);
    }

    public DialogueNode CreateDialogueNode(GUID guidParam, Vector2 position, DialogueItem dialogueItemParam)
    {
        return new DialogueNode(guidParam, position, dialogueItemParam);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        //function override is called when dragging out a connection from a node's port
        //finds which nodes the current one can connect to and adds this to compatiblePorts
        List<Port> compatiblePorts = new List<Port>();
        ports.ForEach(port =>
        {
            if(startPort != port &&
            startPort.node != port.node &&
            startPort.direction != port.direction)
            {
                compatiblePorts.Add(port);
            }
        });
        return compatiblePorts;
    }
}
