using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public void DesktopScene()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void VRScene()
    {
        SceneManager.LoadScene("MainGame_VR");
    }
}