using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class Interactor : MonoBehaviour
{
    #region LAYERS
    [Header("LAYERS")]
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask obstacleLayer;
    #endregion

    #region DELAYS
    [Header("DELAYS")]
    private float lockedUIDelay = 1f;
    private float doorCollidersDelay = 1f;
    private float toggleDelay = 0.5f;
    private float telephoneCallDelay = 4f;
    private float heartbeatAudioDelay = 2f;
    #endregion

    #region STATES
    [Header("STATES")]
    private bool isLocked = false;
    private bool isToggled = true;
    private bool hasFlashlight = false;
    private bool canToggle = true;
    #endregion

    #region PLAYER
    [Header("PLAYER")]
    [SerializeField] private Transform interactionSource;
    public float interactionRange;
    private int keyCounter = 0;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private TriggerFlickering triggerFlickering;
    [SerializeField] private Flashlight flashlight;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private HUD headsUpDisplay;
    [SerializeField] private InventoryManager inventory;
    #endregion

    #region TEXT
    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI keysCounter;
    #endregion

    #region ROOM_ENVIRONMENT
    [Header("1. ROOM & ENVIRONMENT")]
    [SerializeField] private GameObject lockedMessagePanel;
    [SerializeField] private GameObject kidsRoomVolume;
    [SerializeField] private GameObject[] switchLights;
    [SerializeField] private List<GameObject> interactableObjects;
    #endregion

    #region KEYS_ACCESS
    [Header("2. KEYS & ACCESS ITEMS")]
    [SerializeField] private GameObject roomKey;
    [SerializeField] private GameObject mainDoorKey;
    [SerializeField] private GameObject garageKey;
    #endregion

    #region LIGHT_SOUND
    [Header("3. LIGHT & SOUND SOURCES")]
    [SerializeField] private GameObject equippedFlashlightObj;
    [SerializeField] private GameObject flashlightObj;
    [SerializeField] private GameObject openedDoorAudioSourceObject;
    [SerializeField] private GameObject closedDoorAudioSourceObject;
    [SerializeField] private GameObject candleLight;
    #endregion

    #region ENTITY_TRIGGERS
    [Header("4. ENTITY & FEAR TRIGGERS")]
    [SerializeField] private GameObject creepyEntityTrigger;
    [SerializeField] private GameObject doorKnockTrigger;
    [SerializeField] private GameObject creepyEntity;
    [SerializeField] private GameObject doll;
    #endregion

    #region TABLE_PICKUPS_NORMAL
    [Header("5. TABLE PICKUPS (NORMAL)")]
    [SerializeField] private GameObject tableCrucifix;
    [SerializeField] private GameObject tableKnife;
    [SerializeField] private GameObject tableBook;
    [SerializeField] private GameObject phone;
    #endregion

    #region CURSED_SPECIAL_ITEMS
    [Header("6. CURSED/SPECIAL ITEMS")]
    [SerializeField] private GameObject cursedBook;
    [SerializeField] private GameObject cursedCrucifix;
    [SerializeField] private GameObject cursedKnife;
    #endregion

    #region NOTE_UI
    [Header("7. NOTE & UI ELEMENTS")]
    [SerializeField] private GameObject note;
    [SerializeField] private GameObject noteMenu;
    #endregion

    #region AUDIO
    [Header("AUDIO EFFECTS")]
    [SerializeField] private AudioLowPassFilter radioAudioLowPassFilter;
    [SerializeField] private AudioLowPassFilter telephoneAudioLowPassFilter;

    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource radioAudioSource;
    [SerializeField] private AudioSource telephoneAudioSource;
    [SerializeField] private AudioSource brokenLightAudioSource;
    [SerializeField] private AudioSource demonCryAudioSource;

    [Header("AUDIO CLIPS")]
    [SerializeField] private AudioClip brokenLightAudioClip;
    [SerializeField] private AudioClip demonCryAudioClip;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private BoxCollider telephoneCollider;
    [SerializeField] private BoxCollider radioCollider;
    [SerializeField] private BoxCollider tableCollider;
    [SerializeField] private BoxCollider doorBoxCollider;
    [SerializeField] private BoxCollider demonCryCollider;
    [SerializeField] private BoxCollider creepyEntityTriggerObstacle;
    [SerializeField] private BoxCollider[] doorColliders;
    [SerializeField] private BoxCollider[] doorHandleColliders;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATORS")]
    [SerializeField] private Animator garageLightAnimator;
    #endregion

    #region VISUAL EFFECTS
    [Header("VFX")]
    [SerializeField] private ParticleSystem candleSmoke;
    #endregion

    #region LIGHTING
    [Header("LIGHTING")]
    [SerializeField] private Light kidRoomLight;
    [SerializeField] private Light garageRoomLight;
    #endregion

    public GameObject NoteMenu => noteMenu;
    public GameObject LockedMessagePanel => lockedMessagePanel;
    public bool HasFlashlight => hasFlashlight;
    public bool CanToggle { get => canToggle; set => canToggle = value; }

    private void Start()
    {
        DisplayItems(roomKey, mainDoorKey, phone, garageKey);
    }

    void Update()
    {
        DetectInteractable();
        DebugRaycast();
        HandleInputs();
    }

    public void DisplayItems(GameObject roomKey, GameObject mainDoorKey,
    GameObject phone, GameObject garageKey)
    {
        roomKey.SetActive(true);
    }
}
