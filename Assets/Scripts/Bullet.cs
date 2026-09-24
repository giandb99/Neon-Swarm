using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 12f; // Velocidad de la bala
    [SerializeField] private float lifetime = 2f; // Tiempo de vida de la bala
    [SerializeField] private int damage = 1; // Daño que inflige la bala

    private IObjectPool<Bullet> pool; // Pool de objetos para la bala
    private float timer; // Temporizador para controlar el tiempo de vida de la bala

    public int Damage => damage; // Propiedad para obtener el daño de la bala

    // Lo llama el PlayerShooter al crear la bala
    public void SetPool(IObjectPool<Bullet> bulletPool)
    {
        pool = bulletPool;
    }

    private void OnEnable()
    {
        timer = lifetime; // Reinicia el temporizador cuando la bala se activa
    }

    private void Update()
    {
        // "Adelante" es transform.right (el Sprite mira a la derecha)
        transform.position += transform.right * speed * Time.deltaTime; // Mueve la bala hacia adelante

        timer -= Time.deltaTime; // Decrementa el temporizador
        if (timer <= 0f) Release(); // Libera la bala al pool si el tiempo de vida se ha agotado
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Solo dañamos objetos que sean enemigos y tengan vida
        if (other.GetComponent<Enemy>() != null && other.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
            Release();
        }
    }

    public void Release()
    {
        if (pool != null)
        {
            pool.Release(this); // Libera la bala al pool
        }
        else
        {
            Destroy(gameObject); // Destruye la bala si no hay un pool asignado
        }
    }
}