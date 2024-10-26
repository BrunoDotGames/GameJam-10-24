using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDisposable
{
    public CharacterController controller;

    private float speed;
    public float walkSpeed = 16f;
    public float runSpeed = 26f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    private bool canPlayerMove = true;

    // Event
    public Action<bool, float> damageHandler;

    void Update()

    {
        if (!canPlayerMove) return;
        // This should be in InputHandler.cs
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
    }

    public void SetCanPlayerMove(bool value)
    {
        canPlayerMove = value;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log($"Player Take Damage");
        Debug.Log($"Damage Taken : {damage}");
        damageHandler?.Invoke(true, damage);    
    }

    public void Dispose()
    {
        damageHandler = null;
    }
}
