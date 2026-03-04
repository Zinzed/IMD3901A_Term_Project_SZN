using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class wandBehaviour : MonoBehaviour
{
    [Header("Materials")]
    public Material[] materialColours;

    public MeshRenderer lightVisual;

    private int currentIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (lightVisual == null)
            lightVisual = GetComponent<MeshRenderer>();

        if (lightVisual != null && materialColours != null && materialColours.Length > 0)
        {
            lightVisual.sharedMaterial = materialColours[0]; // Set the initial material
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateMaterial();
        }
        
    }

    void UpdateMaterial()
    {
        if (lightVisual == null) return;

       // if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        //{
        // Increment index
        currentIndex++;

        // Wrap around
        currentIndex %= materialColours.Length;

        // Update material
        lightVisual.sharedMaterial = materialColours[currentIndex];
        //}
    } 
}
