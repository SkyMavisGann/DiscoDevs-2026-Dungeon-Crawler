using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{

  
    private void OnTriggerEnter(Collider other)
    {
        //Make sure it's just the charactercontroller that can pick up the key.
        CharacterController controller = other.GetComponent<CharacterController>();
        //if it's not the controlled character ignore
        if (controller == null) return;

        //Add one key to the number of keys total
        Player.Instance.AddKey(1);

        //Remove the key from the scene.
        Destroy(gameObject);
    }

}
