using UnityEngine;
using UnityEngine.UI;

public class uiBehaviour : MonoBehaviour
{
    public Image crosshair;
    public Color defaultCol = Color.white;
    public Color interactCol = Color.magenta;

    private Color currentEnemyColor;
    private bool isOverEnemy = false;

    public void SetCrosshairColor(Color enemyColor)
    {
        isOverEnemy = true;
        currentEnemyColor = enemyColor;
        crosshair.color = enemyColor;
    }

    public void SetInteract(bool canInteract)
    {
        // set to default/white if we aren't currently aiming at an enemy/interactable object
        if (!canInteract)
        {
            isOverEnemy = false;
            crosshair.color = defaultCol;
        }
        else if (!isOverEnemy)
        {
            // if inetractable but not an enemy use magenta
            crosshair.color = interactCol;
        }
    }

    public void SetCrosshairToDefault()
    {
        isOverEnemy = false;
        crosshair.color = defaultCol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
