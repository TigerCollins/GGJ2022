using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbilities : MonoBehaviour
{
    public enum Ability
    {
        DimensionSwap,
        Teleport,
        Dash,
        TimeStop,
        HangmanVine,
        RockThrow,
        ProjectileAttack
    }

    public List<CardDetails> abilityCards;
}
