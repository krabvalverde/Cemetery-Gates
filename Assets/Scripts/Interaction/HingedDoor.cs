using System.Collections;
using UnityEngine;

public class HingedDoor : MonoBehaviour
{
    [SerializeField] float openAngle = 95f;
    [SerializeField] float duration = 1.2f;
    [SerializeField] Collider blocker;

    bool opened;

    public void Open()
    {
        if (opened) return;
        opened = true;
        if (blocker) blocker.enabled = false;
        StartCoroutine(Swing());
    }

    IEnumerator Swing()
    {
        Quaternion from = transform.localRotation;
        Quaternion to = from * Quaternion.Euler(0f, openAngle, 0f);
        for (float t = 0f; t < 1f; t += Time.deltaTime / duration)
        {
            transform.localRotation = Quaternion.Slerp(from, to, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        transform.localRotation = to;
    }
}
