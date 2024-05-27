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

    //private PlayerDish _playerDish => GetComponent<PlayerDish>();
    //private GameObject _dish => _interactables[0];

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
                    if(hit.transform.tag == "Dish" && !_isHandFull && !_isDishHand)
                    {
                        foreach (GameObject obj in _dishObjects)
                        {
                            obj.SetActive(false);
                        }
                        _interactables[0].SetActive(true);
                        HeldObject = _interactables[0];
                        _isHandFull = true;
                        _isDishHand = true;
                        return;
                    }
                    if(hit.transform.tag == "Dish" && _isDishHand)
                    {
                        foreach (GameObject obj in _dishObjects)
                        {
                            obj.SetActive(false);
                        }
                        HeldObject.SetActive(false);
                        HeldObject = null;
                        _isHandFull = false;
                        _isDishHand = false;
                        return;
                    }
                    if (hit.transform.tag == "BeerPick")
                    {
                        if (HeldObject != null && HeldObject.tag == "Mug")
                        {
                            bool allChildrenInactive = true;
                            for (int i = 1; i < HeldObject.transform.childCount; i++)
                            {
                                GameObject child = HeldObject.transform.GetChild(i).gameObject;
                                Debug.Log("Checking child at index " + i + ", active: " + child.activeSelf);
                    
                                if (child.activeSelf)
                                {
                                    allChildrenInactive = false;
                                    Debug.Log("Found an active child at index " + i);
                                    break;
                                }
                            }
                            if (allChildrenInactive)
                            {
                                Debug.Log("All children (except the first one) are inactive");
                                if (interactable.Interact(true))
                                {
                                    HeldObject.SetActive(false);
                                    _isHandFull = true;
                                }
                                return;
                            }
                            else
                            {
                                Debug.Log("Not all children are inactive");
                            }
                        }
                    }
                    if(hit.transform.TryGetComponent(out ICoin coin))
                    {
                        EventManager.MoneyChanger?.Invoke(coin.ReturnCoins());
                        hit.transform.gameObject.SetActive(false);
                        return;
                    }
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
                                for (int i = 0; i < HeldObject.transform.childCount; i++)
                                {
                                    HeldObject.transform.GetChild(i).gameObject.SetActive(true);
                                }
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
                                Debug.Log("Dish");
                                obj.SetActive(true);
                                foreach (GameObject child in _dishObjects)
                                {
                                    child.gameObject.SetActive(false);
                                }
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
                                for(int i = 0; i < obj.transform.childCount; i++)
                                {
                                    obj.transform.GetChild(i).gameObject.SetActive(false);
                                }
                            }
                            if (obj.tag == hit.transform.tag && hit.transform.tag != "BeerPick")
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
                                if((HeldObject.tag == "VegetablePick" || HeldObject.tag == "TomatoPick") || (HeldObject.TryGetComponent(out IHeldFood heldFood) && heldFood.IsCooked()))
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
                            if (HeldObject.tag == "Bowl")
                            {
                                if (counterHolder.TakeObject(HeldObject.GetComponent<HeldFood>().MyId(), HeldObject,
                                        true, HeldObject.GetComponent<HeldFood>().IsSliced, HeldObject.GetComponent<HeldFood>()._isCooked))
                                {
                                    HeldObject.SetActive(false);
                                    HeldObject = null;
                                    _isHandFull = false;
                                    return;
                                }
                            }
                            if (_isDishHand)
                            {
                                if (counterHolder.TakeObject(HeldObject.GetComponent<HeldFood>().MyId(), HeldObject,
                                        true, HeldObject.GetComponent<HeldFood>().IsSliced, HeldObject.GetComponent<HeldFood>()._isCooked))
                                {
                                    HeldObject.SetActive(false);
                                    HeldObject = null;
                                    _isHandFull = false;
                                    return;
                                }
                            }
                            if (counterHolder.TakeObject(HeldObject.GetComponent<HeldFood>().MyId(), HeldObject,
                                    false, HeldObject.GetComponent<HeldFood>().IsSliced, HeldObject.GetComponent<HeldFood>()._isCooked) && !_isDishHand)
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
                                        bool[] foodOnDish = counterHolder.ReleaseDish(true);
                                        obj.SetActive(true);
                                        for (int i = 0; i < foodOnDish.Length; i++)
                                        {
                                            _dishObjects[i].SetActive(foodOnDish[i]);
                                        }
                                        HeldObject = obj;
                                        _isHandFull = true;
                                        counterHolder.DestroyDish();
                                        return;
                                    }
                                    if(obj.tag == "Bowl")
                                    {
                                        bool[] foodOnBowl = counterHolder.ReleaseDish(false);
                                        obj.SetActive(true);
                                        for (int i = 0; i < foodOnBowl.Length; i++)
                                        {
                                            obj.transform.GetChild(i).gameObject.SetActive(foodOnBowl[i]);
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
            return;
        }
        EventManager.ResetPopUp?.Invoke();
    }

    private void GrillInteraction(GameObject obj)
    {
        if (_isHandFull && HeldObject.TryGetComponent(out IHeldFood heldFood) && heldFood._isCooked != 2)
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
        Debug.Log(state + " state");
        
        GameObject toChange = InteractableCicle(tag, material);
        toChange.TryGetComponent(out IHeldFood heldFood);
        heldFood._isCooked = state;
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
    
    public int ActiveFood()
    {
        if (HeldObject.TryGetComponent(out IDish dish))
        {
            return dish.IDReturner();
        }
        return 0;
    }
    
    public int ActiveMug()
    {
        if (HeldObject.TryGetComponent(out IMug mug))
        {
            return mug.IDReturner();
        }
        return 0;
    }
    
    public int ActiveBowl()
    {
        if (HeldObject.TryGetComponent(out IBowl bowl))
        {
            return bowl.IDReturner();
        }
        return 0;
    }
}
