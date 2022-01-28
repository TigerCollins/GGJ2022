using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;

public class SideScrollCamera : MonoBehaviour
{
    [SerializeField] GameObject cameraObject;
    [SerializeField] Transform cameraTarget;
    [SerializeField] PlayerController playerController;

    [Header("Camera Movement Settings")]
    [Tooltip("This clamps the movable range from the original player")]
    [SerializeField] bool useCameraBounds;
    [SerializeField] Vector2 cameraXBounds;
    Vector3 originalPosition;

    [Space(10)]
    [Tooltip("The lower the number, the shorter the delay. 0 for instant follow.")]
    [SerializeField]float cameraDelay = .2f;
    [SerializeField] Vector3 cameraOffset = new Vector3(0,2.5f,-9f);
    Vector3 velocity = Vector3.zero;

    [Space(10)]
    [SerializeField] bool moveDirectionChangesXOffset;
    [SerializeField] float moveDirectionXOffset;

    [Header("Juice")]
    [SerializeField] MMFeedbacks screenShakeFeedback;

    [Space(10)]

    [SerializeField] float shakeTime;
    [SerializeField] float shakeStrength;
    [SerializeField] float shakeFrequency;
    [SerializeField] Vector3 amplitude;


    private void Start()
    {
        originalPosition = cameraTarget.transform.position;
    }

    void FixedUpdate()
    {
        CameraLogic();
    }

    void CameraLogic()
    {
        float effectiveXOffset = cameraOffset.x;
        if (moveDirectionChangesXOffset)
        {
            if(playerController.IsFacingRight)
            {
                effectiveXOffset = moveDirectionXOffset;
            }

            else
            {
                effectiveXOffset = -moveDirectionXOffset;
            }
        }

        Vector3 desiredPosition = new Vector3(cameraTarget.position.x + effectiveXOffset, cameraTarget.position.y + cameraOffset.y, cameraTarget.position.z + cameraOffset.z);
        if(useCameraBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, cameraXBounds.x, cameraXBounds.y);
        }

        cameraObject.transform.position = Vector3.SmoothDamp(cameraObject.transform.position, desiredPosition, ref velocity, cameraDelay);



    }

    public void ShakeCamera(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            ShakeCamera();
        }
    }

    void ShakeCamera()
    {
        if(screenShakeFeedback.TryGetComponent(out MMFeedbackCameraShake wiggle))
        {
            wiggle.CameraShakeProperties.Duration = shakeTime;
            wiggle.CameraShakeProperties.Amplitude = shakeStrength;
            wiggle.CameraShakeProperties.Frequency = shakeFrequency;
            wiggle.CameraShakeProperties.AmplitudeX = amplitude.x;
            wiggle.CameraShakeProperties.AmplitudeY = amplitude.y;
            wiggle.CameraShakeProperties.AmplitudeZ = amplitude.z;

            screenShakeFeedback.Initialization();
            screenShakeFeedback.PlayFeedbacks();
        }
    }

    void ShakeCameraOneTime(float time,float strength, Vector3 newAmplitude)
    {
        if (screenShakeFeedback.TryGetComponent(out MMFeedbackCameraShake wiggle))
        {
            wiggle.CameraShakeProperties.Duration = time;
            wiggle.CameraShakeProperties.Amplitude = strength;
            wiggle.CameraShakeProperties.Frequency = shakeFrequency;
            wiggle.CameraShakeProperties.AmplitudeX = newAmplitude.x;
            wiggle.CameraShakeProperties.AmplitudeY = newAmplitude.y;
            wiggle.CameraShakeProperties.AmplitudeZ = newAmplitude.z;
            screenShakeFeedback.Initialization();
            screenShakeFeedback.PlayFeedbacks();
        }
    }
}
