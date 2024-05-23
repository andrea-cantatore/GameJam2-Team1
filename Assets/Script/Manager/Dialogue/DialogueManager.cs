using System;
using System.Collections;
using System.Collections.Generic;
using DS.Enumerations;
using DS.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private GameObject _singleChoicePanel, _multipleChoicePanel;
    [SerializeField] private GameObject[] _choices;
    [SerializeField] private Transform[] _choicesPos;
    [SerializeField] private TMP_Text _choice1Text, _choice2Text, _choice3Text;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private TMP_Text _customerName;
    private DSDialogueContainerSO _currentDialogue;
    private DSDialogueSO _currentDialogueSO;
    private DSDialogueSO _nextDialogueSO1, _nextDialogueSO2, _nextDialogueSO3;

    private void OnEnable()
    {
        EventManager.OnStartingDialogue += OnStartingDialogue;
    }

    private void OnDisable()
    {
        EventManager.OnStartingDialogue -= OnStartingDialogue;
    }

    private void OnStartingDialogue(DSDialogueContainerSO dialogueContainer, String CustomerName)
    {
        _currentDialogue = dialogueContainer;
        _dialoguePanel.SetActive(true);
        _customerName.text = CustomerName;
        foreach (var DsDialogueSO in _currentDialogue.UngroupedDialogues)
        {
            if (DsDialogueSO.IsStartingDialogue)
            {
                _currentDialogueSO = DsDialogueSO;
                UpdateDialogue();
                return;
            }
        }
    }

    private void UpdateDialogue()
    {
        if (_currentDialogueSO.DialogueType == DSDialogueType.SingleChoice)
        {
            _multipleChoicePanel.SetActive(false);
            _singleChoicePanel.SetActive(true);
            _dialogueText.text = _currentDialogueSO.Text;
        }
        else if(_currentDialogueSO.DialogueType == DSDialogueType.MultipleChoice)
        {
            _singleChoicePanel.SetActive(false);
            _multipleChoicePanel.SetActive(true);
            _dialogueText.text = _currentDialogueSO.Text;
            _choice1Text.text = _currentDialogueSO.Choices[0].Text;
            _choice2Text.text = _currentDialogueSO.Choices[1].Text;
            _choice3Text.text = _currentDialogueSO.Choices[2].Text;
            _nextDialogueSO1 = _currentDialogueSO.Choices[0].NextDialogue;
            _nextDialogueSO2 = _currentDialogueSO.Choices[1].NextDialogue;
            _nextDialogueSO3 = _currentDialogueSO.Choices[2].NextDialogue;
            RandomChoicesPos();
        }
        
    }

    private void RandomChoicesPos()
    {
        List<int> randomPos = new List<int> {0, 1, 2};
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, randomPos.Count);
            _choices[i].transform.position = _choicesPos[randomPos[randomIndex]].position;
            randomPos.RemoveAt(randomIndex);
        }
    }
    
    public void NextDialogue(int i)
    {
        if (_currentDialogueSO.DialogueType == DSDialogueType.SingleChoice)
        {
            _currentDialogueSO = _currentDialogueSO.Choices[0].NextDialogue;
            if (_currentDialogueSO == null)
            {
                EventManager.OnDialogueEnd?.Invoke();
                _dialoguePanel.SetActive(false);
                return;
            }
            UpdateDialogue();
        }
        else if(_currentDialogueSO.DialogueType == DSDialogueType.MultipleChoice)
        {
            if (i == 0)
            {
                _currentDialogueSO = _nextDialogueSO1;
            }
            else if (i == 1)
            {
                _currentDialogueSO = _nextDialogueSO2;
            }
            else if (i == 2)
            {
                _currentDialogueSO = _nextDialogueSO3;
            }
            if (_currentDialogueSO == null)
            {
                EventManager.OnDialogueEnd?.Invoke();
                _dialoguePanel.SetActive(false);
                return;
            }
            UpdateDialogue();
        }
        
    }

}
