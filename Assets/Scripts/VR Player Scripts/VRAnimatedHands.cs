using UnityEngine;
using UnityEngine.InputSystem;

public class VRAnimatedHands : MonoBehaviour
{
    public InputActionProperty grabValue;
    public InputActionProperty releaseValue;

    public Animator handAnimator;

    // Update is called once per frame
    void Update()
    {
        //animation logic based on animation tree in unity
        float grab = grabValue.action.ReadValue<float>();
        float release = releaseValue.action.ReadValue<float>();

        handAnimator.SetFloat("Grab", grab);
        handAnimator.SetFloat("Release", release);


    }
}
