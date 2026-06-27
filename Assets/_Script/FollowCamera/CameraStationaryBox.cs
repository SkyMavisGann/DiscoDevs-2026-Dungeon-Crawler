using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStationaryBox : MonoBehaviour
{
    public bool PlayerInside;
    public Transform CameraPositionHere;
    public float size;
    public int priority = 0;

    public void SetPlayerInside(bool value)
    {
        PlayerInside = value;
    }
}
