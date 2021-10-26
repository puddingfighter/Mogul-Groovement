using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] float jumpSpeed;
    private void OnTriggerEnter(Collider other)
    {
        other.transform.GetComponent<Player>().Jump(jumpSpeed,true);
    }
}
