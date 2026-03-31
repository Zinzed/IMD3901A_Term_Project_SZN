
using UnityEngine;

public class FinalBossBehaviour : MonoBehaviour
{
    public GameObject finalBoss;
    public float teleportRate = 5f;
    public progressBar playerProgress;

    private Transform playerTransform;
    private playerInteraction playerInteraction;
    private VRPlayerInteraction vrInteraction;
    private wandBehaviour wandBehaviour;
    public VRWandBehaviour vrWandBehaviour;
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
            vrInteraction = player.GetComponent<VRPlayerInteraction>();
            wandBehaviour = player.GetComponentInChildren<wandBehaviour>();
            //vrWandBehaviour = player.GetComponentInChildren<VRWandBehaviour>();
        }


        // get the goal
        totalEnemiesToKill = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (finalBoss != null)
            finalBoss.SetActive(false);

        Debug.Log("Total enemies to kill: " + totalEnemiesToKill);
    }

    void Update()
    {

        if (bossSpawned) return;

        if (playerInteraction == null && vrInteraction == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject p in players)
            {
                // ONLY lock onto the player that is actually active/enabled
                if (p.activeInHierarchy)
                {
                    playerTransform = p.transform;
                    playerInteraction = p.GetComponent<playerInteraction>();
                    vrInteraction = p.GetComponent<VRPlayerInteraction>();
                    wandBehaviour = p.GetComponentInChildren<wandBehaviour>();
                    //vrWandBehaviour = p.GetComponentInChildren<VRWandBehaviour>();

                    if (playerInteraction != null || vrInteraction != null)
                    {
                        Debug.Log("Locked onto ACTIVE player: " + p.name);
                        break;
                    }
                }
            }
            return;
        }
        int currentKills = 0;
        if (playerInteraction != null) currentKills = playerInteraction.enemiesKilled;
        //else if (vrInteraction != null) currentKills = vrInteraction.enemiesKilled;
        else return; // Still haven't found a player script, so stop here

        Debug.Log("Current Kills: " + currentKills + " / Goal: " + totalEnemiesToKill);

        // check if the number of killed enemies matches the total found at the start
        if (currentKills >= totalEnemiesToKill && totalEnemiesToKill > 0)
        {
            if (playerProgress != null)
            {
                playerProgress.UpdateProgress(+10); // Increment progress
            }
            else
            {
                Debug.LogError("Player has no progress script!"); // Debug missing component
            }

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

        if (wandBehaviour != null) {

            // "combine"/clear all colours/powers for a new one to defeat final boss 
            wandBehaviour.SetColour(darkPurple);
            wandBehaviour.colours.Clear();
            wandBehaviour.colours.Add(darkPurple);
        }

        else if (vrWandBehaviour != null)
        {
            vrWandBehaviour.SetColour(darkPurple);
            vrWandBehaviour.colours.Clear();
            vrWandBehaviour.colours.Add(darkPurple);
        }
    }

    void Teleport()
    {
        Vector3 randomOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        finalBoss.transform.position = playerTransform.position + randomOffset;

    }

}