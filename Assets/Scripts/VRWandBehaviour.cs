using System.Collections.Generic;
using UnityEngine;

public class VRWandBehaviour : MonoBehaviour
{
    [Header("Colours")]
    public List<Color> colours = new List<Color>();

    public Renderer wandLight;
    public ParticleSystem wandParticles;
    public Color CurrentColor { get; private set; }

    private int currentIndex = 0;

    void Start()
    {
        // Initialize with first color if list isn't empty
        if (colours.Count > 0)
        {
            SetColour(colours[0]);
        }
    }

    // Public method that can be called from anywhere
    public void CycleColour()
    {
        if (colours.Count == 0) return;

        Debug.Log("Cycling to next color!");

        currentIndex++;
        currentIndex %= colours.Count;
        SetColour(colours[currentIndex]);
    }

    public void SetColour(Color newColour)
    {
        CurrentColor = newColour;

        // Change light color
        if (wandLight != null)
        {
            wandLight.material.color = newColour;
            wandLight.material.SetColor("_EmissionColor", newColour);
        }

        // Change particle color
        if (wandParticles != null)
        {
            var main = wandParticles.main;
            main.startColor = newColour;
        }
    }
}