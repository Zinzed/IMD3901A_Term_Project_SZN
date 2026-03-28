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
            if (status != null)
            {
                status.RestoreMaxHealth();
            }
        }
    }
    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
