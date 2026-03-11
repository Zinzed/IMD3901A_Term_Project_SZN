
using UnityEngine;

public class FinalBossBehaviour : MonoBehaviour
{
    public GameObject finalBoss;

    private playerInteraction playerInteraction;
    private wandBehaviour wandBehaviour;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    // Find the player and get components
    playerInteraction = GetComponent<playerInteraction>();
    wandBehaviour = GetComponent<wandBehaviour>();


        // make sure final boss starts disabled
        if (finalBoss != null)
            finalBoss.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int enemyCount = enemies.Length;


        // only spawn final boss if all enemies are killed 
        if (playerInteraction.enemiesKilled == enemyCount)
        {
            finalBoss.SetActive(true);

            // "combine"/clear all colours/powers for a new one
            wandBehaviour.colours.Clear();
            Color darkPurple;
            ColorUtility.TryParseHtmlString("#FF5733", out darkPurple); //converts hexcode to colour
            wandBehaviour.colours.Add(darkPurple);

            Debug.Log("All enemies killed. Final boss spawned!");
        }

    }
}
