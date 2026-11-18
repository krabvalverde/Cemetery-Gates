using UnityEngine;

// Livro que desfaz a maldição, no altar do salão das catacumbas. Só pode ser pego depois que o guardião (esqueleto) cai.
public class CurseBook : MonoBehaviour
{
    [SerializeField] Enemy guardian;
    [SerializeField] float pickupDistance = 1.6f;
    [SerializeField] AudioClip pickupSound;

    DetectiveHealth player;

    void Start()
    {
        player = FindAnyObjectByType<DetectiveHealth>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) TryTake();
    }

    public void TryTake()
    {
        if (!player || player.Current == 0 || InventoryUI.IsOpen || PauseMenu.IsOpen || EndingUI.IsShowing) return;

        Vector3 offset = player.transform.position - transform.position;
        offset.y = 0f;
        if (offset.sqrMagnitude > pickupDistance * pickupDistance) return;

        if (guardian && !guardian.Dead)
        {
            InventoryUI.ShowMessage("O guardião não deixa você se aproximar do livro.");
            return;
        }
        Sfx.Play(pickupSound, transform.position);
        EndingUI.Show();
    }
}
