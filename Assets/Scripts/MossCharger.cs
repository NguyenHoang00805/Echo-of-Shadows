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

    [SerializeField] DetectionZone detectionZone;
    [SerializeField] GameObject AttackZone;

    public bool _isCharging;

    public Vector3 startPos;
    public float chargeSpeed = 5f;
    public float waitBeforeDive = 0.5f;

    private bool _isActive = false;
    private bool hasSurfaced = false;

    public bool IsCharging
    {
        get
        {
            return _isCharging;
        }
        private set
        {
            _isCharging = value;
            animator.SetBool(AnimationStrings.isCharging, value);
        }
    }
    public bool IsActive //set spriteRenderer + AttackZone = value
    {
        get
        {
            return _isActive;
        }
        private set
        {
            _isActive = value;
            animator.SetBool(AnimationStrings.isActive, value);
            spriteRenderer.enabled = value;
            AttackZone.SetActive(value);
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        damageFlash = GetComponent<DamageFlash>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        damageable = GetComponent<Damageable>();
        startPos = transform.position;
        IsActive = false;
    }

    private void Update()
    {
        if (detectionZone.detectedColliders.Count > 0 && !hasSurfaced)
        {
            animator.SetTrigger(AnimationStrings.surfaceTrigger);
            IsCharging = true;
            IsActive = true;
            hasSurfaced = true;
        }

        if (detectionZone.detectedColliders.Count == 0)
        {
            hasSurfaced = false;
        }
    }

    private void FixedUpdate()
    {
        if (IsCharging)
        {
            rb.velocity = new Vector2(chargeSpeed * transform.localScale.x, rb.velocity.y);
        }
    }

    public void onCliffDetected()
    {
        animator.SetTrigger(AnimationStrings.buryTrigger);
    }

    public void onHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        damageFlash.CallDamageFlash();
    }

    public void onPlayerDetected()
    {
        animator.SetTrigger(AnimationStrings.surfaceTrigger);
        IsCharging = true;
        IsActive = true;
        //animator.SetBool(AnimationStrings.isCharging, true);
    }
    public void onDiveStart()
    {
        IsCharging = false;
        //animator.SetBool(AnimationStrings.isCharging, false);
        rb.velocity = Vector2.zero;
    }
    public void onDiveEnd()
    {
        transform.position = startPos;
        IsActive = false;
    }
    public void onDeath()
    {
        IsCharging = false;
        rb.velocity = Vector2.zero;
        AttackZone.SetActive(false);
    }
}

