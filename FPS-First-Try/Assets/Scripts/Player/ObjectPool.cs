using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    [SerializeField]private List<GameObject> _pooledObjects;
    [SerializeField]private  GameObject _objectToPool;
    [SerializeField]private int amountToPool;

    private void Awake()
    {
        SharedInstance = this;
    }
    
    private void Start()
    {
        _pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(_objectToPool);
            tmp.SetActive(false);
            _pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy) return _pooledObjects[i];
        }
        return null;
    }
}
