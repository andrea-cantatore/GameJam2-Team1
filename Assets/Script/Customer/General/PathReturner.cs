using UnityEngine;

public class PathReturner : MonoBehaviour, CustomerTargetPos
{
    [SerializeField] private Transform[] _path;
    public Transform[] GetOtherPos()
    {
        return _path;
    }
}
