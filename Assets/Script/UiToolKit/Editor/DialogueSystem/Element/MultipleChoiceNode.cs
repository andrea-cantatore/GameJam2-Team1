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
        
        DialogueType = DialogueSystemType.MultipleChoice;
        
        Choices.Add("Choice 1");
        Choices.Add("Choice 2");
    }

    public override void Draw()
    {
        base.Draw();

        Button addChoiceButton = new Button()
        {
            text = "Add Choice"
        };
        
        addChoiceButton.AddToClassList("ds-node__button");
        
        mainContainer.Insert(1, addChoiceButton);
        
        foreach (string choice in Choices)
        {
            Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            
            choicePort.portName = "";
            
            Button deleteButton = new Button()
            {
                text = "X"
            };
            
            deleteButton.AddToClassList("ds-node__button");
            
            TextField choiceTextField = new TextField()
            {
                value = choice
            };
            
            choiceTextField.AddToClassList("ds-node__textfield");
            choiceTextField.AddToClassList("ds-node__choice-textfield");
            choiceTextField.AddToClassList("ds-node__textfield__hidden");
            
            outputContainer.Add(choicePort);
            
            choicePort.Add(choiceTextField);
            choicePort.Add(deleteButton);
        }
        
        
    }
}
