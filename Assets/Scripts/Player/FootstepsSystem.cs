using UnityEngine;

public class FootstepsSystem : MonoBehaviour
{
    [Header("TYPES")]
    [SerializeField] private float holdThreshold = 0.1f;
    private float keyHoldTime = 0f;

    [Header("GAME OBJECTS")]
    [SerializeField] private LayerMask groundLayer;

    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource footstepsAudioSource;
    [SerializeField] private AudioSource grassFootstepsAudioSource;

    private void Update()
    {
        if (RoundManager.Instance.CurrentGameState != GameState.OnPlaying)
        {
            StopFootStepsAudioSource();
            return;
        }

        KeyCode forward = KeybindManager.Instance.ActualKeybinds["MoveForward"];
        KeyCode backward = KeybindManager.Instance.ActualKeybinds["MoveBackward"];
        KeyCode left = KeybindManager.Instance.ActualKeybinds["MoveLeft"];
        KeyCode right = KeybindManager.Instance.ActualKeybinds["MoveRight"];

        bool isMoving = Input.GetKey(forward) || Input.GetKey(backward) || Input.GetKey(left) || Input.GetKey(right);

        if (!isMoving)
        {
            Idle();
            return;
        }

        keyHoldTime += Time.deltaTime;

        if (keyHoldTime < holdThreshold)
            return;

        bool onGround = IsPlayerOnGround();

        switch (RoundManager.Instance.CurrentPlayerState)
        {
            case PlayerState.OnWalking:
                if (onGround) FootstepsGrassWalk();
                else FootstepsGroundWalk();
                break;
            case PlayerState.OnRunning:
                if (onGround) FootstepsGrassRun();
                else FootstepsGroundRun();
                break;
            case PlayerState.OnCrouching:
                if (onGround) FootstepsCrouchGrassWalk();
                else FootstepsCrouchGroundWalk();
                break;
            default:
                Idle();
                break;
        }
    }

    private void StopFootStepsAudioSource()
    {
        footstepsAudioSource.Stop();
        footstepsAudioSource.enabled = false;
        grassFootstepsAudioSource.enabled = false;
    }

    private void Idle()
    {
        if (RoundManager.Instance.CurrentPlayerState != PlayerState.OnCrouching 
            && RoundManager.Instance.CurrentPlayerState != PlayerState.OnWalking
            && RoundManager.Instance.CurrentPlayerState != PlayerState.OnRunning)
        {
            RoundManager.Instance.CurrentPlayerState = PlayerState.OnIdle;
        }

        keyHoldTime = 0f;
        footstepsAudioSource.Stop();
        footstepsAudioSource.enabled = false;
        grassFootstepsAudioSource.enabled = false;
    }

    private void FootstepsGroundWalk()
    {
        footstepsAudioSource.enabled = true;
        grassFootstepsAudioSource.enabled = false;
        footstepsAudioSource.pitch = Random.Range(0.5f, 1.25f);
    }

    private void FootstepsGroundRun()
    {
        footstepsAudioSource.enabled = true;
        grassFootstepsAudioSource.enabled = false;
        footstepsAudioSource.pitch = Random.Range(1f, 1.75f);
    }

    private void FootstepsGrassWalk()
    {
        grassFootstepsAudioSource.enabled = true;
        footstepsAudioSource.enabled = false;
        grassFootstepsAudioSource.pitch = Random.Range(0.5f, 1.5f);
    }

    private void FootstepsGrassRun()
    {
        grassFootstepsAudioSource.enabled = true;
        footstepsAudioSource.enabled = false;
        grassFootstepsAudioSource.pitch = Random.Range(1f, 1.75f);
    }

    private void FootstepsCrouchGroundWalk()
    {
        footstepsAudioSource.enabled = true;
        grassFootstepsAudioSource.enabled = false;
        footstepsAudioSource.pitch = Random.Range(0.25f, 0.5f);
    }

    private void FootstepsCrouchGrassWalk()
    {
        footstepsAudioSource.enabled = false;
        grassFootstepsAudioSource.enabled = true;
        grassFootstepsAudioSource.pitch = Random.Range(0.25f, 0.5f);
    }

    private bool IsPlayerOnGround()
    {
        return Physics.CheckSphere(transform.position, 0.1f, groundLayer);
    }
}

