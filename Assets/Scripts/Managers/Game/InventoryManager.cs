using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private bool isEmpty;

    private float inventoryDelay = 1f;

    private int nextEmptySlot = 0;

    public bool onMenus = false;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    #endregion

    #region STATES
    [Header("GAME STATES")]
    [SerializeField] private bool canOpenInventory = true;
    #endregion

    #region BUTTONS
    [Header("BUTTONS")]
    [SerializeField] private Button[] itemButtons;
    #endregion

    #region INVENTORY
    [Header("INVENTORY")]
    [SerializeField] private GameObject[] inventoryItems;
    [SerializeField] private GameObject[] inventorySlots;
    [SerializeField] private GameObject[] inventoryMenus;

    [SerializeField] private GameObject inventoryMenu;
    [SerializeField] private GameObject inventoryItemsMenu;
    #endregion

    #region AUDIO
    [Header("SOURCES")]
    [SerializeField] private AudioSource inventoryAudioSource;
    [SerializeField] private AudioSource openInventoryItemsAudioSource;

    [Header("CLIPS")]
    [SerializeField] private AudioClip openInventoryAudioClip;
    [SerializeField] private AudioClip closeInventoryAudioClip;
    [SerializeField] private AudioClip openInventoryItemsAudioClip;
    #endregion

    private void Start()
    {
        InitializeInventorySlots();
        GetCallbackOnInventoryButtons();
        DeactivateAllInventoryItems();
    }

    private void Update()
    {
        InventoryInput();
    }

    public void InitializeInventorySlots()
    {
        foreach (GameObject slot in inventorySlots)
        {
            slot.SetActive(false);
            isEmpty = true;
        }
    }

    public void GetCallbackOnInventoryButtons()
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            int index = i;
            itemButtons[i].onClick.AddListener(() => OpenInventoryMenus(inventoryMenus[index]));
        }
    }

    public void OpenInventoryMenus(GameObject menu)
    {
        menu.SetActive(true);
        inventoryItemsMenu.SetActive(false);
        onMenus = true;
        AudioManager.Instance.PlaySFX(openInventoryItemsAudioSource, openInventoryItemsAudioClip);
    }

    public void AddToInventory(int itemIndex)
    {
        if (nextEmptySlot >= inventorySlots.Length)
        {
            Debug.Log("Inventory is full!");
            return;
        }

        GameObject slot = inventorySlots[nextEmptySlot];
        GameObject item = inventoryItems[itemIndex];

        item.transform.position = slot.transform.position;
        item.SetActive(true);

        nextEmptySlot++;
        isEmpty = false;
    }

    public void InventoryInput()
    {
        if (RoundManager.Instance.CurrentMenuState == MenuState.OnNoteMenu) return;
        if (RoundManager.Instance.CurrentGameState != GameState.OnPlaying) return;

        KeyCode inventory = KeybindManager.Instance.ActualKeybinds["Inventory"];

        if (!canOpenInventory) return;

        if (!Input.GetKeyDown(inventory)) return;

        if (RoundManager.Instance.CurrentMenuState == MenuState.None)
        {
            OpenInventory();
        }
        else if (RoundManager.Instance.CurrentMenuState == MenuState.OnInventoryMenu)
        {
            CheckIfOnInventoryMenus();
        }
        StartCoroutine(InventoryDelay());
    }

    public void CheckIfOnInventoryMenus()
    {
        if (onMenus)
        {
            foreach (GameObject obj in inventoryMenus)
            {
                obj.SetActive(false);
            }

            onMenus = false;

            inventoryItemsMenu.SetActive(true);
            inventoryMenu.SetActive(true);
        }
        else
        {
            CloseInventory();
        }
    }

    public void OpenInventory()
    {
        RoundManager.Instance.CurrentMenuState = MenuState.OnInventoryMenu;
        onMenus = false;
        inventoryMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        AudioManager.Instance.PlaySFX(inventoryAudioSource, openInventoryAudioClip);
        HUD.Instance.DisableAllHUDIcons();
    }

    public void CloseInventory()
    {
        RoundManager.Instance.CurrentMenuState = MenuState.None;
        inventoryMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioManager.Instance.PlaySFX(inventoryAudioSource, closeInventoryAudioClip);
        HUD.Instance.ShowDotOnly();
    }

    public void DeactivateAllInventoryItems()
    {
        foreach (GameObject item in inventoryItems)
        {
            item.SetActive(false);
        }
    }

    public IEnumerator InventoryDelay()
    {
        canOpenInventory = false;
        yield return new WaitForSeconds(inventoryDelay);
        canOpenInventory = true;
    }
}
