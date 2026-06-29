using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Singleton<Player>
{
    [Header("Inventory")]
    public int keyCount = 0;


    private CharacterController previousCharController;
    public CharacterController CurrentController;
    public PlayerControls playerControls;

    private Rigidbody rb;
    public List<CharacterController> AllCharacterControllers = new List<CharacterController>();
    public bool playingAsJuniper = true;
    //I want each player instance to have a perspective that way i can record with june and do somehting with nat
    public enum CameraType
    {
        ThirdPerson,
        FirstPerson
    }



    [HideInInspector]
    public InputAction MovementInputAction;

    [HideInInspector]
    public InputAction SpecialSkillInput;

    [HideInInspector]
    public InputAction SwitchCharacterInput;


    private void Start()
    {
        AllCharacterControllers = new List<CharacterController>(FindObjectsOfType<CharacterController>());
        if (CurrentController == null)
        {
            CurrentController = GetComponentInChildren<CharacterController>();
        }
        if (CurrentController != null)
        {
            updateCharacterController();
        }
        previousCharController = CurrentController;
    }
    public PlayerInstance getCurrentPlayerInstance()
    {
        return CurrentController.gameObject.GetComponent<PlayerInstance>();
    }

    public void LockControl(bool value)
    {
        CurrentController.GetComponent<PlayerMovement>().LockMovement(value);
    }
    private void updateCharacterController()
    {

        rb = CurrentController.GetComponentInChildren<Rigidbody>();
        CurrentController.GetComponent<PlayerMovement>().enabled = true;
        foreach (CharacterController charController in AllCharacterControllers)
        {
            if (charController != CurrentController)
            {
                charController.gameObject.GetComponent<PlayerMovement>().enabled = false;
            }
        }
        if (CurrentController.gameObject.name == "Juniper")
        {
            playingAsJuniper = true;
        }
        else
        {
            playingAsJuniper = false;
        }
    }


    private void OnEnable()
    {
        playerControls = new PlayerControls();
        MovementInputAction = playerControls.Player.Move;
        MovementInputAction.Enable();

        SpecialSkillInput = playerControls.Player.TurnOnCamera;
        SpecialSkillInput.Enable();

        SwitchCharacterInput = playerControls.Player.SwitchPlayer;
        SwitchCharacterInput.Enable();

        SwitchCharacterInput.performed += switchCharacter;
    }
    private void OnDisable()
    {
        MovementInputAction.Disable();
        SpecialSkillInput.Disable();
        SwitchCharacterInput.Disable();

    }

    public void switchCharacter(InputAction.CallbackContext context)
    {
        //if (Perspective == CameraType.FirstPerson) 
        //{
        //    GetComponentInChildren<FilmStation>().SwitchCameraType();
        //}
        if (AllCharacterControllers.IndexOf(CurrentController) + 1 >= AllCharacterControllers.Count)
        {
            CurrentController = AllCharacterControllers[0];
        }
        else
        {
            CurrentController = AllCharacterControllers[AllCharacterControllers.IndexOf(CurrentController) + 1];
        }


        updateCharacterController();
    }

    public void TeleportToPosition(Vector3 pos)
    {
        if (CurrentController == null)
        {
            CurrentController = GetComponentInChildren<CharacterController>();
        }
        CurrentController.enabled = false;
        transform.position = pos;
        CurrentController.enabled = true;
    }
    public void RotatePlayer(float angle)
    {
        if (CurrentController == null)
        {
            CurrentController = GetComponentInChildren<CharacterController>();
        }
        CurrentController.enabled = false;
        transform.DORotate(new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z), 0.0f);
        CurrentController.enabled = true;
    }


    public void AddKey(int amount = 1)
    {
        keyCount += amount;
        Debug.Log("Keys: " + keyCount);
    }

    public bool UseKey()
    {
        if (keyCount <= 0)
        {
            Debug.Log("No keys available.");
            return false;
        }

        keyCount--;
        Debug.Log("Used key. Keys left: " + keyCount);
        return true;
    }
}