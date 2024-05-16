using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : MonoBehaviour, IInteract
{

    private Transform _popUpPos;
    [SerializeField] private Transform _cameraOnInteraction;
    private Camera _mainCamera;

    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
        _mainCamera = Camera.main;
    }
    public bool Interact(bool isToAdd)
    {
        
        return true;
    }

    public void InteractionPopUp()
    {
        InteractionManager.Instance.InteractionPannel.transform.position = _popUpPos.position;
        InteractionManager.Instance.InteractionText.GetComponent<TMPro.TextMeshProUGUI>().text =
            "press E to Interact " + gameObject.name;
    }

}
