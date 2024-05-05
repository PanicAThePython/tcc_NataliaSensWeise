using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;
using UnityEngine.UI;

public class MeuCubo : MeuModelo
{
    public JSONObject props = new JSONObject();

    public Toggle ativo;
    public TMP_InputField nome;
    public TMP_InputField[] tamanhoCubo;
    public TMP_InputField[] posicaoCubo;
    public Material texturaPadrao;

    public void addProps(string nomePeca)
    {
        if (Global.propriedadePecas.ContainsKey(nomePeca))
        {
            var cubo = Global.propriedadePecas[nomePeca];
            props.Add("nome", cubo.Nome);

            JSONArray tamanho = new JSONArray();
            tamanho.Add("x", cubo.Tam.X);
            tamanho.Add("y", cubo.Tam.Y);
            tamanho.Add("z", cubo.Tam.Z);
            props.Add("tamanho", tamanho);

            JSONArray posicao = new JSONArray();
            posicao.Add("x", cubo.Pos.X);
            posicao.Add("y", cubo.Pos.Y);
            posicao.Add("z", cubo.Pos.Z);
            props.Add("posicao", posicao);

            props.Add("cor", cubo.Cor.ToString());
            if (cubo.Textura != null)
            {
                var textura = cubo.Textura.ToString().Replace(" (UnityEngine.Texture2D)", "");
                props.Add("textura", textura);
            }

            props.Add("ativo", cubo.Ativo);

            JSONArray posPeca = new JSONArray();
            posPeca.Add("x", this.transform.position.x);
            posPeca.Add("y", this.transform.position.y);
            posPeca.Add("z", this.transform.position.z);
            props.Add("posPeca", posPeca);
        }
        else
        {
            props.Add("nome", nomePeca);

            JSONArray tamanho = new JSONArray();
            tamanho.Add("x", "1");
            tamanho.Add("y", "1");
            tamanho.Add("z", "1");
            props.Add("tamanho", tamanho);

            JSONArray posicao = new JSONArray();
            posicao.Add("x", "0");
            posicao.Add("y", "0");
            posicao.Add("z", "0");
            props.Add("posicao", posicao);

            props.Add("ativo", true);

            JSONArray posPeca = new JSONArray();
            posPeca.Add("x", this.transform.position.x);
            posPeca.Add("y", this.transform.position.y);
            posPeca.Add("z", this.transform.position.z);
            props.Add("posPeca", posPeca);

            props.Add("cor", "RGBA(1.000, 1.000, 1.000, 1.000)");
        }
       
    }

    public JSONObject getProps()
    {
        return props;
    }
}
