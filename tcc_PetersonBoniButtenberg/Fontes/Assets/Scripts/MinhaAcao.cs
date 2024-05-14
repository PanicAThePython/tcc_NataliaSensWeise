using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;
using UnityEngine.UI;

public class MinhaAcao : MeuModelo
{
    public JSONObject props = new JSONObject();

    public TMP_InputField nome;
    public Toggle ativo;
    public TMP_InputField[] valores;

    public string ConverterNomes(string nomePeca)
    {
        var nomeOriginal = "";
        if (nomePeca.Contains("Escala")) nomeOriginal = "Escalar";
        else if (nomePeca.Contains("Trans")) nomeOriginal = "Transladar";
        else nomeOriginal = "Rotacionar";

        if (nomePeca.Length > nomeOriginal.Length)
        {
            int num = int.Parse(nomePeca.Replace(nomeOriginal, ""));
            print(num);
            if (num == 1)
            {
                //n rola com o escalar...
                if (!Global.propriedadePecas.ContainsKey(nomeOriginal)) nomePeca = nomeOriginal;
            }
            else
            {
                num -= 1;
                var nomeNum = nomeOriginal + num;
                if (Global.propriedadePecas.ContainsKey(nomeNum))
                {
                    int numNovo = int.Parse(Global.propriedadePecas[nomeNum].Nome.Replace(nomeOriginal, ""));
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
            var acao = Global.propriedadePecas[nomePeca];
            nomePeca = ConverterNomes(nomePeca);
            props.Add("nome", acao.Nome);
            props.Add("ativo", acao.Ativo);

            if (nomePeca.Contains("Escala"))
            {
                JSONArray vals = new JSONArray();
                vals.Add("x", acao.Tam.X.ToString());
                vals.Add("y", acao.Tam.Y.ToString());
                vals.Add("z", acao.Tam.Z.ToString());
                props.Add("valores", vals);
            }
            else
            {
                JSONArray vals = new JSONArray();
                vals.Add("x", acao.Pos.X.ToString());
                vals.Add("y", acao.Pos.Y.ToString());
                vals.Add("z", acao.Pos.Z.ToString());
                props.Add("valores", vals);
            }
            

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

            if (nomePeca.Contains("Escala"))
            {
                JSONArray vals = new JSONArray();
                vals.Add("x", "1");
                vals.Add("y", "1");
                vals.Add("z", "1");
                props.Add("valores", vals);
            }
            else
            {
                JSONArray vals = new JSONArray();
                vals.Add("x", "0");
                vals.Add("y", "0");
                vals.Add("z", "0");
                props.Add("valores", vals);
            }
        }
        
    }

    public JSONObject getProps()
    {
        return props;
    }
}
