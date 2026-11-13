using UnityEngine;

public class DetectiveHealth : MonoBehaviour
{
    [SerializeField] int maxHits = 10;

    public int Current { get; private set; }
    public int Max => maxHits;
    public bool IsFull => Current >= maxHits;

    void Awake()
    {
        Current = maxHits;
    }

    public void Heal(int amount)
    {
        Current = Mathf.Min(maxHits, Current + amount);
    }

    public void TakeHit(int hits = 1)
    {
        Current = Mathf.Max(0, Current - hits);
    }
}
