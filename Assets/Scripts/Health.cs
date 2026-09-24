using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3; // Salud máxima del objeto

    public int Current { get; private set; } // Salud actual del objeto
    public int Max => maxHealth; // Salud máxima del objeto

    // (vida actual / vida máxima)
    public event Action<int, int> OnHealthChanged;
    public event Action OnDied;

    private void Awake()
    {
        Current = maxHealth; // Inicializa la salud actual al máximo
    }

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        if (Current <= 0) return; // Ya está muerto, no hacer nada

        Current = Mathf.Max(0, Current - damage); // Reduce la salud actual y asegura que no sea menor que 0
        Debug.Log($"{name} recibió {damage} de daño. Vida: {Current}/{maxHealth}");
        OnHealthChanged?.Invoke(Current, maxHealth); // Invoca el evento de cambio de salud

        if (Current == 0)
        {
            OnDied?.Invoke(); // Invoca el evento de muerte si la salud llega a 0
        }
    }
}