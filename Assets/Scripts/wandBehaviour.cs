using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class wandBehaviour : MonoBehaviour
{
    [Header("Colours")]
    public List<Color> colours = new List<Color>();

    public Renderer wandLight;
    public ParticleSystem wandParticles;
    public Color CurrentColor { get; private set; }

    private int currentIndex = 0;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed");
            //UpdateMaterial();
            CycleColour();
        }
        
    }

    void CycleColour()
    {
        // increment
        currentIndex++;

        // Wrap around
        currentIndex %= colours.Count;

        // Update colour
        SetColour(colours[currentIndex]);

    }
   

    void SetColour(Color newColour)
    {
        CurrentColor = newColour;

        // Change light color
        if (wandLight != null)
            wandLight.material.color = newColour;
            wandLight.material.SetColor("_EmissionColor", newColour);

        // Change particle color
        if (wandParticles != null)
        {
            var main = wandParticles.main;
            main.startColor = newColour;
        }

    }
    
}

