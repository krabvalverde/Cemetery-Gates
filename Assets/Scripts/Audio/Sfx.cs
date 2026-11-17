using UnityEngine;

// Toca um efeito sonoro solto num ponto do mundo. O pitch varia um pouco a cada vez para não soar repetido.
public static class Sfx
{
    public static void Play(AudioClip[] clips, Vector3 position, float volume = 1f, float pitchJitter = 0.08f)
    {
        if (clips != null && clips.Length > 0) Play(clips[Random.Range(0, clips.Length)], position, volume, pitchJitter);
    }

    public static void Play(AudioClip clip, Vector3 position, float volume = 1f, float pitchJitter = 0.08f)
    {
        if (!clip) return;
        var source = new GameObject("Sfx " + clip.name).AddComponent<AudioSource>();
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume;
        source.pitch = 1f + Random.Range(-pitchJitter, pitchJitter);
        // ponytail: meio 2D/meio 3D porque o AudioListener fica na câmera fixa, longe da ação;
        // dá direção sem sumir com a distância. Listener no detetive se precisar de 3D de verdade.
        source.spatialBlend = 0.5f;
        source.minDistance = 6f;
        source.maxDistance = 40f;
        source.Play();
        Object.Destroy(source.gameObject, clip.length / source.pitch + 0.1f);
    }
}
