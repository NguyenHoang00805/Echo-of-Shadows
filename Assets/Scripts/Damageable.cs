using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    DamageFlash DamageFlash;
    Animator animator;
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent<int, Vector2> damageableDeath;
    public UnityEvent<int,int> healthChanged;

    [SerializeField]
    private int _maxHealth = 100;
    public Vector2 startPos;
    public GameObject slashEffectPrefab;

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }
    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get
        {
            return _health;
        } set
        {
            _health = value;
            healthChanged?.Invoke(_health,MaxHealth);
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }
    [SerializeField]
    private bool _isAlive = true;

    [SerializeField]
    private bool isInvincible = false;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set"+ value);

            if(value == false)
            {
                damageableDeath.Invoke(1,startPos);
            }
        }
    }
    // Velovity shounldnt be changed but needs to be replaced by other physics components like
    // the player controller
    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        DamageFlash = GetComponent<DamageFlash>();
    }

    private void Start()
    {
        startPos = transform.position;
    }
    public void Update()
    {
        if ( isInvincible)
        {
            if(timeSinceHit > invincibilityTime)
            {
                //Invincibility
                isInvincible = false ;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
            //DamageFlash.CallDamageFlash();
            return true;
        }
        return false;
    }

    public void PlaySlashEffect()
    {
        if (slashEffectPrefab != null)
        {
            Instantiate(slashEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    //public bool Death (int delay)
    //{
    //    if(!IsAlive && Health <= 0)
    //    {
    //        damageableDeath?.Invoke(1, startPos);
    //        StartCoroutine(Respawn(delay));
    //        return true;
    //    } return false;
    //}

    //private IEnumerator Respawn(int delay)
    //{
    //    animator.SetBool(AnimationStrings.isRespawned,true);
    //    IsAlive = true;
    //    Health = MaxHealth;
    //    yield return new WaitForSeconds(delay);
    //    animator.SetBool(AnimationStrings.isRespawned,false);
    //}

    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }
        return false ;
    }
}
