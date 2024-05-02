using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

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

    public void Draw()
    {
        TextField dialogueNameField = new TextField();
        dialogueNameField.value = DialogueName;
        
        titleContainer.Insert(0, dialogueNameField);
        
        Port inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        
        inputPort.portName = "Dialogue Input";
        inputContainer.Add(inputPort);
        
        
        VisualElement customDataContainer = new VisualElement();
        Foldout textFoldout = new Foldout();
        textFoldout.text = "Text";
        
        TextField textField = new TextField();
        textField.value = Text;
        
        textFoldout.Add(textField);
        customDataContainer.Add(textFoldout);
        extensionContainer.Add(customDataContainer);
        RefreshExpandedState();
    }
}

