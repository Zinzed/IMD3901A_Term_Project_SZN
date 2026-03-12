using UnityEngine;

public class TelescopeView : MonoBehaviour
{
   
    public Camera mainCamera;
    public Camera telescopeCamera;

    private bool usingTelescope = false;

    public void ToggleTelescope()
    {
        Debug.Log("Telescope on");
        usingTelescope = !usingTelescope;

        mainCamera.enabled = !usingTelescope;
        telescopeCamera.enabled = usingTelescope;
    }

}
