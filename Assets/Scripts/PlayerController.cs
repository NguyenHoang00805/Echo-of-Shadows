using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    Vector2 moveInput;

    public float airWalkSpeed = 4f;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float dashingCooldown = 1f;

    private bool _canDash = true;
    private bool _isDashing;
    private float _timeSinceGrounded;
    private bool _coyoteUsable;
    private bool _canVarJump = true;


    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isRunning = false;
    [SerializeField] private float coyoteTimer = 0.1f;
    //[SerializeField] private float maxFallSpeed = -50f;
    //[SerializeField] private float fallAcceleration = 6f;
    [SerializeField] private float jumpImpulse = 10f;
    //[SerializeField] private float fallMultiplier = 2.5f;
    //[SerializeField] float lowJumpMultiplier = 2f;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float landingGravity = 2.5f;
    [SerializeField] private float jumpLandingGravity = 1.0f;
    [SerializeField] private float varJumpForce = 0.5f;

    [SerializeField] public TrailRenderer trailRenderer;
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference attackDirAction;
    [SerializeField] private GameObject slashEffect;
    [SerializeField] private Transform slashHolder;


    [HideInInspector] bool wasGroundedLastFrame;

    public bool _isFacingRight = true;
    public UIManager UIManager;
    public GameOverScreen gameOverScreen;
    TouchingDirections touchingDirections;
    Damageable damageable;
    private PlayerInput playerInput;
    private Squish Squish;
    DamageFlash damageFlash;
    private InputAction jumpAction;
    private float normalGravity;
    private Animator slashAnimator;


    public float currentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        //Air move
                        return airWalkSpeed;
                    }
                }
                else
                {
                    return 0;   //idle speed is 0
                }
            } else
            {
                //Movement locked
                return 0;
            }

        }
    }


    public bool IsMoving
    {
        get { return _isMoving; }
        set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }


    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = true;
        Squish = GetComponent<Squish>();
        damageFlash = GetComponent<DamageFlash>();
        jumpAction = playerInput.actions["Jump"];
        normalGravity = rb.gravityScale;
        slashAnimator = slashEffect.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            return;
        }
        if (touchingDirections.IsGrounded && !wasGroundedLastFrame)
        {
            if (Squish != null)
            {
                Squish.ApplySquish();
                Debug.Log("Squish applied");
            }
        }
        if (!damageable.LockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * currentMoveSpeed, rb.velocity.y);
        }
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        HandleGravity();
        if (touchingDirections.IsGrounded && IsAlive)
        {
            _timeSinceGrounded = Time.time;
            _coyoteUsable = true;
            _canVarJump = true;
        }
        wasGroundedLastFrame = touchingDirections.IsGrounded;
    }

    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }

    }

    public bool isFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }
    public bool CanMove { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            isFacingRight = false;
        }
    }

    public void onRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    //Check if player can use coyote jump
    private bool CanCoyoteJump()
    {
        return _coyoteUsable && !touchingDirections.IsGrounded && Time.time <  _timeSinceGrounded + coyoteTimer;
    }

    public void onJump(InputAction.CallbackContext context)
    {
        //Normal jump when grounded
        if (context.started && touchingDirections.IsGrounded && CanMove && IsAlive || CanCoyoteJump())
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            _coyoteUsable = false;
        }
        //Variable jump when cancel early
        if (context.canceled && rb.velocity.y > 0f && !touchingDirections.IsGrounded && CanMove && IsAlive)        
        {
            animator.ResetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * varJumpForce);
            _coyoteUsable = false;
        }
        //Lands faster if not holding jump button
        //if(context.canceled && rb.velocity.y < 0f && !touchingDirections.IsGrounded)
        //{
        //    animator.ResetTrigger(AnimationStrings.jumpTrigger);
        //    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 1.5f);
        //}
    }


    private void HandleGravity()
    {
        float OGGravity = normalGravity;
        if (!touchingDirections.IsGrounded && rb.velocity.y < 0f && !jumpAction.IsPressed())
        {
            rb.gravityScale = landingGravity;
        }
        else if (!touchingDirections.IsGrounded && rb.velocity.y < 0f && jumpAction.IsPressed())
        {
            rb.gravityScale = jumpLandingGravity;
        }
        else if (touchingDirections.IsGrounded)
        {
            rb.gravityScale = OGGravity;
        }
    }

    private void OnEnable()
    {
        attackAction.action.Enable();
        attackDirAction.action.Enable();

        attackAction.action.performed += onAttack;
        attackDirAction.action.performed += onAttack;
    }

    private void OnDisable()
    {
        attackAction.action.performed -= onAttack;
        attackDirAction.action.performed -= onAttack;

        attackAction.action.Disable();
        attackDirAction.action.Disable();
    }

    public void onAttack(InputAction.CallbackContext context)
    {
        if (context.started && UIManager != null && !UIManager._isPaused)
        {
            if (UIManager.getMenuCliked())
            {
                //Do nothing, consuming the click if in menu
            }
            else
            {
                float dir = attackDirAction.action.ReadValue<float>();

                if (dir > 0.5f) // W pressed
                {
                    animator.SetTrigger(AnimationStrings.upAttackTrigger);
                }
                else if (dir < -0.5f && !touchingDirections.IsGrounded) // S pressed
                {
                    animator.SetTrigger(AnimationStrings.downAttackTrigger);
                }
                else
                {
                    animator.SetTrigger(AnimationStrings.attackTrigger);
                }
            }
        }
    }

    public void PlaySlash(string direction)
    {
        //if (slashAnimator == null) return;

        // Reset rotation
        slashHolder.localRotation = Quaternion.identity;

        switch (direction)
        {
            case "Up":
                slashHolder.localRotation = Quaternion.Euler(0, 0, 90);
                break;
            case "Down":
                slashHolder.localRotation = Quaternion.Euler(0, 0, -90);
                break;
            default: // Side
                slashHolder.localRotation = Quaternion.identity;
                break;
        }
        slashEffect.SetActive(true);
        slashAnimator.Play("slash",0 );
    }

    public void HideSlash()
    {
        slashEffect.SetActive(false);
    }

    public void onHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        damageFlash.CallDamageFlash();
    }

    public void onBowAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.bowAttackTrigger);
        }
    }

    public void onDeath(int delay, Vector2 startPos)
    {
        if (!IsAlive && damageable.Health <= 0)
        {
            gameOverScreen.Setup();
            StartCoroutine(waitUntilDeathAnim(0.6f));
        }
    }

    private IEnumerator waitUntilDeathAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0f;
    }

    private IEnumerator Dash()
    {
        animator.SetTrigger(AnimationStrings.isDashing);
        _canDash = false;
        _isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        rb.gravityScale = originalGravity;
        _isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        _canDash = true;
    }

    public void onDash(InputAction.CallbackContext context)
    {
        if(context.started && IsAlive && CanMove && _canDash)
        {
            StartCoroutine(Dash());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FinishLine"))
        {
            playerInput.enabled = false;
        }
    }
}
