using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;

    private Rigidbody2D rb;
    private Camera cam;
    private Vector2 moveInput;
    private Vector2 aimPoint;

    // Awake es llamado cuando la instancia del script se carga, antes de Start.
    // Se utiliza para inicializar variables y referencias a componentes.
    private void Awake()
    {
        // Inicializo el Rigidbody2D y la cámara principal
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    // El Update es llamado una vez por frame y se utiliza para manejar la entrada del jugador.
    private void Update()
    {
        // Lectura de input en Update
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        aimPoint = cam.ScreenToWorldPoint(Input.mousePosition); // ScreenToWorldPoint convierte la posición del mouse de la pantalla a coordenadas del mundo
    }

    // FixedUpdate es llamado cada frame fijo y se utiliza para manejar la física del juego.
    private void FixedUpdate()
    {
        // Movimiento normalizado para que la diagonal no sea más rápida que el movimiento horizontal o vertical
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        // Apuntado hacia el mouse
        Vector2 dir = aimPoint - rb.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // Calcula el ángulo en grados y ajusta para que la parte superior del sprite apunte hacia el mouse
        rb.rotation = angle; // Rota el Rigidbody2D para que apunte hacia el mouse
    }
}