using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderController : MonoBehaviour {    

    GameObject otherBase;
    const float INC_BASE_VERDE = 0.53f; // Valor alcançado através de testes visuais.
    const float INC_BASE_CINZA = 1.4f;  // Valor alcançado através de testes visuais.
    private int mudaSinal;

    public void ResizeBases(GameObject baseRender, string tipoPeca, bool incrementa)
    {
        string numObj = "";
        int val = 0;
        mudaSinal = 1;

        if (!incrementa)
            mudaSinal = -1;

        if (Equals(tipoPeca, Consts.ObjetoGrafico))
        {
            numObj = "";

            if (baseRender.name.Length > "ObjGraficoSlot".Length)
            {
                Int32.TryParse(baseRender.name.Substring(baseRender.name.IndexOf("Slot") + 4, 1), out val);                

                if (val > 0)
                    numObj = Convert.ToString(val);
            }

            otherBase = GameObject.Find("BaseRenderLateralGO" + numObj);
            otherBase.transform.GetChild(0).gameObject.SetActive(true); //Base lateral cinza

            otherBase = GameObject.Find("BaseObjetoGraficoGO" + numObj);
            otherBase.transform.GetChild(0).gameObject.SetActive(true); //Base objeto gráfico verde
        }
        else if (Consts.IsTransformacao(tipoPeca))
        {
            numObj = "";

            if (baseRender.name.Length > "TransformacoesSlot".Length)
            {
                Int32.TryParse(baseRender.name.Substring(baseRender.name.IndexOf("Slot") + 4, 1), out val);                

                if (val > 0)
                    numObj = Convert.ToString(val);
            }

            // Redimensiona base verde
            otherBase = GameObject.Find("BaseObjetoGraficoGO" + numObj);

            float ScaleY = otherBase.transform.localScale.y;
            otherBase.transform.localScale = new Vector3(otherBase.transform.localScale.x, ScaleY + (INC_BASE_VERDE * mudaSinal), otherBase.transform.localScale.z);

            // Redimensiona base cinza
            otherBase = GameObject.Find("BaseRenderLateralGO" + numObj);

            ScaleY = otherBase.transform.localScale.y;
            otherBase.transform.localScale = new Vector3(otherBase.transform.localScale.x, ScaleY + (INC_BASE_CINZA * mudaSinal), otherBase.transform.localScale.z);            
        }
    }
}
