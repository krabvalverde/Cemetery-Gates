using UnityEngine;

// Câmera fixa por área: quando o detetive entra no trigger desta zona, a câmera dela vira a ativa.
// Cada câmera leva o próprio AudioListener, então liga/desliga o GameObject inteiro.
[RequireComponent(typeof(BoxCollider))]
public class CameraZone : MonoBehaviour
{
    [SerializeField] Camera zoneCamera;
    [SerializeField, Range(0f, 0.4f)] float edgeMargin = 0.12f; // fração do quadro que conta como "borda"
    [SerializeField] float panSpeed = 5f;

    static Camera active;
    static Transform player;
    Quaternion homeRotation;

    void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        zoneCamera.gameObject.SetActive(false);
        homeRotation = zoneCamera.transform.rotation;
    }

    void Start()
    {
        // Trigger não garante OnTriggerEnter para quem já começa dentro: ativa a zona inicial na mão.
        var movement = FindAnyObjectByType<DetectiveMovement>();
        if (!movement) return;
        player = movement.transform;
        if (GetComponent<BoxCollider>().bounds.Contains(player.position + Vector3.up))
            Activate();
    }

    void LateUpdate()
    {
        if (active == zoneCamera) KeepPlayerInFrame(1f - Mathf.Exp(-panSpeed * Time.deltaTime));
    }

    // Rede de segurança: a câmera não sai do lugar, só gira o mínimo para o detetive não sair do quadro.
    void KeepPlayerInFrame(float blend)
    {
        if (!player) return;
        Transform cam = zoneCamera.transform;
        Vector3 target = player.position + Vector3.up;
        Vector3 v = zoneCamera.WorldToViewportPoint(target);
        Quaternion wanted;
        if (v.z <= 0f)
        {
            wanted = Quaternion.LookRotation(target - cam.position);
        }
        else
        {
            var inside = new Vector3(Mathf.Clamp(v.x, edgeMargin, 1f - edgeMargin), Mathf.Clamp(v.y, edgeMargin, 1f - edgeMargin), v.z);
            if (inside == v) return;
            Vector3 from = zoneCamera.ViewportPointToRay(inside).direction;
            Vector3 forward = Quaternion.FromToRotation(from, target - cam.position) * cam.forward;
            wanted = Quaternion.LookRotation(forward, Vector3.up);
        }
        cam.rotation = Quaternion.Slerp(cam.rotation, wanted, blend);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DetectiveMovement>()) Activate();
    }

    // Zona dentro de outra (interior da casa dentro da área do coveiro): ao sair da ativa, volta para a que ainda contém o detetive.
    void OnTriggerExit(Collider other)
    {
        if (active != zoneCamera || !other.GetComponent<DetectiveMovement>()) return;
        foreach (var zone in FindObjectsByType<CameraZone>(FindObjectsInactive.Exclude))
            if (zone != this && zone.GetComponent<BoxCollider>().bounds.Contains(other.transform.position + Vector3.up))
            {
                zone.Activate();
                return;
            }
    }

    void Activate()
    {
        if (active == zoneCamera) return;
        if (active) active.gameObject.SetActive(false);
        zoneCamera.gameObject.SetActive(true);
        active = zoneCamera;
        zoneCamera.transform.rotation = homeRotation;
        KeepPlayerInFrame(1f); // já entra com o detetive no quadro, sem esperar o giro
    }

    void OnDrawGizmos()
    {
        var box = GetComponent<BoxCollider>();
        Gizmos.color = new Color(0f, 0.8f, 1f, 0.15f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(box.center, box.size);
        if (zoneCamera)
        {
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.TransformPoint(box.center), zoneCamera.transform.position);
        }
    }
}
