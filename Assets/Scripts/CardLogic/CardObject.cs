using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;

public class CardObject : MonoBehaviour
{
    [SerializeField] MMFeedbacks cardFlipFeedback;
    float zRotation;

    [Header("Logic")]
    [SerializeField] TextMeshGroups sideAText;
    [SerializeField] TextMeshGroups sideBText;
    [SerializeField] Renderer thisCardRenderer;
    [SerializeField] CardDetails sideACardDetails;
    [SerializeField] CardDetails sideBCardDetails;
    public void Init()
    {
        PlayCardFlip();
        SetCardTextures();
        SetCardText();
    }

    void SetCardTextures()
    {
        if(thisCardRenderer != null)
        {
            thisCardRenderer.material.SetTexture("Back_Face_Texture", sideACardDetails.Art);
            thisCardRenderer.material.SetTexture("Card_Colour_Texture", sideBCardDetails.Art);
        }
      
    }

    void SetCardText()
    {
        sideAText.title.text = sideACardDetails.CardName;
        sideAText.body.text = sideACardDetails.UnlockedAbilityFlavour;
        sideBText.title.text = sideBCardDetails.CardName;
        sideBText.body.text = sideBCardDetails.UnlockedAbilityFlavour;
    }

    public void PlayCardFlip()
    {
        if(DimensionSwitcher.instance.CurrentDimension() == GlobalHelper.Dimensions.dimensionA)
        {

            if(cardFlipFeedback.TryGetComponent(out MMFeedbackRotation rotationFeedback))
            {
                rotationFeedback.DestinationAngles = new Vector3(90, 0, 180);   
            }
        }

        else
        {
            if (cardFlipFeedback.TryGetComponent(out MMFeedbackRotation rotationFeedback))
            {
                rotationFeedback.DestinationAngles = new Vector3(90, 0, 0);
            }
        }
        cardFlipFeedback.Initialization();
        cardFlipFeedback.PlayFeedbacks();
    }

    bool BothSidesLocked()
    {
        bool bothLocked = false;
        if(!sideACardDetails.IsUnlocked && !sideBCardDetails.IsUnlocked)
        {
            bothLocked = true;
        }

        return bothLocked;
    }

    public void ActivateAbility()
    {
        if(DimensionSwitcher.instance.CurrentDimension() == GlobalHelper.Dimensions.dimensionA )
        {
            if(sideACardDetails.IsUnlocked)
            {
                switch (sideACardDetails.CardAbility)
                {
                    case CardAbilities.Ability.DimensionSwap:
                        DimensionSwitcher.instance.onDimensionChange.Invoke();
                        break;
                    case CardAbilities.Ability.WorldFlip:
                        break;
                    case CardAbilities.Ability.Ability2:
                        break;
                    case CardAbilities.Ability.Ability3:
                        break;
                    case CardAbilities.Ability.Ability4:
                        break;
                    case CardAbilities.Ability.Ability5:
                        break;
                    case CardAbilities.Ability.Ability6:
                        break;
                    default:
                        break;
                }
            }
            
        }

        else
        {
            if (sideBCardDetails.IsUnlocked)
            {
                switch (sideBCardDetails.CardAbility)
                {
                    case CardAbilities.Ability.DimensionSwap:
                        DimensionSwitcher.instance.onDimensionChange.Invoke();
                        break;
                    case CardAbilities.Ability.WorldFlip:
                        break;
                    case CardAbilities.Ability.Ability2:
                        break;
                    case CardAbilities.Ability.Ability3:
                        break;
                    case CardAbilities.Ability.Ability4:
                        break;
                    case CardAbilities.Ability.Ability5:
                        break;
                    case CardAbilities.Ability.Ability6:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

[System.Serializable]
public class TextMeshGroups
{
    public TextMeshPro title;
    public TextMeshPro body;
}