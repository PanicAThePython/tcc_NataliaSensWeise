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
        if (toggleField != null && !Arquivo.importando)//
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
    
    public void AtualizaCubo(bool isOn, string nomeReserva = "", string cuboNome = "")
    {
        //aqui funciona mais ou menos... dps q eu ligo de volta, o cubo n volta
        //e tbm, o toggle tá com problema, aí complica
        string nome = Global.gameObjectName;
        if (nome == null || nomeReserva != "") nome = nomeReserva;

        string nomeSlotPai = Global.listaEncaixes[nome];
        string num = nomeSlotPai[nomeSlotPai.Length - 1].ToString();
        if (num == "t") num = "0";
        int number = int.Parse(num) + 1;

        if (!isOn)
        {
            //daria certo... se a peça estivesse na lista de props (oq n é o caso)
            //pq isso? pq eu n cliquei pra abrir as props... e elas só são criadas qnd abro o painel
            //se eu clicar pra ver as props do objeto filho, funciona, mas precisa funcionar INDEPENDENTE DISSO
            //agr acontece independente de clicar no painel de props ou n 
            if (cuboNome != "") GameObject.Find(cuboNome).GetComponent<MeshRenderer>().enabled = false;
            else GameObject.Find(Global.propriedadePecas[nome].NomeCuboAmbiente).GetComponent<MeshRenderer>().enabled = false;

            //desativando o objeto filho
            string proxSlot = "ObjGraficoSlot" + number;
            if (number % 2 != 0)
            {
                foreach (KeyValuePair<string, string> enc in Global.listaEncaixes)
                {
                    if (Equals(enc.Value, proxSlot))
                    {
                        if (!Global.propriedadePecas.ContainsKey(enc.Key)) AtualizaCubo(isOn, enc.Key, "CuboAmbiente" + number);
                        else AtualizaCubo(isOn, enc.Key);
                    }
                }
            }

        }
        else
        {
            GameObject goObjGraficoSlot = GameObject.Find(Global.listaEncaixes[nome]);
            string formasSlot = string.Empty;
            string formasSlot1 = string.Empty;
            string peca = string.Empty;
            string peca1 = string.Empty;

            // Descobre nome do FormasSlot correto.
            for (int i = 0; i < goObjGraficoSlot.transform.childCount; i++)
            {
                if (goObjGraficoSlot.transform.GetChild(i).name.Contains("FormasSlot"))
                {
                    formasSlot = goObjGraficoSlot.transform.GetChild(i).name;
                    if (number % 2 != 0) continue;
                    else break;
                }
                if (goObjGraficoSlot.transform.GetChild(i).name.Contains("ObjGraficoSlot"))
                {
                    for (int k = 0; k < goObjGraficoSlot.transform.GetChild(i).transform.childCount; k++)
                    {
                        if (goObjGraficoSlot.transform.GetChild(i).GetChild(k).name.Contains("FormasSlot"))
                        {
                            formasSlot1 = goObjGraficoSlot.transform.GetChild(i).GetChild(k).name;
                            break;
                        }
                    }
                }
            }

            // Descobre nome da peça para acessar suas propriedades.
            foreach (KeyValuePair<string, string> enc in Global.listaEncaixes)
            {
                if (Equals(enc.Value, formasSlot))
                    peca = enc.Key;
                else if (Equals(enc.Value, formasSlot1))
                {
                    peca1 = enc.Key;
                }
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
                    GameObject.Find(Global.propriedadePecas[nome].NomeCuboAmbiente).GetComponent<MeshRenderer>().enabled = Global.propriedadePecas[peca].Ativo;
                else
                    GameObject.Find(Global.propriedadePecas[nome].NomeCuboAmbiente).GetComponent<MeshRenderer>().enabled = true;
            }

            if (!Equals(peca1, string.Empty))
            {
                GameObject.Find("CuboAmbiente" + number).GetComponent<MeshRenderer>().enabled = true;
                GameObject.Find("CuboVis" + number).GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

}
