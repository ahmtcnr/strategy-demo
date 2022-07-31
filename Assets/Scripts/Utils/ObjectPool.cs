using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Stack<GameObject> _objectPool = new Stack<GameObject>();
      
    public GameObject Pull()
    {
        return _objectPool.Pop();
    }

    public void Push(GameObject objectToPush)
    {
        _objectPool.Push(objectToPush);
    }

}
