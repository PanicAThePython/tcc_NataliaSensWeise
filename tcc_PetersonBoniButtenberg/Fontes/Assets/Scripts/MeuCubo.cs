using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;
using UnityEngine.UI;

public class MeuCubo : MonoBehaviour
{
    public JSONObject props = new JSONObject();

    //NÃO É ASSIM Q SE SALVA COR E TEXTURA! COMO Q É???
    public Material textura;
    public Material cor;
    public Toggle ativo;
    public TMP_InputField nome;
    public TMP_InputField[] tamanhoCubo;
    public TMP_InputField[] posicaoCubo;

    public void AddPropsCubo()
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

        props.Add("cor", cor.color.ToString());
        props.Add("textura", textura.color.ToString());
        props.Add("ativo", ativo.enabled);
    }
}
