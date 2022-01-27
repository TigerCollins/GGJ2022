using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatsController : MonoBehaviour
{

    [SerializeField] StatsDetails statsProfile;

    [Header("Health")]
    [SerializeField] int maxHealth;
    [SerializeField] int health;

    [Space (10)]

    [SerializeField] int defence;

    [Space(10)]

    [SerializeField] int maxLivesRemaining;
    [SerializeField] int livesRemaining;

    [Header("Combat")]
    [SerializeField] int attack;

    [Header("On Death")]
    [SerializeField] bool isAlive = true;

    [Space(10)]

    [SerializeField] bool destroyOnDeath = true;
    [SerializeField] float destroyTimer = 1.5f;

    [Header("Events")]
    public UnityEvent onHealthGained;
    public UnityEvent onHealthLost;
    public UnityEvent onLifeLost;
    public UnityEvent onDeath;

    


    private void Start()
    {
        if(statsProfile != null)
        {
            health = statsProfile.Health;
            defence = statsProfile.Defence;
            livesRemaining = statsProfile.Lives;
            attack = statsProfile.Attack;
            maxHealth = statsProfile.MaxHealth;
            maxLivesRemaining = statsProfile.MaxLives;
        }
    }

    public void DealDamageToOther(StatsController otherStatsController)
    {
        otherStatsController.ReceiveDamage(attack);

    }

    public void ReceiveDamage(int damage)
    {
        int effectiveDamage = damage - defence;
        CurrentHealth = CurrentHealth - effectiveDamage;
    }


    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
    }

    public int Defence
    {
        get
        {
            return defence;
        }
    }

    public int CurrentHealth
    {
        set
        {
            //Event Logic
            if (value > health)
            {
                onHealthGained.Invoke();
            }

            else if(value < health)
            {
                Debug.Log(transform.name + " lost health");
                onHealthLost.Invoke();
            }

            //New health Value
            health = Mathf.Clamp(value,0,maxHealth);

            //What to do when no health 
            if(health <= 0)
            {
                if (LivesRemaining > 0)
                {
                    health = statsProfile.Health;
                    onLifeLost.Invoke();
                }
                LivesRemaining = LivesRemaining - 1;

            }
        }

        get
        {
            return health;
        }
    }

    public int LivesRemaining
    {
        set
        {

            if (livesRemaining <= 0 && isAlive == true)
            {

                DeathCountdown();
                onDeath.Invoke();
                isAlive = false;

            }


            livesRemaining = Mathf.Clamp(value, 0, maxLivesRemaining);

          
        }

        get
        {
            return livesRemaining;
        }
    }

    void DeathCountdown()
    {
        if(destroyOnDeath)
        {
            Destroy(gameObject, destroyTimer);
        }
      
    }
}
