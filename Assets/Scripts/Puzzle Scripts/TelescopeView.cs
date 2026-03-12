using UnityEngine;

public class TelescopeView : MonoBehaviour
{
   
    public GameObject mainCamera;
    public GameObject telescopeCamera;

    private bool usingTelescope = false;

    public void ToggleTelescope()
    {
        Debug.Log("Telescope on");
        usingTelescope = !usingTelescope;

        mainCamera.SetActive(!usingTelescope);
        telescopeCamera.SetActive(usingTelescope);
    }

}
