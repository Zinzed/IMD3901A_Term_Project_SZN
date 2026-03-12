using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InstructionsManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button nextBttn;
    [SerializeField] private Button previousBttn;
    [SerializeField] private List<GameObject> instructionPages;

    private int currentPageIndex = 0;

    private void Start()
    {
        // Initialize first page
        UpdatePageDisplay();
    }

    public void NextPage()
    {
        if (currentPageIndex < instructionPages.Count - 1)
        {
            currentPageIndex++;
            UpdatePageDisplay();
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdatePageDisplay();
        }
    }

    private void UpdatePageDisplay()
    {
        // hide all pages
        foreach (var page in instructionPages)
        {
            if (page != null)
                page.SetActive(false);
        }

        // show current page
        if (currentPageIndex >= 0 && currentPageIndex < instructionPages.Count)
        {
            instructionPages[currentPageIndex].SetActive(true);
        }

        // update button states
        nextBttn.interactable = currentPageIndex + 1 < instructionPages.Count;
        previousBttn.interactable = currentPageIndex > 0;
    }
}