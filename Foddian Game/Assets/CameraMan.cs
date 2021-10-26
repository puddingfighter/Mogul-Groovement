using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraMan : MonoBehaviour
{
    
    public GameObject followingObj;
    public bool ignoreY=false;
    [SerializeField] float camSpeed = 11;
    public bool airborne=false;
    [SerializeField] private InputAction inputRightVertical;
    
    
    [SerializeField] private InputAction inputRightHorizontal;
    
    // Update is called once per frame
    void Start()
    {
        inputRightVertical.Enable();
        inputRightHorizontal.Enable();
    }
    void LateUpdate()
    {
        this.transform.eulerAngles= new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,0);
        float mouseY = Input.GetAxis("RightV");
        float mouseX = Input.GetAxis("RightH");
        if(mouseY==0)
            mouseY= inputRightVertical.ReadValue<float>();
        if(mouseX==0)
            mouseX= inputRightHorizontal.ReadValue<float>()/2;
        transform.RotateAround(transform.position, Vector3.up, mouseX * 300*Time.deltaTime);


        if (!ignoreY)
        {
            if(!airborne)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, followingObj.transform.position, camSpeed * Time.deltaTime);
            }
            else
            {
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(followingObj.transform.position.x,followingObj.transform.position.y-0.8f,
                followingObj.transform.position.z), camSpeed * Time.deltaTime);
            
            }

        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(followingObj.transform.position.x,this.transform.position.y,
                followingObj.transform.position.z), camSpeed * Time.deltaTime);
        }


            if (mouseY > 0.4f || mouseY < -0.4f)
            {
                transform.Rotate(new Vector3(mouseY*Time.deltaTime*100, 0, 0));
                if (transform.eulerAngles.x >= 30 && transform.eulerAngles.x <= 300)
                    transform.eulerAngles = new Vector3(30, transform.eulerAngles.y, transform.eulerAngles.z);
                else if (transform.eulerAngles.x <= 340 && transform.eulerAngles.x >= 310)
                    transform.eulerAngles = new Vector3(340, transform.eulerAngles.y, transform.eulerAngles.z);
            }
    }
}
