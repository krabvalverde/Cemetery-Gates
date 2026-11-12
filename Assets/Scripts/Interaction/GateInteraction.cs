using UnityEngine;

// Abre o portão quando o detetive está perto e aperta E.
// Toca a animação Legacy de cada folha (Animation nos filhos) e libera o collider que bloqueia a passagem.
public class GateInteraction : MonoBehaviour
{
    [SerializeField] float interactDistance = 2.5f;
    [SerializeField] KeyCode interactKey = KeyCode.E;

    Animation[] leaves;
    Collider blocker;
    Transform player;
    bool opened;

    void Awake()
    {
        leaves = GetComponentsInChildren<Animation>();
        blocker = GetComponent<Collider>();
        foreach (var leaf in leaves)
        {
            leaf.playAutomatically = false;
            leaf.Stop();
            if (leaf.gameObject.isStatic)
                Debug.LogWarning($"{leaf.name} está marcado como Static: a malha não vai se mover. Desmarque Static nas folhas do portão.", leaf);
            foreach (AnimationState state in leaf)
            {
                state.wrapMode = WrapMode.ClampForever; // fica parado aberto no último frame
                leaf.clip = state.clip;                 // nem toda folha tem clipe padrão definido
            }
        }
    }

    void Start()
    {
        var detective = FindAnyObjectByType<DetectiveMovement>();
        if (detective) player = detective.transform;
    }

    void Update()
    {
        if (opened || !player || !Input.GetKeyDown(interactKey)) return;

        Vector3 offset = player.position - transform.position;
        offset.y = 0f;
        if (offset.sqrMagnitude > interactDistance * interactDistance) return;

        Open();
    }

    public void Open()
    {
        opened = true;
        foreach (var leaf in leaves) leaf.Play();
        if (blocker) blocker.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
