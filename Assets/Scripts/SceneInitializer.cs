using UnityEngine;

public class SceneInitializer : MonoBehaviour
{

    public GameObject DesktopPlayer;
    public GameObject VRPlayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (DataManager.Instance != null)
        {
            // Enable desktop player
            if (DataManager.Instance.button1Clicked)
            {
                DesktopPlayer.SetActive(true);
                VRPlayer.SetActive(false);

            }
            // Enable HMD player
            if (DataManager.Instance.button2Clicked)
            {
                VRPlayer.SetActive(true);
                DesktopPlayer.SetActive(false);

            }
        }

    }

}