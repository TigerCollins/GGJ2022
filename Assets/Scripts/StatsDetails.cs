using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Scriptable Objects/NPC & Player Stats", order = 1)]
public class StatsDetails : ScriptableObject
{
    [SerializeField] int maxHealth;
    [SerializeField] int baseHealth;

    [Space(10)]

    [SerializeField] int baseAttack;
    [SerializeField] int baseDefence;
    [SerializeField] float knockbackPower;

    [Space(10)]

    [SerializeField] int maxLives;
    [SerializeField] int baseLives;
  

    public int Health
    {
        get
        {
            return baseHealth;
        }
    }
     public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    public int Defence
    {
        get
        {
            return baseDefence;
        }
    }

    public int Lives
    {
        get
        {
            return baseLives;
        }
    }

    public int MaxLives
    {
        get
        {
            return maxLives;
        }
    }

    public int Attack
    {
        get
        {
            return baseAttack;
        }
    }

    public float KnockBackStrength

    {
        get
        {
            return knockbackPower;
        }
    }


}
