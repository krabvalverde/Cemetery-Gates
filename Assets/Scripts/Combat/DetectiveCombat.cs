using System.Collections;
using UnityEngine;

public class DetectiveCombat : MonoBehaviour
{
    [SerializeField] ItemData revolver;
    [SerializeField] ItemData ammo;
    [SerializeField] int cylinderSize = 6;
    [SerializeField] int shotDamage = 5;
    [SerializeField] float shotRange = 15f;
    [SerializeField] float lockAngle = 60f;
    [SerializeField] float shotCooldown = 0.6f;
    [SerializeField] float aimTurnSpeed = 150f;
    [SerializeField] int punchDamage = 2;
    [SerializeField] float punchRadius = 0.5f;
    [SerializeField] float punchHitDelay = 0.35f;
    [SerializeField] float punchDuration = 0.9f;
    [SerializeField] Transform upperArm;
    [SerializeField] Transform lowerArm;
    [SerializeField] Transform hand;
    [SerializeField] Transform revolverModel;
    [SerializeField] Transform muzzle;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Transform bulletPrefab;
    [SerializeField] float bulletSpeed = 60f;
    [SerializeField] float flashDuration = 0.05f;
    [SerializeField] float recoilAngle = 25f;
    [SerializeField] float recoilRecovery = 150f; // graus por segundo
    [SerializeField] AudioClip shotSound;
    [SerializeField] AudioClip dryFireSound;
    [SerializeField] AudioClip reloadSound;
    [SerializeField] AudioClip punchSound;
    [SerializeField] AudioClip punchHitSound;

    public int Loaded { get; private set; }
    public int CylinderSize => cylinderSize;
    public bool Busy => aiming || punching;

    Inventory inventory;
    DetectiveHealth health;
    DetectiveMovement movement;
    Enemy target;
    bool aiming;
    bool punching;
    float nextShot;
    float recoil;

    void Awake()
    {
        inventory = GetComponent<Inventory>();
        health = GetComponent<DetectiveHealth>();
        movement = GetComponent<DetectiveMovement>();
        Loaded = cylinderSize;
    }

    void Update()
    {
        if (InventoryUI.IsOpen || health.Current == 0 || punching) return;

        bool wasAiming = aiming;
        aiming = Input.GetMouseButton(1) && inventory.EquippedWeapon == revolver;
        if (aiming && !wasAiming) movement.Play("Idle");
        if (aiming) Aim();

        if (Input.GetKeyDown(KeyCode.R)) Reload();
        if (!Input.GetMouseButtonDown(0)) return;
        if (aiming) Shoot();
        else StartCoroutine(Punch());
    }

    void LateUpdate()
    {
        if (health.Current == 0) aiming = false;
        revolverModel.gameObject.SetActive(aiming);
        recoil = Mathf.MoveTowards(recoil, 0f, recoilRecovery * Time.deltaTime);
        if (!aiming) return;
        Vector3 direction = AimDirection(upperArm.position);
        // Recuo: o tiro joga o cano para cima, mais na mão que no ombro, e volta aos poucos.
        Vector3 Kick(float amount) => Quaternion.AngleAxis(-recoil * amount, transform.right) * direction;
        upperArm.rotation = Quaternion.FromToRotation(upperArm.up, Kick(0.3f)) * upperArm.rotation;
        lowerArm.rotation = Quaternion.FromToRotation(lowerArm.up, Kick(0.7f)) * lowerArm.rotation;
        hand.rotation = Quaternion.FromToRotation(hand.up, Kick(1f)) * hand.rotation;
        revolverModel.SetPositionAndRotation(hand.position, Quaternion.LookRotation(Kick(1f)));
    }

    Vector3 AimDirection(Vector3 from) => target ? (target.Center - from).normalized : transform.forward;

