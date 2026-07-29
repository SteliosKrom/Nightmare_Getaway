using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region STATES
    [Header("STATES")]
    [SerializeField] private GameState currentGameState;
    [SerializeField] private MenuState currentMenuState;
    [SerializeField] private PlayerState currentPlayerState;
    [SerializeField] private EnvironmentState currentEnvironmentState;
    [SerializeField] private ItemState currentItemState;
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
    #endregion
    private void Awake()
    {
        ServiceManager.RegisterService<GameManager>(this);
    }

    private void Start()
    {
        // Change game state to none later
        CurrentGameState = GameState.None;
        CurrentMenuState = MenuState.OnTitleMenu; // Change this to none later

        CurrentPlayerState = PlayerState.OnIdle;
        CurrentEnvironmentState = EnvironmentState.none;
        CurrentItemState = ItemState.none;
    }
}


