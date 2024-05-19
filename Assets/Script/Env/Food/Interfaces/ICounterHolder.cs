using UnityEngine;

public interface ICounterHolder
{
    public bool TakeObject(int id, GameObject obj, bool isDish);
    public bool ReleaseObject(int id);
    public Material GetMaterial();
    public void DestroyObject();
    public bool HaveDishOn();
}

