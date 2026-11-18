using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth = 10;
    [SerializeField] int hitsPerAttack = 1;
    [SerializeField] float speed = 1.2f;
    [SerializeField] float wanderSpeed = 0.5f;
    [SerializeField] float sightRange = 10f;
    [SerializeField] float fieldOfView = 120f;
    [SerializeField] float hearRange = 3f;
    [SerializeField] float loseRange = 18f;
    [SerializeField] float wanderRadius = 4f;
    [SerializeField] float attackRange = 1.3f;
    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField] float attackHitDelay = 0.45f; // momento do golpe na animação Attack
    [SerializeField] float turnSpeed = 360f;
    [SerializeField] float gravity = -20f;
    [SerializeField] AudioClip[] groanSounds;
    [SerializeField] Vector2 groanInterval = new Vector2(5f, 12f);
    [SerializeField] AudioClip alertSound;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip deathSound;
    [Header("Chefe")]
    [SerializeField] string[] attackStates = { "Attack" }; // sorteia um a cada golpe
    [SerializeField] float wakeRange = 0f;    // > 0: fica enterrado até o detetive chegar perto (estados Underground → Spawn → Scream)
    [SerializeField] float wakeDuration = 2.4f; // Spawn + Scream; invulnerável até terminar
    [SerializeField] float attackCommitTime = 0f; // > 0: o golpe vai até o fim, parado e sem virar, por esse tempo (golpe + recuperação) — a janela do jogador
    [SerializeField] float hitAngle = 360f;       // cone do golpe (graus): fora dele, o golpe erra

    public int Health { get; private set; }
    public bool Emerged => !dormant && Time.time >= awakeAt; // já surgiu: pode ser alvo e levar dano
    public Vector3 Center => controller.bounds.center;

    // ponytail: NavMesh gerado em runtime ao abrir a cena (sem pacote AI Navigation);
    // trocar por NavMeshSurface baked no editor se o tempo de carga crescer.
    // Gerado de novo ao trocar de cena (cemitério → catacumbas), senão o caminho seria o da cena anterior.
    static NavMeshDataInstance navMesh;
    static UnityEngine.SceneManagement.Scene navMeshScene;

    CharacterController controller;
    Animator animator;
    DetectiveHealth player;
    NavMeshPath path;
    Vector3 home, destination;
    bool chasing, hasPath;
    int corner;
    float nextAttack, hitAt, nextRepath, nextWander, nextGroan, verticalVelocity, moveSpeed, awakeAt, committedUntil;
    bool dormant;
    string currentState = "Idle";

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        animator.applyRootMotion = false;
        Health = maxHealth;
        path = new NavMeshPath();
        home = destination = transform.position;
    }

    void Start()
    {
        player = FindAnyObjectByType<DetectiveHealth>();
        BakeNavMeshOnce();
        nextWander = Time.time + Random.Range(0f, 4f);
        nextGroan = Time.time + Random.Range(1f, groanInterval.y);
        dormant = wakeRange > 0f;
        if (dormant) animator.Play(currentState = "Underground");
    }

    void Update()
    {
        if (dormant)
        {
            if (!player || player.Current == 0 || Flat(player.transform.position - transform.position).magnitude > wakeRange) return;
            dormant = false;
            awakeAt = Time.time + wakeDuration;
            animator.CrossFade(currentState = "Spawn", 0.1f);
            Sfx.Play(alertSound, transform.position);
            return;
        }
        if (Time.time < awakeAt) return; // surgindo e gritando
        if (currentState == "Spawn") { chasing = true; nextRepath = 0f; }
        verticalVelocity = controller.isGrounded ? -1f : verticalVelocity + gravity * Time.deltaTime;
        Vector3 motion = Vector3.up * verticalVelocity;
        string state = "Idle";
        bool attackNow = false;

        if (player && player.Current > 0) UpdateAwareness();
        else chasing = false;

        if (chasing)
        {
            Vector3 to = Flat(player.transform.position - transform.position);
            if (Time.time < committedUntil)
            {
                state = "Attack"; // golpe em andamento: não anda nem gira, mesmo que o detetive se afaste
            }
            else if (to.magnitude <= attackRange)
            {
                if (to.sqrMagnitude > 0.0001f) transform.rotation = Quaternion.LookRotation(to);
                state = "Attack";
                if (Time.time >= nextAttack)
                {
                    nextAttack = Time.time + attackCooldown;
                    hitAt = Time.time + attackHitDelay;
                    committedUntil = Time.time + attackCommitTime;
                    attackNow = true;
                    Sfx.Play(attackSound, transform.position);
                }
            }
            else
            {
                motion += Steer(player.transform.position, speed);
                moveSpeed = speed;
                state = "Walk";
            }
        }
        else
        {
            // Sem alvo: perambula perto de onde nasceu (e volta para lá depois de perder o detetive).
            if (Time.time >= nextWander)
            {
                nextWander = Time.time + Random.Range(4f, 8f);
                Vector2 r = Random.insideUnitCircle * wanderRadius;
                destination = home + new Vector3(r.x, 0f, r.y);
                nextRepath = 0f;
            }
            if (Flat(destination - transform.position).magnitude > 0.3f)
            {
                motion += Steer(destination, wanderSpeed);
                moveSpeed = wanderSpeed;
                state = "Walk";
            }
        }

        // O dano sai no meio do golpe, se o detetive ainda estiver ao alcance e na frente — senão o golpe erra.
        if (hitAt > 0f && Time.time >= hitAt)
        {
            hitAt = 0f;
            Vector3 to = player ? Flat(player.transform.position - transform.position) : Vector3.zero;
            if (player && player.Current > 0 && to.magnitude <= attackRange + 0.3f && Vector3.Angle(transform.forward, to) <= hitAngle * 0.5f)
                player.TakeHit(hitsPerAttack);
        }

        if (Time.time >= nextGroan && !attackNow)
        {
            nextGroan = Time.time + Random.Range(groanInterval.x, groanInterval.y);
            Sfx.Play(groanSounds, transform.position, 0.8f, 0.12f);
        }

        controller.Move(motion * Time.deltaTime);
        Play(state);
        animator.speed = currentState == "Walk" ? moveSpeed / speed : 1f; // passos acompanham a velocidade
        if (attackNow && currentState == "Attack") animator.Play(attackStates[Random.Range(0, attackStates.Length)], 0, 0f);
    }

    public bool Dead => Health <= 0;

    public void TakeDamage(int amount)
    {
        if (Dead || !Emerged) return;
        chasing = true;
        Health -= amount;
        Sfx.Play(Dead ? deathSound : hurtSound, transform.position);
        if (!Dead) return;

        // Morre: toca Death e o corpo fica no chão, sem IA e sem colisão (tiros e passagem não esbarram nele).
        if (!animator.HasState(0, Animator.StringToHash("Death")))
        {
            Destroy(gameObject);
            return;
        }
        controller.enabled = false;
        enabled = false;
        animator.speed = 1f;
        animator.CrossFade("Death", 0.1f);
    }

    void UpdateAwareness()
    {
        Vector3 to = Flat(player.transform.position - transform.position);
        float distance = to.magnitude;
        if (chasing)
        {
            if (distance > loseRange) chasing = false;
            return;
        }
        chasing = distance <= hearRange
            || (distance <= sightRange && Vector3.Angle(transform.forward, to) <= fieldOfView * 0.5f && CanSeePlayer());
        if (!chasing) return;
        nextRepath = 0f;
        nextGroan = Time.time + groanInterval.x; // o rosnado de alerta substitui o próximo gemido
        Sfx.Play(alertSound, transform.position);
    }

    bool CanSeePlayer()
    {
        Vector3 eye = transform.position + Vector3.up * controller.height * 0.9f;
        Vector3 chest = player.transform.position + Vector3.up * 1.2f;
        return !Physics.Linecast(eye, chest, out var hit, ~0, QueryTriggerInteraction.Ignore)
            || hit.collider.GetComponentInParent<DetectiveHealth>() == player;
    }

    Vector3 Steer(Vector3 target, float moveSpeed)
    {
        if (Time.time >= nextRepath)
        {
            nextRepath = Time.time + 0.3f;
            hasPath = NavMesh.SamplePosition(transform.position, out var from, 1.5f, NavMesh.AllAreas)
                && NavMesh.SamplePosition(target, out var to, 2f, NavMesh.AllAreas)
                && NavMesh.CalculatePath(from.position, to.position, NavMesh.AllAreas, path)
                && path.corners.Length > 1;
            corner = 1;
        }

        Vector3 next = target;
        if (hasPath)
        {
            var corners = path.corners;
            while (corner < corners.Length - 1 && Flat(corners[corner] - transform.position).sqrMagnitude < 0.04f) corner++;
            next = corners[corner];
        }

        Vector3 dir = Flat(next - transform.position);
        if (dir.sqrMagnitude < 0.0001f) return Vector3.zero;
        dir.Normalize();
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
        return dir * moveSpeed;
    }

    void BakeNavMeshOnce()
    {
        if (navMesh.valid && navMeshScene == gameObject.scene) return;
        if (navMesh.valid) navMesh.Remove();
        navMeshScene = gameObject.scene;

        var area = new Bounds(player ? player.transform.position : transform.position, Vector3.zero);
        foreach (var enemy in FindObjectsByType<Enemy>(FindObjectsInactive.Exclude)) area.Encapsulate(enemy.transform.position);
        area.Expand(new Vector3(40f, 20f, 40f));

        var sources = new List<NavMeshBuildSource>();
        NavMeshBuilder.CollectSources(area, ~0, NavMeshCollectGeometry.PhysicsColliders, 0, new List<NavMeshBuildMarkup>(), sources);
        sources.RemoveAll(s => s.component is Collider c && (c.isTrigger || c is CharacterController));

        var settings = NavMesh.GetSettingsByID(0);
        settings.agentRadius = controller.radius;
        settings.agentClimb = controller.stepOffset;
        settings.overrideVoxelSize = true;
        settings.voxelSize = 0.1f;
        var data = NavMeshBuilder.BuildNavMeshData(settings, sources, area, Vector3.zero, Quaternion.identity);
        navMesh = NavMesh.AddNavMeshData(data);
    }

    static Vector3 Flat(Vector3 v) { v.y = 0f; return v; }

    void Play(string state)
    {
        string clip = state == "Attack" ? attackStates[0] : state; // ataque pode ter variações (Slash01, Stab…)
        if (!animator.HasState(0, Animator.StringToHash(clip))) state = clip = "Idle";
        if (state == currentState) return;
        currentState = state;
        animator.CrossFade(clip, 0.15f);
    }
}
