using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using UnityEngine.UI;

enum Transforms {PosX, PosY, PosZ};

public class PropCuboScript : PropCuboPadrao {

    public Toggle toggleField;   

    public void Start()
    {
        if (toggleField != null)
            toggleField.onValueChanged.AddListener(delegate { AdicionaValorPropriedadeToggle(); });
    }        

    void Update()
    {
        if (jaClicouEmAlgumObjeto() && Global.gameObjectName.Contains(Consts.Cubo))
        {
            EnableLabelZ();

            if (!inicializou() || pieceChanged())
                mainMethod();

            if (Input.GetKey(KeyCode.Return))
                updatePosition();

            if (gameObject.name == "PropCubo")
                updatePosition();
        }
    }

    private void AdicionaValorPropriedadeToggle()
    {
        toggleChanged();
    }

    private void EnableLabelZ()
    {
        GameObject labelTam = GameObject.Find("LabelTamanhoZCubo");
        GameObject labelPos = GameObject.Find("LabelPosicaoZCubo");

        if (Global.Grafico2D)
        {
            if (labelTam != null)
                labelTam.transform.localPosition = new Vector3(labelTam.transform.localPosition.x, labelTam.transform.localPosition.y, 100000);

            if (labelPos != null)
                labelPos.transform.localPosition = new Vector3(labelPos.transform.localPosition.x, labelPos.transform.localPosition.y, 100000);
        }
        else
        {
            if (labelTam != null)
                labelTam.transform.localPosition = new Vector3(labelTam.transform.localPosition.x, labelTam.transform.localPosition.y, 0.000111648f);

            if (labelPos != null)
                labelPos.transform.localPosition = new Vector3(labelPos.transform.localPosition.x, labelPos.transform.localPosition.y, 0.000111648f);            
        }        
    }


}
