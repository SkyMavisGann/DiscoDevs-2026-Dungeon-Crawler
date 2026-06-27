using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateGraphic : MonoBehaviour
{
    public Transform Parent;
    public GameObject Graphic;

    [HideInInspector]
    public GameObject createdObject;
    public bool instantiateOnLoad = false;

    private void Start()
    {
        if (instantiateOnLoad)
        {
            SpawnGraphic();
        }
    }

    public void SpawnGraphic()
    {
        if (Parent == null)
        {
            Parent = GameObject.Find("Canvases").transform;
        }
        if (createdObject == null)
            createdObject = Instantiate(Graphic, Parent);
    }

    public void DespawnGraphic()
    {
        Destroy(createdObject);
    }
}
