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
        if (!aiming) return;
        Vector3 direction = AimDirection(upperArm.position);
        upperArm.rotation = Quaternion.FromToRotation(upperArm.up, direction) * upperArm.rotation;
        lowerArm.rotation = Quaternion.FromToRotation(lowerArm.up, direction) * lowerArm.rotation;
        hand.rotation = Quaternion.FromToRotation(hand.up, direction) * hand.rotation;
        revolverModel.SetPositionAndRotation(hand.position, Quaternion.LookRotation(direction));
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
            InventoryUI.ShowMessage("Sem balas no tambor. R para recarregar.");
            return;
        }

        Loaded--;
        nextShot = Time.time + shotCooldown;
        Vector3 origin = transform.position + Vector3.up * 1.4f;
        Vector3 direction = AimDirection(origin);
        if (!Physics.Raycast(origin, direction, out var hit, shotRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) return;
        var enemy = hit.collider.GetComponentInParent<Enemy>();
        if (enemy) enemy.TakeDamage(shotDamage);
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
        InventoryUI.ShowMessage($"Revólver recarregado ({Loaded}/{cylinderSize}).");
    }

    IEnumerator Punch()
    {
        punching = true;
        movement.Play("Punch");
        yield return new WaitForSeconds(punchHitDelay);

        Vector3 center = transform.position + Vector3.up + transform.forward * 0.8f;
        foreach (var col in Physics.OverlapSphere(center, punchRadius, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            var enemy = col.GetComponentInParent<Enemy>();
            if (enemy) enemy.TakeDamage(punchDamage);
        }

        yield return new WaitForSeconds(punchDuration - punchHitDelay);
        punching = false;
    }
}
