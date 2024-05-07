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
            Port choicePort = ElementUtilitys.CreatePort(this, "New Choice", Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            
            choicePort.portName = "";
            
            Button deleteButton = ElementUtilitys.CreateButton("X");
            
            deleteButton.AddToClassList("ds-node__button");
            
            TextField choiceTextField = ElementUtilitys.CreateTextField("New Choice");
            
            choiceTextField.AddClasses("ds-node__textfield", "ds-node__choice-textfield", "ds-node__textfield__hidden");
            
            outputContainer.Add(choicePort);
            
            choicePort.Add(choiceTextField);
            choicePort.Add(deleteButton);
        });
        
        addChoiceButton.AddToClassList("ds-node__button");
        
        mainContainer.Insert(1, addChoiceButton);
        
        foreach (string choice in Choices)
        {
            Port choicePort = ElementUtilitys.CreatePort(this, choice, Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            
            choicePort.portName = "";
            
            Button deleteButton = ElementUtilitys.CreateButton("X");
            
            deleteButton.AddToClassList("ds-node__button");
            
            TextField choiceTextField = ElementUtilitys.CreateTextField(choice);
            
            choiceTextField.AddToClassList("ds-node__textfield");
            choiceTextField.AddToClassList("ds-node__choice-textfield");
            choiceTextField.AddToClassList("ds-node__textfield__hidden");
            
            outputContainer.Add(choicePort);
            
            choicePort.Add(choiceTextField);
            choicePort.Add(deleteButton);
        }
        
        
    }
}
