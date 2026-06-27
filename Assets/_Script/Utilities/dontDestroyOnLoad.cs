using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroyOnLoad : MonoBehaviour
{
    void Start()
    {
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }
        DontDestroyOnLoad(gameObject);
    }
}
