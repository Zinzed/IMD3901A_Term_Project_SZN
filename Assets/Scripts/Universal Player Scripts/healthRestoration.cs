using UnityEngine;

public class healthRestoration : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string trigger = "isInside";

    [SerializeField] private AudioSource spin;

    private health status; 

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Something entered the trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            

            status = other.GetComponent<health>();
            if (status != null)
            {
                status.RestoreMaxHealth();

                animator.SetTrigger(trigger);
                spin.Play();
            }


            Debug.Log("Player entered! Target for healing: " + status.gameObject.name);

        }
    }

    public void HealPlayer()
    {
        if (status != null)
        {
          status.RestoreMaxHealth();
            Debug.Log("Healing function triggered by animation event!");
        }
        else
        {
            Debug.LogWarning("HealPlayer called but 'status' is null! Did the player leave the trigger?");
        }
    }
}