    void Aim()
    {
        target = FindTarget();
        if (!target)
        {
            transform.Rotate(0f, Input.GetAxisRaw("Horizontal") * aimTurnSpeed * Time.deltaTime, 0f);
            return;
        }
        Vector3 to = target.transform.position - transform.position;
        to.y = 0f;
        if (to.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to), 360f * Time.deltaTime);
    }

    Enemy FindTarget()
    {
        Enemy best = null;
        float bestDistance = shotRange;
        foreach (var enemy in FindObjectsByType<Enemy>(FindObjectsInactive.Exclude))
        {
            if (enemy.Dead) continue;
            Vector3 to = enemy.transform.position - transform.position;
            to.y = 0f;
            float distance = to.magnitude;
            if (distance < bestDistance && Vector3.Angle(transform.forward, to) <= lockAngle)
            {
                best = enemy;
                bestDistance = distance;
            }
        }
        return best;
    }

    void Shoot()
    {
        if (Time.time < nextShot) return;
        if (Loaded == 0)
        {
            Sfx.Play(dryFireSound, transform.position);
            InventoryUI.ShowMessage("Sem balas no tambor. R para recarregar.");
            return;
        }

        Loaded--;
        nextShot = Time.time + shotCooldown;
        recoil = recoilAngle;
        Sfx.Play(shotSound, muzzle.position, 1f, 0.05f);
        StartCoroutine(Flash());

        Vector3 origin = transform.position + Vector3.up * 1.4f;
        Vector3 direction = AimDirection(origin);
        Vector3 end = origin + direction * shotRange;
        Enemy enemy = null;
        if (Physics.Raycast(origin, direction, out var hit, shotRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            end = hit.point;
            enemy = hit.collider.GetComponentInParent<Enemy>();
        }
        StartCoroutine(FlyBullet(end, enemy));
    }

    IEnumerator Flash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        muzzleFlash.SetActive(false);
    }

    // O projétil sai do cano e o dano só acontece quando ele chega.
    IEnumerator FlyBullet(Vector3 end, Enemy enemy)
    {
        var bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.LookRotation(end - muzzle.position));
        while (bullet.position != end)
        {
            bullet.position = Vector3.MoveTowards(bullet.position, end, bulletSpeed * Time.deltaTime);
            yield return null;
        }
        if (enemy) enemy.TakeDamage(shotDamage);
        bullet.GetComponent<MeshRenderer>().enabled = false;
        Destroy(bullet.gameObject, 0.1f); // deixa o rastro sumir
    }

    void Reload()
    {
        if (inventory.EquippedWeapon != revolver) return;
        int amount = Mathf.Min(cylinderSize - Loaded, inventory.Count(ammo));
        if (amount == 0)
        {
            InventoryUI.ShowMessage(Loaded == cylinderSize ? "O tambor já está cheio." : "Sem munição.");
            return;
        }
        for (int i = 0; i < amount; i++) inventory.RemoveOne(ammo);
        Loaded += amount;
        Sfx.Play(reloadSound, transform.position, 0.8f, 0.03f);
        InventoryUI.ShowMessage($"Revólver recarregado ({Loaded}/{cylinderSize}).");
    }

    IEnumerator Punch()
    {
        punching = true;
        movement.Play("Punch");
        yield return new WaitForSeconds(punchHitDelay - 0.15f);
        Sfx.Play(punchSound, transform.position, 0.7f); // o "vush" começa um pouco antes do contato
        yield return new WaitForSeconds(0.15f);

        Vector3 center = transform.position + Vector3.up + transform.forward * 0.8f;
        bool hitSomething = false;
        foreach (var col in Physics.OverlapSphere(center, punchRadius, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            var enemy = col.GetComponentInParent<Enemy>();
            if (!enemy) continue;
            enemy.TakeDamage(punchDamage);
            hitSomething = true;
        }
        if (hitSomething) Sfx.Play(punchHitSound, center);

        yield return new WaitForSeconds(punchDuration - punchHitDelay);
        punching = false;
    }
}
