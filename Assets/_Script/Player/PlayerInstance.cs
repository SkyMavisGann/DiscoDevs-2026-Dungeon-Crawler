using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    public Player.CameraType Perspective = Player.CameraType.ThirdPerson;
    private Rigidbody rb;
    public Transform FilmCamera;
   
    public void UpdateCameraType()
    {
        switch (Perspective)
        {
            case Player.CameraType.ThirdPerson:
                rb.gameObject.SetActive(true);
                FilmCamera?.gameObject.SetActive(false);
                Camera.main.GetComponent<AudioListener>().enabled = true;
                break;
            case Player.CameraType.FirstPerson:
                Camera.main.GetComponent<AudioListener>().enabled = false;
                rb?.gameObject.SetActive(false);
                FilmCamera?.gameObject.SetActive(true);
                break;
        }
    }
    public void updateMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
