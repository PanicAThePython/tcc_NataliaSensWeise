using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoPropPadrao : MonoBehaviour
{
    private Util_VisEdu visEdu = new Util_VisEdu();

    private GameObject botaoArquivo;
    private GameObject botaoFabrica;
    private GameObject botaoPropriedade;
    private GameObject botaoAjuda;

    private GameObject painelArquivo;
    private GameObject painelFabrica;
    private GameObject painelPropriedade;
    private GameObject painelAjuda;

    public GameObject BotaoArquivo { get; set; }
    public GameObject BotaoFabrica { get; set; }
    public GameObject BotaoPropriedade { get; set; }
    public GameObject BotaoAjuda { get; set; }

    public BotaoPropPadrao()
    {
        botaoArquivo = GameObject.Find("BtnArquivo");
        botaoFabrica = GameObject.Find("BtnFabPecas");
        botaoPropriedade = GameObject.Find("BtnPropPecas");
        botaoAjuda = GameObject.Find("BtnAjuda");
    }

    public void setButton(GameObject go, GameObject painel, bool clicouBotao = false)
    {
        visEdu.enableColliderFabricaPecas(false, true);

        botaoArquivo.transform.localScale = new Vector3(botaoArquivo.transform.localScale.x, 30, botaoArquivo.transform.localScale.z);
        botaoFabrica.transform.localScale = new Vector3(botaoFabrica.transform.localScale.x, 30, botaoFabrica.transform.localScale.z);
        botaoPropriedade.transform.localScale = new Vector3(botaoPropriedade.transform.localScale.x, 30, botaoPropriedade.transform.localScale.z);
        botaoAjuda.transform.localScale = new Vector3(botaoAjuda.transform.localScale.x, 30, botaoAjuda.transform.localScale.z);

        go.transform.localScale = new Vector3(go.transform.localScale.x, 45, go.transform.localScale.z);

        GameObject go_Painel = GameObject.Find("GO_FabricaPecas");

        for (int i = 0; i < go_Painel.transform.childCount; i++)
        {
            if (!Equals(go_Painel.transform.GetChild(i).name, "FabricaPecas"))
            {   
                go_Painel.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = false;

                for (int j = 0; j < go_Painel.transform.GetChild(i).transform.childCount; j++)
                {
                    go_Painel.transform.GetChild(i).GetChild(j).gameObject.SetActive(false);
                }
            }
        }

        if (!Equals(go.name, "BtnFabPecas"))
        {
            if (clicouBotao && ((painel == null) || (!Equals(painel.name, "Arquivo") && !Equals(painel.name, "Ajuda"))))
            {
                if (Global.lastPressedButton == null) // Se for null é porque nenhum objeto foi selecionado ainda
                {
                    Global.lastPressedButton = GameObject.Find("PropriedadePecas");
                }
                
                Global.lastPressedButton.transform.gameObject.GetComponent<MeshRenderer>().enabled = true;

                for (int i = 0; i < Global.lastPressedButton.transform.childCount; i++)
                {
                    Global.lastPressedButton.transform.GetChild(i).gameObject.SetActive(true);
                }                
            }
            else
            {
                painel.transform.gameObject.GetComponent<MeshRenderer>().enabled = true;

                for (int i = 0; i < painel.transform.childCount; i++)
                {
                    painel.transform.GetChild(i).gameObject.SetActive(true);
                }

                if (painel.name.Contains("Iluminacao"))
                {
                    for (int i = 0; i < painel.transform.GetChild(2).childCount; i++) 
                        painel.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);

                    painel.transform.GetChild(2).GetChild(Global.propriedadePecas[Global.gameObjectName].TipoLuz).gameObject.SetActive(true);
                }
            }
        }                 
    }    
}
