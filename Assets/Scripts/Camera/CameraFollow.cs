using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlayerController playerController;

    [Header("CAMERA")]
    [SerializeField] private float crouchHeightCameraOffset = 9.0f;
    private Vector3 baseOffset;
    private Vector3 crouchOffset;
    private Vector3 velocity = Vector3.zero;
    private Vector3 smoothVelocity = Vector3.zero;

    [Header("IDLE BOBBING")]
    [SerializeField] private float idleBobAmount;
    [SerializeField] private float idleBobFrequency;
    [SerializeField] private float idleSmooth;

    [Header("WALK BOBBING")]
    [SerializeField] private float walkBobAmount;
    [SerializeField] private float walkBobFrequency;
    [SerializeField] private float walkSmooth;

    [Header("RUN BOBBING")]
    [SerializeField] private float runBobAmount;
    [SerializeField] private float runBobFrequency;
    [SerializeField] private float runSmooth;

    [Header("CROUCH WALK BOBBING")]
    [SerializeField] private float crouchWalkBobAmount;
    [SerializeField] private float crouchWalkBobFrequency;
    [SerializeField] private float crouchWalkSmooth;
    [SerializeField] private float crouchIdleSmooth;

    [Header("OBJECTS")]
    [SerializeField] private GameObject player;

    private void Start()
    {
        baseOffset = transform.position - player.transform.position;
        crouchOffset = baseOffset - new Vector3(0, crouchHeightCameraOffset, 0);
    }

    public void ApplyWalkHeadBobbing()
    {
        Vector3 playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 bobOffset = Vector3.zero;

        if (playerMovementInput.magnitude > 0.1f)
        {
            bobOffset.y = Mathf.Sin(Time.time * walkBobFrequency) * walkBobAmount;
            bobOffset.x = Mathf.Cos(Time.time * walkBobFrequency / 2f) * walkBobAmount;

            Vector3 finalPosition = player.transform.position + baseOffset + bobOffset;
            transform.position = Vector3.Lerp(transform.position, finalPosition, walkSmooth * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + baseOffset, walkSmooth * Time.deltaTime);
        }
    }

    public void ApplyRunHeadBobbing()
    {
        Vector3 playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 bobOffset = Vector3.zero;

        if (playerMovementInput.magnitude > 0.1f)
        {
            bobOffset.y = Mathf.Sin(Time.time * runBobFrequency) * runBobAmount;
            bobOffset.x = Mathf.Cos(Time.time * runBobFrequency / 2f) * runBobAmount;

            Vector3 finalPosition = player.transform.position + baseOffset + bobOffset;
            transform.position = Vector3.Lerp(transform.position, finalPosition, runSmooth * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + baseOffset, runSmooth * Time.deltaTime);
        }
    }

    public void ApplyCrouchWalkHeadBobbing()
    {
        float speed = playerController.CharacterController.velocity.magnitude;
        Vector3 bobOffset = Vector3.zero;

        if (speed > 0.1f)
        {
            bobOffset.y = Mathf.Sin(Time.time * crouchWalkBobFrequency) * crouchWalkBobAmount;
            bobOffset.x = Mathf.Cos(Time.time * crouchWalkBobFrequency / 2f) * crouchWalkBobAmount;

            Vector3 currentOffset = crouchOffset;
            Vector3 finalPosition = player.transform.position + currentOffset + bobOffset;
            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref velocity, crouchWalkSmooth * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + baseOffset, crouchWalkSmooth * Time.deltaTime);
        }
    }

    public void ApplyCrouchIdleHeadBobbing()
    {
        Vector3 target = player.transform.position + crouchOffset;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref smoothVelocity, crouchIdleSmooth);
    }

    public void ApplyIdleHeadBobbing()
    {
        Vector3 bobOffset = Vector3.zero;
        bobOffset.y = Mathf.Sin(Time.time * idleBobFrequency) * idleBobAmount;
        bobOffset.x = Mathf.Cos(Time.time * idleBobFrequency / 2f) * idleBobAmount;
        Vector3 finalPosition = player.transform.position + baseOffset + bobOffset;
        transform.position = Vector3.Lerp(transform.position, finalPosition, idleSmooth * Time.deltaTime);
    }
}

