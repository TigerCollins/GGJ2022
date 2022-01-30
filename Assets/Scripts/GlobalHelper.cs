
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalHelper : MonoBehaviour
{
    public Color normalColour;
    public Color overGrowthColor;

    public static GlobalHelper instance;
    float universalTimeScale = 1f;
    float playerTimeScale = 1f;

    public void DimensionColour(Dimensions newDimension)
    {
        switch (newDimension)
        {
            case Dimensions.dimensionA:
                RenderSettings.fogColor = normalColour;
                break;
            case Dimensions.dimensionB:
                RenderSettings.fogColor = overGrowthColor;
                break;
            default:
                break;
        }
    }

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

    public bool IsPaused
    {
        get
        {
            bool value = false;
            if (UIManager.instance != null)
            {
                value = UIManager.instance.IsPaused;
                AudioManager.instance.IsSoundtrackLoweredVolume(value);
            }
            return value;
        }
    }

}
