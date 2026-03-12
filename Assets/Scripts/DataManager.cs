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
        //desktop button
        if (buttonId == 1)
        {
            button1Clicked = true;
        }
        //HMD button
        else if (buttonId == 2)
        {
            button2Clicked = true;
        }
    }
}
