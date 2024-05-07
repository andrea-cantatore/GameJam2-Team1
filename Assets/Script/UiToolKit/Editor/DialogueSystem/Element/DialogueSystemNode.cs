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

    public virtual void Initialize(Vector2 position)
    {
        DialogueName = "DialogueName";
        Choices = new List<string>();
        Text = "Text";
        
        SetPosition(new Rect(position, Vector2.zero));
        
        extensionContainer.AddToClassList("ds-node__main-container");
        mainContainer.AddToClassList("ds-node__extension-container");
    }

    public virtual void Draw()
    {
        TextField dialogueNameField = new TextField();
        dialogueNameField.value = DialogueName;
        
        dialogueNameField.AddToClassList("ds-node__textfield");
        dialogueNameField.AddToClassList("ds-node__filename-textfield");
        dialogueNameField.AddToClassList("ds-node__textfield__hidden");
        
        titleContainer.Insert(0, dialogueNameField);
        
        Port inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        
        inputPort.portName = "Dialogue Input";
        inputContainer.Add(inputPort);
        
        
        VisualElement customDataContainer = new VisualElement();
        
        customDataContainer.AddToClassList("ds-node__custom-data-container");
        
        Foldout textFoldout = new Foldout();
        textFoldout.text = "Text";
        
        TextField textField = new TextField();
        textField.value = Text;
        
        textField.AddToClassList("ds-node__textfield");
        textField.AddToClassList("ds-node__quote-textfield");
        
        textFoldout.Add(textField);
        customDataContainer.Add(textFoldout);
        extensionContainer.Add(customDataContainer);
    }
}

