using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool canWalk;
    public Animator anim;
    public Rigidbody rb;
    Vector3 rotationtnput = Vector3.zero;
    public float rotSens = 10;
    public float speed;

    void Start(){
        canWalk = false;
    }

    void Update()
    {
        if(canWalk){
            Look();
            Move();
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
        }
        else{
            rb.velocity = Vector3.zero;
            anim.SetBool("Walking", false);
        }
    }

    public void CanWalk(){
        canWalk = true;
    }
}
