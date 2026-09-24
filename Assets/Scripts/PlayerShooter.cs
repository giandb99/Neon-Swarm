using UnityEngine;
using UnityEngine.Pool;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab; // Prefab de la bala
    [SerializeField] private Transform firePoint; // Punto de disparo
    [SerializeField] private float fireRate = 6f; // Tasa de disparo por segundo

    private IObjectPool<Bullet> pool; // Pool de objetos para las balas
    private float nextFireTime; // Siguiente tiempo de disparo

    private void Awake()
    {
        // Inicializar el pool de balas
        pool = new ObjectPool<Bullet>(                                      // Crear un nuevo pool de balas
            createFunc: CreateBullet,                                       // Función para crear una nueva bala
            actionOnGet: bullet => bullet.gameObject.SetActive(true),       // Acción al obtener una bala del pool
            actionOnRelease: bullet => bullet.gameObject.SetActive(false),  // Acción al liberar una bala al pool
            actionOnDestroy: bullet => Destroy(bullet.gameObject),          // Acción al destruir una bala
            collectionCheck: false,                                         // Desactivar la verificación de colección para mejorar el rendimiento
            defaultCapacity: 20,                                            // Capacidad inicial del pool
            maxSize: 100                                                    // Tamaño máximo del pool
        );
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab); // Instanciar una nueva bala
        bullet.SetPool(pool); // Asignar el pool a la bala
        return bullet; // Devolver la bala creada
    }

    private void Update()
    {
        // Mantener el click izquierdo para disparar de forma continua
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate; // Calcular el siguiente tiempo de disparo
            Shoot(); // Llamar a la función de disparo
        }
    }

    private void Shoot()
    {
        Bullet bullet = pool.Get(); // Obtener una bala del pool
        bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation); // Establecer la posición y rotación de la bala
    }
}