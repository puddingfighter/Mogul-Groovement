using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] float rotatingSpeed;
    [SerializeField] Vector3 RotatingAxis;
    

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(RotatingAxis.x *rotatingSpeed*Time.deltaTime 
        ,RotatingAxis.y*rotatingSpeed*Time.deltaTime, RotatingAxis.z*rotatingSpeed*Time.deltaTime),Space.World);  
    }
}
