using UnityEngine;

public class VRWandAttack : MonoBehaviour
{
    public VRWandBehaviour wandBehaviour;

    private void OnTriggerEnter(Collider other)
    {
        enemyBehaviour enemy = other.GetComponentInParent<enemyBehaviour>();

        if (enemy == null)
            return;

        Debug.Log("Enemy touched!");

        // get enemy color
        Renderer enemyRenderer = other.GetComponentInChildren<Renderer>();
        if (enemyRenderer == null)
            return;

        Color enemyColor = enemyRenderer.material.GetColor("_BaseColor");

        // get wand color
        Color wandColor = wandBehaviour.CurrentColor;

        Debug.Log("Enemy color: " + enemyColor);
        Debug.Log("Wand color: " + wandColor);

        // compare colors
        float colorDiff = Vector3.Distance(
            new Vector3(enemyColor.r, enemyColor.g, enemyColor.b),
            new Vector3(wandColor.r, wandColor.g, wandColor.b)
        );

        if (colorDiff < 0.15f)
        {
            Destroy(enemy.gameObject);
            Debug.Log("Correct color! Enemy destroyed.");
        }
        else
        {
            Debug.Log("Wrong color!");
        }
    }
}