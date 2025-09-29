using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerPogo : MonoBehaviour
{
    [SerializeField] private InputActionReference attackDirAction;
    public float bounceForce = 10f;
    private Rigidbody2D rb;
    private Vector2 lastAttackDir = Vector2.zero;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Bounce()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
    }

    public bool IsDoingDownAttack()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        bool pressingDown = vertical < -0.5f;
        bool falling = rb.velocity.y < -0.1f;

        return pressingDown && falling;
    }
}
