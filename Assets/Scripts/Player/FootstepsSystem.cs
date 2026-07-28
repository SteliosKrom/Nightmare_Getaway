using UnityEngine;

public class FootstepsSystem : MonoBehaviour
{
    private float holdThreshold = 0.1f;
    private float keyHoldTime = 0f;

    #region SERVICES
    private GameManager gameManager;
    private KeybindManager keybindManager;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private LayerMask groundLayer;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField] private AudioSource footstepsAudioSource;
    [SerializeField] private AudioSource grassFootstepsAudioSource;
    #endregion

    private void Start()
    {
        gameManager = ServiceManager.GetService<GameManager>();
        keybindManager = ServiceManager.GetService<KeybindManager>();
    }

    private void Update()
    {
        if (gameManager.CurrentGameState != GameState.OnPlaying)
        {
            StopFootStepsAudioSource();
            return;
        }

        KeyCode forward = keybindManager.ActualKeybinds["MoveForward"];
        KeyCode backward = keybindManager.ActualKeybinds["MoveBackward"];
        KeyCode left = keybindManager.ActualKeybinds["MoveLeft"];
        KeyCode right = keybindManager.ActualKeybinds["MoveRight"];

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

        switch (gameManager.CurrentPlayerState)
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
        if (gameManager.CurrentPlayerState != PlayerState.OnCrouching 
            && gameManager.CurrentPlayerState != PlayerState.OnWalking
            && gameManager.CurrentPlayerState != PlayerState.OnRunning)
        {
            gameManager.CurrentPlayerState = PlayerState.OnIdle;
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

