using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public Transform mainCam;

    public float damage = 0.9f;
    public float maxHealth = 100;
    public Image bloodImage;
    private float a;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public bool aiming;
    public float aimRotSens = 150;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();

    float cureAmount;
    public static bool Healed = false;
    private bool firstHealed = false;
    public bool canWalk;
    public Animator anim;

    public CharacterController chController;
    private float cameraVerticalAngle;
    Vector3 rotationInput = Vector3.zero;
    public float rotSens = 300;

    public float gravity = -9.8f;
    public float jumpHeight = 3;
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;

    public float speed = 4;
    public float x, y;

    void Start(){
        a = bloodImage.color.a;
        canWalk = false;
    }

    void Update(){
        a = damage;
        Mathf.Clamp(a,0,1);
        Color c = new Color(0.35f,0,0,a);
        bloodImage.color = c;
        if(Healed && !firstHealed){
            damage = 0;
            firstHealed = true;
        }
    }

    void FixedUpdate()
    {
        if(canWalk){
            Move();
            Shoot();
            Attack();
            Look();
        }
    }

    private void Move(){
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);
        if(isGrounded && velocity.y < 0){
            velocity.y = -2f;
            anim.SetBool("Grounded", true);
            anim.SetBool("Jump", false);
        }

        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x,0f,y).normalized;

        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);

        if(direction.magnitude >= 0.1f){
            /* float targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg + mainCam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle, ref turnSmoothVelocity,turnSmoothTime);
            Vector3 moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
            transform.rotation = Quaternion.Euler(0f,angle,0f); */

            /* chController.Move(moveDir.normalized*speed*Time.deltaTime); */
            direction = transform.TransformDirection(direction) * speed;
            chController.Move(direction * Time.deltaTime);
            anim.SetBool("Walking", true);

            //Correr
            if(Healed){
                if(Input.GetKey(KeyCode.LeftShift)){
                    anim.SetBool("Running", true);
                    speed = 12;
                }
                else{
                    anim.SetBool("Running", false);
                    speed = 6;
                }
            }
        }
        else{
            anim.SetBool("Walking", false);
        }

        if(Healed && Input.GetButton("Jump") && isGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            anim.SetBool("Jump", true);
            anim.SetBool("Grounded", false);
        }
        velocity.y += gravity * Time.deltaTime;
        chController.Move(velocity*Time.deltaTime);
    }

    private void Look(){
        rotationInput.x = Input.GetAxis("Mouse X") * rotSens * Time.deltaTime;
        rotationInput.y = Input.GetAxis("Mouse Y") * rotSens * Time.deltaTime;

        cameraVerticalAngle += rotationInput.y;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -65,65);

        transform.Rotate(Vector3.up * rotationInput.x);
        mainCam.transform.localRotation = Quaternion.Euler(-cameraVerticalAngle,0,0);
    }

/*     private void Aim(){
        if(Input.GetKey(KeyCode.Mouse1)){
            aiming = true;
            aimCam.gameObject.SetActive(true);
        }
        else{
            aiming = false;
            aimCam.gameObject.SetActive(false);
        }
    } */

    public void Shoot(){
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        Transform hitTransform = null;
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask)){
            hitTransform = raycastHit.transform;
        }
    }

    public void Attack(){
        if(Input.GetKey(KeyCode.Mouse0)){
            anim.SetTrigger("Attack");
        }
    }

    public void CanWalk(){
        canWalk = true;
    }

    public void NoWalk(){
        canWalk = false;
    }

    public void Interact(){
        anim.SetBool("Walking", false);
        anim.SetTrigger("Interact");
    }

    public void ActivateAxe(){
        anim.SetBool("Axe",true);
        anim.SetBool("Rifle",false);
    }

    public void ActivateRifle(){
        anim.SetBool("Rifle",true);
        anim.SetBool("Axe",false);
    }
}
