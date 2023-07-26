using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PropObjetoGraficoScript : MonoBehaviour {

    public Toggle toggleField;
    private GameObject goObjetoGrafico;
    private string nomePeca;

    public void Start()
    {
        if (toggleField != null)
            toggleField.onValueChanged.AddListener(delegate {
                Global.propriedadePecas[Global.gameObjectName].Ativo = toggleField.isOn;
                AtualizaCubo(toggleField.isOn);
            });

        // Iniciando.
        //if (Global.gameObjectName == null)
        //{
        //    Global.propriedadePecas[Global.gameObjectName].Ativo = true;
        //}

        if (Global.gameObjectName != null)
        {
            if (!Global.propriedadePecas[Global.gameObjectName].JaInstanciou)
            {
                Global.propriedadePecas[Global.gameObjectName].JaInstanciou = true;
                Global.propriedadePecas[Global.gameObjectName].Ativo = true;
            }

            goObjetoGrafico = GameObject.Find("PropObjGrafico");

            nomePeca = "ObjetoGraficoP";

            if (Global.gameObjectName.Length > nomePeca.Length)
                nomePeca = "Objeto Gráfico " + Global.gameObjectName.Substring(nomePeca.Length, 1);
            else
                nomePeca = "Objeto Gráfico";

            // Nome.
            goObjetoGrafico.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = nomePeca;

            // Toggle.
            goObjetoGrafico.transform.GetChild(2).GetComponent<Toggle>().isOn = Global.propriedadePecas[Global.gameObjectName].Ativo;
        }    
        
    }  
    
    private void AtualizaCubo(bool isOn)
    {
        if (!isOn)
            GameObject.Find(Global.propriedadePecas[Global.gameObjectName].NomeCuboAmbiente).GetComponent<MeshRenderer>().enabled = false;
        else
        {
            GameObject goObjGraficoSlot = GameObject.Find(Global.listaEncaixes[Global.gameObjectName]);
            string formasSlot = string.Empty;
            string peca = string.Empty;

            // Descobre nome do FormasSlot correto.
            for (int i = 0; i < goObjGraficoSlot.transform.childCount; i++)
            {
                if (goObjGraficoSlot.transform.GetChild(i).name.Contains("FormasSlot"))
                {
                    formasSlot = goObjGraficoSlot.transform.GetChild(i).name;
                    break;
                }
            }

            // Descobre nome da peça para acessar suas propriedades.
            foreach (KeyValuePair<string, string> enc in Global.listaEncaixes)
            {
                if (Equals(enc.Value, formasSlot))
                    peca = enc.Key;
            }

            // Se o cubo estiver ativo então demonstra a peça, senão continua desabilitada.
            if (!Equals(peca, string.Empty))
            {
                bool existePropriedade = false;

                // Verifica se a peça ja foi iniciada.
                foreach (KeyValuePair<string, PropriedadePeca> prop in Global.propriedadePecas)
                {
                    if (Equals(prop.Key, peca))
                    {
                        existePropriedade = true;
                        break;
                    }                        
                }

                if (existePropriedade)
                    GameObject.Find(Global.propriedadePecas[Global.gameObjectName].NomeCuboAmbiente).GetComponent<MeshRenderer>().enabled = Global.propriedadePecas[peca].Ativo;
                else
                    GameObject.Find(Global.propriedadePecas[Global.gameObjectName].NomeCuboAmbiente).GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

}
