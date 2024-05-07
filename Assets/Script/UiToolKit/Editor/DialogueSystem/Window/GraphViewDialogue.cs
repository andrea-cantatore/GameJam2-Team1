using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GraphViewDialogue : GraphView
{ 
    public SearchWindow _searchWindow;
    
   public GraphViewDialogue()
   {
       GridBackgroundAdder();
       
       AddStyle();
    
       AddManipulators();
       
       AddSearchWindow();
   }
   public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
   {
       List<Port> compatiblePorts = new List<Port>();
       ports.ForEach((port) =>
       {
           if (startPort == port)
           {
               return;
           }
           
           if(startPort.node== port.node)
           {
               return;
           }
           
           if(startPort.direction == port.direction)
           {
               return;
           }
           
           compatiblePorts.Add(port);
       });
       return compatiblePorts;
   }
   private void GridBackgroundAdder()
   {
       GridBackground gridBackground = new GridBackground();
       gridBackground.StretchToParentSize();
       Insert(0, gridBackground);
   }
   
    private void AddStyle()
    {
        this.AddStyleSheets("DialogueSystem/DialogueStyle.uss", "DialogueSystem/NodesStyler.uss");
    }
    
    private void AddManipulators()
    {
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new ContentDragger());
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        
        this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DialogueSystemType.SingleChoice));
        this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DialogueSystemType.MultipleChoice));
        
        this.AddManipulator(CreateGroupMenu());
    }

    public IManipulator CreateGroupMenu()
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
        menuEvent => menuEvent.menu.AppendAction("Add Group", action =>
        {
            AddElement(CreateGroup("Dialogue Group", action.eventInfo.localMousePosition));
        })
            );
        return contextualMenuManipulator;
    }
    
    public GraphElement CreateGroup(string groupName, Vector2 position)
    {
        Group group = new Group()
        {
            title = groupName
        };
        
        group.SetPosition(new Rect(position, Vector2.zero));
        return group;
    }
    
    private IManipulator CreateNodeContextualMenu(string actionTitle, DialogueSystemType dialogueType)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
        menuEvent => menuEvent.menu.AppendAction(actionTitle, action =>
        {
            AddElement(CreateElements(action.eventInfo.localMousePosition, dialogueType));
        })
            );
        return contextualMenuManipulator;
    }

    public DialogueSystemNode CreateElements(Vector2 mousePosition, DialogueSystemType dialogueType)
    {
        Type nodeType = Type.GetType($"{dialogueType}Node");
        DialogueSystemNode node = (DialogueSystemNode)Activator.CreateInstance(nodeType);
        
        node.Initialize(mousePosition);
        node.Draw();

        return node;
    }
    
    private void AddSearchWindow()
    {
        if(_searchWindow == null)
        {
            _searchWindow = ScriptableObject.CreateInstance<SearchWindow>();
            _searchWindow.Initialize(this);
        }
    }
}
