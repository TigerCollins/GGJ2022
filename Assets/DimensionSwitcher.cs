using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DimensionSwitcher : MonoBehaviour
{
    internal static DimensionSwitcher instance;

    [SerializeField] GlobalHelper.Dimensions currentDimension;
    GlobalHelper.Dimensions lastDimension;
    [Space(10)]
    public UnityEvent onDimensionChange;


    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTickDimensionCheck();
    }

    //THIS IS ONLY FOR DEBUG. NEEDS TO BE REMOVED
    void UpdateTickDimensionCheck()
    {
        
        if (currentDimension != lastDimension)
        {
            lastDimension = currentDimension;
            onDimensionChange.Invoke();
        }
    }

    public GlobalHelper.Dimensions CurrentDimension()
    {
        return currentDimension;
    }
}
