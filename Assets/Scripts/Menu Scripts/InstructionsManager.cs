using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Oculus.Interaction;

public class InstructionsManager : MonoBehaviour
{

    [Header("UI References")]
    [SerializeField] private Button nextBttn;
    [SerializeField] private Button previousBttn;
    [SerializeField] private List<GameObject> instructionPages;
    [SerializeField] private List<GameObject> tabs;
    [SerializeField] private List<Image> tabBttns;
    [SerializeField] private Sprite inactiveTabBG, activeTabBG;

    private int currentPageIndex = 0;

    private void Start()
    {
        // Initialize first page
        UpdatePageDisplay();
    }

    public void SwitchToTab(int tabIndex)
    {
        AudioManager.Instance.PlaySFX("SecondaryButton");
        // for content panels
        for (int i = 0; i < tabs.Count; i++)
        {
            tabs[i].SetActive(i == tabIndex);
        }

        //for tab buttons 
        for (int i = 0; i < tabBttns.Count; i++)
        {
            if (i == tabIndex)
            {
                // active: white
                tabBttns[i].color = Color.white;
                //tabBttns[i].sprite = activeTabBG;
            }
            else
            {
                // inactive: grey
                tabBttns[i].color = new Color(0.7f, 0.7f, 0.7f);
                //tabBttns[i].sprite = inactiveTabBG;
            }
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousPage();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextPage();
        }
    }

    public void NextPage()
    {
        if (currentPageIndex < instructionPages.Count - 1)
        {
            currentPageIndex++;
            UpdatePageDisplay();
            AudioManager.Instance.PlaySFX("SecondaryButton");
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdatePageDisplay();
            AudioManager.Instance.PlaySFX("SecondaryButton");
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