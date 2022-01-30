using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrow : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print("Hit and deal damage needed");
        Destroy(this.gameObject);
    }
}
