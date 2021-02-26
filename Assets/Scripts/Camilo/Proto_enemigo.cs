using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Proto_enemigo : MonoBehaviour
{
    //Referencia al juagdor.
    public GameObject yo;
    //parametro de animación para que el enemigo localice al jugador.
    bool detectado = false;
    //Velocidad de rotación.
    float velRot = 12.0f;

    // Start is called before the first frame update
    void Start()
    {
        yo = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posicion = transform.position;
        Vector3 posicionYo = yo.transform.position;
        float distancia = Vector3.Distance (posicionYo, posicion);
        if (detectado)
        {
            float toWayPoint = Vector3.Distance(posicionYo,transform.position);
        }
        
    }
    void seguir(GameObject target)
    {
        Vector3 posicion = transform.position;
        Vector3 posicionTarget = target.transform.position;
        Vector3 direccion = posicionTarget - posicion;
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(direccion, Vector3.up), velRot * Time.deltaTime);
    }
}
