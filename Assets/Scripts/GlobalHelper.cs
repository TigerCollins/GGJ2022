using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalHelper : MonoBehaviour
{
    public Color normalColour;
    public Color overGrowthColor;

    internal static GlobalHelper instance;
    private void Awake()
    {
        instance = this;
    }

    public enum Dimensions
    {
        dimensionA,
        dimensionB
    }

    public bool IsPaused
    {
        get
        {
            bool value = false;
            if(UIManager.instance != null)
            {
                value = UIManager.instance.IsPaused;
                AudioManager.instance.IsSoundtrackLoweredVolume(value);
            }
            return value;
        }
    }

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
}
