using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    
    public float bumpPower;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HIT???");
        collision.contacts[0].otherCollider.GetComponent<Player>().Bumped(collision.contacts[0].normal,bumpPower);
    }
}
