using UnityEngine;

public class DetectiveHealth : MonoBehaviour
{
    [SerializeField] int maxHits = 10;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip deathSound;

    public int Current { get; private set; }
    public int Max => maxHits;
    public bool IsFull => Current >= maxHits;

    void Awake()
    {
        Current = maxHits;
    }

    public void Restore(int value) => Current = Mathf.Clamp(value, 1, maxHits);

    public void Heal(int amount)
    {
        if (Current == 0) return;
        Current = Mathf.Min(maxHits, Current + amount);
    }

    public void TakeHit(int hits = 1)
    {
        if (Current == 0) return;
        Current = Mathf.Max(0, Current - hits);
        Sfx.Play(Current == 0 ? deathSound : hurtSound, transform.position);
        if (Current == 0) GetComponent<DetectiveMovement>().Play("Death");
    }
}
