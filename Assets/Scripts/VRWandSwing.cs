using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

//Resource used:
//https://gist.github.com/ttruty/1c04f1ac2ea55cf04e2fec5ecae77d7b

public class VRWandSwing : MonoBehaviour
{
    [Header("References")]
    public Transform wandTip;
    private XRGrabInteractable grabInteractable;
    private XROrigin xrOrigin;

    [Header("Swing Settings")]
    //how fast the wand movement should be
    public float swingThreshold = 0.8f;   
    //time between swings
    public float swingCooldown = 0.30f;
    //how long each swing stays on
    public float swingActiveTime = 0.12f;

    private Vector3 lastLocalPos;
    private float lastSwingTime;
    private float swingTimer;

    public bool IsSwinging { get; private set; }

    void Awake()
    {
        //gets the grab component from the wand and the player rig
        grabInteractable = GetComponent<XRGrabInteractable>();
        xrOrigin = FindObjectOfType<XROrigin>();
    }

    void Start()
    {
        //tracks wand tip
        Transform tracked = wandTip != null ? wandTip : transform;
        //stores the start pos from the player
        if (xrOrigin != null)
            lastLocalPos = xrOrigin.transform.InverseTransformPoint(tracked.position);
    }

    void Update()
    {
        if (xrOrigin == null)
            return;
        //checks if the wand is being held in order to detect the swings
        if (grabInteractable != null && !grabInteractable.isSelected)
        {
            IsSwinging = false;
            return;
        }

        Transform tracked = wandTip != null ? wandTip : transform;
        //gets the current position relative to the player but ignores walking movment
        Vector3 currentLocalPos = xrOrigin.transform.InverseTransformPoint(tracked.position);

        //tracks only vertical movement of the swing
        float verticalMove = currentLocalPos.y - lastLocalPos.y;
        //converting movement to speed
        float verticalSpeed = Mathf.Abs(verticalMove) / Time.deltaTime;

        //detects the swing; checks is movement was fast enough and the cooldown has passed
        if (verticalSpeed > swingThreshold && Time.time > lastSwingTime + swingCooldown)
        {
            IsSwinging = true;
            swingTimer = swingActiveTime;
            lastSwingTime = Time.time;

            Debug.Log("VERTICAL SWING!");
        }
        else
        {
            //reduce timer every frame
            swingTimer -= Time.deltaTime;
            //stop swinging if the timer runs out
            if (swingTimer <= 0f)
                IsSwinging = false;
        }
        
        lastLocalPos = currentLocalPos;
    }
}