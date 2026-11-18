using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Pausa com Esc durante o jogo. Roda antes do InventoryUI para o Esc que fecha o inventário não abrir a pausa junto.
[DefaultExecutionOrder(-10)]
public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject screen;
    [SerializeField] GameObject firstButton;

    public static bool IsOpen { get; private set; }

    Behaviour[] playerControls;

    void Start()
    {
        var player = FindAnyObjectByType<DetectiveMovement>();
        playerControls = new Behaviour[] { player, player.GetComponent<DetectiveCombat>() };
        screen.SetActive(false);
    }

    void OnDestroy()
    {
        IsOpen = false;
        AudioListener.pause = false;
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape) || MainMenu.IsOpen || GameOverUI.IsShowing || InventoryUI.IsOpen || ControlsScreen.IsOpen) return;
        SetOpen(!IsOpen);
    }

    public void Resume() => SetOpen(false);

    // Volta ao menu inicial recarregando a cena (ainda não há save para guardar o progresso).
    public void QuitToMenu() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    void SetOpen(bool open)
    {
        IsOpen = open;
        screen.SetActive(open);
        Time.timeScale = open ? 0f : 1f;
        AudioListener.pause = open;
        foreach (var control in playerControls) control.enabled = !open;
        EventSystem.current.SetSelectedGameObject(open ? firstButton : null);
    }
}
