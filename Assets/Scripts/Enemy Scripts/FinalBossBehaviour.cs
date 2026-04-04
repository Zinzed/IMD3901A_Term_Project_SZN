using System.Collections;
using UnityEditor.Rendering.LookDev;
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
    private bool bossSpawned = false;

    [SerializeField] private AudioClip bossIntro;
    [SerializeField] private float spawnDelay = 21.5f;
    [SerializeField] private float firstTeleportDelay = 2.0f;
    [SerializeField] private float teleportCooldown = 10.0f;

    [SerializeField] private AudioClip[] bossSounds;
    [SerializeField] private AudioSource bossAudioSource;

    private FinalBossActions bossActions;

    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float moveRadius = 4.0f;
    [SerializeField] private float directionChangeTime = 2.0f;
    private Vector3 moveDirection;

    private Rigidbody rb;

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

        if (finalBoss != null)
            finalBoss.SetActive(false);        
    }

    void Update()
    {
        HandleMovement();

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
        }

        // check how many normal enemies are still left
        GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("Enemies left: " + remainingEnemies.Length);

        // check if the number of killed enemies matches the total found at the start
        if (remainingEnemies.Length == 0)
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
        StartCoroutine(SpawnSequence());
    }

    void CombinePowers()
    {
        Color darkPurple;
        ColorUtility.TryParseHtmlString("#261D5B", out darkPurple); // converts hexcode to colour

        if (wandBehaviour != null)
        {
            // "combine"/clear all colours/powers for a new one to defeat final boss 
            wandBehaviour.SetColour(darkPurple);
            wandBehaviour.colours.Clear();
            wandBehaviour.colours.Add(darkPurple);
            Debug.Log("Desktop wand changed to purple.");
        }

        if (vrWandBehaviour != null)
        {
            vrWandBehaviour.SetColour(darkPurple);
            vrWandBehaviour.colours.Clear();
            vrWandBehaviour.colours.Add(darkPurple);
            Debug.Log("VR wand changed to purple.");
        }
        else
        {
            Debug.LogWarning("vrWandBehaviour is not assigned.");
        }
    }

    IEnumerator SpawnSequence()
    {
        Debug.Log("Playing boss intro..");

        if (bossIntro != null && playerTransform != null)
        {
            AudioSource.PlayClipAtPoint(bossIntro, playerTransform.position);
        }

        yield return new WaitForSeconds(spawnDelay);

        finalBoss.SetActive(true);

        rb = finalBoss.GetComponent<Rigidbody>();

        bossActions = finalBoss.GetComponent<FinalBossActions>();
        Debug.Log("All enemies killed. Final boss spawned!");

        CombinePowers();

        yield return null;

        StartCoroutine(BossSoundLoop());

        StartCoroutine(TeleportLoop());

        StartCoroutine(MovementLoop());
    }

    IEnumerator TeleportLoop()
    {
        while (true)
        {
            Teleport();
            yield return new WaitForSeconds(teleportCooldown);
        }
    }

    IEnumerator MovementLoop()
    {
        while (true)
        {
            SetNewMoveDirection();
            yield return new WaitForSeconds(directionChangeTime);
        }
    }

    void Teleport()
    {
        if (finalBoss == null || playerTransform == null)
        {
            return;
        }

        Vector3 randomOffset = new Vector3(Random.Range(-5.0f, 5.0f), 0, Random.Range(-5.0f, 5.0f));
        Vector3 targetPos = playerTransform.position + randomOffset;

        RaycastHit hit;
        if (Physics.Raycast(targetPos + Vector3.up * 10.0f, Vector3.down, out hit, 20.0f))
        {
            targetPos = hit.point;
        }

        if (Physics.CheckSphere(targetPos + Vector3.up * 0.5f, 0.8f))
        {
            randomOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            targetPos = playerTransform.position + randomOffset;

            if (Physics.Raycast(targetPos + Vector3.up * 10f, Vector3.down, out hit, 20f))
            {
                targetPos = hit.point;
            }
        }

        finalBoss.transform.position = targetPos;
        moveDirection = Vector3.zero;

        if (bossActions != null)
        {
            bossActions.OnTeleport();
        }
    }

    IEnumerator BossSoundLoop()
    {
        if (bossAudioSource == null || bossSounds.Length == 0)
        {
            yield break;
        }

        int lastIndex = -1;

        while (true)
        {
            if (finalBoss == null)
            {
                yield break;
            }
            
            //make sure the same sound doesn't play twice in a row
            int newIndex = Random.Range(0, bossSounds.Length);

            if (newIndex == lastIndex)
            {
                newIndex = (newIndex + 1) % bossSounds.Length;
            }
            lastIndex = newIndex;

            AudioClip clip = bossSounds[newIndex];

            bossAudioSource.pitch = Random.Range(0.9f, 1.1f);
            bossAudioSource.volume = Random.Range(0.85f, 1f);

            Debug.Log("Playing: " + clip.name);
            bossAudioSource.PlayOneShot(clip);

            yield return new WaitForSeconds(clip.length);
            yield return new WaitForSeconds(Random.Range(0.3f, 1.2f));
        }
    }

    void SetNewMoveDirection()
    {
        if (playerTransform == null)
        {
            return;
        }

        Vector3 toPlayer = (playerTransform.position - finalBoss.transform.position).normalized;
        Vector3 side = Vector3.Cross(toPlayer, Vector3.up).normalized;
        moveDirection = (Random.value > 0.5f) ? side : -side;

        moveDirection += new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-0.3f, 0.3f));
        moveDirection.Normalize();
    }

    void HandleMovement()
    {
        if (rb == null || playerTransform == null)
        {
            return;
        }

        Vector3 toPlayer = playerTransform.position - rb.position;

        if (toPlayer.magnitude > moveRadius)
        {
            moveDirection = toPlayer.normalized;
        }

        RaycastHit hit;
        if (Physics.Raycast(rb.position, moveDirection, out hit, 1.5f))
        {
            SetNewMoveDirection();
        }

        Vector3 velocity = moveDirection * moveSpeed;
        rb.linearVelocity = new Vector3(velocity.x, 0f, velocity.z);

        //look in the direction of the player
        Vector3 lookDirection = playerTransform.position - finalBoss.transform.position;
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
           finalBoss.transform.rotation = Quaternion.Slerp(
               finalBoss.transform.rotation,
               Quaternion.LookRotation(lookDirection),
               Time.deltaTime * 5f);
        }
    }
}