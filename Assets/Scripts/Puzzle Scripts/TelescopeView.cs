using UnityEngine;

public class TelescopeView : MonoBehaviour
{
   
    public GameObject activeMainCamera;
    public GameObject telescopeCamera;

    private bool usingTelescope = false;

    public void ToggleTelescope()
    {
        usingTelescope = !usingTelescope;
        Debug.Log("Telescope Toggle: " + usingTelescope);

        if (usingTelescope)
        {
            //finds which player camera is currently active in the scene
            activeMainCamera = FindActivePlayerCamera();

            if (activeMainCamera != null)
            {
                activeMainCamera.SetActive(false);
                telescopeCamera.SetActive(true);
            }
        }
        else
        {
            // toggle back to active camera found
            if (activeMainCamera != null)
            {
                activeMainCamera.SetActive(true);
                telescopeCamera.SetActive(false);
            }
        }
    }

    private GameObject FindActivePlayerCamera()
    {
        //finds active camera by its tag since both vr and desktop cameras are tagged MainCamera
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");

        foreach (GameObject cam in cameras)
        {
            //avoid getting the telescope camera since its also tagged MainCamera
            if (cam.activeInHierarchy && cam != telescopeCamera)
            {
                return cam;
            }
        }

        //Debug.LogError("No active Player Camera found! );
        return null;
    }
}