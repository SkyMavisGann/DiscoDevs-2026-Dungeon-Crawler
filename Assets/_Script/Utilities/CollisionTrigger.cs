using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionTrigger : MonoBehaviour
{
    public Button.ButtonClickedEvent EnterCollider;
    public Button.ButtonClickedEvent StayCollider;
    public Button.ButtonClickedEvent ExitCollider;
    public Button.ButtonClickedEvent BecameUnfitCollider;
    
    public string requiredTag = "Player";
    public bool RequiresIsActiveCharacter = false;
    public bool RequiresActiveCharacterJuniper = false;
    public bool RequiresActiveCharacterNatalie = false;
    private Collider Objectin = null;


    private bool activeCharacterOrDoesntNeedIt;
    private bool JuniperOrDoesntNeedIt;
    private bool NatalieOrDoesntNeedit;

    private void Start()
    {
        activeCharacterOrDoesntNeedIt = !RequiresIsActiveCharacter;
        JuniperOrDoesntNeedIt = !RequiresActiveCharacterJuniper;
        NatalieOrDoesntNeedit = !RequiresActiveCharacterNatalie;

    }
    private void Update()
    {
        
        if (Player.Instance != null)
        {
            NatalieOrDoesntNeedit = !Player.Instance.playingAsJuniper || (!RequiresActiveCharacterNatalie);

            JuniperOrDoesntNeedIt = Player.Instance.playingAsJuniper || (!RequiresActiveCharacterJuniper);
        } else
        {
            Debug.Log("player instance is null");
        }
        


        if (Objectin != null)
        {
            checkForActive(Objectin);
            if (!activeCharacterOrDoesntNeedIt || !JuniperOrDoesntNeedIt || !NatalieOrDoesntNeedit && Objectin.tag == requiredTag)
            {
                BecameUnfitCollider.Invoke();
                Objectin = null;
            }  
        }
    }
    private void checkForActive(Collider obj)
    {
            activeCharacterOrDoesntNeedIt = (Player.Instance.CurrentController == obj.transform.parent.GetComponent<CharacterController>() || (!RequiresIsActiveCharacter));
        
    }
    private void OnTriggerEnter(Collider other)
    {
        checkForActive(other);
        if (activeCharacterOrDoesntNeedIt && JuniperOrDoesntNeedIt && NatalieOrDoesntNeedit && other.tag == requiredTag)
        {
            EnterCollider.Invoke();
            Objectin = other;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        checkForActive(other);
        if (activeCharacterOrDoesntNeedIt && JuniperOrDoesntNeedIt && NatalieOrDoesntNeedit && other.tag == requiredTag)
        {
            StayCollider.Invoke();
            Objectin = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        checkForActive(other);
        if (activeCharacterOrDoesntNeedIt && JuniperOrDoesntNeedIt && NatalieOrDoesntNeedit && other.tag == requiredTag)
        {
            ExitCollider.Invoke();
            Objectin = null;
        }
    }
}
