using UnityEngine;
using UnityEngine.EventSystems;

// Tela de Controles, aberta tanto pelo menu inicial quanto pela pausa.
// Esconde o painel do botão que a abriu e devolve a seleção para ele ao voltar.
public class ControlsScreen : MonoBehaviour
{
    [SerializeField] GameObject backButton;

    public static bool IsOpen { get; private set; }

    GameObject caller;

    public void Open(GameObject callerButton)
    {
        caller = callerButton;
        caller.transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(true);
        IsOpen = true;
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        caller.transform.parent.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(caller);
    }

    void OnDisable() => IsOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Close();
    }
}
