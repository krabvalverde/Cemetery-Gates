using UnityEngine;

// Câmera fixa por área: quando o detetive entra no trigger desta zona, a câmera dela vira a ativa.
// Cada câmera leva o próprio AudioListener, então liga/desliga o GameObject inteiro.
[RequireComponent(typeof(BoxCollider))]
public class CameraZone : MonoBehaviour
{
    [SerializeField] Camera zoneCamera;

    static Camera active;

    void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        zoneCamera.gameObject.SetActive(false);
    }

    void Start()
    {
        // Trigger não garante OnTriggerEnter para quem já começa dentro: ativa a zona inicial na mão.
        var player = FindAnyObjectByType<DetectiveMovement>();
        if (player && GetComponent<BoxCollider>().bounds.Contains(player.transform.position + Vector3.up))
            Activate();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DetectiveMovement>()) Activate();
    }

    void Activate()
    {
        if (active == zoneCamera) return;
        if (active) active.gameObject.SetActive(false);
        zoneCamera.gameObject.SetActive(true);
        active = zoneCamera;
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
