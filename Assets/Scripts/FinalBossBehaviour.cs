
using UnityEngine;

public class FinalBossBehaviour : MonoBehaviour
{
    public GameObject finalBoss;
    public float teleportRate = 5f;

    private Transform playerTransform;
    private playerInteraction playerInteraction;
    private wandBehaviour wandBehaviour;
    private int totalEnemiesToKill;
    private bool bossSpawned = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // number of enemies at the start of the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerInteraction = player.GetComponent<playerInteraction>();
            wandBehaviour = player.GetComponentInChildren<wandBehaviour>();
        }


        // get the goal
        totalEnemiesToKill = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (finalBoss != null)
            finalBoss.SetActive(false);

    }

    void Update()
    {

        if (bossSpawned) return;



        // check if the number of killed enemies matches the total found at the start
        if (playerInteraction.enemiesKilled >= totalEnemiesToKill && totalEnemiesToKill > 0)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        bossSpawned = true;
        finalBoss.SetActive(true);
        Debug.Log("All enemies killed. Final boss spawned!");

        CombinePowers();
        Teleport();

        // start teleporting after 2 seconds, then every 'teleportRate' seconds
        InvokeRepeating(nameof(Teleport), 2f, teleportRate);

    }

    void CombinePowers()
    {
        Color darkPurple;
        ColorUtility.TryParseHtmlString("#261D5B", out darkPurple); // converts hexcode to colour

        // "combine"/clear all colours/powers for a new one to defeat final boss 
        wandBehaviour.SetColour(darkPurple);
        wandBehaviour.colours.Clear();
        wandBehaviour.colours.Add(darkPurple);
    }

    void Teleport()
    {
        Vector3 randomOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        finalBoss.transform.position = playerTransform.position + randomOffset;

    }

}