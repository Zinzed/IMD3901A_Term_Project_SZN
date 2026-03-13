using UnityEngine;

public class VRWandAttack : MonoBehaviour
{
    public VRWandBehaviour wandBehaviour;

    public float minSwingRotation = 150f;
    private Quaternion lastRotation;
    private float rotationSpeed;

    void Update()
    {
        rotationSpeed = Quaternion.Angle(transform.rotation, lastRotation) / Time.deltaTime;
        lastRotation = transform.rotation;
        Debug.Log(rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        // check if the wand is rotating fast enough
        if (rotationSpeed < minSwingRotation)
        {
            Debug.Log("Swing not strong enough");
            return;
        }

        enemyBehaviour enemy = other.GetComponentInParent<enemyBehaviour>();
        if (enemy == null)
            return;

        Renderer enemyRenderer = other.GetComponentInChildren<Renderer>();
        if (enemyRenderer == null)
            return;

        Color enemyColor = enemyRenderer.material.GetColor("_BaseColor");
        Color wandColor = wandBehaviour.CurrentColor;

        float colorDiff = Vector4.Distance(enemyColor, wandColor);

        if (colorDiff < 0.1f)
        {
            Destroy(enemy.gameObject, 1.2f);
            Debug.Log("Enemy destroyed!");
        }
        else
        {
            Debug.Log("Wrong color!");
        }
    }
}
