using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MossCharger : MonoBehaviour
{
    Rigidbody2D rb;
    DamageFlash damageFlash;
    Animator animator;
    TouchingDirections touchingDirections;
    Damageable damageable;
    SpriteRenderer spriteRenderer;
    PolygonCollider2D polygonCollider2D;

    [SerializeField] float chargeSpeed = 8f;
    [SerializeField] DetectionZone detectionZone;
    [SerializeField] Transform target;

    public bool _isCharging;
    public bool _isBurying;
    public bool _hasTarget;
    public bool _isSurfacing;

    private Vector2 OGpos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        damageFlash = GetComponent<DamageFlash>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        damageable = GetComponent<Damageable>();
        OGpos = gameObject.transform.position;
    }

    private void Update()
    {
        HasTarget = detectionZone.detectedColliders.Count > 0;
        if (HasTarget)
        {
            animator.SetTrigger(AnimationStrings.surfaceTrigger);
        }
    }

    private void FixedUpdate()
    {
        if (!damageable.LockVelocity)
        {
            if (touchingDirections.IsGrounded && CanMove)
            {
                rb.velocity = new Vector2 (chargeSpeed, rb.velocity.y);
                Debug.Log("Charging!");
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            else if (touchingDirections.IsGrounded && !CanMove && !isBurying)
            {
                Debug.Log("Resetting position!");
                gameObject.transform.position = OGpos;
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, 0.05f), rb.velocity.y);
                rb.bodyType = RigidbodyType2D.Kinematic;
            } else
            {
                Debug.Log("Stopped!");
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, 0.05f), rb.velocity.y);
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
        else
        {
            Debug.Log("Velocity is locked!");
        }
    }
    public bool isBurying
    {
        get
        {
            return _isBurying;
        }
        private set
        {
            _isBurying = value;
            animator.SetBool(AnimationStrings.isBurying, value);
        }
    }
    public void onCliffDetected()
    {
        damageable.LockVelocity = true;
        isBurying = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void onHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        damageFlash.CallDamageFlash();
    }

    public void onPlayerDetected()
    {
        damageable.LockVelocity = false;
        animator.SetTrigger(AnimationStrings.surfaceTrigger);
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }


}

