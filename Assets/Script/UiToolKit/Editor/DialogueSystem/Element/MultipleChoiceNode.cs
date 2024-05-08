using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class MultipleChoiceNode : DialogueSystemNode
{
    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);
        this.AddClasses("multiple-choice-node");
        
        DialogueType = DialogueSystemType.MultipleChoice;
        
        Choices.Add("Choice 1");
        Choices.Add("Choice 2");
    }

    public override void Draw()
    {
        base.Draw();

        Button addChoiceButton = ElementUtilitys.CreateButton("Add Choice", () =>
        {
            Port choicePort = CreateChoicePort("New Choice");
            Button deleteButton = CreateDeleteButton();
            TextField choiceTextField = CreateChoiceTextField("New Choice");
            
            AddChoice(choicePort, choiceTextField, deleteButton);
        });
        
        addChoiceButton.AddToClassList("ds-node__button");
        
        mainContainer.Insert(1, addChoiceButton);
        
        foreach (string choice in Choices)
        {
            Port choicePort = CreateChoicePort(choice);
            Button deleteButton = CreateDeleteButton();
            TextField choiceTextField = CreateChoiceTextField(choice);
            
            AddChoice(choicePort, choiceTextField, deleteButton);
        }
        
        Port CreateChoicePort(string buttonName)
        {
            var port = ElementUtilitys.CreatePort(this, "New Choice", Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            port.portName = "";
            return port;
        }

        Button CreateDeleteButton()
        {
            var button = ElementUtilitys.CreateButton("X");
            button.AddToClassList("ds-node__button");
            return button;
        }

        TextField CreateChoiceTextField(string choiceName)
        {
            var textField = ElementUtilitys.CreateTextField(choiceName);
            textField.AddClasses("ds-node__text-field", "ds-node__choice-text-field", "ds-node__text-field__hidden");
            return textField;
        }

        void AddChoice(Port choicePort, TextField choiceTextField, Button deleteButton)
        {
            outputContainer.Add(choicePort);
            choicePort.Add(choiceTextField);
            choicePort.Add(deleteButton);
        }
    }
    
    
}
