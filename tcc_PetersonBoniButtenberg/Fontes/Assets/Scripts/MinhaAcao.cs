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

    public void addProps()
    {
        props.Add("nome", nome.text);
        props.Add("ativo", ativo.enabled);

        JSONArray vals = new JSONArray();
        vals.Add("x", valores[0].text);
        vals.Add("y", valores[1].text);
        vals.Add("z", valores[2].text);
        props.Add("valores", vals);
    }

    public JSONObject getProps()
    {
        return props;
    }
}
