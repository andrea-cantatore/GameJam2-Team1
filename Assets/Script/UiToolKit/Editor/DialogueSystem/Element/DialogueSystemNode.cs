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
        TextField dialogueNameField = ElementUtilitys.CreateTextField(DialogueName);

        dialogueNameField.AddClasses("ds-node__text-field", "ds-node__filename-text-field", "ds-node__text-field__hidden");
        
        titleContainer.Insert(0, dialogueNameField);
        
        Port inputPort = ElementUtilitys.CreatePort(this, "Dialogue Input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        
        inputContainer.Add(inputPort);
        
        
        VisualElement customDataContainer = new VisualElement();
        
        customDataContainer.AddToClassList("ds-node__custom-data-container");
        
        Foldout textFoldout = ElementUtilitys.CreateFoldout("Text");
        
        TextField textField = ElementUtilitys.CreateTextFieldArea(Text);
        
        textField.AddClasses("ds-node__text-field", "ds-node__quote-text-field");
        
        textFoldout.Add(textField);
        customDataContainer.Add(textFoldout);
        extensionContainer.Add(customDataContainer);
    }
}

