using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeuObjetoFilhos : MonoBehaviour
{
    public List<string> children;

    public void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Cubo":
            case "Iluminacao":
            case "Translacao":
            case "Rotacao":
            case "Escala":
                children.Add(other.tag);
                break;
        }
    }

    public void teste()
    {
        print("começou");
        for (int i = 0; i < children.Count; i++)
        {
            print(children[i]);
        }
        print("acabou");
    }
}
