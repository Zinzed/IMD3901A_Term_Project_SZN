using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonProxy : MonoBehaviour
{
    public void ClickButton(int id)
    {
        // always find the one version of DataManager that survived
        if (DataManager.Instance != null)
        {
            DataManager.Instance.SetButtonState(id);
            SceneManager.LoadScene("YourGameScene");
        }
    }
}
