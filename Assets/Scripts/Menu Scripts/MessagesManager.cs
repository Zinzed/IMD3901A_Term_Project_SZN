using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net.NetworkInformation;

public class MessagesManager : MonoBehaviour
{
    [SerializeField] private float m_DelayBetweenCharacters = 0.02f;
    [SerializeField] private List<GameObject> messageBubbles;
    [SerializeField] private bool autoAdvance = true; // Toggle for auto-advance feature

    //public AudioSource notificationSFX;

    [SerializeField] private GameObject startBttn;
    [SerializeField] private Button nextBttn;
    [SerializeField] private Button previousBttn;
    [SerializeField] private GameObject magicEffect;
    [SerializeField] private GameObject magicEffect2;
    [SerializeField] private GameObject spawnPos;

    private int currentBubbleIndex = 0;
    private Coroutine typingCoroutine;
    private TMP_Text currentTextComponent;
    private bool hasSpawned = false;

    void Start()
    {
        AudioManager.Instance.musicSource.Stop();

        //AudioManager.Instance.PlayMusic("IntroCutscene");

        UpdateMessages();
        startBttn.SetActive(false);
        previousBttn.interactable = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousBubble();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextBubble();
        }
    }

    IEnumerator TypeText(TMP_Text textComponent)

    {
        string fullText = textComponent.text;
        textComponent.maxVisibleCharacters = 0;

        // disable buttons while typing
        nextBttn.interactable = false;
        previousBttn.interactable = false;

        // if first message, disable previous button
        if (currentBubbleIndex == 0)
        {
            previousBttn.interactable = false;
        }

        for (int i = 0; i < fullText.Length; i++)
        {
            textComponent.maxVisibleCharacters++;

            // default speed
            float delay = m_DelayBetweenCharacters;

            // check appearing character
            char c = fullText[i];

            // extra delay for punctuation
            if (c == '.' || c == '!' || c == '?')
            {
                delay *= 10f; // long pause for end of sentence
            }
            else if (c == ',' || c == ';' || c == ':')
            {
                delay *= 5f; // medium pause for commas
            }

            yield return new WaitForSeconds(delay);
           
            // re-enable next previous buttons while still typing
             nextBttn.interactable = true;
             previousBttn.interactable = true;

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

            // spawn magic effects for win scene 
            if (magicEffect != null && magicEffect2 != null && !hasSpawned)
            {

                Instantiate(magicEffect, spawnPos.transform.position, Quaternion.identity);
                Invoke(nameof(SpawnSecond), 2.0f);
                AudioManager.Instance.PlaySFX("MagicTransfer");
                hasSpawned = true;
            }

        }
        
    }

    //to spawn second effect
    void SpawnSecond()
    {
        Instantiate(magicEffect2, spawnPos.transform.position, Quaternion.identity);
    }

    public void NextBubble()
    {
        // if typing, skip to end and stop. don't increment index yet.
        if (typingCoroutine != null)
        {
            SkipToEnd();
            return;
        }

        if (currentBubbleIndex < messageBubbles.Count - 1)
        {
            currentBubbleIndex++;
            UpdateMessages();
            AudioManager.Instance.PlaySFX("SecondaryButton");

        }
    }

    public void PreviousBubble()
    {
        // if typing, just show the full text of current bubble
        if (typingCoroutine != null)
        {
            SkipToEnd();
            return;
        }

        if (currentBubbleIndex > 0)
        {
            currentBubbleIndex--;
            UpdateMessages();
            AudioManager.Instance.PlaySFX("SecondaryButton");
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

            // re-enable buttons since typing was interrupted
            nextBttn.interactable = true;
            previousBttn.interactable = true;

            // if this is the last message, handle start button
            if (currentBubbleIndex == messageBubbles.Count - 1)
            {
                startBttn.SetActive(true);
                nextBttn.interactable = false;

                // spawn magic effects for win scene 
                if (magicEffect != null && magicEffect2 != null && !hasSpawned)
                {

                    Instantiate(magicEffect, spawnPos.transform.position, Quaternion.identity);
                    Invoke(nameof(SpawnSecond), 1.0f);
                    AudioManager.Instance.PlaySFX("MagicTransfer");
                    hasSpawned = true;
                }
            }
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
                messageBubbles[i].SetActive(i == currentBubbleIndex);

                if (i == currentBubbleIndex)
                {
                    AudioManager.Instance.PlaySFX("MessageNotification");
                }
            }
        }

        // only type the current message (the newest one)
        if (currentBubbleIndex >= 0 && currentBubbleIndex < messageBubbles.Count)
        {
            GameObject currentBubble = messageBubbles[currentBubbleIndex];
            currentTextComponent = currentBubble.GetComponentInChildren<TMP_Text>();

            if (currentTextComponent != null)
            {
                currentTextComponent.maxVisibleCharacters = 0;
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