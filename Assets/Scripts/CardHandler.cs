using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardHandler : MonoBehaviour
{
    [SerializeField] List<CardObject> cards;
    [SerializeField] DimensionSwitcher dimensionSwitcher;

    private void Awake()
    {
        dimensionSwitcher.onDimensionChange.AddListener(delegate { DimensionChanged(); });

        foreach (CardObject item in cards)
        {
            item.Init();
        }
    }

    void DimensionChanged()
    {
        foreach (CardObject item in cards)
        {
            item.PlayCardFlip();
        }
    }
}
