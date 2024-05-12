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

            // Redimensiona base cinza pra pais e filhos
            if (numObj == "") numObj = "0";

            int num = int.Parse(numObj);

            if (num % 2 != 0)
            {
                //se é filho, ent aumenta o renderer do pai
                num = num - 1;
                if (num == 0) otherBase = GameObject.Find("BaseRenderLateralGO");
                else otherBase = GameObject.Find("BaseRenderLateralGO" + num);

                float ScaleY = otherBase.transform.localScale.y;
                otherBase.transform.localScale = new Vector3(otherBase.transform.localScale.x, ScaleY + (INC_BASE_CINZA * 2), otherBase.transform.localScale.z);

                //arruma posição do slot de objt q tá sobrando
                GameObject objt = GameObject.Find("ObjGraficoSlot" + (num + 2));
                objt.transform.position = new Vector3(objt.transform.position.x, objt.transform.position.y - (3.9f * 2), objt.transform.position.z);
            }
            else if (num == 0) otherBase = GameObject.Find("BaseRenderLateralGO");
            else otherBase = GameObject.Find("BaseRenderLateralGO" + numObj);
            //por causa do droppeca count, a base do objeto 2 tá vindo com 3 no lugar, aí tanto o child quanto o raiz tem o msm nome de base
            //e qnd cria um novo objt, vem com o num 5, isso tbm é um problema
            otherBase.transform.GetChild(0).gameObject.SetActive(true); //Base lateral cinza

            //redimensiona a base verde do objeto da vez
            numObj = "";

            if (baseRender.name.Length > "ObjGraficoSlot".Length)
            {
                Int32.TryParse(baseRender.name.Substring(baseRender.name.IndexOf("Slot") + 4, 1), out val);                

                if (val > 0)
                    numObj = Convert.ToString(val);
            }

            otherBase = GameObject.Find("BaseObjetoGraficoGO" + numObj);
            otherBase.transform.GetChild(0).gameObject.SetActive(true); //Base objeto gráfico verde

            //ObjGraficoSlot1(Clone)
            GameObject clone = GameObject.Find("ObjGraficoSlot" + (num + 1) + "(Clone)");
            if (clone != null) Destroy(clone);
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
            otherBase.transform.position = new Vector3(otherBase.transform.position.x, otherBase.transform.position.y - ScaleY, otherBase.transform.position.z);

            //arruma posição do slot child
            
            if (numObj == "") numObj = "0";
            int num = int.Parse(numObj);

            //ObjGraficoSlot1(Clone)
            GameObject clone = GameObject.Find("ObjGraficoSlot" + (num + 1) + "(Clone)");
            if (clone != null) Destroy(clone);

            GameObject objt1 = GameObject.Find("ObjGraficoSlot" + (num + 1));
            Vector3 pos = objt1.transform.position;
            pos.y -= 3;
            objt1.transform.position = pos;

            if (numObj == "0") numObj = "";
            GameObject peca1 = GameObject.Find("ObjetoGraficoP" + (num + 1));
            pos = peca1.transform.position;
            pos.y -= 3;
            peca1.transform.position = pos;
            Global.listaPosicaoSlot[Global.listaEncaixes["ObjetoGraficoP" + numObj]] = pos.y;

            //objt1.transform.position = new Vector3(objt1.transform.position.x, objt1.transform.position.y - 3f, objt1.transform.position.z);
            
            //Global.listaPosicaoSlot[objt1.name] = objt1.transform.position.y - 3f; //n funcionou
            Global.atualizaListaSlot(); //n funcou
            //a peça n tá descendo junto pq o valor da posição do slot n foi atualizado na lista global!!!

            otherBase = GameObject.Find("BaseRenderLateralGO" + numObj);

            ScaleY = otherBase.transform.localScale.y;
            otherBase.transform.localScale = new Vector3(otherBase.transform.localScale.x, ScaleY + (INC_BASE_CINZA * mudaSinal), otherBase.transform.localScale.z);            
        }
    }
}
