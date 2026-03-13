using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public bool button1Clicked = false;
    public bool button2Clicked = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetButtonState(int buttonId)
    {
        button1Clicked = (buttonId == 1); //desktop button
        button2Clicked = (buttonId == 2); //HMD button

    }
}
