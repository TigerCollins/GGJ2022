using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;


public class DimensionSwitcher : MonoBehaviour
{
    internal static DimensionSwitcher instance;

    [SerializeField] GlobalHelper.Dimensions currentDimension;
    GlobalHelper.Dimensions lastDimension;
    [SerializeField] DimensionEnvironmentHandler environmentHandler;
    [Space(10)]
    public UnityEvent onDimensionChange;


    private void Awake()
    {
        instance = this;
        onDimensionChange.AddListener(delegate {environmentHandler.ChangeEnvironmentDimension(); });
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

[System.Serializable]
public class DimensionEnvironmentHandler
{
    public List<DimensionObject> environmentObject;
    public int environmentObjectsActive;

    public void AddToEnvironmentList(DimensionObject newObject)
    {
        environmentObject.Add(newObject);
        environmentObjectsActive++;
    }

    public void RemoveFromEnvironmentList(DimensionObject objectToDelete)
    {
        int id = 0;
        foreach (DimensionObject item in environmentObject)
        {
            if(item == objectToDelete)
            {
                environmentObject.RemoveRange(id, 1);
            }
            id++;
        }
        environmentObjectsActive--;
    }

    public void ChangeEnvironmentDimension()
    {
        switch (DimensionSwitcher.instance.CurrentDimension())
        {
            case GlobalHelper.Dimensions.dimensionA:
                break;
            case GlobalHelper.Dimensions.dimensionB:
                break;
            default:
                break;
        }
    }
}
