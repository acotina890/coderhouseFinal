using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool Healed = false;
    public bool canWalk;
    public Animator anim;
    public Rigidbody rb;
    Vector3 rotationtnput = Vector3.zero;
    public float rotSens = 10;
    public float speed = 4;

    void Start(){
        canWalk = false;
    }

    void FixedUpdate()
    {
        if(canWalk){
            Look();
            Move();
            InteractHealing();
        }
    }

    private void Look(){
        rotationtnput.x = Input.GetAxis("Mouse X") * rotSens * Time.deltaTime;
        transform.Rotate(Vector3.up * rotationtnput.x);
    }

    private void Move(){
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

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
            }
        }
        else{
            rb.velocity = Vector3.zero;
            anim.SetBool("Walking", false);
        }
    }

    private void InteractHealing(){
        if(Input.GetKey("t")){
            Healed = true;
            anim.SetBool("Walking", false);
            anim.SetTrigger("PickUpHealing");
            speed = 6;
        }
    }

    public void CanWalk(){
        canWalk = true;
    }

    public void NoWalk(){
        canWalk = false;
    }
}
