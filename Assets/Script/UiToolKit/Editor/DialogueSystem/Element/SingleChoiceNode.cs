using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class SingleChoiceNode : DialogueSystemNode
{
    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);
        
        DialogueType = DialogueSystemType.SingleChoice;
        
        Choices.Add("Next");
    }

    public override void Draw()
    {
        base.Draw();

        foreach (string choice in Choices)
        {
            Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            
            choicePort.portName = choice;
            
            outputContainer.Add(choicePort);
            
        }
        
        RefreshExpandedState();
        
    }
}
