using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class CardObject : MonoBehaviour
{
    [SerializeField] MMFeedbacks cardFlipFeedback;
    float zRotation;
    public void Init()
    {
        PlayCardFlip();
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
}
