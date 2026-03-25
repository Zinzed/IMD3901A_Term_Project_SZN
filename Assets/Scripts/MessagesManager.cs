using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MessagesManager : MonoBehaviour
{
    [SerializeField] private float m_DelayBetweenCharacters = 0.02f;
    [SerializeField] private List<GameObject> messageBubbles;
    [SerializeField] private bool autoAdvance = true; // Toggle for auto-advance feature

    public AudioSource notificationSFX;

    [SerializeField] private GameObject startBttn;
    [SerializeField] private Button nextBttn;
    [SerializeField] private Button previousBttn;

    private int currentBubbleIndex = 0;
    private Coroutine typingCoroutine;
    private TMP_Text currentTextComponent;

    void Start()
    {
        UpdateMessages();
        startBttn.SetActive(false);
    }

    IEnumerator TypeText(TMP_Text textComponent)

    {
        string fullText = textComponent.text;
        textComponent.maxVisibleCharacters = 0;

        while (textComponent.maxVisibleCharacters < fullText.Length)
        {
            textComponent.maxVisibleCharacters++;
            yield return new WaitForSeconds(m_DelayBetweenCharacters);

            // disable next previous buttons while still typing 
            nextBttn.interactable = false;
            previousBttn.interactable = false;
        }

        nextBttn.interactable = true;
        previousBttn.interactable = true;

        typingCoroutine = null;

        // go to next bubble if there is one
        if (autoAdvance && currentBubbleIndex < messageBubbles.Count - 1)
        {
      
            yield return new WaitForSeconds(0.8f); // small pause between messages
            NextBubble();
        }

        // if done typing for last message show start button

        if (typingCoroutine == null && currentBubbleIndex == messageBubbles.Count - 1)
        {
            startBttn.SetActive(true);
            nextBttn.interactable = false;
        }
        
    }

    public void NextBubble()
    {
        // cant go to next if we're still typing
        if (typingCoroutine != null)
            return;

        if (currentBubbleIndex < messageBubbles.Count - 1)
        {
            currentBubbleIndex++;
            UpdateMessages();
        }
    }

    public void PreviousBubble()
    {
        // cant go to previous if we're still typing
        if (typingCoroutine != null)
            return;

        if (currentBubbleIndex > 0)
        {
            currentBubbleIndex--;
            UpdateMessages();
        }

        

        
    }

    public void SkipToEnd()
    {
        // immediately show full text of current message
        if (typingCoroutine != null && currentTextComponent != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;

            // show all characters of the current text
            currentTextComponent.maxVisibleCharacters = currentTextComponent.text.Length;
        }
    }

    private void UpdateMessages()
    {

        //startBttn.SetActive(false);

        // stop any ongoing typing coroutine
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        // show all bubbles up to current index, hide future ones
        for (int i = 0; i < messageBubbles.Count; i++)
        {
            if (messageBubbles[i] != null)
            {
                // show bubbles up to current index, hide future ones
                messageBubbles[i].SetActive(i <= currentBubbleIndex);
                notificationSFX.Play();
            }
        }

        // only type the current message (the newest one)
        if (currentBubbleIndex >= 0 && currentBubbleIndex < messageBubbles.Count)
        {
            GameObject currentBubble = messageBubbles[currentBubbleIndex];
            currentTextComponent = currentBubble.GetComponentInChildren<TMP_Text>();

            if (currentTextComponent != null)
            {
                typingCoroutine = StartCoroutine(TypeText(currentTextComponent));
            }
        }
    }
    // reset to first message
    public void ResetToFirst()
    {
        currentBubbleIndex = 0;
        UpdateMessages();
    }

    
}