using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region SERVICES
    private GameManager gameManager;
    private KeybindManager keybindManager;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private DoorBase doorBase;
    [SerializeField] private Interactor interactor;
    [SerializeField] private ClockAudio clockAudio;
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private HUD headsUpDisplay;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private MainGameUIManager mainGameUIManager;
    #endregion

    #region STATES
    [Header("GAME STATES")]
    private bool canCrouch = true;
    #endregion

    #region PLAYER
    [Header("PLAYER")]
    [SerializeField] private CharacterController playerCharacterController;
    public RaycastHit hit;

    private float crouchSpeed = 1.5f;
    private float walkSpeed = 2.5f;
    private float runSpeed = 4.0f;
    private float crouchCooldown = 1f;
    #endregion

    #region CAMERAS
    [Header("CAMERA")]
    [SerializeField] private Transform mainCamera;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATORS")]
    [SerializeField] private Animator playerAnimator;
    #endregion

    public CharacterController CharacterController
    {
        get => playerCharacterController;
        set => playerCharacterController = value;
    }

    private void Start()
    {
        gameManager = ServiceManager.GetService<GameManager>();
        keybindManager = ServiceManager.GetService<KeybindManager>();

        playerCharacterController.slopeLimit = 45f;
        playerCharacterController.stepOffset = 0.5f;
        playerCharacterController.skinWidth = 0.08f;
    }

    private void Update()
    {
        if (gameManager.CurrentMenuState == MenuState.OnInventoryMenu ||
            gameManager.CurrentMenuState == MenuState.OnNoteMenu)
        {
            gameManager.CurrentPlayerState = PlayerState.OnIdle;

            playerCharacterController.Move(Vector3.zero);

            cameraFollow.ApplyIdleHeadBobbing();
            return;
        }

        CrouchInput();
        MovePlayer();
    }

    public void MovePlayer()
    {
        if (gameManager.CurrentGameState == GameState.OnPlaying)
        {
            PlayerMovementInput();
            ApplyMovementAndHeadBobbing();
        }
    }

    public void PlayerMovementInput()
    {
        KeyCode forward = keybindManager.ActualKeybinds["MoveForward"];
        KeyCode backward = keybindManager.ActualKeybinds["MoveBackward"];
        KeyCode left = keybindManager.ActualKeybinds["MoveLeft"];
        KeyCode right = keybindManager.ActualKeybinds["MoveRight"];
        KeyCode sprint = keybindManager.ActualKeybinds["Sprint"];

        bool isMoving = Input.GetKey(forward) || Input.GetKey(backward) || Input.GetKey(left) || Input.GetKey(right);

        if (Input.GetKey(sprint) && isMoving && gameManager.CurrentPlayerState != PlayerState.OnCrouching)
        {
            Run();
            return;
        }
        if (gameManager.CurrentPlayerState == PlayerState.OnCrouching && isMoving)
        {
            CrouchWalk();
            return;
        }
        if (gameManager.CurrentPlayerState == PlayerState.OnCrouching && !isMoving)
        {
            gameManager.CurrentPlayerState = PlayerState.OnCrouching;
            return;
        }
        if (isMoving)
        {
            Walk();
            return;
        }
        gameManager.CurrentPlayerState = PlayerState.OnIdle;
    }

    public void CrouchInput()
    {
        if (!canCrouch)
            return;

        if (gameManager.CurrentGameState == GameState.OnPause) return;
        if (gameManager.CurrentGameState != GameState.OnPlaying) return;

        KeyCode crouch = keybindManager.ActualKeybinds["Crouch"];

        if (Input.GetKeyDown(crouch))
        {
            StartCoroutine(CrouchCooldown());
            if (gameManager.CurrentPlayerState == PlayerState.OnIdle
                || gameManager.CurrentPlayerState == PlayerState.OnWalking
                || gameManager.CurrentPlayerState == PlayerState.OnRunning)
            {
                playerAnimator.SetBool("IsCrouching", true);
                gameManager.CurrentPlayerState = PlayerState.OnCrouching;
            }
            else if (gameManager.CurrentPlayerState == PlayerState.OnCrouching)
            {
                playerAnimator.SetBool("IsCrouching", false);
                gameManager.CurrentPlayerState = PlayerState.OnIdle;
            }
        }
    }

    public void Walk()
    {
        KeyCode forward = keybindManager.ActualKeybinds["MoveForward"];
        KeyCode backward = keybindManager.ActualKeybinds["MoveBackward"];
        KeyCode left = keybindManager.ActualKeybinds["MoveLeft"];
        KeyCode right = keybindManager.ActualKeybinds["MoveRight"];

        Vector3 moveForwardDirection = Vector3.zero;
        Vector3 finalMovement = Vector3.zero;

        gameManager.CurrentPlayerState = PlayerState.OnWalking;

        if (Input.GetKey(forward)) moveForwardDirection += mainCamera.forward;
        if (Input.GetKey(backward)) moveForwardDirection -= mainCamera.forward;
        if (Input.GetKey(right)) moveForwardDirection += mainCamera.right;
        if (Input.GetKey(left)) moveForwardDirection -= mainCamera.right;

        finalMovement = moveForwardDirection.normalized * walkSpeed;
        playerCharacterController.SimpleMove(finalMovement);
    }

    public void Run()
    {
        KeyCode forward = keybindManager.ActualKeybinds["MoveForward"];
        KeyCode backward = keybindManager.ActualKeybinds["MoveBackward"];
        KeyCode left = keybindManager.ActualKeybinds["MoveLeft"];
        KeyCode right = keybindManager.ActualKeybinds["MoveRight"];

        Vector3 moveForwardDirection = Vector3.zero;
        Vector3 finalMovement = Vector3.zero;

        gameManager.CurrentPlayerState = PlayerState.OnRunning;

        if (Input.GetKey(forward)) moveForwardDirection += mainCamera.forward;
        if (Input.GetKey(backward)) moveForwardDirection -= mainCamera.forward;
        if (Input.GetKey(right)) moveForwardDirection += mainCamera.right;
        if (Input.GetKey(left)) moveForwardDirection -= mainCamera.right;

        finalMovement = moveForwardDirection.normalized * runSpeed;
        playerCharacterController.SimpleMove(finalMovement);
    }

    public void CrouchWalk()
    {
        KeyCode forward = keybindManager.ActualKeybinds["MoveForward"];
        KeyCode backward = keybindManager.ActualKeybinds["MoveBackward"];
        KeyCode left = keybindManager.ActualKeybinds["MoveLeft"];
        KeyCode right = keybindManager.ActualKeybinds["MoveRight"];

        Vector3 moveForwardDirection = Vector3.zero;
        Vector3 finalMovement = Vector3.zero;

        gameManager.CurrentPlayerState = PlayerState.OnCrouching;

        if (Input.GetKey(forward)) moveForwardDirection += mainCamera.forward;
        if (Input.GetKey(backward)) moveForwardDirection -= mainCamera.forward;
        if (Input.GetKey(right)) moveForwardDirection += mainCamera.right;
        if (Input.GetKey(left)) moveForwardDirection -= mainCamera.right;

        finalMovement = moveForwardDirection.normalized * crouchSpeed;
        playerCharacterController.SimpleMove(finalMovement);
    }

    public void ApplyMovementAndHeadBobbing()
    {
        KeyCode forward = keybindManager.ActualKeybinds["MoveForward"];
        KeyCode backward = keybindManager.ActualKeybinds["MoveBackward"];
        KeyCode left = keybindManager.ActualKeybinds["MoveLeft"];
        KeyCode right = keybindManager.ActualKeybinds["MoveRight"];

        bool isMoving = Input.GetKey(forward) || Input.GetKey(backward) || Input.GetKey(left) || Input.GetKey(right);

        switch (gameManager.CurrentPlayerState)
        {
            case PlayerState.OnIdle:
                cameraFollow.ApplyIdleHeadBobbing();
                break;
            case PlayerState.OnWalking:
                cameraFollow.ApplyWalkHeadBobbing();
                break;
            case PlayerState.OnRunning:
                cameraFollow.ApplyRunHeadBobbing();
                break;
            case PlayerState.OnCrouching:
                if (isMoving)
                    cameraFollow.ApplyCrouchWalkHeadBobbing();
                else
                    cameraFollow.ApplyCrouchIdleHeadBobbing();
                break;
        }
    }

    public IEnumerator CrouchCooldown()
    {
        canCrouch = false;
        yield return new WaitForSeconds(crouchCooldown);
        canCrouch = true;
    }
}