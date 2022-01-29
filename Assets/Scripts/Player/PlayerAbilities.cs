using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{
    [HideInInspector] public static PlayerAbilities instance;
    GlobalHelper globalHelper;
    PlayerController playerController;
    PlayerAnimation playerAnimation;

    [Header("Teleport Variables")]

    [SerializeField] GameObject teleportCardPrefab;
    [SerializeField] float teleportCardSpeed;

    [Header("Dash Variables")]

    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;

    [Header("Time Freeze Variables")]

    [SerializeField] float freezeDuration;
    [Tooltip("This is how much the freeze effects the player. 1 is real time and 0 is frozen completely")]
    [Range(1f, 0f)] [SerializeField] float playerSlowRate;
    [Tooltip("This is how much the freeze effects the World. 1 is real time and 0 is frozen completely")]
    [Range(1f, 0f)] [SerializeField] float worldSlowRate;
    Rigidbody[] rigidbodies;


    [Header("Events")]
    public UnityEvent onTimeStop;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.instance;
        globalHelper = GlobalHelper.instance;
        playerAnimation = PlayerAnimation.instance;
        rigidbodies = FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
    }

    public void ThrowTeleportCard()
    {
        print("throwing Teleport Card");
        GameObject obj = Instantiate(teleportCardPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.Euler(Vector3.zero));
        if (obj.TryGetComponent(out Rigidbody rb))
        {
            if (playerController.IsFacingRight)
                rb.velocity = new Vector3(teleportCardSpeed, 3, 0);
            else
                rb.velocity = new Vector3(-teleportCardSpeed, 3, 0);
        }
    }

    public void DashMovement()
    {
        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        playerController.pauseMove = true;
        while (Time.time < startTime + dashTime)
        {
            if (playerController.IsFacingRight)
            {
                playerController.SetMoveDirection = new Vector3(1 * dashSpeed, 0, 0);
            }
            else
            {
                playerController.SetMoveDirection = new Vector3(-1 * dashSpeed, 0, 0);
            }
            yield return null;
        }
        playerController.pauseMove = false;
    }

    public void FreezeTimeAbility()
    {

        StartCoroutine(FreezeTime());
    }

    IEnumerator FreezeTime()
    {
        // freeze and slow down everything
        globalHelper.UniversalTimeScale = worldSlowRate;
        globalHelper.PlayerTimeScale = playerSlowRate;
        playerAnimation.GetPlayerAnimator.speed = playerSlowRate;
        playerAnimation.GetCardAnimator.speed = playerSlowRate;
        ChangeRigidBodiesDrag(10f);
        onTimeStop.Invoke();

        yield return new WaitForSecondsRealtime(freezeDuration);

        // reset the changes 
        playerAnimation.GetPlayerAnimator.speed = 1;
        playerAnimation.GetCardAnimator.speed = 1;
        globalHelper.ResetTimeScales();
        ChangeRigidBodiesDrag(-10);
        onTimeStop.Invoke();
    }

    private void ChangeRigidBodiesDrag(float amountToChange)
    {
        foreach (Rigidbody rb in rigidbodies )
        {
            rb.drag += amountToChange;
        }
    }

}
