using UnityEditor;
using UnityEditor.Experimental.GraphView;
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
        
        styleSheets.Add(styleSheet);
    }
    
    private void AddManipulators()
    {
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new ContentDragger());
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(CreateNodeContextualMenu());
    }
    private IManipulator CreateNodeContextualMenu()
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
        menuEvent => menuEvent.menu.AppendAction("Add Node", action => CreateElements())
            );
        return contextualMenuManipulator;
    }

    private DialogueSystemNode CreateElements()
    {
        DialogueSystemNode node = new DialogueSystemNode();
        
        node.Initialize();
        node.Draw();

        return node;
    }
}
