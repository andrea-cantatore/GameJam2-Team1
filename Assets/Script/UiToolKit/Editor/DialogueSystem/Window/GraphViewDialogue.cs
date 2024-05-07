using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GraphViewDialogue : GraphView
{
   public GraphViewDialogue()
   {
       GridBackgroundAdder();
       
       AddStyle();
    
       AddManipulators();
       
   }
   
   private void GridBackgroundAdder()
   {
       GridBackground gridBackground = new GridBackground();
       gridBackground.StretchToParentSize();
       Insert(0, gridBackground);
   }
   
    private void AddStyle()
    {
        StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/DialogueStyle.uss");
        StyleSheet styleSheet1 = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/NodesStyler.uss");
        
        styleSheets.Add(styleSheet);
        styleSheets.Add(styleSheet1);
    }
    
    private void AddManipulators()
    {
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new ContentDragger());
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        
        this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DialogueSystemType.SingleChoice));
        this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DialogueSystemType.MultipleChoice));
    }
    private IManipulator CreateNodeContextualMenu(string actionTitle, DialogueSystemType dialogueType)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
        menuEvent => menuEvent.menu.AppendAction(actionTitle, action =>
        {
            AddElement(CreateElements(menuEvent.mousePosition, dialogueType));
        })
            );
        return contextualMenuManipulator;
    }

    private DialogueSystemNode CreateElements(Vector2 mousePosition, DialogueSystemType dialogueType)
    {
        Type nodeType = Type.GetType($"{dialogueType}Node");
        DialogueSystemNode node = (DialogueSystemNode)Activator.CreateInstance(nodeType);
        
        node.Initialize(mousePosition);
        node.Draw();

        return node;
    }
}
