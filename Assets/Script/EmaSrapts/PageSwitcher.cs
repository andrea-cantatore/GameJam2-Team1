using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectSwitcher : MonoBehaviour, IInteract
{
    [SerializeField] private Transform[] _objects;
    [SerializeField] private Animator _pageAnimator;
    [SerializeField] private float _switchDelay = 1.0f;
    [SerializeField] private Transform _cameraOnInteraction;
    [SerializeField] private Transform _originalCameraPos;
    [SerializeField] private AudioData _audioData;

    private int _currentIndex = 0;
    private bool _canSwitch = true;
    private bool _isInteracting = false;
    private Camera _mainCamera;
    private Transform _popUpPos;

    void Awake()
    {
        _mainCamera = Camera.main;
        _popUpPos = transform.GetChild(0);
    }

    void Start()
    {
        for (int i = 0; i < _objects.Length; i++)
        {
            if (i == _currentIndex)
                _objects[i].gameObject.SetActive(true);
            else
                _objects[i].gameObject.SetActive(false);
        }
    }
    
    private void OnEnable()
    {
        EventManager.OnBookInteraction += BookInteraction;
    }

    private void OnDisable()
    {
        EventManager.OnBookInteraction -= BookInteraction;
    }

    void Update()
    {

        if (_isInteracting && _canSwitch)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                SwitchToNextObject();
                AudioManager.instance.PlaySFX(_audioData.PageChange, transform);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                SwitchToPreviousObject();
                AudioManager.instance.PlaySFX(_audioData.PageChange, transform);
            }
        }
    }

    void SwitchToNextObject()
    {
        if (_currentIndex < _objects.Length - 1)
        {
            _objects[_currentIndex].gameObject.SetActive(false);
            _currentIndex++;
            _objects[_currentIndex].gameObject.SetActive(true);

            if (_pageAnimator != null)
            {
                _pageAnimator.SetTrigger("toRight");
            }

            StartCoroutine(SwitchCooldown());
        }
    }

    void SwitchToPreviousObject()
    {
        if (_currentIndex > 0)
        {
            _objects[_currentIndex].gameObject.SetActive(false);
            _currentIndex--;
            _objects[_currentIndex].gameObject.SetActive(true);

            if (_pageAnimator != null)
            {
                _pageAnimator.SetTrigger("toLeft");
            }

            StartCoroutine(SwitchCooldown());
        }
    }

    IEnumerator SwitchCooldown()
    {
        _canSwitch = false;
        yield return new WaitForSeconds(_switchDelay);
        _canSwitch = true;
    }

    public bool Interact(bool isToAdd)
    {
        _isInteracting = !_isInteracting;
        EventManager.OnBookInteraction?.Invoke(_isInteracting);

        if (_isInteracting)
        {
            _mainCamera.transform.position = _cameraOnInteraction.position;
            _mainCamera.transform.rotation = _cameraOnInteraction.rotation;
        }
        return true;
    }

    public void InteractionPopUp()
    {
        if (_isInteracting)
            return;

        InteractionManager.Instance.InteractionPannel.transform.position = _popUpPos.position;
        InteractionManager.Instance.InteractionText.GetComponent<TMPro.TextMeshProUGUI>().text =
            "press E to Interact " + gameObject.name;
    }


    private void BookInteraction(bool isCutting)
    {
        _isInteracting = isCutting;
    }
}