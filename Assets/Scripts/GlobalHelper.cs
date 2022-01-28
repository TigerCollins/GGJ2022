using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalHelper : MonoBehaviour
{
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

    public enum CardSide
    {
        aSide,
        bSide
    }
}
