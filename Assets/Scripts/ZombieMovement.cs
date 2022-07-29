using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    private enum TypeOfZombie{
        basic,
        miniboss,
        boss
    }

    [SerializeField] private TypeOfZombie typeOfZombie;

    public Animator zAnim; // para setear las animaciones
    public float zSpeed = 0.1f; // velocidad de movimiento del zombie
    public Transform delJugador; // para cuando detecte al jugador / player
    public int caseNumber; //trabajar con funciones con base al switch
    public float distancia; //distancia del jugador

    void FixedUpdate()
    {
        distancia = Vector3.Distance(transform.position, delJugador.position);
        if(caseNumber == 0){
            basicZombieBehaviour();
        }
    }

    void basicZombieBehaviour(){
        if(distancia < 25){
            transform.position = Vector3.Lerp(transform.position, delJugador.position, zSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, delJugador.rotation, 3*Time.deltaTime);
            zAnim.SetTrigger("Chase");
            zAnim.SetBool("NearPlayer",true);
        }else{
            zAnim.SetBool("NearPlayer",false);
        }
        
        if(distancia < 2){
            zAnim.SetTrigger("Attack");
        }
    }

    private void SetTypeOfZombie(TypeOfZombie typeOfZombie)
    {
        switch (typeOfZombie)
        {
            case TypeOfZombie.basic:
                caseNumber = 0;
                break;
            case TypeOfZombie.miniboss:
                caseNumber = 1;
                break;
            case TypeOfZombie.boss:
                caseNumber = 2;
                break;
        }
    }
}
