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
        crosshair.color = canInteract ? interactCol : defaultCol;
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
