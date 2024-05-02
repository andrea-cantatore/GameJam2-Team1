using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogueSystemNode : Node
{
    public string DialogueName { get; set; }
    public List<string> Choices { get; set; }
    public string Text { get; set; }
    
    public DialogueSystemType DialogueType { get; set; }

    public void Initialize()
    {
        DialogueName = "DialogueName";
        Choices = new List<string>();
        Text = "Text";
    }
}

