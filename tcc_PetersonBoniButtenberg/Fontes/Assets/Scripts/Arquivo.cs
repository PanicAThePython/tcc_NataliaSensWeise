using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using TMPro;

public class Arquivo : MonoBehaviour
{
    public static JSONObject cena = new JSONObject();
    public GameObject mensagem;
    public TMP_InputField cenaJSON;

    MeuObjetoGrafico objetoAtual = new MeuObjetoGrafico();
    string nomeObjetoAtual = "";

    List<GameObject> ordenarCena()
    {
		for (int j = Global.listaObjetos.Count - 1; j > 0; j--)
		{
			for (int i = 0; i < j; i++)
			{
				if (Global.listaObjetos[i].transform.position.y < Global.listaObjetos[i + 1].transform.position.y)
				    trocarPosicao(Global.listaObjetos, i, i + 1);
            }
		}
		return Global.listaObjetos;
    }

    List<GameObject> trocarPosicao(List<GameObject> array, int m, int n)
    {
        GameObject temp;
        temp = array[m];
        array[m] = array[n];
        array[n] = temp;
        return array;
    }
    
    JSONArray limpandoListaChildren()
    {
        return new JSONArray();
    }
    
    void adicionarCameraNoJSON(int l)
    {
        var camera = Global.listaObjetos[l].GetComponent<MinhaCamera>();
        camera.addProps();
        cena.Add(Global.listaObjetos[l].name, camera.getProps());
    }

    void adicionarObjetoGraficoNoJSON(int l)
    {
        if (nomeObjetoAtual.Length > 0) {
            cena.Add(nomeObjetoAtual, objetoAtual.getProps());
            objetoAtual.setChildren(limpandoListaChildren());
        }
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

    //faltou exportar a textura!!!!!!!!!
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
        
        //remover chaves repetidas!!!!!!!!!!!!
        objetoAtual.setChildren(limpandoListaChildren());

        //começar novo ObjetoGráfico
        if (nomeObjetoAtual.Length > 0) cena.Add(nomeObjetoAtual, objetoAtual.getProps());

        //salvando arquivo
        string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "arquivoGRADE.json");
        File.WriteAllText(path, cena.ToString());
        
        //informando usuário do salvamento
        mensagem.SetActive(true);
        StartCoroutine(TutorialNovo.apagarTela(mensagem));
    }

    public void Importar()
    {
        JSONObject cenaImportada = (JSONObject) JSONObject.Parse(cenaJSON.text);
        //string chaves = Object.(cenaImportada);
        //fazer um foreach, pegar a key, e já ir criando os objetos
        //ele só vai conseguir acessar os de primeiro nível, ent vou ter que armazenar os OG pra dps acessar os filhos
        //e quando estiver acessando os OGs, adicionar o OG e aí os filhos em seguida na lista de objetos global
        //talvez transformar todas as peças em prefabs, colocar como props do Arquivo e quando tiver que criar, pedir pra criar uma cópia 
        //a partir delas
        //para instanciar as peças: Instantiate(nomePrefab, new Vector3(0,0,0), Quaternion.identity);
        //ir atualizando a lista Global.listObjectCount, pros nomes ficarem com os números atualizados, pra n gerar conflito
        foreach (KeyValuePair<string, JSONNode> entry in cenaImportada)
        {
            var key = entry.Key;
            var value = entry.Value;
            if (key == "CameraP") { }
            if (key.Contains("Objeto")) { }
        }

        //decompor objeto
        //com base na chave, vou criar objetos 
        //dentro deles, acessar o <Meu...> de cada um e setar as props
        //vou ter que criar slots tbm
    }
}
