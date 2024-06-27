using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamVisualizador : MonoBehaviour {

    public float inc;

    private Vector3 posIni;
    private Vector3 rotIni;
    private float valorAnt = 0;

    void Start()
    {
        posIni = transform.position;
        rotIni = transform.eulerAngles;
    }


    void Update()
    {
        if (inc == 2.2f)
        {
            inc = 0;
        }

        if (inc >= 0)
        {
            if (valorAnt != inc && inc < 2.2f)
            {
                posIni.x -= inc * 14f;
                posIni.y += (inc * 5);
                rotIni.x += inc;

                transform.position = posIni;
                transform.eulerAngles = rotIni;
            }
        }else
        {
            if (valorAnt != inc && inc > -2.2f)
            {
                posIni.x += inc * 14f;
                posIni.y -= (inc * 5);
                rotIni.x -= inc;

                transform.position = posIni;
                transform.eulerAngles = rotIni;
            }
        }
         

        valorAnt = inc;
    }
}
