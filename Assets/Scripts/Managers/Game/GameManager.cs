using UnityEngine;


// Just add is locked boolean variable to the door class and control the state for each door seperately
// ... My eyes hurt!!
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

public class GameManager : MonoBehaviour
{
    #region STATES
    [Header("STATES")]
    [SerializeField] private GameState currentGameState;
    [SerializeField] private MenuState currentMenuState;
    [SerializeField] private PlayerState currentPlayerState;
    [SerializeField] private EnvironmentState currentEnvironmentState;
    [SerializeField] private ItemState currentItemState;
    [SerializeField] private KidsDoorState currentKidsDoorState;
    [SerializeField] private GarageDoorState currentGarageDoorState;
    [SerializeField] private MainDoorState currentMainDoorState;
    #endregion

    #region PROPERTIES
    public GameState CurrentGameState { get => currentGameState; set => currentGameState = value; }
    public MenuState CurrentMenuState { get => currentMenuState; set => currentMenuState = value; }
    public PlayerState CurrentPlayerState { get => currentPlayerState; set => currentPlayerState = value; }
    public EnvironmentState CurrentEnvironmentState 
    { 
        get => currentEnvironmentState; 
        set => currentEnvironmentState = value;
    }
    public ItemState CurrentItemState { get => currentItemState; set => currentItemState = value; }
    public KidsDoorState CurrentKidsDoorState { get => currentKidsDoorState; set => currentKidsDoorState = value; }
    public GarageDoorState CurrentGarageDoorState 
    { 
        get => currentGarageDoorState; 
        set => currentGarageDoorState = value; 
    }
    public MainDoorState CurrentMainDoorState { get => currentMainDoorState; set => currentMainDoorState = value; }
    #endregion
    private void Awake()
    {
        ServiceManager.RegisterService<GameManager>(this);
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


