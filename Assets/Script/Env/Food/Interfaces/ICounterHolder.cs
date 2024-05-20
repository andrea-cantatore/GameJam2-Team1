using UnityEngine;

public interface ICounterHolder
{
    public bool TakeObject(int id, GameObject obj, bool isDish, bool isSliced);
    public bool ReleaseObject(int id);
    public Material GetMaterial();
    public void DestroyObject();
    public bool[] ReleaseDish();
    public void DestroyDish();
}

