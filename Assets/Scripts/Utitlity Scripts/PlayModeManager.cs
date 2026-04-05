using UnityEngine;
using UnityEngine.XR.Hands;

public class PlayModeManager : MonoBehaviour
{
    public GameObject keyboardPlayer;

    public GameObject VRPlayer;
    public GameObject VRWand;
    public GameObject portal;

    void Awake()
    {
        //gets input from the player prefs and sets the players as active or disabled based on that

        int mode = PlayerPrefs.GetInt("PlayerMode", -1);

        if (mode == -1)
        {
            Debug.LogWarning("Player mode was not set, defaulting to keyboard mode");
            mode = 0;
        }

        Debug.Log("player mode loaded: " + mode);

        keyboardPlayer.SetActive(mode == 0);

        VRPlayer.SetActive(mode == 1);
        VRWand.SetActive(mode == 1);
        portal.SetActive(mode == 1);
    }
}