using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Tela de Game Over: aparece com fade depois que a animação de morte do detetive termina.
public class GameOverUI : MonoBehaviour
{
    [SerializeField] CanvasGroup screen;
    [SerializeField] GameObject firstButton;
    [SerializeField] float delay = 2.3f;    // animação Death dura 2,1 s
    [SerializeField] float fadeTime = 1.5f;
    [SerializeField] string restartLabel = ""; // vazio mantém o texto do botão ("Recomeçar do início")

    public static bool IsShowing { get; private set; }

    DetectiveHealth health;
    float deathTime = -1f;

    void Start()
    {
        health = FindAnyObjectByType<DetectiveHealth>();
        screen.gameObject.SetActive(false);
        IsShowing = false;
        if (restartLabel != "") firstButton.GetComponentInChildren<UnityEngine.UI.Text>().text = restartLabel;
    }

    void Update()
    {
        if (IsShowing || !health || health.Current > 0) return;
        if (deathTime < 0f) deathTime = Time.time;
        if (Time.time - deathTime >= delay) StartCoroutine(Show());
    }

    IEnumerator Show()
    {
        IsShowing = true;
        screen.gameObject.SetActive(true);
        screen.interactable = false;
        for (float t = 0f; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            screen.alpha = t / fadeTime;
            yield return null;
        }
        screen.alpha = 1f;
        screen.interactable = true;
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    // Recarrega a cena atual: no cemitério volta do zero; nas catacumbas o detetive volta com o estado da entrada (GameState).
    public void RestartFromBeginning()
    {
        MainMenu.SkipOnNextLoad = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
