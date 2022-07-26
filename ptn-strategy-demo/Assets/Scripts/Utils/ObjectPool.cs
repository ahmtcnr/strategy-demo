using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{

    private Stack<GameObject> _objectPool = new Stack<GameObject>();
    private GameObject prefab;
    
    public void CreatePool(GameObject poolObject, int count)
    {
        prefab = poolObject;
        for (int i = 0; i < count; i++)
        {
           Spawn();
        }
        
    }


    public GameObject Pull()
    {
        if (_objectPool.Count == 0)
        {
            Spawn();
        }
        return _objectPool.Pop();
    }

    public void Push(GameObject objectToPush)
    {
        _objectPool.Push(objectToPush);
    }


    private void Spawn()
    {
        var spawnedObj=  GameObject.Instantiate(prefab);
        _objectPool.Push(spawnedObj);
    }

}
