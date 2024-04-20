using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

public class Arquivo : MonoBehaviour
{
    public static JSONObject cena = new JSONObject();

    MeuObjetoGrafico objetoAtual = new MeuObjetoGrafico();
    string nomeObjetoAtual = "";
    void ordenarCena()
    {
        var temp = new GameObject();
        for (int t = 0; t < Global.listaObjetos.Count; t++)
        {
            if (t < Global.listaObjetos.Count - 1)
            {
                if (Global.listaObjetos[t].transform.position.y < Global.listaObjetos[t + 1].transform.position.y)
                {
                    temp = Global.listaObjetos[t];
                    Global.listaObjetos[t] = Global.listaObjetos[t + 1];
                    Global.listaObjetos[t + 1] = temp;
                }
            }
        }
    }

    void adicionarCameraNoJSON(int l)
    {
        var camera = Global.listaObjetos[l].GetComponent<MinhaCamera>();
        camera.addProps();
        cena.Add(Global.listaObjetos[l].name, camera.getProps());
    }

    void adicionarObjetoGraficoNoJSON(int l)
    {
        if (nomeObjetoAtual.Length > 0) cena.Add(nomeObjetoAtual, objetoAtual.getProps());
        Global.listaObjetos[l].GetComponent<MeuObjetoGrafico>().addProps("Objeto Gráfico");
        objetoAtual = Global.listaObjetos[l].GetComponent<MeuObjetoGrafico>();
        nomeObjetoAtual = Global.listaObjetos[l].name;
    }

    void adicionarIluminacaoNoJSON(int l)
    {
        var luz = Global.listaObjetos[l].GetComponent<MinhaIluminacao>();
        luz.addProps();
        var propsLuz = luz.getProps();
        if (propsLuz["nome"] == "")
        {
            JSONObject props = new JSONObject();
            props.Add("nome", "Iluminação");
            props.Add("tipoLuz", "Ambiente");

            JSONArray posicao = new JSONArray();
            posicao.Add("x", "100");
            posicao.Add("y", "300");
            posicao.Add("z", "0");
            props.Add("posicao", posicao);

            props.Add("ativo", true);
            propsLuz = props;
        }
        JSONObject filho = new JSONObject();
        filho.Add(Global.listaObjetos[l].name, propsLuz);
        objetoAtual.addChildren(filho);
    }

    void adicionarCuboNoJSON(int l)
    {
        var cubo = Global.listaObjetos[l].GetComponent<MeuCubo>();
        cubo.addProps();
        var propsCubo = cubo.getProps();
        if (propsCubo["nome"] == "")
        {
            JSONObject props = new JSONObject();
            props.Add("nome", "Cubo");

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
            propsCubo = props;
        }
        JSONObject filho = new JSONObject();
        filho.Add(Global.listaObjetos[l].name, propsCubo);
        objetoAtual.addChildren(filho);
    }

    void adicionarEscalaNoJSON(int l)
    {
        var acao = Global.listaObjetos[l].GetComponent<MinhaAcao>();
        acao.addProps();
        var propsAcao = acao.getProps();
        if (propsAcao["nome"] == "")
        {
            JSONObject props = new JSONObject();
            props.Add("nome", "Escalar");
            props.Add("ativo", true);

            JSONArray posicao = new JSONArray();
            posicao.Add("x", "1");
            posicao.Add("y", "1");
            posicao.Add("z", "1");
            props.Add("valores", posicao);

            propsAcao = props;
        }
        JSONObject filho = new JSONObject();
        filho.Add(Global.listaObjetos[l].name, propsAcao);
        objetoAtual.addChildren(filho);
    }

    void adicionarOutrasAcoesNoJSON(int l)
    {
        var acao = Global.listaObjetos[l].GetComponent<MinhaAcao>();
        acao.addProps();
        var propsAcao = acao.getProps();
        if (propsAcao["nome"] == "")
        {
            JSONObject props = new JSONObject();
            if (Global.listaObjetos[l].name.Contains("Trans")) props.Add("nome", "Transladar");
            else props.Add("nome", "Rotacionar");
            props.Add("ativo", true);

            JSONArray posicao = new JSONArray();
            posicao.Add("x", "0");
            posicao.Add("y", "0");
            posicao.Add("z", "0");
            props.Add("valores", posicao);

            propsAcao = props;
        }
        JSONObject filho = new JSONObject();
        filho.Add(Global.listaObjetos[l].name, propsAcao);
        objetoAtual.addChildren(filho);
    }

    public void Exportar()
    {
        ordenarCena();
        for (int l = 0; l < Global.listaObjetos.Count; l++)
        {
            if (Global.listaObjetos[l].name.Contains("Camera"))
            {
                adicionarCameraNoJSON(l);
            }
            else if (Global.listaObjetos[l].name.Contains("Objeto"))
            {
                adicionarObjetoGraficoNoJSON(l);
            }
            else if (Global.listaObjetos[l].name.Contains("Iluminacao"))
            {
                adicionarIluminacaoNoJSON(l);
            }
            else if (Global.listaObjetos[l].name.Contains("Cubo"))
            {
                adicionarCuboNoJSON(l);
            }
            else if (Global.listaObjetos[l].name.Contains("Escala"))
            {
                adicionarEscalaNoJSON(l);
            }
            else
            {
                adicionarOutrasAcoesNoJSON(l);
            }
        }
        if (nomeObjetoAtual.Length > 0) cena.Add(nomeObjetoAtual, objetoAtual.getProps());
        print(Application.persistentDataPath);
        string path = Application.persistentDataPath + "/teste.json";
        File.WriteAllText(path, cena.ToString());
    }
}
