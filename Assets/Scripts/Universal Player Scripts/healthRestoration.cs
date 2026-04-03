using UnityEngine;

public class healthRestoration : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string trigger = "isInside";

    [SerializeField] private AudioSource spin;

    private health status; 

    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Player"))
        {
            animator.SetTrigger(trigger);
            spin.Play();

            health status = other.GetComponent<health>();
            
        }
    }

    public void HealPlayer()
    {
        if (status != null)
        {
          status.RestoreMaxHealth();
        }
    }
}
