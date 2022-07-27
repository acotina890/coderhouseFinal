using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    #region Variables a utilizar
    public Animator zAnim; // para setear las animaciones
    public float zSpeed = 0.1f; // velocidad de movimiento del zombie
    public float countDown = 0f; // temporizador para las animaciones
    public Transform delJugador; // para cuando detecte al jugador / player
    public Rigidbody zRb; // para las fuerzas u otro tipo
    #endregion

    float distancia, hor, ver;
    Vector3 position;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    #region Metodo Update
    // Update is called once per frame
    void Update()
    {
        // Transition();
        // FollowAndAttack();
    }
    #endregion

    void FixedUpdate()
    {
        Transition();
        
    }

    void Transition()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
        Vector3 inputZombie = new Vector3(hor, 0, ver);

        float rango = Random.Range(-8, 8);

        transform.position += Vector3.forward;
    }

    #region Para el temporizador
    void ResetTempo()
    {
        countDown = 0f;
    }

    void StartTempo()
    {
        countDown += Time.deltaTime;
    }

    void SpawnZombies()
    {
        
    }
    #endregion

    #region Otros
    void FollowAndAttack()
    {
        DistanceFromObject();
        if (distancia > 1.5f)
        {
            // PredMovement();
        }
    }

    void DistanceFromObject()
    {
        distancia = Vector3.Distance(transform.position, delJugador.position);
    }

    void PredMovement()
    {
        distancia = Vector3.Distance(transform.position, delJugador.position);
        transform.position = Vector3.Lerp(transform.position, delJugador.position, zSpeed * Time.deltaTime);
    }
    #endregion
}
