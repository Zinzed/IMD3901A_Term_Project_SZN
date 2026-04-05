using UnityEngine;
using UnityEngine.SceneManagement;

public class lunaBehaviour : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainGame":
                animator.Play("Idle");
                break;

            case "LoseScene":
                animator.Play("sad");
                break;

            case "WinScene":
                animator.Play("happy");
                break;
        }
    }
}