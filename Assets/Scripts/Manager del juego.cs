using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managerdeljuego : MonoBehaviour
{
    public static Managerdeljuego varManager;

    void Awake()
    {
        if(Managerdeljuego.varManager == null)
        {
            Managerdeljuego.varManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
