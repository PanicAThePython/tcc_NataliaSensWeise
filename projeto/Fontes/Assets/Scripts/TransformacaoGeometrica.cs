using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Consts
{
    public const string Rotacionar = "Rotacionar";
    public const string Transladar = "Transladar";
    public const string Escalar = "Escalar";
    public const string Iluminacao = "Iluminacao";
    public const string ObjetoGrafico = "ObjetoGraficoP";
    public const string Cubo = "Cubo";
    public const string Poligono = "Poligono";
    public const string Spline = "Spline";
    public const string Camera = "CameraP";

    public static bool IsTransformacao(string tranf)
    {
        return (Equals(tranf, Rotacionar)) || (Equals(tranf, Transladar)) || (Equals(tranf, Escalar));        
    }

}

static class Objetos
{    
   public const string FabricaPecas = "FabricaPecas";
}

public class TransformacaoGeometrica : MonoBehaviour {

    public float escalaX, escalaY, escalaZ;
    private float escalaAux;
    public GameObject obj;
    private bool verify;

	// Use this for initialization
	void Start () {
        escalaAux = obj.transform.localScale.x;
        verify = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (verify)
        {
            escalaAux += escalaX;
            //transform.localScale = new Vector3(escalaAux,0,0);
            obj.transform.localScale = new Vector3(escalaAux, escalaAux, escalaAux);
        }
        
    }

    private void OnMouseUp()
    {
        switch (gameObject.name)
        {
            case Consts.Transladar:
                break;
            case Consts.Rotacionar:
                break;
            case Consts.Escalar:
                verify = true;
               // transform.localScale.x += escalaX;
                break;
        }
    }
}
