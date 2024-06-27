using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePecaObj : MonoBehaviour {

    float moveX = 0.0f;
    float moveY = 0.0f;
    float sensitivity = 150f;
    static float sensLateral = 1.9f;
    Vector3 posInicial, rotacaoInicial;
    public GameObject go;

    private void Start()
    {
        posInicial = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    void Update()
    {
        moveAmbienteBotaoDireito();
    }

    private void moveAmbienteBotaoDireito()
    {
        if (Input.GetAxis("Mouse X") != 0 && Input.GetButton("Fire1"))
        {
            moveX += Input.GetAxis("Mouse X") * sensLateral;
            moveY += Input.GetAxis("Mouse Y") * sensLateral;
            go.transform.Translate(Vector3.right * moveX);
            go.transform.Translate(Vector3.up * moveY);
        }
        moveX = 0.0f;
        moveY = 0.0f;
    }
}
