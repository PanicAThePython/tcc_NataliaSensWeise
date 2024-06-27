using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class MeuModelo : MonoBehaviour
{
    public JSONObject props;

    public string ConverterNomes(string nomePeca)
    {
        return nomePeca;
    }
    public void addProps(string nome)
    {
    }

    public JSONObject getProps()
    {
        return props;
    }
}
