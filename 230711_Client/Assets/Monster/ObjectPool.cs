using System;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

public class ObjectPool
{
    public readonly Transform parent;
    public readonly GameObject prefab;
    public readonly Type component;
    public readonly Stack<object> fObj; // active = false
    public readonly List<object> tObj; // active = true

    public ObjectPool(GameObject prefab, Transform parent, Type component)
    {
        this.parent = parent;
        this.prefab = prefab;
        this.component = component;
        fObj = new Stack<object>();
        tObj = new List<object>();
    }

    public object Get()
    {
        GameObject gObj;
        object ret;
        if (fObj.Count == 0)
        {
            gObj = UObject.Instantiate(prefab, parent);
            ret = component != null ? gObj.GetComponent(component) : (object)gObj;
        }
        else
        {
            ret = fObj.Pop();
            gObj = component != null ? ((Component)ret).gameObject : (GameObject)ret;
        }
        gObj.SetActive(true);
        tObj.Add(ret);
        return ret;
    }

    public void Return(object obj)
    {
        int index = tObj.IndexOf(obj);
        if (index != -1)
        {
            GameObject gObj = component != null ? ((Component)obj).gameObject : (GameObject)obj;
            gObj.SetActive(false);
            tObj.RemoveAt(index);
            fObj.Push(obj);
        }
    }
    public void ReturnAll()
    {
        bool flag = component != null;
        foreach (object obj in tObj)
        {
            (flag ? ((Component)obj).gameObject : (GameObject)obj).SetActive(false);
            fObj.Push(obj);
        }
        tObj.Clear();
    }
}