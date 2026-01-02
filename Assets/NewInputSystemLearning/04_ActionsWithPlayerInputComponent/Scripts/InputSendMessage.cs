using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSendMessage : MonoBehaviour
{
    private Vector2 direction;
    [SerializeField] private Rigidbody2D rb;
    private float moveSpeed = 5f;

    private void FixedUpdate()
    {
        rb.linearVelocity = direction * moveSpeed;
    }

    public void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    private void OnAttack()
    {
        Debug.Log("Attack triggered!");
    }
}
