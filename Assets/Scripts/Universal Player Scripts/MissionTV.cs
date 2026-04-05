using UnityEngine;
using TMPro;

public class MissionTV : MonoBehaviour
{
    public TextMeshProUGUI tvText;

    private string[] instructions = {
        "Welcome. You have entered the Safe Zone. Recharge your health in the capsule.",
        "Look for the wand on the table and Grab it.",
        "Wand Acquired. Press the TRIGGER button to cycle through colors.",
        "Warning: Enemies detected ahead. Swing your wand FAST to defeat them!"
    };

    private int currentStep = 0;

    void Start()
    {
        UpdateDisplay();
    }

    public void NextInstruction()
    {
        if (currentStep < instructions.Length - 1)
        {
            currentStep++;
            UpdateDisplay();
        }
    }

    void UpdateDisplay()
    {
        if (tvText != null)
            tvText.text = instructions[currentStep];
    }
}
