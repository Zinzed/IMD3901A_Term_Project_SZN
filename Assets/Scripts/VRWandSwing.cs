using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRWandSwing : MonoBehaviour
{
    [Header("References")]
    public Transform wandTip; // optional
    private XRGrabInteractable grabInteractable;
    private XROrigin xrOrigin;

    [Header("Swing Settings")]
    public float swingThreshold = 0.12f;   // raise if tiny movements still trigger
    public float swingCooldown = 0.30f;
    public float swingActiveTime = 0.12f;  // keeps swing true briefly for hit detection

    private Vector3 lastLocalPos;
    private float lastSwingTime;
    private float swingTimer;

    public bool IsSwinging { get; private set; }

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        xrOrigin = FindObjectOfType<XROrigin>();
    }

    void Start()
    {
        Transform tracked = wandTip != null ? wandTip : transform;

        if (xrOrigin != null)
            lastLocalPos = xrOrigin.transform.InverseTransformPoint(tracked.position);
    }

    void Update()
    {
        if (xrOrigin == null)
            return;

        if (grabInteractable != null && !grabInteractable.isSelected)
        {
            IsSwinging = false;
            return;
        }

        Transform tracked = wandTip != null ? wandTip : transform;

        // position relative to player rig, not world
        Vector3 currentLocalPos = xrOrigin.transform.InverseTransformPoint(tracked.position);

        // how much the wand moved relative to the rig this frame
        float localMove = Vector3.Distance(currentLocalPos, lastLocalPos);

        // detect real swing
        if (localMove > swingThreshold && Time.time > lastSwingTime + swingCooldown)
        {
            IsSwinging = true;
            swingTimer = swingActiveTime;
            lastSwingTime = Time.time;

            Debug.Log("SWING DETECTED");
        }
        else
        {
            swingTimer -= Time.deltaTime;
            if (swingTimer <= 0f)
                IsSwinging = false;
        }

        lastLocalPos = currentLocalPos;
    }
}