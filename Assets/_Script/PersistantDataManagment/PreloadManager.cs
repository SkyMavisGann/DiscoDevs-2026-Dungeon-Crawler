using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloadManager : MonoBehaviour
{
    public GameData gameData;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Added Persistent Data Scriptable");
        DataHandlerPersistent.PersistentData = gameData;
    }
}
