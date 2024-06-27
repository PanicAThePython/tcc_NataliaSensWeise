using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teste : MonoBehaviour {

    public GameObject targetRotation;
    float speed = 50f;
    public bool podeGirar;
    Vector3 posIni;
    Vector3 positionTarget;
    Vector3 posAux;

    // Use this for initialization
    void Start () {
        podeGirar = true;
        //transform.localPosition = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {

        //if (posIni != transform.position && podeGirar)
        //{           

        //    transform.RotateAround(targetRotation.transform.position, Vector3.up, speed * Time.deltaTime);
        //    posIni = transform.position;
        //}
        //if (podeGirar)
        //{
        //    Vector3 direcao = targetRotation.transform.position - transform.position;
        //    Quaternion novaRotacao = Quaternion.LookRotation(direcao);
        //    GetComponent<Rigidbody>().MoveRotation(novaRotacao);
        //}
        //transform.LookAt(targetRotation.transform);
        transform.LookAt(targetRotation.transform, posAux);        

        if (podeGirar)
        {
            posAux = new Vector3(50, 0, 0);            
            transform.LookAt(targetRotation.transform, posAux);
        }
        

    }


    void FixedUpdate()
    {
        //Vector3 t = transform.position;

        //if (podeGirar)
        //{
        //    transform.RotateAround(targetRotation.transform.position, Vector3.left, speed * Time.deltaTime);
        //    posAux = transform.position;
        //    podeGirar = false;
        //}

       //if (posAux != transform.position)
       //     podeGirar = true;
        
    }

    private void OnMouseDrag()
    {
        //transform.RotateAround(targetRotation.transform.position, Vector3.right, speed * Time.deltaTime);
    }
}
