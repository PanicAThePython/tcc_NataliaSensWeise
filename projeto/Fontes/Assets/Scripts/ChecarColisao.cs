using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChecarColisao : MonoBehaviour
{
    public bool encaixada = false;
    public string peca;

    public void OnTriggerEnter(Collider other)
    {
        encaixada = true;
        peca = other.tag;
    }
}
