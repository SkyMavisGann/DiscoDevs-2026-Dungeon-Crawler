using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBox : MonoBehaviour
{
    public bool PlayerInside;

    public void SetPlayerInside(bool value)
    {
        PlayerInside = value;
    }
}
