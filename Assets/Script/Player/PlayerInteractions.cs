using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private Transform _cam;
    [SerializeField] private GameObject[] _interactables;
    [SerializeField] private GameObject[] _dishObjects;
    [SerializeField] private float _interactionDistance = 5f;
    private bool _isHandFull, _isDishHand;
    private GameObject _heldObject;
    private PlayerDish _playerDish => GetComponent<PlayerDish>();


    private void Awake()
    {
        _cam = Camera.main.transform;
    }

    private void OnEnable()
    {
        EventManager.OnGrillPickUp += GrillPickUp;
        EventManager.OnCutted += CuttedFoodPickUp;
    }

    private void OnDisable()
    {
        EventManager.OnGrillPickUp -= GrillPickUp;
        EventManager.OnCutted -= CuttedFoodPickUp;
    }

    private void Update()
    {
        _isDishHand = _interactables[8].activeSelf;

        RaycastHit hit;
        if (Physics.Raycast(_cam.position, _cam.forward, out hit, _interactionDistance))
        {
            if (hit.transform.TryGetComponent(out IInteract interactable))
            {
                interactable.InteractionPopUp();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if(hit.transform.TryGetComponent(out ICustomer customer))
                    {
                        interactable.Interact(false);
                        return;
                    }
                    if (hit.transform.tag == "RecipeBook")
                    {
                        interactable.Interact(true);
                    }
                    if (!_isHandFull)
                    {
                        if (hit.transform.tag == "Grill")
                        {
                            interactable.Interact(false);
                            return;
                        }
                        foreach (GameObject obj in _interactables)
                        {
                            if (obj.tag == tag && tag == "Dish")
                            {
                                obj.SetActive(true);
                                _heldObject = obj;
                                _isHandFull = true;
                                return;
                            }
                            if (obj.tag == hit.transform.tag)
                            {
                                if (interactable.Interact(false))
                                {
                                    obj.SetActive(true);
                                    obj.TryGetComponent(out IHeldFood heldFood);
                                    heldFood.IBasicMaterial();
                                    heldFood.ICooked(0);
                                    _heldObject = obj;
                                    _isHandFull = true;
                                    return;
                                }
                            }
                        }
                    }
                    if (hit.transform.tag == "Grill")
                    {
                        GrillInteraction(hit.transform.gameObject);
                        return;
                    }
                    if (hit.transform.tag == "CuttingBoard")
                    {
                        if (_isHandFull)
                        {
                            if (hit.transform.TryGetComponent(out ICutting cutting))
                            {
                                if (cutting.CutInteraction(_heldObject))
                                {
                                    interactable.Interact(true);
                                    _heldObject.SetActive(false);
                                    _heldObject = null;
                                    _isHandFull = false;
                                }
                            }
                        }
                        else
                        {
                            if (hit.transform.TryGetComponent(out CuttingBoard cuttingBoard))
                            {
                                if (!cuttingBoard.IsCuttingEmpty)
                                {
                                    interactable.Interact(true);
                                }
                            }
                        }
                        return;
                    }
                    if (_heldObject != null)
                    {
                        if (hit.transform.tag == _heldObject.tag)
                        {
                            interactable.Interact(true);
                            _heldObject.SetActive(false);
                            _heldObject = null;
                            _isHandFull = false;
                            return;
                        }
                    }
                    if (hit.transform.TryGetComponent(out ICounterHolder counterHolder))
                    {
                        if (_isHandFull)
                        {
                            if (_isDishHand)
                            {
                                
                                if (counterHolder.TakeObject(_heldObject.GetComponent<HeldFood>().MyId(), _heldObject,
                                        true, _heldObject.GetComponent<HeldFood>().IsSliced))
                                {
                                    _heldObject.SetActive(false);
                                    _heldObject = null;
                                    _isHandFull = false;
                                    return;
                                }
                            }
                            if (counterHolder.TakeObject(_heldObject.GetComponent<HeldFood>().MyId(), _heldObject,
                                    false, _heldObject.GetComponent<HeldFood>().IsSliced) && !_isDishHand)
                            {
                                _heldObject.SetActive(false);
                                _heldObject = null;
                                _isHandFull = false;
                                return;
                            }
                        }
                        else
                        {
                            foreach (GameObject obj in _interactables)
                            {
                                
                                if (counterHolder.ReleaseObject(obj.GetComponent<HeldFood>().MyId()))
                                {
                                    if (obj.tag == "Dish")
                                    {
                                        bool[] foodOnDish = counterHolder.ReleaseDish();
                                        obj.SetActive(true);
                                        Debug.Log("foodOnDish: " + foodOnDish);
                                        for (int i = 0; i < foodOnDish.Length; i++)
                                        {
                                            _dishObjects[i].SetActive(foodOnDish[i]);
                                        }
                                        _heldObject = obj;
                                        _isHandFull = true;
                                        counterHolder.DestroyDish();
                                        return;
                                    }
                                    obj.SetActive(true);
                                    obj.GetComponent<MeshRenderer>().material = counterHolder.GetMaterial();
                                    counterHolder.DestroyObject();
                                    _heldObject = obj;
                                    _heldObject.SetActive(true);
                                    _isHandFull = true;
                                    return;
                                }
                            }
                        }
                    }

                    Debug.Log("You can't hold more than one object at a time");
                }
            }
            if (hit.transform.TryGetComponent(out IStash stash))
            {
                stash.InteractionPopUp();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (_heldObject != null)
                    {
                        _isHandFull = false;
                        _heldObject.SetActive(false);
                        _heldObject = null;
                    }
                }
            }
        }
    }

    private void GrillInteraction(GameObject obj)
    {
        if (_isHandFull)
        {
            obj.TryGetComponent(out IGrill grill);
            if (grill.GrillInteraction(_heldObject.tag, _heldObject.GetComponent<MeshRenderer>().material))
            {
                _heldObject.SetActive(false);
                _heldObject = null;
                _isHandFull = false;
            }
        }
    }

    private void GrillPickUp(int state, String tag, Material material)
    {
        GameObject toChange = InteractableCicle(tag, material);
        toChange.TryGetComponent(out IHeldFood heldFood);
        heldFood.ICooked(state);
    }

    private GameObject InteractableCicle(String tag, Material material)
    {
        foreach (GameObject obj in _interactables)
        {
            if (obj.tag == tag && tag == "Dish")
            {
                obj.SetActive(true);
                _heldObject = obj;
                _isHandFull = true;
                return obj;
            }
            if (obj.tag == tag)
            {
                obj.GetComponent<MeshRenderer>().material = material;
                obj.SetActive(true);
                _heldObject = obj;
                _isHandFull = true;
                return obj;
            }
        }
        return null;
    }

    private void CuttedFoodPickUp(String tag, Material material)
    {
        GameObject toChange = InteractableCicle(tag, material);
        toChange.TryGetComponent(out IHeldFood heldFood);
        heldFood.ICutted();
    }
}
