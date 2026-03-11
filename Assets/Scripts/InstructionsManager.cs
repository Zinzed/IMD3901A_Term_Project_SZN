using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructionsManager : MonoBehaviour
{
    public Button nextBttn;
    public Button previousBttn;
    public GameObject InstructionsPanel1;
    public GameObject InstructionsPanel2;

    //GameObject myObject;
    public void NextPage()
    {
        InstructionsPanel2.SetActive(true);
        InstructionsPanel1.SetActive(false);
        nextBttn.interactable = false;
        previousBttn.interactable = true;
    }

    public void PreviousPage()
    {
        InstructionsPanel2.SetActive(false);
        InstructionsPanel1.SetActive(true);
        nextBttn.interactable = true;
        previousBttn.interactable = false;
    }
}
