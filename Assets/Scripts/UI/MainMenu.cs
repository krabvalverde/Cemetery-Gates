using UnityEngine;
using UnityEngine.EventSystems;

// Menu inicial por cima da própria cena: o cemitério fica congelado ao fundo até apertar Iniciar.
public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject root;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject firstMenuButton;
    [SerializeField] GameObject creditsBackButton;

    public static bool IsOpen { get; private set; }
    public static bool SkipOnNextLoad; // "Recomeçar do início" recarrega a cena direto no jogo

    Behaviour[] playerControls;

    void Start()
    {
        var player = FindAnyObjectByType<DetectiveMovement>();
        playerControls = new Behaviour[] { player, player.GetComponent<DetectiveCombat>() };
        SetOpen(!SkipOnNextLoad);
        SkipOnNextLoad = false;
    }

    void OnDestroy()
    {
        IsOpen = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (IsOpen && creditsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape)) ShowCredits(false);
    }

    public void Play() => SetOpen(false);

    public void ShowCredits(bool show)
    {
        creditsPanel.SetActive(show);
        menuPanel.SetActive(!show);
        EventSystem.current.SetSelectedGameObject(show ? creditsBackButton : firstMenuButton);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void SetOpen(bool open)
    {
        IsOpen = open;
        root.SetActive(open);
        Time.timeScale = open ? 0f : 1f;
        foreach (var control in playerControls) control.enabled = !open;
        if (open) ShowCredits(false);
    }
}
