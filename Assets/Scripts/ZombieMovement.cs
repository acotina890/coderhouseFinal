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
    



    private enum TypeOfBehaviour
    {
        still,
        withMove
    }

    [SerializeField] private TypeOfBehaviour typeOfBehaviour;
    



    public Animator zAnim; // para setear las animaciones

    public float zSpeed = 10; // velocidad de movimiento del zombie

    public float countDown = 0; // temporizador para las animaciones
    private int caseBehaviour; // para enum Comportamiento
    private int rutine; // para el movimiento automatico
    private Quaternion angle; // para la rotacion
    private float grade; // para la rotacion
    private bool attacking; // para la animacion de ataque
    private Vector3 savedPosition; // para el respawn

    public Transform delJugador; // para cuando detecte al jugador / player
    public int caseNumber; //trabajar con funciones con base al switch
    public float distancia; //distancia del jugador




    void Start()
    {
        savedPosition = transform.position;
    }

    void Update()
    {
        distancia = Vector3.Distance(transform.position, delJugador.position);



        SetTypeOfZombie();
        SetTypeBehaviour();
        


        // zombi normal
        if(caseNumber == 0)
        {
            if(caseBehaviour == 0)
            {
                AtPlace();
            }
            if(caseBehaviour == 1)
            {
                WithMove();
            }
        }

        // mini boss
        if (caseNumber == 1)
        {
            if (caseBehaviour == 0)
            {
                AtPlace();
            }
            if (caseBehaviour == 1)
            {
                WithMove();
            }
        }

        // boss
        if (caseNumber == 2)
        {
            if (caseBehaviour == 0)
            {
                AtPlace();
            }
            if (caseBehaviour == 1)
            {
                WithMove();
            }
        }


        Respawn();
    }




    private void SetTypeOfZombie() // uso enum Zombi
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



    private void SetTypeBehaviour() // uso enum Comportamiento
    {
        switch(typeOfBehaviour)
        {
            case TypeOfBehaviour.still:
                caseBehaviour = 0;
                break;
            case TypeOfBehaviour.withMove:
                caseBehaviour = 1;
                break;
        }
    }




    private void AtPlace() // sin movimiento + deteccion del jugador
    {
        if(distancia > 30)
        {
            zAnim.SetBool("zWalk", false);
            zAnim.SetBool("zRun", false);
            zAnim.SetBool("zAttack", false);
        }
        else
        {
            FollowAndAttack();
        }
    }



    private void WithMove() // movimiento automatico + deteccion del jugador
    {
        if (distancia > 30)
        {
            AutoBehaviour();
        }
        else
        {
            FollowAndAttack();
        }
    }



    private void AutoBehaviour() // movimiento automatico
    {
        zAnim.SetBool("zRun", false);
        zAnim.SetBool("zAttack", false);

        countDown += 1 * Time.deltaTime;
        if (countDown >= 4)
        {
            rutine = Random.Range(0, 2);
            countDown = 0;
        }
        
        switch(rutine)
        {
            case 0:
                zAnim.SetBool("zWalk", false);
                break;
            case 1:
                grade = Random.Range(0, 360);
                angle = Quaternion.Euler(0, grade, 0);
                rutine++;
                break;
            case 2:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, 0.5f);
                transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                zAnim.SetBool("zWalk", true);
                break;
        }
    }



    private void FollowAndAttack() // deteccion del jugador
    {
        if (distancia > 1.4f)
        {
            var lookPos = delJugador.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);

            zAnim.SetBool("zWalk", false);
            zAnim.SetBool("zAttack", false);

            zAnim.SetBool("zRun", true);

            transform.Translate(Vector3.forward * zSpeed * Time.deltaTime);
        }
        else
        {
            zAnim.SetBool("zWalk", false);
            zAnim.SetBool("zRun", false);
            zAnim.SetBool("zAttack", true);
            attacking = true;
        }
    }




    private void Respawn() // en caso de que se caigan del mapa
    {
        if(transform.position.y < -10)
        {
            transform.position = savedPosition;
        }
    }



    public void FinishAnimation() // ���NO TOCAR���
    {
        zAnim.SetBool("zAttack", false);
        attacking = false;
    }
}
