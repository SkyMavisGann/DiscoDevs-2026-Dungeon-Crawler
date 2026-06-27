using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public SceneChange sceneChange;
    public void LoadWithTransition()
    {
        if (GameObject.Find("Fader(Clone)") == null)
        {
            GameObject fader = Instantiate(Resources.Load("Prefabs/Transition/Fader") as GameObject, null);
            fader.GetComponentInChildren<FadeEffect>().sceneChange = true;
            fader.GetComponentInChildren<FadeEffect>().sceneChangeObject = sceneChange;

        }
    }
}
