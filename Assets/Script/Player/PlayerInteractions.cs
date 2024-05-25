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
    private bool _isHandFull, _isDishHand, _isBeerHand;
    public GameObject HeldObject;
    private PlayerDish _playerDish => GetComponent<PlayerDish>();
    private GameObject _dish => _interactables[8];

    [SerializeField] private GameObject[] _beerFoam;
    [SerializeField] private GameObject _beer;


    private void Awake()
    {
        _cam = Camera.main.transform;
    }

    private void OnEnable()
    {
        EventManager.OnFullBeer += FullBeer;
        EventManager.OnGrillPickUp += GrillPickUp;
        EventManager.OnCutted += CuttedFoodPickUp;
    }

    private void OnDisable()
    {
        EventManager.OnFullBeer -= FullBeer;
        EventManager.OnGrillPickUp -= GrillPickUp;
        EventManager.OnCutted -= CuttedFoodPickUp;
    }

    private void Update()
    {
        _isDishHand = _interactables[0].activeSelf;
        if(HeldObject == null)
        {
            _isHandFull = false;
            _isDishHand = false;
        }
        RaycastHit hit;
        if (Physics.Raycast(_cam.position, _cam.forward, out hit, _interactionDistance))
        {
            if (hit.transform.TryGetComponent(out IInteract interactable))
            {
                interactable.InteractionPopUp();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hit.transform.TryGetComponent(out ICustomer customer))
                    {
                        if (!interactable.Interact(false))
                        {
                            HeldObject.SetActive(false);
                            HeldObject = null;
                            _isHandFull = false;
                            return;
                        }
                        return;
                    }
                    if (hit.transform.TryGetComponent(out ICauldron cauldron))
                    {
                        if(HeldObject != null && HeldObject.tag == "Bowl")
                        {
                            if (interactable.Interact(false))
                            {
                                HeldObject.transform.GetChild(0).gameObject.SetActive(true);
                            }
                            return;
                        }
                        else
                        {
                            if (cauldron.TakeObject(HeldObject.tag))
                            {
                                HeldObject.SetActive(false);
                                HeldObject = null;
                                _isHandFull = false;
                                return;
                            }
                        }
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
                                HeldObject = obj;
                                _isHandFull = true;
                                return;
                            }
                            if (obj.tag == hit.transform.tag && obj.tag == "Mug")
                            {
                                obj.SetActive(true);
                                HeldObject = obj;
                                _isHandFull = true;
                                foreach (GameObject obj2 in _beerFoam)
                                {
                                    obj2.SetActive(false);
                                }
                                return;
                            }
                            if(obj.tag == hit.transform.tag && obj.tag == "Bowl")
                            {
                                obj.transform.GetChild(0).gameObject.SetActive(false);
                            }
                            if (obj.tag == hit.transform.tag)
                            {
                                if (interactable.Interact(false))
                                {
                                    obj.SetActive(true);
                                    obj.TryGetComponent(out IHeldFood heldFood);
                                    heldFood.IBasicMaterial();
                                    heldFood.ICooked(0);
                                    HeldObject = obj;
                                    _isHandFull = true;
                                    return;
                                }
                            }
                        }
                    }
                    if (hit.transform.tag == "BeerPick")
                    {
                        if (HeldObject != null && HeldObject.tag == "Mug")
                        {
                            bool allChildrenInactive = true;
                            for (int i = 0; i < HeldObject.transform.childCount; i++)
                            {
                                if (HeldObject.transform.GetChild(i).gameObject.activeSelf)
                                {
                                    allChildrenInactive = false;
                                    break;
                                }
                            }
                            if (allChildrenInactive = true)
                            {
                                if (interactable.Interact(true))
                                {
                                    HeldObject.SetActive(false);
                                    HeldObject = null;
                                }
                                return;
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
                                if((HeldObject.tag == "VegetablePick" || HeldObject.tag == "TomatoPick") || HeldObject.TryGetComponent(out IHeldFood heldFood) && heldFood.IsCooked())
                                {
                                    if (cutting.CutInteraction(HeldObject))
                                    {
                                        interactable.Interact(true);
                                        HeldObject.SetActive(false);
                                        HeldObject = null;
                                        _isHandFull = false;
                                        return;
                                    }
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
                    if (HeldObject != null)
                    {
                        if (hit.transform.tag == HeldObject.tag)
                        {
                            interactable.Interact(true);
                            HeldObject.SetActive(false);
                            HeldObject = null;
                            _isHandFull = false;
                            return;
                        }
                    }
                    if (hit.transform.TryGetComponent(out ICounterHolder counterHolder))
                    {
                        if (_isHandFull || _isBeerHand)
                        {
                            if (_isDishHand)
                            {
                                if (counterHolder.TakeObject(HeldObject.GetComponent<HeldFood>().MyId(), HeldObject,
                                        true, HeldObject.GetComponent<HeldFood>().IsSliced))
                                {
                                    HeldObject.SetActive(false);
                                    HeldObject = null;
                                    _isHandFull = false;
                                    return;
                                }
                            }
                            if (counterHolder.TakeObject(HeldObject.GetComponent<HeldFood>().MyId(), HeldObject,
                                    false, HeldObject.GetComponent<HeldFood>().IsSliced) && !_isDishHand)
                            {
                                HeldObject.SetActive(false);
                                HeldObject = null;
                                _isHandFull = false;
                                _isBeerHand = false;
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
                                        HeldObject = obj;
                                        _isHandFull = true;
                                        counterHolder.DestroyDish();
                                        return;
                                    }
                                    obj.SetActive(true);
                                    if (obj.GetComponent<MeshRenderer>().material != null)
                                    {
                                        obj.GetComponent<MeshRenderer>().material = counterHolder.GetMaterial();
                                    }
                                    counterHolder.DestroyObject();
                                    HeldObject = obj;
                                    HeldObject.SetActive(true);
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
                    
                    if (HeldObject != null)
                    {
                        _isHandFull = false;
                        HeldObject.SetActive(false);
                        HeldObject = null;
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
            if (grill.GrillInteraction(HeldObject.tag, HeldObject.GetComponent<MeshRenderer>().material))
            {
                HeldObject.SetActive(false);
                HeldObject = null;
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
                HeldObject = obj;
                _isHandFull = true;
                return obj;
            }
            if (obj.tag == tag)
            {
                if(material != null)
                    obj.GetComponent<MeshRenderer>().material = material;
                obj.SetActive(true);
                HeldObject = obj;
                _isHandFull = true;
                return obj;
            }
        }
        return null;
    }

    private void CuttedFoodPickUp(String tag)
    {
        GameObject toChange = InteractableCicle(tag, null);
        toChange.TryGetComponent(out IHeldFood heldFood);
        heldFood.ICutted();
    }

    private void FullBeer(int beerType)
    {
        _beerFoam[beerType].SetActive(true);
        _beer.SetActive(true);
        HeldObject = _beer;
        _isBeerHand = true;
    }
    
    public bool[] GetDish()
    {
        int childCount = _dish.transform.childCount;
        bool[] activeChildren = new bool[childCount];
        Debug.Log(childCount + activeChildren.Length);
        

        for (int i = 0; i < childCount; i++)
        {
            activeChildren[i] = _dish.transform.GetChild(i).gameObject.activeSelf;
        }

        return activeChildren;
    }
}
