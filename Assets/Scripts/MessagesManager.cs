using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessagesManager : MonoBehaviour
{
    [SerializeField] private float m_DelayBetweenCharacters = 0.05f;
    [SerializeField] private List<GameObject> messageBubbles;
    [SerializeField] private bool autoAdvance = true; // Toggle for auto-advance feature

    private int currentBubbleIndex = 0;
    private Coroutine typingCoroutine;
    private TMP_Text currentTextComponent;

    void Start()
    {
        UpdateMessages();
    }

    IEnumerator TypeText(TMP_Text textComponent)

    {
        string fullText = textComponent.text;
        textComponent.maxVisibleCharacters = 0;

        while (textComponent.maxVisibleCharacters < fullText.Length)
        {
            textComponent.maxVisibleCharacters++;
            yield return new WaitForSeconds(m_DelayBetweenCharacters);
        }

        typingCoroutine = null;

        // Automatically go to next bubble if there is one
        if (autoAdvance && currentBubbleIndex < messageBubbles.Count - 1)
        {
            yield return new WaitForSeconds(0.5f); // Optional small pause between messages
            NextBubble();
        }
    }

    public void NextBubble()
    {
        // Can't go to next if we're still typing
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
        // Can't go to previous if we're still typing
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
        // Immediately show full text of current message
        if (typingCoroutine != null && currentTextComponent != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;

            // Show all characters of the current text
            currentTextComponent.maxVisibleCharacters = currentTextComponent.text.Length;
        }
    }

    private void UpdateMessages()
    {
        // Stop any ongoing typing coroutine
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        // Show ALL bubbles up to current index, hide future ones
        for (int i = 0; i < messageBubbles.Count; i++)
        {
            if (messageBubbles[i] != null)
            {
                // Show bubbles up to current index, hide future ones
                messageBubbles[i].SetActive(i <= currentBubbleIndex);
            }
        }

        // Only type the current message (the newest one)
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
    // Optional: Public method to reset to first message
    public void ResetToFirst()
    {
        currentBubbleIndex = 0;
        UpdateMessages();
    }

    
}