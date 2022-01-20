using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionObject : MonoBehaviour
{
    [SerializeField] DimensionVariables dimensionVariables;


    public void Init()
    {

    }
}

[System.Serializable]
public class DimensionVariables
{
    public List<GameObject> dimensionAObjects;
    public List<GameObject> dimensionBObjects;
}
