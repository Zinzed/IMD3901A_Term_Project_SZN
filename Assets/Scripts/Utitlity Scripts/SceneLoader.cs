using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    public string scene;

    public AudioSource primaryBttnSFX;

    //switch scene based on string entered in the inspector
    public void next(string scene)
    {
        SceneManager.LoadSceneAsync(scene);

        if (primaryBttnSFX != null)
        {
            primaryBttnSFX.Play();
        }

    }



}