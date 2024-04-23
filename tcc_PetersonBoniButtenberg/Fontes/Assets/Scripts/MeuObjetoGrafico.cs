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

    public void addProps(string n)
    {
        if (nome.text.Length == 0) props.Add("nome", n);
        else props.Add("nome", nome.text);

        props.Add("ativo", ativo.enabled);
        props.Add("children", children);

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

    public void setProps(JSONObject p)
    {
        props = p;
    }
}
