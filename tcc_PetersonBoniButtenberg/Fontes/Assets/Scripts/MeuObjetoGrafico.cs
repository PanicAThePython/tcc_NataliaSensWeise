using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;
using UnityEngine.UI;

public class MeuObjetoGrafico : MonoBehaviour
{
    public JSONObject props = new JSONObject();

    public TMP_InputField nome;
    public Toggle ativo;

    //private JSONArray children = new JSONArray();

    public void addPropsObjetoGrafico()
    {
        props.Add("nome", nome.text);
        props.Add("ativo", ativo.enabled);
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }
}
