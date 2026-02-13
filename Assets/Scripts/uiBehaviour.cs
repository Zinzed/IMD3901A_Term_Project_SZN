using UnityEngine;
using UnityEngine.UI;

public class uiBehaviour : MonoBehaviour
{
    public Image crosshair;
    public Color defaultCol = Color.white;
    public Color interactCol = Color.magenta;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SetInteract(bool canInteract)
    {
        crosshair.color = canInteract ? interactCol : defaultCol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
