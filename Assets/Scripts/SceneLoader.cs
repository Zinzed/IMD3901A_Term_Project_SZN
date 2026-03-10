using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    public string scene;

    //switch scene based on string entered in the inspector
    public void next(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

}