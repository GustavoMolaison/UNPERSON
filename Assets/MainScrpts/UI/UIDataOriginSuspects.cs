using UnityEngine;

public abstract class UIDataOrigin<T> : MonoBehaviour where T : ScriptableObject
{
    public abstract void ApplyData(T data);
      
}