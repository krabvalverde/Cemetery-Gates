using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DetectiveMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 1.6f;
    [SerializeField] float runSpeed = 3.8f;
    [SerializeField] float backSpeed = 1.1f;
    [SerializeField] float turnSpeed = 150f;
    [SerializeField] float gravity = -20f;

    CharacterController controller;
    Animator animator;
    float verticalVelocity;
    string currentState = "Idle";

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        animator.applyRootMotion = false;
    }

    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        bool running = v > 0f && Input.GetKey(KeyCode.LeftShift);

        transform.Rotate(0f, h * turnSpeed * Time.deltaTime, 0f);

        float speed = v > 0f ? (running ? runSpeed : walkSpeed) : backSpeed;
        verticalVelocity = controller.isGrounded ? -1f : verticalVelocity + gravity * Time.deltaTime;
        Vector3 motion = transform.forward * v * speed + Vector3.up * verticalVelocity;
        controller.Move(motion * Time.deltaTime);

        if (v > 0f) Play(running ? "Run" : "Walk");
        else if (v < 0f) Play("WalkBack");
        else if (h != 0f) Play("Walk");
        else Play("Idle");
    }

    void Play(string state)
    {
        if (state == currentState) return;
        currentState = state;
        animator.CrossFade(state, 0.15f);
    }
}
