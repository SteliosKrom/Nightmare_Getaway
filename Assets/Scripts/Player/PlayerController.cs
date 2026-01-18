using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
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
        get { return playerCharacterController; } 
        set { playerCharacterController = value; } 
    }

    private void Start()
    {
        playerCharacterController.slopeLimit = 45f;
        playerCharacterController.stepOffset = 0.5f;
        playerCharacterController.skinWidth = 0.08f;
    }

    private void Update()
    {
        if (RoundManager.Instance.CurrentMenuState == MenuState.OnInventoryMenu ||
            RoundManager.Instance.CurrentMenuState == MenuState.OnNoteMenu)
        {
            RoundManager.Instance.CurrentPlayerState = PlayerState.OnIdle;

            playerCharacterController.Move(Vector3.zero);

            cameraFollow.ApplyIdleHeadBobbing();
            return;
        }

        CrouchInput();
        MovePlayer();
    }

    public void MovePlayer()
    {
        if (RoundManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            PlayerMovementInput();
            ApplyMovementAndHeadBobbing();
        }
    }

    public void PlayerMovementInput()
    {
        KeyCode forward = KeybindManager.Instance.ActualKeybinds["MoveForward"];
        KeyCode backward = KeybindManager.Instance.ActualKeybinds["MoveBackward"];
        KeyCode left = KeybindManager.Instance.ActualKeybinds["MoveLeft"];
        KeyCode right = KeybindManager.Instance.ActualKeybinds["MoveRight"];
        KeyCode sprint = KeybindManager.Instance.ActualKeybinds["Sprint"];

        bool isMoving = Input.GetKey(forward) || Input.GetKey(backward) || Input.GetKey(left) || Input.GetKey(right);

        if (Input.GetKey(sprint) && isMoving && RoundManager.Instance.CurrentPlayerState != PlayerState.OnCrouching)
        {
            Run();
            return;
        }
        if (RoundManager.Instance.CurrentPlayerState == PlayerState.OnCrouching && isMoving)
        {
            CrouchWalk();
            return;
        }
        if (RoundManager.Instance.CurrentPlayerState == PlayerState.OnCrouching && !isMoving)
        {
            RoundManager.Instance.CurrentPlayerState = PlayerState.OnCrouching;
            return;
        }
        if (isMoving)
        {
            Walk();
            return;
        }
        RoundManager.Instance.CurrentPlayerState = PlayerState.OnIdle;
    }

    public void CrouchInput()
    {
        if (!canCrouch)
            return;

        if (RoundManager.Instance.CurrentGameState == GameState.OnPause) return;
        if (RoundManager.Instance.CurrentGameState != GameState.OnPlaying) return;

        KeyCode crouch = KeybindManager.Instance.ActualKeybinds["Crouch"];

        if (Input.GetKeyDown(crouch))
        {
            StartCoroutine(CrouchCooldown());
            if (RoundManager.Instance.CurrentPlayerState == PlayerState.OnIdle
                || RoundManager.Instance.CurrentPlayerState == PlayerState.OnWalking
                || RoundManager.Instance.CurrentPlayerState == PlayerState.OnRunning)
            {
                playerAnimator.SetBool("IsCrouching", true);
                RoundManager.Instance.CurrentPlayerState = PlayerState.OnCrouching;
            }
            else if (RoundManager.Instance.CurrentPlayerState == PlayerState.OnCrouching)
            {
                playerAnimator.SetBool("IsCrouching", false);
                RoundManager.Instance.CurrentPlayerState = PlayerState.OnIdle;
            }
        }
    }

    public void Walk()
    {
        KeyCode forward = KeybindManager.Instance.ActualKeybinds["MoveForward"];
        KeyCode backward = KeybindManager.Instance.ActualKeybinds["MoveBackward"];
        KeyCode left = KeybindManager.Instance.ActualKeybinds["MoveLeft"];
        KeyCode right = KeybindManager.Instance.ActualKeybinds["MoveRight"];

        Vector3 moveForwardDirection = Vector3.zero;
        Vector3 finalMovement = Vector3.zero;

        RoundManager.Instance.CurrentPlayerState = PlayerState.OnWalking;

        if (Input.GetKey(forward)) moveForwardDirection += mainCamera.forward;
        if (Input.GetKey(backward)) moveForwardDirection -= mainCamera.forward;
        if (Input.GetKey(right)) moveForwardDirection += mainCamera.right;
        if (Input.GetKey(left)) moveForwardDirection -= mainCamera.right;

        finalMovement = moveForwardDirection.normalized * walkSpeed;
        playerCharacterController.SimpleMove(finalMovement);
    }

    public void Run()
    {
        KeyCode forward = KeybindManager.Instance.ActualKeybinds["MoveForward"];
        KeyCode backward = KeybindManager.Instance.ActualKeybinds["MoveBackward"];
        KeyCode left = KeybindManager.Instance.ActualKeybinds["MoveLeft"];
        KeyCode right = KeybindManager.Instance.ActualKeybinds["MoveRight"];

        Vector3 moveForwardDirection = Vector3.zero;
        Vector3 finalMovement = Vector3.zero;

        RoundManager.Instance.CurrentPlayerState = PlayerState.OnRunning;

        if (Input.GetKey(forward)) moveForwardDirection += mainCamera.forward;
        if (Input.GetKey(backward)) moveForwardDirection -= mainCamera.forward;
        if (Input.GetKey(right)) moveForwardDirection += mainCamera.right;
        if (Input.GetKey(left)) moveForwardDirection -= mainCamera.right;

        finalMovement = moveForwardDirection.normalized * runSpeed;
        playerCharacterController.SimpleMove(finalMovement);
    }

    public void CrouchWalk()
    {
        KeyCode forward = KeybindManager.Instance.ActualKeybinds["MoveForward"];
        KeyCode backward = KeybindManager.Instance.ActualKeybinds["MoveBackward"];
        KeyCode left = KeybindManager.Instance.ActualKeybinds["MoveLeft"];
        KeyCode right = KeybindManager.Instance.ActualKeybinds["MoveRight"];

        Vector3 moveForwardDirection = Vector3.zero;
        Vector3 finalMovement = Vector3.zero;

        RoundManager.Instance.CurrentPlayerState = PlayerState.OnCrouching;

        if (Input.GetKey(forward)) moveForwardDirection += mainCamera.forward;
        if (Input.GetKey(backward)) moveForwardDirection -= mainCamera.forward;
        if (Input.GetKey(right)) moveForwardDirection += mainCamera.right;
        if (Input.GetKey(left)) moveForwardDirection -= mainCamera.right;

        finalMovement = moveForwardDirection.normalized * crouchSpeed;
        playerCharacterController.SimpleMove(finalMovement);
    }

    public void ApplyMovementAndHeadBobbing()
    {
        KeyCode forward = KeybindManager.Instance.ActualKeybinds["MoveForward"];
        KeyCode backward = KeybindManager.Instance.ActualKeybinds["MoveBackward"];
        KeyCode left = KeybindManager.Instance.ActualKeybinds["MoveLeft"];
        KeyCode right = KeybindManager.Instance.ActualKeybinds["MoveRight"];

        bool isMoving = Input.GetKey(forward) || Input.GetKey(backward) || Input.GetKey(left) || Input.GetKey(right);

        switch (RoundManager.Instance.CurrentPlayerState)
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