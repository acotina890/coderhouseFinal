using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Camera cam;
    public float sensitiviy;

    public Transform player;

    private void FixedUpdate(){
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        if(PlayerMovement.canWalk){
            if(h != 0){
                transform.Rotate(Vector3.up, h * 90 * sensitiviy * Time.deltaTime);
            }
            if(v != 0){
                cam.transform.RotateAround(transform.position, transform.right, -v * 90 * sensitiviy * Time.deltaTime);
            }
        }

        cam.transform.LookAt(player);
        Vector3 ea = cam.transform.rotation.eulerAngles;
        cam.transform.rotation = Quaternion.Euler(new Vector3(ea.x, ea.y, 0));
    }
}
