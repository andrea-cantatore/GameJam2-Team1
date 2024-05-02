using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueSystemWindow : EditorWindow
{
    [MenuItem("Window/CustomTools/Dialogue/DialogueSystemWindow")]
    public static void Open()
    {
        DialogueSystemWindow wnd = GetWindow<DialogueSystemWindow>("Dialogue System");
    }

    private void OnEnable()
    {
        AddGraphView();
        
        AddStyle();
    }
    
    private void AddGraphView()
    {
        GraphViewDialogue graphView = new GraphViewDialogue();
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }
    
    private void AddStyle()
    {
        StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/DialogueVariableStyle.uss");
        
        rootVisualElement.styleSheets.Add(styleSheet);
    }

}
