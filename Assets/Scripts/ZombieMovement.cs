using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    #region definicion de enum Zombie
    private enum TypeOfZombie{
        basic,
        miniboss,
        boss
    }

    [SerializeField] private TypeOfZombie typeOfZombie;
    #endregion

    #region definicion enum Comportamiento
    private enum TypeOfBehaviour
    {
        still,
        withMove
    }

    [SerializeField] private TypeOfBehaviour typeOfBehaviour;
    #endregion

    #region Variables a usar

    public Animator zAnim; // para setear las animaciones

    // public float zSpeed = 0.1f; // velocidad de movimiento del zombie

    public float countDown = 0; // temporizador para las animaciones

    private int caseBehaviour; // para enum Comportamiento
    private int rutine; // para el movimiento automatico
    private Quaternion angle; // para la rotacion
    private float grade; // para la rotacion
    private bool attacking;

    public Transform delJugador; // para cuando detecte al jugador / player
    public int caseNumber; //trabajar con funciones con base al switch
    public float distancia; //distancia del jugador

    #endregion

    void Update()
    {
        distancia = Vector3.Distance(transform.position, delJugador.position);

        #region seteamos los tipos de zombis y comportamientos
        SetTypeOfZombie();
        SetTypeBehaviour();
        #endregion

        #region casos de zombis con sus casos de comportamientos

        // zombi normal
        if(caseNumber == 0)
        {
            if(caseBehaviour == 0)
            {
                AtPlacePlusPersecution();
            }
            if(caseBehaviour == 1)
            {
                WithMovePlusPersecution();
            }
        }

        // mini boss
        if (caseNumber == 1)
        {
            if (caseBehaviour == 0)
            {
                AtPlacePlusPersecution();
            }
            if (caseBehaviour == 1)
            {
                WithMovePlusPersecution();
            }
        }

        // boss
        if (caseNumber == 2)
        {
            if (caseBehaviour == 0)
            {
                AtPlacePlusPersecution();
            }
            if (caseBehaviour == 1)
            {
                WithMovePlusPersecution();
            }
        }

        #endregion
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

    private void AtPlacePlusPersecution() // sin movimiento + deteccion del jugador
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

    private void WithMovePlusPersecution() // movimiento automatico + deteccion del jugador
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

    private void AutoBehaviour()
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
        if (distancia > 1.4)
        {
            var lookPos = delJugador.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);

            zAnim.SetBool("zWalk", false);
            zAnim.SetBool("zAttack", false);

            zAnim.SetBool("zRun", true);

            transform.Translate(Vector3.forward * 9 * Time.deltaTime);
        }
        else
        {
            zAnim.SetBool("zWalk", false);
            zAnim.SetBool("zRun", false);

            zAnim.SetBool("zAttack", true);
            attacking = true;
        }
    }

    public void FinishAnimation() // ���NO TOCAR���
    {
        zAnim.SetBool("zAttack", false);
        attacking = false;
    }
}
