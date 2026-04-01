using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class FadeTeleporter : MonoBehaviour
{
    [SerializeField] public Transform destination;      // Drag your Teleportation Anchor here
    public CanvasGroup faderGroup;    // Drag your Image's Canvas Group here
    public InputActionReference activateAction; // XRI LeftHand/Interaction/Activate
    public float fadeDuration = 0.5f;

    private bool isPlayerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerInside = true;
        Debug.Log("Player in");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerInside = false;
    }

    private void Update()
    {
        if (isPlayerInside && activateAction.action.WasPressedThisFrame())
        {
            StartCoroutine(DoFadeTeleport());
        }
    }

    IEnumerator DoFadeTeleport()
    {
        // 1. Fade to Black
        yield return StartCoroutine(Fade(1));

        // 2. Teleport
        GameObject.FindWithTag("Player").transform.position = destination.position;

        // 3. Fade back to Clear
        yield return StartCoroutine(Fade(0));
    }

    IEnumerator Fade(float targetAlpha)
    {
        float speed = 1f / fadeDuration;
        while (!Mathf.Approximately(faderGroup.alpha, targetAlpha))
        {
            faderGroup.alpha = Mathf.MoveTowards(faderGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
    }
}
