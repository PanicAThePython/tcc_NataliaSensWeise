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

    public void addProps()
    {
        props.Add("nome", nome.text);

        JSONArray tamanho = new JSONArray();
        tamanho.Add("x", tamanhoCubo[0].text);
        tamanho.Add("y", tamanhoCubo[1].text);
        tamanho.Add("z", tamanhoCubo[2].text);
        props.Add("tamanho", tamanho);

        JSONArray posicao = new JSONArray();
        posicao.Add("x", posicaoCubo[0].text);
        posicao.Add("y", posicaoCubo[1].text);
        posicao.Add("z", posicaoCubo[2].text);
        props.Add("posicao", posicao);

        if (Global.propriedadePecas.ContainsKey(nome.text))
        {
            props.Add("cor", Global.propriedadePecas[nome.text].Cor.ToString());
            //problema com a textura... n consegui definir uma pra ser padrão...
            if (Global.propriedadePecas[nome.text].Cor.ToString() == "RGBA(1.000, 1.000, 1.000, 1.000)") props.Add("textura", Global.propriedadePecas[nome.text].Textura.ToString());
        }

        props.Add("ativo", ativo.enabled);

        JSONArray posPeca = new JSONArray();
        posPeca.Add("x", this.transform.position.x);
        posPeca.Add("y", this.transform.position.y);
        posPeca.Add("z", this.transform.position.z);
        props.Add("posPeca", posPeca);
    }

    public JSONObject getProps()
    {
        return props;
    }
}
