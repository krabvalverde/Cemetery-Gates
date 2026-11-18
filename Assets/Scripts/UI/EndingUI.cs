using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Tela de fim: o livro desfaz a maldição. Fade como o do Game Over, depois o jogo congela até voltar ao menu.
public class EndingUI : MonoBehaviour
{
    [SerializeField] CanvasGroup screen;
    [SerializeField] GameObject firstButton;
    [SerializeField] float fadeTime = 2f;

    public static bool IsShowing { get; private set; }

    static EndingUI instance;

    void Awake() => instance = this;

    void Start()
    {
        screen.gameObject.SetActive(false);
        IsShowing = false;
    }

    void OnDestroy()
    {
        if (instance != this) return;
        instance = null;
        IsShowing = false;
    }

    public static void Show()
    {
        if (instance && !IsShowing) instance.StartCoroutine(instance.FadeIn());
    }

    IEnumerator FadeIn()
    {
        IsShowing = true;
        var player = FindAnyObjectByType<DetectiveMovement>();
        player.enabled = false;
        player.GetComponent<DetectiveCombat>().enabled = false;
        screen.gameObject.SetActive(true);
        screen.interactable = false;
        for (float t = 0f; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            screen.alpha = t / fadeTime;
            yield return null;
        }
        screen.alpha = 1f;
        screen.interactable = true;
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void BackToMenu()
    {
        GameState.Clear();
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameState.CemeteryScene);
    }
}
