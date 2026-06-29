using System.Collections;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [Header("Gate Movement")]
    [SerializeField] private float dropDistance = 3f;
    [SerializeField] private float moveSpeed = 2f;

    private bool isOpen = false;
    private bool isMoving = false;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        // Remember where the gate starts.
        closedPosition = transform.position;

        // Calculate where the gate should end up.
        openPosition = closedPosition + Vector3.down * dropDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Don't do anything if the gate is already opening or open.
        if (isOpen || isMoving)
            return;

        // Make sure the player entered the trigger.
        CharacterController controller = other.GetComponent<CharacterController>();

        if (controller == null)
            return;

        // Make sure it's the currently controlled player.
        if (controller != Player.Instance.CurrentController)
            return;

        // Try to consume a key.
        if (!Player.Instance.UseKey())
            return;

        // Open the gate.
        StartCoroutine(OpenGate());
    }

    private IEnumerator OpenGate()
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, openPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                openPosition,
                moveSpeed * Time.deltaTime);

            yield return null;
        }

        transform.position = openPosition;

        isOpen = true;
        isMoving = false;
    }
}