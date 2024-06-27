using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DropPeca : MonoBehaviour {

    enum PecasSlot {Camera, ObjGrafico, Formas, Traformacoes, Iluminacao};

    public static int countTransformacoes;
    public static int countObjetosGraficos;
    public static int countFormas;

    public bool estaEncaixada;
    private Vector3 posDestino;
    private string[] nomeObj = new string[3];
    private Transform parent;
    private string transformacoes;
    private GameObject ilumunacaoAux;
    private Vector3 positionIluminacao;
    private GameObject ilumunacao;
    private string nomeGameObjectCollider;

    public static List<string> listaTransformacoes;
    public static bool passouDropPecas;
    public static string colliderName;
    public static string parentName;

    void Start()
    {
        posDestino = gameObject.transform.position;
        estaEncaixada = false;

        if (listaTransformacoes == null)
            listaTransformacoes = new List<string>();        

        //transformacoes = "TransformacoesSlot" + Convert.ToString(countTransformacoes);

        //if (countTransformacoes == 0)
        //    transformacoes = "TransformacoesSlot";

        if (gameObject.name == "TransformacoesSlot" + countTransformacoes)
        {
            nomeObj[0] = "Transladar";
            nomeObj[1] = "Rotacionar";
            nomeObj[2] = "Escalar";
        }
        else if (gameObject.name == "ObjGraficoSlot" + countObjetosGraficos)
        {
            nomeObj[0] = "ObjetoGraficoP";
        }
        else if (gameObject.name == "FormasSlot" + countFormas)
        {
            nomeObj[0] = "Cubo";
        }
        {
            switch (gameObject.name)
            {
                case "CameraSlot":
                    nomeObj[0] = "CameraP";
                    break;
                case "ObjGraficoSlot":
                    nomeObj[0] = "ObjetoGraficoP";
                    break;
                case "FormasSlot":
                    nomeObj[0] = "Cubo";
                    //nomeObj[1] = "Poligono";
                    //nomeObj[2] = "Spline";
                    break;
                case "TransformacoesSlot":
                    nomeObj[0] = "Transladar";
                    nomeObj[1] = "Rotacionar";
                    nomeObj[2] = "Escalar";
                    break;
                case "IluminacaoSlot":
                    nomeObj[0] = "Iluminacao";
                    break;
            }
        }
        
    }    

    public void OnTriggerEnter(Collider other)
    {
        colliderName = GetComponent<Collider>().gameObject.name;
        parentName = GetComponent<Collider>().gameObject.transform.parent.name;
        Global.slotName = GetComponent<Collider>().gameObject.transform.name;
       // Debug.Log(Global.slotName);
        estaEncaixada = true;

        for (int i = 0; i < nomeObj.Length; i++)
        {
            if (!other.name.Contains("Mesh") && other.name == nomeObj[i])
            {                
                Global.addObject(other.gameObject);
                nomeGameObjectCollider = other.gameObject.name;
            }
        }

    }

    //public void OnTriggerStay(Collider other)
    //{
    //    colliderName = GetComponent<Collider>().gameObject.name;
    //    parentName = GetComponent<Collider>().gameObject.transform.parent.name;
    //    estaEncaixada = true;
    //    //Global.removeObject(other.gameObject);
    //    for (int i = 0; i < nomeObj.Length; i++)
    //    {
    //        if (other.name == nomeGameObjectCollider)
    //        {
    //            Debug.Log("Stay");
    //        }
    //    }
    //}

    //public void OnTriggerExit(Collider other)
    //{
    //    estaEncaixada = false;
    //    //Global.removeObject(other.gameObject);
    //    parentName = GetComponent<Collider>().gameObject.transform.parent.name;
    //    for (int i = 0; i < nomeObj.Length; i++)
    //    {
    //        if (other.name == nomeGameObjectCollider)
    //        {
    //            Debug.Log("Exit");
    //        }
    //    }
    //}
    
    public void OnMouseEnter()
    {
        parentName = GetComponent<Collider>().gameObject.transform.parent.name;
        // Global.slotName = gameObject.transform.name;
        //Global.slotName = gameObject.transform.name;
        //Debug.Log(Global.slotName);

        Global.podeExcruiObjeto = true;
    }

    public void OnMouseExit()
    {
        Global.podeExcruiObjeto = false;
    }


    public void chamaCollider()
    {
        OnTriggerEnter(GetComponent<Collider>());
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.name);
    }

    void OnMouseOver()
    {

    }


}
