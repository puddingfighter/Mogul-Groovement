using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdJumper : MonoBehaviour
{
    [SerializeField] Transform[] Crowds;
    [SerializeField] bool[] crowdJumpers;
    [SerializeField] Vector3[] originalPos;
    float timeSinceJump;
    bool haveJumped=false;
    float speed= 2f;
    // Start is called before the first frame update
    void Start()
    {
        for(int idx=0; idx<Crowds.Length;idx++)
        {
            originalPos[idx]=Crowds[idx].position;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(timeSinceJump>0.5f)
            haveJumped=false;
        if(haveJumped)
        {
            if(timeSinceJump<0.25f)
            {
                for(int idx=0; idx<Crowds.Length;idx++)
                {
                    if(crowdJumpers[idx])
                    {
                        Crowds[idx].position= new Vector3(Crowds[idx].position.x,Crowds[idx].position.y+speed*Time.deltaTime, Crowds[idx].position.z);
                    }
                }
            }
            else
            {
                for(int idx=0; idx<Crowds.Length;idx++)
                {
                    if(crowdJumpers[idx])
                    {
                        Crowds[idx].position= new Vector3(Crowds[idx].position.x,Crowds[idx].position.y-speed*Time.deltaTime, Crowds[idx].position.z);
                    }
                }
            }
        }

        if(!haveJumped)
        {
            for(int idx=0; idx<Crowds.Length;idx++)
            {
                if(Random.Range(0,10)<5)
                    crowdJumpers[idx]=true;
                else
                {
                    crowdJumpers[idx]=false;
                    Crowds[idx].position = originalPos[idx];
                }
            }
            haveJumped=true;
            timeSinceJump=0;
        }

        timeSinceJump+=Time.deltaTime;
    }
}
