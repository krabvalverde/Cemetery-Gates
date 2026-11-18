using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DetectiveMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 1.6f;
    [SerializeField] float runSpeed = 3.8f;
    [SerializeField] float backSpeed = 1.1f;
    [SerializeField] float turnSpeed = 150f;
    [SerializeField] float gravity = -20f;
    [SerializeField] AudioClip[] footsteps;
    [SerializeField] float walkStepInterval = 0.5f;  // clipe Walk: 2 passos em 1,0 s
    [SerializeField] float runStepInterval = 0.45f;  // clipe Run: 2 passos em 0,9 s

    CharacterController controller;
    float stepTimer;
    DetectiveCombat combat;
    DetectiveHealth health;
    Animator animator;
    float verticalVelocity;
    string currentState = "Idle";
    readonly HeldInput vertical = new(), horizontal = new(), run = new();

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        combat = GetComponent<DetectiveCombat>();
        health = GetComponent<DetectiveHealth>();
        animator = GetComponentInChildren<Animator>();
        animator.applyRootMotion = false;
    }

    void Update()
    {
        if (combat.Busy || health.Current == 0) return;

        float v = vertical.Read(Input.GetAxisRaw("Vertical"));
        float h = horizontal.Read(Input.GetAxisRaw("Horizontal"));
        bool running = v > 0f && run.Read(Input.GetKey(KeyCode.LeftShift) ? 1f : 0f) > 0f;

        transform.Rotate(0f, h * turnSpeed * Time.deltaTime, 0f);

        float speed = v > 0f ? (running ? runSpeed : walkSpeed) : backSpeed;
        verticalVelocity = controller.isGrounded ? -1f : verticalVelocity + gravity * Time.deltaTime;
        Vector3 motion = transform.forward * v * speed + Vector3.up * verticalVelocity;
        controller.Move(motion * Time.deltaTime);

        if (v > 0f) Play(running ? "Run" : "Walk");
        else if (v < 0f) Play("WalkBack");
        else if (h != 0f) Play("Walk");
        else Play("Idle");

        if (v == 0f && h == 0f) stepTimer = 0f; // o primeiro passo sai assim que começa a andar
        else if ((stepTimer -= Time.deltaTime) <= 0f)
        {
            stepTimer = running ? runStepInterval : walkStepInterval;
            Sfx.Play(footsteps, transform.position, 0.6f);
        }
    }

    public void Play(string state)
    {
        if (state == currentState) return;
        currentState = state;
        animator.CrossFade(state, 0.15f);
    }
}

// Acesso remoto (AnyDesk) entrega tecla segurada como "apertou/soltou" a cada repetição do teclado (~30 ms),
// e o Input Manager antigo passa isso adiante: o detetive picotava. Segura o último valor por um instante.
// ponytail: a pausa do atraso inicial de repetição (~0,5 s) ainda aparece via remoto; jogando local não muda nada.
public class HeldInput
{
    const float Grace = 0.1f;
    float value, lastSeen;

    public float Read(float raw)
    {
        if (raw != 0f) { value = raw; lastSeen = Time.unscaledTime; }
        else if (Time.unscaledTime - lastSeen > Grace) value = 0f;
        return value;
    }
}
