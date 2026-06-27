using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MoveObjectToLocation : MonoBehaviour
{
    public List<Transform> Locations = new List<Transform>();
    public Transform rootObject;
    public List<bool> GotoLocation = new List<bool>();
    public float moveSpeed = 0.1f;
    public bool toggle = false;
    public bool scale = false;
    public float scaleSpeed = 0.1f;
    public int atLocation = 0;
    private void Start()
    {
        foreach (Transform go in Locations)
        {
            GotoLocation.Add(false);
        }
    }
    public void GoToLocationIndex(int i)
    {
        if (toggle)
        {
            if (GotoLocation[i])
            {
                GotoLocation[i] = false;
                GotoLocation[Convert.ToInt32(!(i != 0))] = true;

            }
            else
            {
                GotoLocation[Convert.ToInt32(!(i != 0))] = false;
                GotoLocation[i] = true;
            }
        }
        else
        {
            GotoLocation[i] = true;
            atLocation = i;
            for (int j = 0; j < Locations.Count; ++j)
            {
                if (j != i)
                {
                    GotoLocation[j] = false;
                }
            }
        }
    }
    public void GoToLocationIndexWithoutToggle(int i)
    {
        GotoLocation[i] = true;
        atLocation = i;
        for (int j = 0; j < Locations.Count; ++j)
        {
            if (j != i)
            {
                GotoLocation[j] = false;
            }
        }
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < Locations.Count; ++i)
        {
            if (GotoLocation[i])
            {
                //set all others false
                for (int j = 0; j < Locations.Count; ++j)
                {
                    if (j != i)
                    {
                        GotoLocation[j] = false;
                    }
                }

                rootObject.position = Vector3.Lerp(rootObject.position, Locations[i].position, Time.deltaTime * moveSpeed);
                if (math.abs(Locations[i].position.x - rootObject.position.x) < 0.1 && ((scale && math.abs(Locations[i].localScale.x - rootObject.localScale.x) < 0.01) || !scale))
                {
                    if (!toggle)
                    {
                        GotoLocation[i] = false;
                    }
                }
                if (scale)
                {
                    rootObject.localScale = Vector3.Lerp(rootObject.localScale, Locations[i].localScale, Time.deltaTime * scaleSpeed);
                }
            }
        }
    }
}
