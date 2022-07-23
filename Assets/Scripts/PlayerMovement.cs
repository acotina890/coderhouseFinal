using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimCam;
    public bool aiming;
    public float aimRotSens = 150;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();

    float cureAmount;
    public static bool Healed = false;
    public bool canWalk;
    public Animator anim;
    public Rigidbody rb;
    public float jumpForce = 100;
    public bool canJump = false;

    Vector3 rotationtnput = Vector3.zero;
    public float rotSens = 300;
    public float speed = 4;

    void Start(){
        canWalk = false;
    }

    void FixedUpdate()
    {
        if(canWalk){
            Look();
            Move();
            Aim();
            Shoot();
        }
    }

    private void Look(){
        if(!aiming){
            rotationtnput.x = Input.GetAxis("Mouse X") * rotSens * Time.deltaTime;
        }
        else{
            rotationtnput.x = Input.GetAxis("Mouse X") * aimRotSens * Time.deltaTime;
        }
        
        transform.Rotate(Vector3.up * rotationtnput.x);
    }

    private void Move(){
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        anim.SetFloat("VelX", hor);
        anim.SetFloat("VelY", ver);

        if(hor != 0 || ver != 0){
            Vector3 direction = (transform.forward * ver + transform.right * hor).normalized;
            rb.velocity = direction * speed;
            anim.SetBool("Walking", true);
            if(Healed){
                if(Input.GetKey(KeyCode.LeftShift)){
                    anim.SetBool("Running", true);
                    speed = 12;
                }
                else{
                    anim.SetBool("Running", false);
                    speed = 6;
                }

                //Se implementará el salto más adelante
                /* Vector3 floor = transform.TransformDirection(Vector3.down);
                if(Physics.Raycast(transform.position, floor, 6)){
                    canJump = true;
                }
                else{
                    canJump = false;
                }

                if(canJump){
                    if(Input.GetKeyDown(KeyCode.Space)){
                        anim.SetBool("Jump", true);
                        rb.AddForce(new Vector3(0,jumpForce,0),ForceMode.Impulse);
                    }
                    anim.SetBool("Grounded", true);
                }
                else{
                    Falling();
                } */
            }
        }
        else{
            rb.velocity = Vector3.zero;
            anim.SetBool("Walking", false);
        }

        if(Healed && Input.GetKey(KeyCode.Space)){
                anim.SetTrigger("Jump");
        }
    }

    private void Aim(){
        if(Input.GetKey(KeyCode.Mouse1)){
            aiming = true;
            aimCam.gameObject.SetActive(true);
        }
        else{
            aiming = false;
            aimCam.gameObject.SetActive(false);
        }
    }

    /* public void Falling(){
        anim.SetBool("Grounded", false);
        anim.SetBool("Jump", false);
    } */

    public void Shoot(){
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        Transform hitTransform = null;
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask)){
            hitTransform = raycastHit.transform;
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
}
