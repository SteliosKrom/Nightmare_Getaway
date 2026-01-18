using UnityEngine;

public enum GameState
{
    None,
    OnPlaying,
    OnPause,
    OnIntro,
}

public enum MenuState
{
    None,
    OnMenuSettings,
    OnGameSettings,
    OnCategorySettings,
    OnMainMenu,
    OnNoteMenu,
    OnPauseMenu,
    OnTitleMenu,
    OnInventoryMenu,
}

public enum PlayerState
{
    OnWalking,
    OnRunning,
    OnCrouching,
    OnIdle
}

public enum EnvironmentState
{
    OnOutdoors,
    OnIndoors,
    none
}

public enum ItemState
{
    kidsRoomKey,
    garageKey,
    mainDoorKey,
    book,
    knife,
    cross,
    none
}

public enum KidsDoorState
{
    unlocked,
    locked
}

public enum GarageDoorState
{
    unlocked,
    locked
}

public enum MainDoorState
{
    unlocked,
    locked
}

class RoundManager : MonoBehaviour
{
    [Header("SCRIPT REFERENCES")]
    public static RoundManager Instance;

    [Header("GAME STATES")]
    [SerializeField] private GameState currentGameState;
    [SerializeField] private MenuState currentMenuState;
    [SerializeField] private PlayerState currentPlayerState;
    [SerializeField] private EnvironmentState currentEnvironmentState;
    [SerializeField] private ItemState currentItemState;
    [SerializeField] private KidsDoorState currentKidsDoorState;
    [SerializeField] private GarageDoorState currentGarageDoorState;
    [SerializeField] private MainDoorState currentMainDoorState;

    public GameState CurrentGameState { get => currentGameState; set => currentGameState = value; }
    public MenuState CurrentMenuState { get => currentMenuState; set => currentMenuState = value; }
    public PlayerState CurrentPlayerState { get => currentPlayerState; set => currentPlayerState = value; }
    public EnvironmentState CurrentEnvironmentState { get => currentEnvironmentState; set => currentEnvironmentState = value; }
    public ItemState CurrentItemState { get => currentItemState; set => currentItemState = value; }
    public KidsDoorState CurrentKidsDoorState { get => currentKidsDoorState; set => currentKidsDoorState = value; }
    public GarageDoorState CurrentGarageDoorState { get => currentGarageDoorState; set => currentGarageDoorState = value; }
    public MainDoorState CurrentMainDoorState { get => currentMainDoorState; set => currentMainDoorState = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Game object" + name + "is not found");
            return;
        }
    }

    private void Start()
    {
        // Change game state to none later
        currentGameState = GameState.None;
        currentMenuState = MenuState.OnTitleMenu; // Change this to none later

        currentPlayerState = PlayerState.OnIdle;
        currentEnvironmentState = EnvironmentState.none;
        currentItemState = ItemState.none;

        currentKidsDoorState = KidsDoorState.locked;
        currentGarageDoorState = GarageDoorState.locked;
        currentMainDoorState = MainDoorState.locked;
    }
}


