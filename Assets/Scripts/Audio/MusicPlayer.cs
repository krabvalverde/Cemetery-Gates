using UnityEngine;

// Trilha da cena: escolhe a faixa pelo estado do jogo (menu, jogando, fim) e troca com fade.
// No Game Over a música some e fica só o som da morte.
[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip menuMusic;   // menu inicial
    [SerializeField] AudioClip levelMusic;  // a cena em jogo (cemitério / catacumbas)
    [SerializeField] AudioClip endingMusic; // tela de fim
    [SerializeField, Range(0f, 1f)] float volume = 0.6f;
    [SerializeField] float fadeTime = 1.5f;

    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        source.volume = 0f;
    }

    void Update()
    {
        AudioClip wanted = EndingUI.IsShowing ? endingMusic
            : GameOverUI.IsShowing ? null
            : MainMenu.IsOpen ? menuMusic
            : levelMusic;

        // Fade por tempo real: menu e fim congelam o jogo (timeScale 0).
        float step = volume / fadeTime * Time.unscaledDeltaTime;
        if (source.clip != wanted)
        {
            source.volume = Mathf.MoveTowards(source.volume, 0f, step);
            if (source.volume > 0f && source.isPlaying) return;
            source.clip = wanted;
            if (wanted) source.Play();
        }
        else if (wanted) source.volume = Mathf.MoveTowards(source.volume, volume, step);
    }
}
