using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject panelDie;
    public GameObject buttonRespawn;

    public float damage = 0.9f;
    public float maxHealth = 100;
    public Image bloodImage;
    private float a;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public bool aiming;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();

    float cureAmount;
    public static bool Healed = false;
    private bool firstHealed = false;
    public static bool canWalk;
    public Animator anim;

    public CharacterController chController;
    private float cameraVerticalAngle;

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
        Healed = false;
        firstHealed = false;
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

        if(damage >= 1){
            anim.SetTrigger("Die");
            panelDie.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        if(canWalk){
            Move();
            Shoot();
            Attack();
        }
    }

    // mensaje en caso de recibir da�o
    void OnTriggerEnter(Collider col)
    {
        if(col.transform.gameObject.tag == "zombi normal")
        {
            Debug.Log("Da�o normal");
            damage += 0.1f;
        }

        if(col.transform.gameObject.tag == "mini boss")
        {
            Debug.Log("Da�o pesado");
            damage += 0.3f;
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

    public void Die(){
        buttonRespawn.SetActive(true);
        Time.timeScale = 0;
    }

    public void Respawn(){
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
}
