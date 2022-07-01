using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public void Init()
    {
    }

    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefeb = Load<GameObject>($"Prefabs/{path}");
        if (prefeb == null)
        {
            return null;
        }
        else
        {
            return Instantiate(prefeb, parent);
        }       
    }

    public GameObject Instantiate(GameObject prefeb, Transform parent = null)
    {
        GameObject go = Object.Instantiate(prefeb, parent);
        go.name = prefeb.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}
