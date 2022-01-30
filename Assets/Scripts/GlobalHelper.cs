using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalHelper : MonoBehaviour
{
    public static GlobalHelper instance;
    float universalTimeScale = 1f;
    float playerTimeScale = 1f;
    private void Awake()
    {
        instance = this;
    }

    public enum Dimensions
    {
        dimensionA,
        dimensionB
    }

    public enum CardSide
    {
        aSide,
        bSide
    }

    public void ResetTimeScales()
    {
        universalTimeScale = 1f;
        playerTimeScale = 1f;
    }

    public float UniversalTimeScale
    {
        get
        {
            return universalTimeScale;
        }

        set
        {
            universalTimeScale = value;
        }
    }

    public float PlayerTimeScale
    {
        get
        {
            return playerTimeScale;
        }

        set
        {
            playerTimeScale = value;
        }
    }

}
