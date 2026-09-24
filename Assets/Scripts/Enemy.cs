using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float attackCooldown = 1f;

    private Rigidbody2D rb;
    private Health health;
    private SpriteRenderer sr;
    private Transform target;
    private Color baseColor;
    private float nextAttackTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        sr = GetComponentInChildren<SpriteRenderer>();
        baseColor = sr.color;
    }

    private void Start()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null) target = player.transform;
    }

    private void OnEnable()
    {
        health.OnHealthChanged += HandleHealthChanged;
        health.OnDied += HandleDied;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= HandleHealthChanged;
        health.OnDied -= HandleDied;
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        // Perseguir al jugador y mirar hacia él
        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
        rb.rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    // Daño por contacto con cooldown para no dañar en cada frame
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time < nextAttackTime) return;
        if (collision.gameObject.GetComponent<PlayerController>() == null) return;

        if (collision.gameObject.TryGetComponent(out Health playerHealth))
        {
            playerHealth.TakeDamage(contactDamage);
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void HandleHealthChanged(int current, int max)
    {
        StopAllCoroutines();
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    { 
        sr.color = Color.red;
        yield return new WaitForSeconds(0.08f);
        sr.color = baseColor;
    }

    private void HandleDied()
    {
        Destroy(gameObject);
    }
}