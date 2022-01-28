using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{
    [HideInInspector] public static PlayerAbilities instance;
    PlayerController playerController;

    [Header("Player ability related prefabs")]

    [SerializeField] GameObject teleportCardPrefab;
    [SerializeField] float teleportCardSpeed;

    [SerializeField] float dashSpeed;

    private void Awake()
    {
        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.instance;
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
        playerController.SetMoveHigh();
        //print("Hi");
        //playerController.SetMoveDirection += dashSpeed * Vector3.right * (playerController.IsFacingRight ? 1 : -1);
        //print(playerController.GetMoveDirection);
    }
}
