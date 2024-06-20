using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ObjectPool<T> where T : Object
{
    static Queue<T> objectPool = new Queue<T>();

    #region Instantiate
    public static T Instantiate(T targetObject)
    {
        if (objectPool.Count > 0)
        {
            T dequeuedObject = objectPool.Dequeue();
            GameObject gameObject = dequeuedObject.GameObject();
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.parent = null;
            gameObject.SetActive(true);
            return dequeuedObject;
        }
        else
        {
            return Object.Instantiate(targetObject);
        }
    }
    public static T Instantiate(T targetObject, Transform parent)
    {
        if (objectPool.Count > 0)
        {
            T dequeuedObject = objectPool.Dequeue();
            GameObject gameObject = dequeuedObject.GameObject();
            gameObject.transform.position = parent.position;
            gameObject.transform.rotation = parent.rotation;
            gameObject.transform.parent = parent;
            gameObject.SetActive(true);
            return dequeuedObject;
        }
        else
        {
            return Object.Instantiate(targetObject, parent.position, parent.rotation, parent);
        }
    }
    public static T Instantiate(T targetObject, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (objectPool.Count > 0)
        {
            T dequeuedObject = objectPool.Dequeue();
            GameObject gameObject = dequeuedObject.GameObject();
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            gameObject.transform.parent = parent;
            gameObject.SetActive(true);
            return dequeuedObject;
        }
        else
        {
            return Object.Instantiate(targetObject, position, rotation, parent);
        }
    }
    #endregion

    #region Destroy
    public static void Destroy(T targetObject)
    {
        if(objectPool.Count > 10)
        {
            targetObject.GameObject().SetActive(false);
            objectPool.Enqueue(targetObject);
        }
        else
        {
            Object.Destroy(targetObject);
            return;
        }
    }

    /*public static void Destroy(GameObject targetObject, float sec)
    {
        
    }*/
    #endregion
}
