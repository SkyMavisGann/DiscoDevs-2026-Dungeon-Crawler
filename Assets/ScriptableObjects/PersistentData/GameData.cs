using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New GameData", menuName = "GameData")]
public class GameData : ScriptableObject
{
    public string CurrentDoorCode;


    private void OnEnable()
    {
        Debug.Log("Added Persistent Data Scriptable");
        DataHandlerPersistent.PersistentData = this;
    }
}
