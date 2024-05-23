using System;
using System.Collections;
using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    public Transform[] objects;
    public Animator pageAnimator;
    public float switchDelay = 1.0f;
    public Transform cameraOnInteraction;
    public Transform originalCameraPos;

    private int currentIndex = 0;
    private bool canSwitch = true;
    private bool isInteracting = false;
    private Camera mainCamera;
    private Transform popUpPos;

    void Awake()
    {
        mainCamera = Camera.main;
        popUpPos = transform.GetChild(0);
    }

    void Start()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (i == currentIndex)
                objects[i].gameObject.SetActive(true);
            else
                objects[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isInteracting)
            {
                Interact(false);
            }
            else
            {
                Interact(true);
            }
        }

        if (isInteracting && canSwitch)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                SwitchToNextObject();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                SwitchToPreviousObject();
            }
        }
    }

    void SwitchToNextObject()
    {
        if (currentIndex < objects.Length - 1)
        {
            objects[currentIndex].gameObject.SetActive(false);
            currentIndex++;
            objects[currentIndex].gameObject.SetActive(true);

            if (pageAnimator != null)
            {
                pageAnimator.SetTrigger("toRight");
            }

            StartCoroutine(SwitchCooldown());
        }
    }

    void SwitchToPreviousObject()
    {
        if (currentIndex > 0)
        {
            objects[currentIndex].gameObject.SetActive(false);
            currentIndex--;
            objects[currentIndex].gameObject.SetActive(true);

            if (pageAnimator != null)
            {
                pageAnimator.SetTrigger("toLeft");
            }

            StartCoroutine(SwitchCooldown());
        }
    }

    IEnumerator SwitchCooldown()
    {
        canSwitch = false;
        yield return new WaitForSeconds(switchDelay);
        canSwitch = true;
    }

    void Interact(bool isToAdd)
    {
        isInteracting = isToAdd;
        EventManager.OnCuttingInteraction?.Invoke(isInteracting);

        if (isInteracting)
        {
            mainCamera.transform.position = cameraOnInteraction.position;
            mainCamera.transform.rotation = cameraOnInteraction.rotation;
        }
        else
        {
            mainCamera.transform.position = originalCameraPos.position;
            mainCamera.transform.rotation = originalCameraPos.rotation;
        }
    }

    public void InteractionPopUp()
    {
        if (isInteracting)
            return;

        Transform interactionPannel = popUpPos.GetChild(0);
        interactionPannel.position = popUpPos.position;
        TMPro.TextMeshProUGUI interactionText = interactionPannel.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        interactionText.text = "Press E to Interact " + gameObject.name;
    }

    private void OnEnable()
    {
        EventManager.OnCuttingInteraction += CuttingInteraction;
    }

    private void OnDisable()
    {
        EventManager.OnCuttingInteraction -= CuttingInteraction;
    }

    private void CuttingInteraction(bool isCutting)
    {
        isInteracting = isCutting;
    }
}