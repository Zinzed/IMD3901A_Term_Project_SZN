using UnityEngine;

public class enemyBehaviour : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float wanderRadius = 10.0f;
    public float chaseRadius = 5.0f;
    public float minHeight = 0.5f;
    public float maxHeight = 1.5f;

    private Vector3 target;
    private float changeTarget = 0.5f;
    private Transform playerTransform;

    public float damageCooldown = 1f;
    private float lastDamageTime;

    void Start()
    {
        pickTarget();
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Prioritize chasing if player is within range
            if (distanceToPlayer <= chaseRadius)
            {
                target = playerTransform.position;
            }
            else if (Vector3.Distance(transform.position, target) < changeTarget)
            {
                // Pick new target only when not chasing
                pickTarget();
            }
        }

        // Move towards the current target
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    void pickTarget()
    {
        Vector2 randomXZ = Random.insideUnitCircle * wanderRadius;
        float targetY = Random.Range(minHeight, maxHeight);
        target = new Vector3(
            transform.position.x + randomXZ.x,
            targetY,
            transform.position.z + randomXZ.y
        );
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggered with: {other.gameObject.name}"); // Debug to confirm trigger

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player triggered!"); // Confirm player detection
            health playerHealth = other.GetComponent<health>();
            if (playerHealth != null)
            {
                playerHealth.UpdateHealth(-1); // Deduct health
            }
            else
            {
                Debug.LogError("Player has no health script!"); // Debug missing component
            }
        }
    }
}