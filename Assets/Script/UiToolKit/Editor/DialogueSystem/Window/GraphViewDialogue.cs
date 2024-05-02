using System.Collections;
using System.Collections.Generic;
using PlasticPipe.PlasticProtocol.Server.Stubs;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphViewDialogue : GraphView
{
   public GraphViewDialogue()
   {
       GridBackgroundAdder();
       
       AddStyle();
    
       AddManipulators();
       
       CreateElements();
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
        
        styleSheets.Add(styleSheet);
    }
    
    private void AddManipulators()
    {
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new ContentDragger());
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
    }

    private void CreateElements()
    {
        DialogueSystemNode node = new DialogueSystemNode();
        node.Initialize();
        AddElement(node);
    }
}
