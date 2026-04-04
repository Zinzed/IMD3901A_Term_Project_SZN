using System.Collections.Generic;
using UnityEngine;

public class VRWandBehaviour : MonoBehaviour
{
    [Header("Colours")]
    public List<Color> colours = new List<Color>();

    public Light wandLight;
    public Renderer lightBulbMat;
    public ParticleSystem wandParticles;
    public Color CurrentColor { get; private set; }

    private int currentIndex = 0;

    void Start()
    {
        //Initializes with first color if list isnt empty
        if (colours.Count > 0)
        {
            SetColour(colours[0]);
        }
    }

    //Public function called from wand grab interactable component
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
            wandLight.color = newColour;

        // Change light bubl material   
        if (lightBulbMat != null)
        {
            lightBulbMat.material.color = newColour;
            lightBulbMat.material.SetColor("_EmissionColor", newColour);
        }

        //change particle color
        if (wandParticles != null)
        {
            var main = wandParticles.main;
            main.startColor = newColour;
        }
    }
}