using UnityEngine;

public class PathReturner : MonoBehaviour, CustomerTargetPos
{
    [SerializeField] private Transform[] _path;
    [SerializeField] private GameObject[] _coins;
    public Transform[] GetOtherPos()
    {
        return _path;
    }

    public GameObject[] Coins()
    {
        return _coins;
    }
}
