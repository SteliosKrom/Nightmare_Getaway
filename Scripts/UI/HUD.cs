using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD Instance;

    [Header("OBJECTS")]
    [SerializeField] private GameObject[] allHUDIcons;
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private GameObject lockedIcon;
    [SerializeField] private GameObject dotIcon;

    public GameObject[] AllHUDIcons => allHUDIcons;
    public GameObject InteractIcon => interactIcon;
    public GameObject LockedIcon => lockedIcon;
    public GameObject DotIcon => dotIcon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DisableAllHUDIcons();
    }

    public void DisableAllHUDIcons()
    {
        foreach (GameObject icon in allHUDIcons)
        {
            icon.SetActive(false);
        }
    }

    public void ShowDotOnly()
    {
        dotIcon.SetActive(true);
        interactIcon.SetActive(false);
        lockedIcon.SetActive(false);
    }

    public void ShowInteract()
    {
        dotIcon.SetActive(false);
        interactIcon.SetActive(true);
        lockedIcon.SetActive(false);
    }

    public void ShowLockedOnly()
    {
        dotIcon.SetActive(false);
        interactIcon.SetActive(false);
        lockedIcon.SetActive(true);
    }
}
