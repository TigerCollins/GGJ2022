using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class DimensionSwitcher : MonoBehaviour
{
    internal static DimensionSwitcher instance;

    [SerializeField] bool debugMode;
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
        
        if (currentDimension != lastDimension && debugMode)
        {
            lastDimension = currentDimension;
            onDimensionChange.Invoke();
        }
    }

    public GlobalHelper.Dimensions CurrentDimension()
    {
        return currentDimension;
    }

    public void ChangeDimensionViaInput(InputAction.CallbackContext input)
    {
        if(input.performed)
        {
            if (currentDimension != GlobalHelper.Dimensions.dimensionA)
            {
                currentDimension = GlobalHelper.Dimensions.dimensionA;
            }

            else
            {
                currentDimension = GlobalHelper.Dimensions.dimensionB;
            }
            onDimensionChange.Invoke();
        }
       
    }

}

[System.Serializable]
public class DimensionEnvironmentHandler
{
    public List<DimensionObject> environmentDimensionObjects;
    public List<GameObject> dimensionAObjects;
    public List<GameObject> dimensionBObjects;
    public int environmentObjectsActive;

    public void AddToEnvironmentList(DimensionObject newObject)
    {
        environmentObjects.Add(newObject);
        environmentObjectsActive++;
    }

    public void RemoveFromEnvironmentList(DimensionObject objectToDelete)
    {
        int id = 0;
        foreach (DimensionObject item in environmentObjects)
        {
            if(item == objectToDelete)
            {
                environmentObjects.RemoveRange(id, 1);
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
                foreach (GameObject item in dimensionAObjects)
                {
                    item.SetActive(false);
                }

                foreach (GameObject item in dimensionBObjects)
                {
                    item.SetActive(true);
                }
                break;
            case GlobalHelper.Dimensions.dimensionB:
                foreach (GameObject item in dimensionAObjects)
                {
                    item.SetActive(true);
                }

                foreach (GameObject item in dimensionBObjects)
                {
                    item.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
}
