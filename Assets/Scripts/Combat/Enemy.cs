using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth = 10;
    [SerializeField] int hitsPerAttack = 1;
    [SerializeField] float speed = 1.2f;
    [SerializeField] float sightRange = 10f;
    [SerializeField] float attackRange = 1.3f;
    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField] float gravity = -20f;

    public int Health { get; private set; }
    public Vector3 Center => controller.bounds.center;

    CharacterController controller;
    Animator animator;
    DetectiveHealth player;
    float nextAttack;
    float verticalVelocity;
    string currentState = "Idle";

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        animator.applyRootMotion = false;
        Health = maxHealth;
    }

    void Start()
    {
        player = FindAnyObjectByType<DetectiveHealth>();
    }

    void Update()
    {
        verticalVelocity = controller.isGrounded ? -1f : verticalVelocity + gravity * Time.deltaTime;
        Vector3 motion = Vector3.up * verticalVelocity;
        string state = "Idle";
        bool attackNow = false;

        if (player && player.Current > 0)
        {
            Vector3 to = player.transform.position - transform.position;
            to.y = 0f;
            float distance = to.magnitude;
            if (distance <= sightRange && distance > 0.01f) transform.rotation = Quaternion.LookRotation(to);

            if (distance <= attackRange)
            {
                state = "Attack";
                if (Time.time >= nextAttack)
                {
                    nextAttack = Time.time + attackCooldown;
                    player.TakeHit(hitsPerAttack);
                    attackNow = true;
                }
            }
            else if (distance <= sightRange)
            {
                motion += to / distance * speed;
                state = "Walk";
            }
        }

        controller.Move(motion * Time.deltaTime);
        Play(state);
        if (attackNow && currentState == "Attack") animator.Play("Attack", 0, 0f);
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0) Destroy(gameObject);
    }

    void Play(string state)
    {
        if (!animator.HasState(0, Animator.StringToHash(state))) state = "Idle";
        if (state == currentState) return;
        currentState = state;
        animator.CrossFade(state, 0.15f);
    }
}
