using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public bool fadeOut = false;
    public bool fadeIn = false;

    public float TimeToFadeOut;
    public float TimeToFadeIn;
    private GameObject black;
    public bool sceneChange = false;
    public SceneChange sceneChangeObject;
    public GameObject ObjectToDestroy;

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = transform.parent.GetComponent<CanvasGroup>();
        }
    }
    private void Start()
    {
        fadeOut = true;
        
    }
    public void FadeIn()
    {
        if (canvasGroup.alpha == 0)
        {
            canvasGroup.alpha = 1;
        }
        
        fadeIn = true;
        fadeOut = false;
    }
    public void FadeOut(int fadeBackInTime)
    {
        if (canvasGroup.alpha >= 1)
        {
            canvasGroup.alpha = 0;
        }
        
        fadeIn = false;
        fadeOut = true;
        Invoke("FadeIn", fadeBackInTime);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (fadeOut)
        {
            if (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += TimeToFadeOut;
                if (canvasGroup.alpha >= 1)
                {
                    fadeOut = false;

                    
                    
                    //if you aren't changing the scene then don't autostart unfade
                    if (sceneChange)
                    {
                        //i create a black image bc the canvas is invisible for one second at load

                        black = new GameObject("Black", typeof(Image));
                        black.transform.SetParent(transform.parent, true);
                        black.GetComponent<Image>().color = Color.black;
                        black.GetComponent<RectTransform>().localScale = new Vector3(500, 500, 1);
                        
                        sceneChangeObject.BeginSceneLoading();
                        fadeIn = true;
                    }
                    
                }
            }
        }

        if (fadeIn)
        {
            if ((sceneChange && !GameManager.Instance.Loading.activeSelf) || !sceneChange)
            {
                if (black != null) { Destroy(black); }
                
                if (canvasGroup.alpha >= 0)
                {
                    canvasGroup.alpha -= TimeToFadeIn;
                    if (canvasGroup.alpha == 0)
                    {
                        fadeIn = false;

                        Destroy(ObjectToDestroy);
                    }
                }
            }
        }
    }
}
