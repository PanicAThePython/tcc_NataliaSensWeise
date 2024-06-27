using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class PropCameraScript : PropCameraPadrao {

    public TMP_InputField mainInputField;
    public Transform cuboAmb;
    public Camera cam;

    private float time;

    public void Start()
    {
        if (mainInputField != null)
            mainInputField.onValueChanged.AddListener(delegate { AdicionaValorPropriedade(); });
        else
            preencheCampos();
    }    

    void Update()
    {
        time += Time.deltaTime;

        if (time >= 3 && GameObject.Find("PropCamera").GetComponent<MeshRenderer>().enabled)
        {
            AtualizaPainel();
            time = 0;
        }

        if (jaClicouEmAlgumObjeto() && Global.propCameraGlobal.ExisteCamera)
        {
            if (!Global.propCameraGlobal.JaIniciouValores)
                preencheCampos();
        }

        if (podeAtualizarCamera)
        {
            AjustaCameraEmX();
            podeAtualizarCamera = false;
        }            
    }

    public void AdicionaValorPropriedade()
    {
        if (gameObject.GetComponent<TMP_InputField>().text != string.Empty)
            updatePosition(cam);
    }

    private void AtualizaPainel()
    {
        GameObject FOV = GameObject.Find("LabelFOVCamera");
        GameObject labelPosicao = GameObject.Find("LabelPosicaoCamera");
        GameObject lookat = GameObject.Find("LabelLookAtCamera");
        GameObject near = GameObject.Find("LabelNearCamera");
        GameObject far = GameObject.Find("LabelFarCamera");

        float FovY = FOV.transform.localPosition.y;

        if (Global.Grafico2D)
        {
            labelPosicao.transform.localPosition = new Vector3(labelPosicao.transform.localPosition.x, labelPosicao.transform.localPosition.y, 50);
            lookat.transform.localPosition = new Vector3(lookat.transform.localPosition.x, lookat.transform.localPosition.y, 50);
            near.transform.localPosition = new Vector3(near.transform.localPosition.x, near.transform.localPosition.y, 50);
            far.transform.localPosition = new Vector3(far.transform.localPosition.x, far.transform.localPosition.y, 50);
            FOV.transform.localPosition = new Vector3(FOV.transform.localPosition.x, 0.110f, 50);
        }
        else
        {
            labelPosicao.transform.localPosition = new Vector3(labelPosicao.transform.localPosition.x, labelPosicao.transform.localPosition.y, -2.999623f);
            FOV.transform.localPosition = new Vector3(FOV.transform.localPosition.x, 0, -2.999623f);
            lookat.transform.localPosition = new Vector3(lookat.transform.localPosition.x, lookat.transform.localPosition.y, -2.999622f);
            near.transform.localPosition = new Vector3(near.transform.localPosition.x, near.transform.localPosition.y, -2.999623f);
            far.transform.localPosition = new Vector3(far.transform.localPosition.x, far.transform.localPosition.y, -2.999623f);
        }
    }
}
