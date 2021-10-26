using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    CharacterController controller;
    public Vector3 moveDirection = Vector3.zero;
    [SerializeField] float gravity = 25.0f;
    [SerializeField] GameObject shadow;
    [SerializeField] Camera mainCam;
    [SerializeField] CameraMan lakitu;

    [SerializeField] private InputAction inputVertical;

    [SerializeField] private InputAction inputHorizontal;

    [SerializeField] private InputAction inputJump;

    [SerializeField] private InputAction inputSprint;
    [SerializeField] float drag=0;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClip;


    Vector3 axis;
    float t = 0.0f;
    float axisMagnitude;
    [SerializeField] float speed;
    [SerializeField] float sprintSpeed;
    float currentSpeed;
    Animator anim;

    [SerializeField] float jumpSpeed;
    bool isSliding = false;
    bool ateShit = false;
    bool wasSpring = false;
    Vector3 lastHeight;
    bool sprinting = false;
    bool Tumble = false;
    bool wasBumped;

    bool wasGrounded = false;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        inputHorizontal.Enable();
        inputJump.Enable();

        inputVertical.Enable();
        inputSprint.Enable();

    }

    // Update is called once per frame




    void Update()
    {

        //    Debug.Log(up.ReadValue<float>());

        if (Tumble)
        {
            anim.Play("Bumped");
            Bumping();
        }
        else
        {


            if (axisMagnitude > 0)
                t += 2f * Time.deltaTime;
            else
                t = 0;

            anim.SetBool("Sprinting", sprinting);


            if (new Vector3(moveDirection.x, 0, moveDirection.z).magnitude > 0 && (controller.isGrounded || isSliding))
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z)), Time.deltaTime * 10f);
            }
            controller.Move(moveDirection * Time.deltaTime);

            if (controller.isGrounded)
            {
                wasGrounded = true;
                moveDirection.y = -0.5f;
                lakitu.ignoreY = false;
                lastHeight = transform.position;
                wasSpring = false;
                if (inputSprint.ReadValue<float>() > 0.1f)
                {
                    sprinting = true;

                }
                else
                    sprinting = false;
            }
            else
            {
                if (!wasSpring)
                {
                    if (lastHeight.y < transform.position.y)
                        lakitu.ignoreY = true;
                    else
                        lakitu.ignoreY = false;
                }

                if (wasGrounded)
                {
                    wasGrounded = false;
                    moveDirection.y = 0;
                }
            }
            Shadower();
            Movement();
            Jump(jumpSpeed, false);



        }
    }
    void Movement()
    {
        float axisH = 0;
        float axisV = 0;
        axisH = inputHorizontal.ReadValue<float>();
        axisV = inputVertical.ReadValue<float>();
        anim.SetBool("isSliding", isSliding);
        if (!isSliding)
        {
            axisMagnitude = new Vector3(axisH, axisV, 0).magnitude;
            anim.SetFloat("axisMagnitude", axisMagnitude);
            anim.SetBool("IsGrounded", controller.isGrounded);
            axis = new Vector3(axisH, 0, axisV);
            axis = Vector3.Normalize(axis);


            currentSpeed = Mathf.Lerp(0, speed * axisMagnitude, t);

            RaycastHit hit;
            if (!sprinting)
                moveDirection = new Vector3(axis.x * currentSpeed, moveDirection.y - (gravity * Time.deltaTime), axis.z * currentSpeed);
            else
            {
                moveDirection = new Vector3(axis.x * sprintSpeed, moveDirection.y - (gravity * Time.deltaTime), axis.z * sprintSpeed);
            }
            moveDirection = Quaternion.AngleAxis(mainCam.transform.eulerAngles.y, Vector3.up) * moveDirection;
        }
        else
        {
            moveDirection.y = -40;
        }
        if (!controller.isGrounded && isSliding)
        {
            isSliding = false;
            moveDirection.y = 3;
        }





    }
    public void Jump(float jumpSpeed, bool spring)
    {

        if (spring)
        {
            Debug.Log("YEYEYEYEYE");
            lastHeight = transform.position;
            lakitu.ignoreY = false;
            wasGrounded = false;
            isSliding = false;
            wasSpring = true;


            if (Random.Range(0, 10) < 5)
                anim.Play("JumpingAscend");
            else
                anim.Play("Jump2");
            //transform.rotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));

            moveDirection.y = jumpSpeed;
            audioSource.clip=audioClip[0];
            audioSource.Play();
        }
        else
        if ((inputJump.triggered && inputJump.ReadValue<float>() == 1) && controller.isGrounded)
        {
            sprinting = false;
            wasGrounded = false;
            wasSpring = false;
            if (Random.Range(0, 10) < 5)
                anim.Play("JumpingAscend");
            else
                anim.Play("Jump2");
            //transform.rotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));

            moveDirection = new Vector3(0, jumpSpeed, 0);
            audioSource.clip=audioClip[0];
            audioSource.Play();

        }
    }
    void Shadower()
    {
        Vector3 down = transform.TransformDirection(Vector3.down);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10))
        {

            //Debug.DrawRay(hit.point,hit.normal,Color.green);
            shadow.transform.position = new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        RaycastHit hitter;
        if (hit.transform.tag == "Bumpers")
        {
            Bumped(hit.normal,hit.transform.GetComponent<Bumper>().bumpPower);
            wasBumped=true;
        }
        else
        {
            if(!wasBumped)
                Tumble=false;
            if (Physics.Raycast(transform.position, Vector3.down, out hitter, 0.15f))
            {
                if (controller.isGrounded)
                {
                    isSliding = false;
                    if (hitter.normal != Vector3.up)
                    {
                        moveDirection.y = -60000;
                    }
                }


            }
            else if (controller.isGrounded)
            {
                isSliding = true;
                anim.Play("Sliding");
                moveDirection = new Vector3(hit.normal.x * 10, -17f, hit.normal.z * 10);
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(hit.normal.x,-1f,hit.normal.z)), Time.deltaTime * 10f);

            }
        }
    }
    
    
    public void Bumped(Vector3 angle, float power)
    {
        Tumble = true;
        audioSource.clip=audioClip[1];
        audioSource.Play();
        if(angle.y<0.1f)
        {
            angle= new Vector3(angle.x, 1, angle.z);
        }
        Debug.Log("BAM POWER WAS" + power + " angle was " + angle);
        moveDirection= angle*power*Time.deltaTime;
    }
    void Bumping()
    {
        moveDirection = new Vector3(moveDirection.x-drag*Time.deltaTime,moveDirection.y-gravity*2*Time.deltaTime, moveDirection.z-drag*Time.deltaTime);
        controller.Move(moveDirection*Time.deltaTime);
        transform.rotation= Quaternion.LookRotation(moveDirection);
        transform.Rotate(Vector3.right,90);
        wasBumped=false;
    }


}
