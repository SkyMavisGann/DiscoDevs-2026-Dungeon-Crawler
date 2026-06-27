using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParent : MonoBehaviour
{
    public GameObject RootObject;
    public Transform NewParent;
    private Transform CurrentParent;
    // Start is called before the first frame update
    void Start()
    {
        if (RootObject == null)
        {
            RootObject = gameObject;
        }
        CurrentParent = RootObject.transform.parent;
    }
    public void ChangeParentTo(Transform newParent)
    {
        RootObject.transform.SetParent(newParent);
    }
    public void ChangeParentTo()
    {
        RootObject.transform.SetParent(NewParent);
    }
    public void ReturnToOldParent()
    {
        RootObject.transform.SetParent(CurrentParent);
    }
    public void ChangeParentToCanvas()
    {
        RootObject.transform.SetParent(GameObject.Find("Canvas").transform);
    }

}
