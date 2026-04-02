using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class FadeTeleporter : MonoBehaviour
{
    [SerializeField] public Transform destination;      
    public CanvasGroup faderGroup;   
    public InputActionReference activateAction; 
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
        //fade to black, set alpha to 1
        yield return StartCoroutine(Fade(1));

        //teleport the player to the destinations position
        GameObject.FindWithTag("Player").transform.position = destination.position;

        //fade back to clear
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
