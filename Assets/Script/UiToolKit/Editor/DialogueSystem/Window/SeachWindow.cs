using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SeachWindow : ScriptableObject, ISearchWindowProvider
{
    private GraphViewDialogue _graphViewDialogue;
    
    public void Initialize(GraphViewDialogue graphViewDialogue)
    {
        _graphViewDialogue = graphViewDialogue;
        
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
        {
            new SearchTreeGroupEntry(new GUIContent("Create Element"), 0),
            new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
            new SearchTreeEntry(new GUIContent("single Choice"))
            {
                level = 2,
                userData = DialogueSystemType.SingleChoice
            },
            new SearchTreeEntry(new GUIContent("Multiple Choice"))
            {
                level = 2,
                userData = DialogueSystemType.MultipleChoice
            },
            new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
            new SearchTreeEntry(new GUIContent("Single Group"))
            {
                level = 2,
                userData = new Group()
            }
            
        };
        
        return searchTreeEntries;
    }
    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        switch (SearchTreeEntry.userData)
        {
            case DialogueSystemType.SingleChoice:
            {
                SingleChoiceNode singleChoiceNode = (SingleChoiceNode) _graphViewDialogue.CreateElements(context.screenMousePosition, DialogueSystemType.SingleChoice);
                _graphViewDialogue.AddElement(singleChoiceNode);
                return true;
            }
            case DialogueSystemType.MultipleChoice:
            {
                MultipleChoiceNode multipleChoiceNode = (MultipleChoiceNode) _graphViewDialogue.CreateElements(context.screenMousePosition, DialogueSystemType.MultipleChoice);
                _graphViewDialogue.AddElement(multipleChoiceNode);
                return true;
            }
            case Group group:
            {
                Group createGroup = (Group) _graphViewDialogue.CreateGroup("DialogueGroup", context.screenMousePosition);
                
                _graphViewDialogue.AddElement(createGroup);
                return true;
            }

            default:
            {
                return false;
            }
                
            
        }
    }
}
