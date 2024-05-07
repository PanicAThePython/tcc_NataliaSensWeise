using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;
using UnityEngine.UI;

public class MeuObjetoGrafico : MeuModelo
{
    public JSONObject props = new JSONObject();
    public JSONArray children = new JSONArray();

    public TMP_InputField nome;
    public Toggle ativo;

    public void addChildren(JSONObject filho)
    {
        children.Add(filho);
    }

    public void setChildren(JSONArray c)
    {
        children = c;
    }

    public string ConverterNomes(string nomePeca)
    {
        print(nomePeca);
        if (nomePeca.Length > "ObjetoGraficoP".Length)
        {
            int num = int.Parse(nomePeca.Replace("ObjetoGraficoP", ""));
            if (num == 1)
            {
                if (!Global.propriedadePecas.ContainsKey("ObjetoGraficoP")) nomePeca = "ObjetoGraficoP";
            }
            else
            {
                num -= 1;
                var nomeNum = "ObjetoGraficoP" + num;
                if (Global.propriedadePecas.ContainsKey(nomeNum))
                {
                    int numNovo = int.Parse(Global.propriedadePecas[nomeNum].Nome.Replace("ObjetoGraficoP", ""));
                    if (numNovo != num) nomePeca = nomeNum;
                }
                else nomePeca = nomeNum;
            }
        }
        return nomePeca;
    }

    public void addProps(string nomePeca)
    {
        if (Global.propriedadePecas.ContainsKey(nomePeca))
        {
            var objeto = Global.propriedadePecas[nomePeca];
            nomePeca = ConverterNomes(nomePeca);
            props.Add("nome", nomePeca);
            props.Add("ativo", objeto.Ativo);
            props.Add("children", children);

            JSONArray posPeca = new JSONArray();
            posPeca.Add("x", this.transform.position.x);
            posPeca.Add("y", this.transform.position.y);
            posPeca.Add("z", this.transform.position.z);
            props.Add("posPeca", posPeca);
        }
        else
        {
            nomePeca = ConverterNomes(nomePeca);
            props.Add("nome", nomePeca);
            props.Add("ativo", true);
            props.Add("children", children);

            JSONArray posPeca = new JSONArray();
            posPeca.Add("x", this.transform.position.x);
            posPeca.Add("y", this.transform.position.y);
            posPeca.Add("z", this.transform.position.z);
            props.Add("posPeca", posPeca);
        }
    }
    public JSONObject getProps()
    {
        return props;
    }

    public void setProps(JSONObject p)
    {
        props = p;
    }
}
