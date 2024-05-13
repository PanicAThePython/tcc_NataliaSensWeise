using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderController : MonoBehaviour {    

    GameObject otherBase;
    const float INC_BASE_VERDE = 0.53f; // Valor alcançado através de testes visuais.
    const float INC_BASE_CINZA = 1.4f;  // Valor alcançado através de testes visuais.
    private int mudaSinal;

    private void destroyExtraObject(int num)
    {
        //ObjGraficoSlot1(Clone)
        GameObject clone = GameObject.Find("ObjGraficoSlot" + (num + 1) + "(Clone)");
        if (clone != null) Destroy(clone);
    }
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


                //arruma posição próximo slot
                GameObject objt = GameObject.Find("ObjGraficoSlot" + (num + 2));
                objt.transform.position = new Vector3(objt.transform.position.x, objt.transform.position.y - (INC_BASE_CINZA * 5.5f), objt.transform.position.z);
                //arrumando o slot q sobra
                int filhos = GameObject.Find("Render").transform.childCount;
                GameObject ultimo = GameObject.Find("Render").transform.GetChild(filhos-1).gameObject;
                ultimo.transform.position = new Vector3(ultimo.transform.position.x, ultimo.transform.position.y - (INC_BASE_CINZA * 3.5f), ultimo.transform.position.z);

            }
            else if (num == 0) otherBase = GameObject.Find("BaseRenderLateralGO");
            else otherBase = GameObject.Find("BaseRenderLateralGO" + numObj);
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

            destroyExtraObject(num);
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

            if (numObj == "") numObj = "0";
            int num = int.Parse(numObj);

            //arruma posicionamento da lateral verde conforme tipo de objeto (pai ou filho)
            if (num % 2 == 0) otherBase.transform.position = new Vector3(otherBase.transform.position.x, otherBase.transform.position.y - 1.4f, otherBase.transform.position.z);
            else otherBase.transform.position = new Vector3(otherBase.transform.position.x, otherBase.transform.position.y, otherBase.transform.position.z);

            //arruma posição do slot de objt q tá sobrando
            if (num % 2 == 0)
            {   
                GameObject objt = GameObject.Find("ObjGraficoSlot" + (num + 2));
                objt.transform.position = new Vector3(objt.transform.position.x, objt.transform.position.y - INC_BASE_VERDE, objt.transform.position.z);
                
                if (num > 0) otherBase = GameObject.Find("BaseRenderLateralGO" + num);
                else otherBase = GameObject.Find("BaseRenderLateralGO");

                ScaleY = otherBase.transform.localScale.y;
                otherBase.transform.localScale = new Vector3(otherBase.transform.localScale.x, ScaleY + INC_BASE_CINZA, otherBase.transform.localScale.z);
            }
            else
            {
                
                GameObject objt = GameObject.Find("ObjGraficoSlot" + (num + 1));
                objt.transform.position = new Vector3(objt.transform.position.x, objt.transform.position.y - INC_BASE_VERDE, objt.transform.position.z);

                /*
                //arrumando o slot q sobra
                int filhos = GameObject.Find("Render").transform.childCount;
                GameObject ultimo = GameObject.Find("Render").transform.GetChild(filhos).gameObject;
                print(ultimo.name);
                ultimo.transform.position = new Vector3(ultimo.transform.position.x, ultimo.transform.position.y - (INC_BASE_CINZA * 5f), ultimo.transform.position.z);
                */

                if (num - 1 == 0) otherBase = GameObject.Find("BaseRenderLateralGO");
                else otherBase = GameObject.Find("BaseRenderLateralGO" + (num - 1));
                ScaleY = otherBase.transform.localScale.y;
                otherBase.transform.localScale = new Vector3(otherBase.transform.localScale.x, ScaleY + INC_BASE_CINZA, otherBase.transform.localScale.z);
            }

            //ObjGraficoSlot1(Clone)
            destroyExtraObject(num);

            GameObject objt1 = GameObject.Find("ObjGraficoSlot" + (num + 1));
            Vector3 pos = objt1.transform.position;
            pos.y -= 3;
            objt1.transform.position = pos;
            //arruma posição do próximo slot
            /*
           

            if (numObj == "0") numObj = "";

            //se eu estiver mexendo com objeto pai e ele tá com o filho funcionando, ele vai arrumar a posição das peças do filho
            GameObject forma1 = GameObject.Find("FormaSlot" + (num + 1));
            GameObject trans1 = GameObject.Find("TransformacoesSlot" + (num + 1));
            GameObject ilu1 = GameObject.Find("IluminacaoSlot" + (num + 1));
            if (num % 2 == 0 && (forma1 != null || trans1 != null || ilu1 != null))
            {
                GameObject peca1 = GameObject.Find("ObjetoGraficoP" + (num + 1));
                pos = peca1.transform.position;
                pos.y -= 3;
                peca1.transform.position = pos;
                Global.listaPosicaoSlot[Global.listaEncaixes["ObjetoGraficoP" + numObj]] = pos.y;
                for (int i = 0; i < objt1.transform.childCount; i++)
                {
                    pos = objt1.transform.GetChild(i).position;
                    //pos.y -= 3;
                    objt1.transform.GetChild(i).position = pos;
                    if (Global.listaPosicaoSlot.ContainsKey(objt1.transform.GetChild(i).name))
                    {
                        foreach (KeyValuePair<string, string> encaixe in Global.listaEncaixes)
                        {
                           if (encaixe.Value == objt1.transform.GetChild(i).name)
                            {
                                GameObject teste = GameObject.Find(encaixe.Key);
                                pos = teste.transform.position;
                                pos.y -= 3;
                                teste.transform.position = pos;
                            }
                        }
                    }
                }
            }
           */
            Global.atualizaListaSlot();       
        }
    }
}
