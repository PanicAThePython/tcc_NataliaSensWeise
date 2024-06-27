using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Cubo
{
    private float posX;
    private float posY;
    private float posZ;

    private float tamX;
    private float tamY;
    private float tamZ;

    private float rotX;
    private float rotY;
    private float rotZ;

    private bool teclaEnterApertada;

    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }

    public float TamX { get; set; }
    public float TamY { get; set; }
    public float TamZ { get; set; }

    public float RotX { get; set; }
    public float RotY { get; set; }
    public float RotZ { get; set; }

    public bool TeclaEnterApertada { get; set; }
}

public class CuboObject : MonoBehaviour {

    public static Cubo cuboGlobal = new Cubo();
    public static float rotationIniX, rotationIniY, rotationIniZ;

    private float posX, posY, posZ;
    private float tamX, tamY, tamZ;
    private float rotX, rotY, rotZ;
    private const float ScaleDefault = 1000f;  
    public float rotXAux, rotYAux, rotZAux;
    private GameObject gameAux;

    private TMP_InputField PosX;
    private TMP_InputField PosY;
    private TMP_InputField PosZ;

    private TMP_InputField TamX;
    private TMP_InputField TamY;
    private TMP_InputField TamZ;

    private TMP_InputField RotX;
    private TMP_InputField RotY;
    private TMP_InputField RotZ;

    // Use this for initialization
    void Start () {
        if (cuboGlobal == null)
        {
            cuboGlobal = new Cubo();
        }

    }   

}
