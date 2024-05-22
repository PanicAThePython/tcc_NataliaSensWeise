using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using TMPro;
using System;
using System.Globalization;
using UnityEngine.UI;

public class Arquivo : MonoBehaviour
{
    public static JSONObject cena = new JSONObject();
    public static bool importando = false;
    public bool limpou = false;
    public GameObject[] mensagens;
    public TMP_InputField cenaJSON;
    public GameObject[] pecasPrefabs;

    public GameObject Ambiente;
    public GameObject Directional;
    public GameObject Point;
    public GameObject Spot;

    public GameObject[] texturas;

    private int numObjetoAtual = 0;

    MeuObjetoGrafico objetoAtual;
    MeuObjetoGrafico objetoPai = null;
    string nomeObjetoAtual = "";
    string nomeObjetoPai = "";

    void setImportando(bool val)
    {
        importando = val;
    }

    public List<GameObject> ordenarCena(List<GameObject> lista)
    {
        for (int j = lista.Count - 1; j > 0; j--)
        {
            for (int i = 0; i < j; i++)
            {
                if (lista[i].transform.position.y < lista[i + 1].transform.position.y)
                    trocarPosicao(lista, i, i + 1);
            }
        }
        return Global.listaObjetos;
    }

    public List<GameObject> trocarPosicao(List<GameObject> array, int m, int n)
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

    void adicionarCameraNoJSON(int l, List<GameObject> listaOrdenada)
    {
        var camera = listaOrdenada[l].GetComponent<MinhaCamera>();
        camera.addProps();
        cena.Add(listaOrdenada[l].name, camera.getProps());
    }

    void adicionarObjetoGraficoNoJSON(int l, List<GameObject> listaOrdenada)
    {
        if (numObjetoAtual % 2 == 0 && objetoPai != null)
        {
            JSONObject filho = new JSONObject();
            filho.Add(nomeObjetoAtual, objetoAtual.getProps());
            objetoPai.addChildren(filho);
            cena.Add(nomeObjetoPai, objetoPai.getProps());
            objetoPai.setChildren(limpandoListaChildren());
        }

        listaOrdenada[l].GetComponent<MeuObjetoGrafico>().addProps(listaOrdenada[l].name);
        objetoAtual = listaOrdenada[l].GetComponent<MeuObjetoGrafico>();
        nomeObjetoAtual = listaOrdenada[l].name;
        if (numObjetoAtual % 2 == 0)
        {
            nomeObjetoPai = nomeObjetoAtual;
            objetoPai = objetoAtual;
        }
    }

    void adicionarIluminacaoNoJSON(int l, List<GameObject> listaOrdenada)
    {
        var luz = listaOrdenada[l].GetComponent<MinhaIluminacao>();
        luz.addProps(listaOrdenada[l].name);
        var propsLuz = luz.getProps();
        JSONObject filho = new JSONObject();
        filho.Add(listaOrdenada[l].name, propsLuz);
        objetoAtual.addChildren(filho);
    }

    void adicionarCuboNoJSON(int l, List<GameObject> listaOrdenada)
    {
        var cubo = listaOrdenada[l].GetComponent<MeuCubo>();
        cubo.addProps(listaOrdenada[l].name);
        var propsCubo = cubo.getProps();
        JSONObject filho = new JSONObject();
        var nome = cubo.ConverterNomes(listaOrdenada[l].name);
        filho.Add(nome, propsCubo);
        objetoAtual.addChildren(filho);
    }

    void adicionarAcoesNoJSON(int l, List<GameObject> listaOrdenada)
    {
        var acao = listaOrdenada[l].GetComponent<MinhaAcao>();
        acao.addProps(listaOrdenada[l].name);
        var propsAcao = acao.getProps();
        JSONObject filho = new JSONObject();
        var nome = acao.ConverterNomes(listaOrdenada[l].name);
        filho.Add(nome, propsAcao);
        objetoAtual.addChildren(filho);
    }

    public string GetPath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    public void Exportar()
    {
        if (Global.listaObjetos != null && Global.listaObjetos.Count > 0)
        {
            List<GameObject> ordenada = ordenarCena(Global.listaObjetos);

            for (int l = 0; l < ordenada.Count; l++)
            {
                if (ordenada[l].name.Contains("Camera"))
                {
                    adicionarCameraNoJSON(l, ordenada);
                }
                else if (ordenada[l].name.Contains("Objeto"))
                {
                    adicionarObjetoGraficoNoJSON(l, ordenada);
                    numObjetoAtual++;
                }
                else if (ordenada[l].name.Contains("Iluminacao"))
                {
                    adicionarIluminacaoNoJSON(l, ordenada);
                }
                else if (ordenada[l].name.Contains("Cubo"))
                {
                    adicionarCuboNoJSON(l, ordenada);
                }
                else
                {
                    adicionarAcoesNoJSON(l, ordenada);
                }
            }

            if (numObjetoAtual % 2 == 0 && objetoPai != null)
            {
                JSONObject filho = new JSONObject();
                filho.Add(nomeObjetoAtual, objetoAtual.getProps());
                objetoPai.addChildren(filho);
            }
            //remover chaves repetidas!!!!!!!!!!!!
            objetoAtual.setChildren(limpandoListaChildren());

            //come�ar novo ObjetoGr�fico
            if (nomeObjetoPai.Length > 0)
            {
                cena.Add(nomeObjetoPai, objetoPai.getProps());
                objetoPai.setChildren(limpandoListaChildren());
            }
            //if (nomeObjetoAtual.Length > 0) cena.Add(nomeObjetoAtual, objetoAtual.getProps());

            //salvando arquivo
            //string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "arquivoGRADE.json");
            //File.WriteAllText(GetPath("arquivoGRADE.json"), cena.ToString());

            //ele salva sim o arquivo em json qnd t� buildado, mas com o GetPath, n consigo achar onde q foi salvo
            //tentei salvar na �rea de trabalho, mas o path pela web fica "/home/web_user/Desktop/arquivoGRADE.json", da� d� erro
            //teria q ver como q ajeita o caminho qnd t� na web, mas por quest�es de tempo vou s� jogar o json na caixa de texto
            cenaJSON.text = cena.ToString();

            //informando usu�rio do salvamento
            //mensagem.SetActive(true); --> essa mensagem s� era necess�ria qnd eu criava um arquivo de fato, pq agr eu clico em 
            //exportar e j� aparece na caixa de texto, ent o usu�rio tem o retorno visual
            //StartCoroutine(TutorialNovo.apagarTela(mensagem));

        }
        else if (Global.listaObjetos == null || Global.listaObjetos.Count == 0)
        {
            mensagens[0].SetActive(true);
            StartCoroutine(TutorialNovo.apagarTela(mensagens[0]));
        }
    }

    // checar posicao da camera
    void setPropsCamera(Controller controller, JSONNode values, string nome)
    {

        PropriedadePeca prPeca = new PropriedadePeca();
        prPeca.Nome = values["nome"];
        prPeca.PodeAtualizar = true;
        prPeca.NomeCuboAmbiente = "CuboAmbiente";
        prPeca.NomeCuboVis = "CuboVis";
        prPeca.TipoLuz = 0;
        Global.propriedadePecas.Add(nome, prPeca);

        controller.abrePropriedade.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["nome"];

        controller.abrePropriedade.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][0];
        controller.abrePropriedade.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][1];
        controller.abrePropriedade.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][2];

        controller.abrePropriedade.transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<TMP_InputField>().text = values["lookAt"][0];
        controller.abrePropriedade.transform.GetChild(2).GetChild(1).GetChild(1).GetComponent<TMP_InputField>().text = values["lookAt"][1];
        controller.abrePropriedade.transform.GetChild(2).GetChild(2).GetChild(1).GetComponent<TMP_InputField>().text = values["lookAt"][2];

        controller.abrePropriedade.transform.GetChild(3).GetChild(1).GetComponent<TMP_InputField>().text = values["fov"];
        controller.abrePropriedade.transform.GetChild(4).GetChild(1).GetComponent<TMP_InputField>().text = values["near"];
        controller.abrePropriedade.transform.GetChild(5).GetChild(1).GetComponent<TMP_InputField>().text = values["far"];

        Global.propCameraGlobal.PosX = -float.Parse(values["posicao"][0], CultureInfo.InvariantCulture.NumberFormat);
        Global.propCameraGlobal.PosY = float.Parse(values["posicao"][1], CultureInfo.InvariantCulture.NumberFormat);
        Global.propCameraGlobal.PosZ = float.Parse(values["posicao"][2], CultureInfo.InvariantCulture.NumberFormat);

        Global.propCameraGlobal.FOV = new Vector2(float.Parse(values["fov"], CultureInfo.InvariantCulture.NumberFormat), float.Parse(values["fov"], CultureInfo.InvariantCulture.NumberFormat));
        
        Global.propCameraGlobal.LookAtX = -float.Parse(values["lookAt"][0], CultureInfo.InvariantCulture.NumberFormat);
        Global.propCameraGlobal.LookAtY = float.Parse(values["lookAt"][1], CultureInfo.InvariantCulture.NumberFormat);
        Global.propCameraGlobal.LookAtZ = float.Parse(values["lookAt"][2], CultureInfo.InvariantCulture.NumberFormat);
        
        Global.propCameraGlobal.Near = float.Parse(values["near"], CultureInfo.InvariantCulture.NumberFormat);
        Global.propCameraGlobal.Far = float.Parse(values["far"], CultureInfo.InvariantCulture.NumberFormat);

        GameObject.Find("CameraVisInferior").GetComponent<Camera>().fieldOfView = float.Parse(values["fov"], CultureInfo.InvariantCulture.NumberFormat);
        GameObject.Find("CameraVisInferior").GetComponent<Camera>().nearClipPlane = Global.propCameraGlobal.Near;
        GameObject.Find("CameraVisInferior").GetComponent<Camera>().farClipPlane = Global.propCameraGlobal.Far;

        //Atualiza posição da camera
        GameObject goCameraObj = GameObject.Find("CameraObjetoMain");
        goCameraObj.transform.position =
            new Vector3(Global.propCameraGlobal.PropInicial.PosX + Global.propCameraGlobal.PosX,
                        Global.propCameraGlobal.PropInicial.PosY + Global.propCameraGlobal.PosY,
                        Global.propCameraGlobal.PropInicial.PosZ + Global.propCameraGlobal.PosZ);

        GameObject goCameraPos = GameObject.Find("CameraObjectPos");
        goCameraPos.transform.localPosition =
            new Vector3(-Global.propCameraGlobal.PosX * 14,
                        Global.propCameraGlobal.PosY * 13.333333f,
                        -Global.propCameraGlobal.PosZ * 16);

        goCameraObj.transform.GetChild(0).transform.LookAt(GameObject.Find("CuboAmb").transform, new Vector3(Global.propCameraGlobal.LookAtX, Global.propCameraGlobal.LookAtY, Global.propCameraGlobal.LookAtZ));
        //Atualiza FOV da camera (Scale)
        goCameraObj.transform.localScale =
            new Vector3(Global.propCameraGlobal.PropInicial.FOV.x * Global.propCameraGlobal.FOV.x,
                        Global.propCameraGlobal.PropInicial.FOV.y * Global.propCameraGlobal.FOV.y,
                        goCameraObj.transform.localScale.z);
    }


    void setPropsObjetoGrafico(Controller controller, JSONNode values, int countObjt, string nome)
    {
        controller.abrePropriedade.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["nome"];
        controller.abrePropriedade.transform.GetChild(2).GetComponent<Toggle>().isOn = bool.Parse(values["ativo"]);

        PropriedadePeca prPeca = new PropriedadePeca();
        prPeca.Nome = values["nome"];
        prPeca.PodeAtualizar = true;
        var cuboAmb = "CuboAmbiente";
        if (countObjt > 0) cuboAmb += countObjt;
        prPeca.NomeCuboAmbiente = cuboAmb;
        var cuboVis = "CuboVis";
        if (countObjt > 0) cuboVis += countObjt;
        prPeca.NomeCuboVis = cuboVis;
        prPeca.TipoLuz = 0;
        Global.propriedadePecas.Add(nome, prPeca);

        Global.propriedadePecas[nome].Ativo = bool.Parse(values["ativo"]);
        //ele até desativa a visualização, mas o toggle tá true

        // Nome.
        GameObject.Find("PropObjGrafico").transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["nome"];

        // Toggle.
        GameObject.Find("PropObjGrafico").transform.GetChild(2).GetComponent<Toggle>().isOn = Global.propriedadePecas[nome].Ativo;

       // GameObject.Find("PropObjGrafico").GetComponent<PropObjetoGraficoScript>().AtualizaCubo(values["ativo"], nome);
       // GameObject.Find("PropObjGrafico").GetComponent<PropObjetoGraficoScript>().toggleField.isOn = bool.Parse(values["ativo"]);
    }

    void setPropsCubo(Controller controller, JSONNode values, int countObjt, string nome)
    {
        PropriedadePeca prPeca = new PropriedadePeca();
        prPeca.Nome = nome;
        prPeca.PodeAtualizar = true;
        var cuboAmb = "CuboAmbiente";
        if (countObjt > 0) cuboAmb += countObjt;
        prPeca.NomeCuboAmbiente = cuboAmb;
        var cuboVis = "CuboVis";
        if (countObjt > 0) cuboVis += countObjt;
        prPeca.NomeCuboVis = cuboVis;
        prPeca.TipoLuz = 0;

        prPeca.Ativo = bool.Parse(values["ativo"]);

        prPeca.Tam = new Tamanho();
        prPeca.Tam.X = float.Parse(values["tamanho"][0]);
        prPeca.Tam.Y = float.Parse(values["tamanho"][1]);
        prPeca.Tam.Z = float.Parse(values["tamanho"][2]);

        prPeca.Pos = new Posicao();
        prPeca.Pos.X = float.Parse(values["posicao"][0]);
        prPeca.Pos.Y = float.Parse(values["posicao"][1]);
        prPeca.Pos.Z = float.Parse(values["posicao"][2]);

        //como importar cor e textura???
        if (values["cor"])
        {
            var teste = values["cor"].ToString().Split(',');
            teste[0] = teste[0].Split('(')[1];
            teste[3] = teste[3].Split(')')[0];
            float pos0conv = float.Parse(teste[0]);
            float pos1conv = float.Parse(teste[1]);
            float pos2conv = float.Parse(teste[2]);
            float pos3conv = float.Parse(teste[3]);

            float[] colors = new float[4];
            colors[0] = pos0conv / 1000;
            colors[1] = pos1conv / 1000;
            colors[2] = pos2conv / 1000;
            colors[3] = pos3conv / 1000;
            prPeca.Cor = new Color(colors[0], colors[1], colors[2], colors[3]);
        }

        if (values["textura"])
        {
            for (int i = 0; i < texturas.Length; i++)
            {
                string textura = texturas[i].gameObject.GetComponent<MeshRenderer>().material.name;
                //aparentemente, nem sempre o material e a textura vão ter o mesmo nome... preciso renomear algumas coisas pra funcionar
                //grass vs grama

                if (textura.Contains(values["textura"]))
                {
                    prPeca.Textura = texturas[i].gameObject.GetComponent<MeshRenderer>().material.mainTexture;
                }
            }
        }

        Global.propriedadePecas.Add(nome, prPeca);

        GameObject.Find(prPeca.NomeCuboAmbiente).GetComponent<MeshRenderer>().materials[0].color = prPeca.Cor;
        GameObject.Find(prPeca.NomeCuboVis).GetComponent<MeshRenderer>().materials[0].color = prPeca.Cor;

        if (prPeca.Textura) //PAROU DE FUNCIONAR PQ?!
        {
            //Texturiza os cubos
            GameObject.Find(Global.propriedadePecas[prPeca.Nome].NomeCuboAmbiente).GetComponent<MeshRenderer>().materials[0].mainTexture = prPeca.Textura;
            GameObject.Find(Global.propriedadePecas[prPeca.Nome].NomeCuboVis).GetComponent<MeshRenderer>().materials[0].mainTexture = prPeca.Textura;
        }
        /*
        GameObject.Find(prPeca.NomeCuboAmbiente).transform.position = new Vector3(prPeca.Pos.X, prPeca.Pos.Y, prPeca.Pos.Z);
        GameObject.Find(prPeca.NomeCuboVis).transform.position = new Vector3(prPeca.Pos.X, prPeca.Pos.Y, prPeca.Pos.Z);

        GameObject.Find(prPeca.NomeCuboAmbiente).transform.localScale = new Vector3(prPeca.Tam.X, prPeca.Tam.Y, prPeca.Tam.Z);
        GameObject.Find(prPeca.NomeCuboVis).transform.localScale = new Vector3(prPeca.Tam.X, prPeca.Tam.Y, prPeca.Tam.Z);
        */
    }

    void setPropsAcoes(Controller controller, JSONNode values, int countObjt, string nome)
    {
        controller.abrePropriedade.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = nome;

        

        controller.abrePropriedade.transform.GetChild(3).GetComponent<Toggle>().isOn = values["ativo"];

        PropriedadePeca prPeca = new PropriedadePeca();
        prPeca.Nome = nome;
        prPeca.PodeAtualizar = true;
        var nomeAmb = nome + "Amb";
        if (countObjt > 0) nomeAmb += countObjt;
        prPeca.NomeCuboAmbiente = nomeAmb;
        var nomeVis = nome + "Vis";
        if (countObjt > 0) nomeVis += countObjt;
        prPeca.NomeCuboVis = nomeVis;
        prPeca.TipoLuz = 0;

        prPeca.Ativo = bool.Parse(values["ativo"]);

        if (nome.Contains("Escal"))
        {
            prPeca.Tam = new Tamanho();
            prPeca.Tam.X = float.Parse(values["valores"][0]);
            prPeca.Tam.Y = float.Parse(values["valores"][1]);
            prPeca.Tam.Z = float.Parse(values["valores"][2]);
            controller.abrePropriedade.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = prPeca.Tam.X.ToString();
            controller.abrePropriedade.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = prPeca.Tam.Y.ToString();
            controller.abrePropriedade.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = prPeca.Tam.Z.ToString();
        }
        else
        {
            prPeca.Pos = new Posicao();
            prPeca.Pos.X = float.Parse(values["valores"][0]);
            prPeca.Pos.Y = float.Parse(values["valores"][1]);
            prPeca.Pos.Z = float.Parse(values["valores"][2]);
            controller.abrePropriedade.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = prPeca.Pos.X.ToString();
            controller.abrePropriedade.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = prPeca.Pos.Y.ToString();
            controller.abrePropriedade.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = prPeca.Pos.Z.ToString();
        }
        Global.propriedadePecas.Add(nome, prPeca);
    }

    void setPropsIluminacao(Controller controller, JSONNode values, string nome)
    {
        PropriedadePeca[] pecas = new PropriedadePeca[4];

        for (int i = 0; i < pecas.Length; i++)
        {
            PropriedadePeca peca = new PropriedadePeca();

            peca.Pos = new Posicao();
            peca.ValorIluminacao = new ValorIluminacao();

            peca.Pos.X = 100;
            peca.Pos.Y = 300;
            peca.Pos.Z = 0;
            peca.Cor = Color.white;
            peca.Ativo = true;
            peca.Intensidade = 1.5f;
            peca.ValorIluminacao.X = 0;
            peca.ValorIluminacao.Y = 0;
            peca.ValorIluminacao.Z = 0;
            peca.Distancia = 1000;
            peca.Angulo = 30f;
            peca.Expoente = 10f;

            pecas[i] = peca;
        }

        PropriedadePeca prPeca = new PropriedadePeca();
        prPeca.Nome = nome;
        prPeca.PodeAtualizar = true;
        var nomeAmb = values["nome"] + "Amb";
        prPeca.NomeCuboAmbiente = nomeAmb;
        var nomeVis = values["nome"] + "Vis";
        prPeca.NomeCuboVis = nomeVis;
        prPeca.Ativo = bool.Parse(values["ativo"]);

        prPeca.Pos = new Posicao();
        prPeca.Pos.X = float.Parse(values["posicao"][0]);
        prPeca.Pos.Y = float.Parse(values["posicao"][1]);
        prPeca.Pos.Z = float.Parse(values["posicao"][2]);

        if (values["cor"])
        {
            var teste = values["cor"].ToString().Split(',');
            teste[0] = teste[0].Split('(')[1];
            teste[3] = teste[3].Split(')')[0];
            float pos0conv = float.Parse(teste[0]);
            float pos1conv = float.Parse(teste[1]);
            float pos2conv = float.Parse(teste[2]);
            float pos3conv = float.Parse(teste[3]);

            float[] colors = new float[4];
            colors[0] = pos0conv / 1000;
            colors[1] = pos1conv / 1000;
            colors[2] = pos2conv / 1000;
            colors[3] = pos3conv / 1000;
            prPeca.Cor = new Color(colors[0], colors[1], colors[2], colors[3]);
        }
        //else prPeca.Cor = Color.white;

        string tipoLuz = values["tipoLuz"].ToString();
        if (tipoLuz.Contains("Ambiente")) prPeca.TipoLuz = 0; //2000x0
        else if (tipoLuz.Contains("Directional"))//2100x0
        {
            prPeca.Intensidade = float.Parse(values["intensidade"]);
            ValorIluminacao val = new ValorIluminacao();
            val.X = float.Parse(values["valores"][0]);
            val.Y = float.Parse(values["valores"][1]);
            val.Z = float.Parse(values["valores"][2]);
            prPeca.ValorIluminacao = val;
            prPeca.TipoLuz = 1;

        }
        else if (tipoLuz.Contains("Point")) //2200x0
        {
            prPeca.Intensidade = float.Parse(values["intensidade"]);
            prPeca.Distancia = float.Parse(values["distancia"]);
            prPeca.TipoLuz = 2;

        }
        else if (tipoLuz.Contains("Spot")) //2300x0
        {
            prPeca.Intensidade = float.Parse(values["intensidade"]);
            prPeca.Distancia = float.Parse(values["distancia"]);
            prPeca.Angulo = float.Parse(values["angulo"]);
            prPeca.Expoente = float.Parse(values["expoente"]);

            ValorIluminacao val1 = new ValorIluminacao();
            val1.X = float.Parse(values["valores"][0]);
            val1.Y = float.Parse(values["valores"][1]);
            val1.Z = float.Parse(values["valores"][2]);
            prPeca.ValorIluminacao = val1;

            prPeca.TipoLuz = 3;
        }

        pecas[prPeca.TipoLuz] = prPeca;
        Global.propriedadeIluminacao.Add(nome, pecas);
        prPeca.JaInstanciou = true;
        Global.propriedadePecas.Add(nome, prPeca);
        if (Global.gameObjectName == null) Global.gameObjectName = prPeca.Nome;
        GameObject.Find("PropIluminacao").GetComponent<PropIluminacaoPadrao>().preencheCamposIluminacao(prPeca.TipoLuz);
        PropTipoLuz ptl = new PropTipoLuz();
        ptl.AdicionaValorPropriedade(prPeca.TipoLuz, Ambiente, Directional, Point, Spot);

    }

    void adicionarEncaixe(GameObject peca, int countAcoes = 0)
    {
        Controller control = peca.GetComponent<Controller>();
        foreach (KeyValuePair<string, float> slot in Global.listaPosicaoSlot)  // Slot / posição no eixo y
        {
            string key = slot.Key;
            if (numObjetoAtual > 0) key += numObjetoAtual;

            List<string> preenchidos = new List<string>();

            foreach (KeyValuePair<string, string> enc in Global.listaEncaixes) //peça /slot
            {
                if (Equals(enc.Value, slot.Key)) {
                    preenchidos.Add(slot.Key); //se o slot é igual ao valor, ent já tem peça no slot
                }
            }

            //se o slot contem parte do nome padrão daqla peça e a peça ainda não foi encaixada
            if (slot.Key.Contains(Global.GetSlot(peca.name)) && !Global.listaEncaixes.ContainsKey(peca.name))
            {
                //se o slot tá visível, adiciona no encaixe
                if (GameObject.Find(slot.Key) != null && !preenchidos.Contains(slot.Key))
                    Global.listaEncaixes.Add(GameObject.Find(peca.name).name, key);
                else if (Global.listaEncaixes.ContainsKey(peca.name) //se já contém a peça, mas o slot armazenado é diferente do atual, reposiciona slots
                        && Global.listaEncaixes[peca.name] != key)
                {
                    control.reposicionaSlots(Global.listaEncaixes[peca.name], key);
                }
            }
        }
    }
    int[] adicionarCriancas(JSONArray children, int countObjt, int countTrans, int countRot, int countEsc, int countAcoes)
    {
        foreach (KeyValuePair<string, JSONNode> son in children)
        {
            foreach (KeyValuePair<string, JSONNode> props in son.Value)
            {
                var key = props.Key;
                var value = props.Value;
                float x = value["posPeca"][0];
                float z = value["posPeca"][2];

                if (key.Contains("Cubo"))
                {
                    var nome = "Cubo";
                    if (DropPeca.countObjetosGraficos > 0) nome += DropPeca.countObjetosGraficos;
                    var cubo = GameObject.Find(nome);
                    var controller = cubo.GetComponent<Controller>();
                    controller.GeraCopiaPeca();
                    var nomeSlot = "FormasSlot";

                    string numObjetoGrafico = countObjt.ToString();
                    if (numObjetoGrafico == "0") numObjetoGrafico = "";
                    if (countObjt > 0)
                    {
                        nomeSlot += countObjt;
                        //if (numObjetoGrafico == "") numObjetoGrafico = countObjt.ToString();
                    }

                    if (GameObject.Find("CuboAmbiente" + numObjetoGrafico) != null)
                    {
                        float y = GameObject.Find(nomeSlot).gameObject.transform.position.y;

                        cubo.transform.position = new Vector3(x, y, z);
                        cubo.GetComponent<BoxCollider>().enabled = true;
                        cubo.GetComponent<Controller>().posicaoColliderDestino = GameObject.Find(nomeSlot).gameObject;
                        Global.addObject(cubo);
                        adicionarEncaixe(cubo);

                        string numFormas = "";
                        GameObject t;

                        numFormas = controller.getNumeroSlotObjetoGrafico();

                        t = GameObject.Find("FormasSlot" + numFormas);

                        controller.posicaoColliderDestino = t;
                        controller.adicionaObjetoRender();

                        if (GameObject.Find(Global.listaEncaixes[nome]) != null)
                        {
                            if (Global.cameraAtiva && new PropIluminacaoPadrao().existeIluminacao())
                                GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

                            // Verificar se o Objeto Gráfico pai está ativo para demonstrar o cubo.
                            string goObjGraficoSlot = GameObject.Find(Global.listaEncaixes[nome]).transform.parent.name;

                            string peca = string.Empty;

                            // Verifica peça conectada ao slot
                            foreach (KeyValuePair<string, string> pecas in Global.listaEncaixes)
                            {
                                if (Equals(pecas.Value, goObjGraficoSlot))
                                {
                                    peca = pecas.Key;
                                    break;
                                }
                            }

                            MeshRenderer mr = GameObject.Find("CuboAmbiente" + controller.getNumeroSlotObjetoGrafico()).GetComponent<MeshRenderer>();

                            bool statusCubo = false;

                            // Verifica se o Objeto Gráfico ja foi clicado.
                            foreach (KeyValuePair<string, PropriedadePeca> pec in Global.propriedadePecas)
                            {
                                if (Equals(pec.Key, peca))
                                {
                                    if (Global.propriedadePecas[peca].Ativo)
                                        statusCubo = true;
                                }
                                else
                                    statusCubo = true;
                            }

                            if (statusCubo || Global.propriedadePecas.Count == 0)
                                mr.enabled = true;

                            #region Código antigo             

                            foreach (KeyValuePair<string, string> slot in Global.listaEncaixes)
                            {
                                if (Equals(slot.Value, "IluminacaoSlot" + controller.getNumeroSlotObjetoGrafico()))
                                {
                                    mr = GameObject.Find("CuboVis" + controller.getNumeroSlotObjetoGrafico()).GetComponent<MeshRenderer>();

                                    if (Global.cameraAtiva)
                                        mr.enabled = true;

                                    Global.cuboVisComIluminacao.Add(mr.name);
                                    break;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            GameObject.Find("CuboAmbiente" + controller.getNumeroSlotObjetoGrafico()).GetComponent<MeshRenderer>().enabled = true;
                        }
                        setPropsCubo(controller, value, countObjt, nome);
                    }
                }
                if (key.Contains("Ilumi"))
                {
                    var nome = "Iluminacao";
                    if (DropPeca.countObjetosGraficos - 2 > 0) nome += DropPeca.countObjetosGraficos - 2;
                    var nomeSlot = "IluminacaoSlot";
                    if (countObjt > 0)
                    {
                        nomeSlot += countObjt;
                    }

                    var luz = GameObject.Find(nome);
                    var controller = luz.GetComponent<Controller>();
                    controller.GeraCopiaPeca();

                    float y = GameObject.Find(nomeSlot).gameObject.transform.position.y;
                    luz.transform.position = new Vector3(x, y, z); 
                    luz.GetComponent<BoxCollider>().enabled = true;
                    Global.addObject(luz);
                    adicionarEncaixe(luz);

                    var iluminSlot = "IluminacaoSlot";
                    if (countObjt > 0) iluminSlot += countObjt;
                    GameObject t = GameObject.Find(iluminSlot);

                    controller.posicaoColliderDestino = t;
                    controller.adicionaObjetoRender();

                    //Verifica se há câmera e aplica luz aos objetos com Layer "Formas"
                    if (Global.cameraAtiva)
                        GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

                    PropriedadePeca prPeca = new PropriedadePeca();
                    prPeca.Nome = gameObject.name;
                    prPeca.PodeAtualizar = true;
                    prPeca.NomeCuboAmbiente = "CuboAmbiente";
                    prPeca.NomeCuboVis = "CuboVis";
                    if (countObjt > 0)
                    {
                        prPeca.NomeCuboAmbiente += countObjt;
                        prPeca.NomeCuboVis += countObjt;
                    }

                    prPeca.TipoLuz = 0;
                    Global.propriedadePecas.Add(gameObject.name, prPeca);

                    if (gameObject.name.Length > "Iluminacao".Length)
                        controller.CriaLightObject(gameObject.name.Substring("Iluminacao".Length, 1));

                    setPropsIluminacao(controller, value, nome);
                }
                if (key.Contains("Trans") || key.Contains("Rot") || key.Contains("Esc"))
                {
                    var nome = "";

                    if (key.Contains("Trans")) nome += "Transladar";
                    if (key.Contains("Rot")) nome += "Rotacionar";
                    if (key.Contains("Esc")) nome += "Escalar";

                    if (countTrans > 0 && key.Contains("Trans")) nome += countTrans;
                    if (countRot > 0 && key.Contains("Rot")) nome += countRot;
                    if (countEsc > 0 && key.Contains("Esc")) nome += countEsc;

                    var acao = GameObject.Find(nome);
                    var controller = acao.GetComponent<Controller>();
                    controller.GeraCopiaPeca();

                    string numObjetoGrafico = controller.getNumeroSlotObjetoGrafico();
                    GameObject ObjGrafSlot = GameObject.Find("ObjGraficoSlot");
                    if (countObjt > 0)
                    {
                        ObjGrafSlot = GameObject.Find("ObjGraficoSlot" + countObjt);
                        if (numObjetoGrafico == "") numObjetoGrafico = countObjt.ToString();
                    }

                    string slot = "";
                    var nomeSlot = "TransformacoesSlot";
                    if (countObjt > 0) nomeSlot += countObjt;
                    if (countAcoes > 0) nomeSlot += "_" + countAcoes;

                    float y = GameObject.Find(nomeSlot).gameObject.transform.position.y;
                    acao.transform.position = new Vector3(x, y, z);
                    acao.GetComponent<BoxCollider>().enabled = true;
                    Global.addObject(acao);
                    adicionarEncaixe(acao, countAcoes);

                    //Retorna  de TransformacoesSlot
                    for (int i = 0; i < ObjGrafSlot.transform.childCount; i++)
                    {
                        if (ObjGrafSlot.transform.GetChild(i).name.Contains("TransformacoesSlot"))
                        {
                            if (Global.listaEncaixes.ContainsValue(ObjGrafSlot.transform.GetChild(i).name))
                            {
                                slot = ObjGrafSlot.transform.GetChild(i).name;
                            }
                        }
                    }

                    if (Tutorial.estaExecutandoTutorial)
                        slot = "TransformacoesSlot";

                    int val = 0;
                    string countTransformacoes = "";
                    Int32.TryParse(slot.Substring(slot.IndexOf("_") + 1), out val);

                    if (countAcoes > 0)
                        countTransformacoes = Convert.ToString(countAcoes + 1);
                    else
                        countTransformacoes = "1";
                    //-------------

                    GameObject t = GameObject.Find(nomeSlot);
                    GameObject cloneTrans = Instantiate(t, t.transform.position, t.transform.rotation, t.transform.parent);
                    cloneTrans.name = "TransformacoesSlot" + numObjetoGrafico + "_" + countTransformacoes;
                    cloneTrans.transform.position = new Vector3(t.transform.position.x, t.transform.position.y - 3f, t.transform.position.z);

                    controller.addTransformacoeSequenciaSlots(cloneTrans.name);
                    Global.atualizaListaSlot();
                    controller.posicaoColliderDestino = t;

                    GameObject.Find(cloneTrans.name).GetComponent<BoxCollider>().enabled = true;

                    if (controller.renderController == null)
                        controller.renderController = new RenderController();

                    controller.renderController.ResizeBases(t, Consts.Transladar, true); // o Segundo parâmetro pode ser qualquer tranformação 

                    controller.concatNumber = numObjetoGrafico;

                    if (!Tutorial.estaExecutandoTutorial)
                    {
                        controller.addGameObjectTree("GameObjectAmb" + numObjetoGrafico, "Amb", "CuboAmbiente" + numObjetoGrafico);
                        controller.addGameObjectTree("CuboVisObject" + numObjetoGrafico, "Vis", "CuboVis" + numObjetoGrafico);
                    }

                    controller.configuraIluminacao("-");

                    controller.reorganizaObjetos(numObjetoGrafico);
                    if (key.Contains("Trans")) countTrans++;
                    if (key.Contains("Rot")) countRot++;
                    if (key.Contains("Esc")) countEsc++;
                    setPropsAcoes(controller, value, countObjt, nome);
                    countAcoes++;
                }
                if (key.Contains("Objeto"))
                {
                    countObjt++;
                    countAcoes = 0;
                    var nome = "ObjetoGraficoP";
                    var nomeSlot = "ObjGraficoSlot";
                    if (countObjt > 0)
                    {
                        nomeSlot += countObjt;
                        nome += countObjt;
                    }
                    
                    var objeto = GameObject.Find(nome);
                    var controller = objeto.GetComponent<Controller>();
                    controller.GeraCopiaPeca();

                    float y = GameObject.Find(nomeSlot).gameObject.transform.position.y;

                    objeto.transform.position = new Vector3(x, y, z);
                    objeto.GetComponent<BoxCollider>().enabled = true;
                    if (objeto != null) Global.addObject(objeto);

                    DropPeca.countObjetosGraficos = countObjt;
                    if (DropPeca.countObjetosGraficos == 0)
                    {
                        if (controller.criaFormas == null)
                            controller.criaFormas = new Util_VisEdu();

                        controller.criaFormas.criaFormasVazias();
                    }

                    string countObjGrafico = "";
                    if (DropPeca.countObjetosGraficos > 0)
                        countObjGrafico = Convert.ToString(DropPeca.countObjetosGraficos);

                    if (!Equals(countObjGrafico, string.Empty))
                        controller.createGameObjectTree(DropPeca.countObjetosGraficos);

                    Global.iniciaListaSequenciaSlots(DropPeca.countObjetosGraficos);

                    GameObject t;
                    if (limpou) t = GameObject.Find("Render").transform.GetChild(1).gameObject;
                    else t = GameObject.Find("ObjGraficoSlot" + countObjGrafico);
                    //qnd estiver importando dps da limpeza, pegar o filho index 1 do render para slot

                    DropPeca.countObjetosGraficos++;
                    GameObject cloneObjGrafico = GameObject.Find("ObjGraficoSlot" + DropPeca.countObjetosGraficos);
                   // DropPeca.countObjetosGraficos += 2;
                   // cloneObjGrafico.name = "ObjGraficoSlot" + Convert.ToString(DropPeca.countObjetosGraficos);
                   // cloneObjGrafico.transform.position = new Vector3(t.transform.position.x, t.transform.position.y - 14f, t.transform.position.z);


                    objeto.GetComponent<Controller>().setActiveAndRenameGameObject(t, cloneObjGrafico);
                    controller.posicaoColliderDestino = t;

                    if (controller.renderController == null)
                        controller.renderController = new RenderController();

                    controller.renderController.ResizeBases(t, Consts.ObjetoGrafico, true);

                    objeto.GetComponent<Controller>().adicionaObjetoRender();
                    Global.atualizaListaSlot();

                    adicionarEncaixe(objeto);
                    DropPeca.countObjetosGraficos -= 1;
                    if (value["children"].Count > 0)
                    {
                        if (countObjt == numObjetoAtual) countAcoes = 0;
                        int[] valores = adicionarCriancas((JSONArray)value["children"], countObjt, countTrans, countRot, countEsc, countAcoes);
                        countTrans = valores[0];
                        countRot = valores[1];
                        countEsc = valores[2];
                        countAcoes = valores[3];
                        numObjetoAtual++;
                    }

                    setPropsObjetoGrafico(controller, value, countObjt, nome);
                }
            }
        }

        int[] vals = new int[4];
        vals[0] = countTrans;
        vals[1] = countRot;
        vals[2] = countEsc;
        vals[3] = countAcoes;

        return vals;
    }

    public void Importar()
    {
        //TEM Q CHECAR SE A LISTA TA VAZIA, SENÃO DÁ ERRO!!!
        //limparListaObjetos();
        /*
         * tentei limpas a cena, mas o slot de objeto para de funcionar, ent como alternativa vou pedir pro usuário limpar a cena
        if (Global.listaObjetos != null)
        {
            while (Global.listaObjetos.Count > 0)
            {
                var controller = Global.listaObjetos[0].GetComponent<Controller>();
                controller.processaExclusao(Global.listaObjetos[0].name, Global.listaObjetos[0]);
                //Global.atualizaListaSlot();
                //tá delentando, mas tá dando ruim qnd eu tento setar uma cena, dá ruim no slot de objt, ele n encaixa
            }
            Global.atualizaListaSlot();
            limpou = true;
        }
        */

        if (Global.listaObjetos != null)
        {
            mensagens[1].SetActive(true);
            StartCoroutine(TutorialNovo.apagarTela(mensagens[1]));
        }
       
        var countObjt = 0;
        var countTrans = 0;
        var countRot = 0;
        var countEsc = 0;
        var countAcoes = 0;
        JSONObject cenaImportada = null;
        if (cenaJSON.text.Length > 0) cenaImportada = (JSONObject)JSONObject.Parse(cenaJSON.text);
        //string chaves = Object.(cenaImportada);
        //fazer um foreach, pegar a key, e j� ir criando os objetos
        //ele s� vai conseguir acessar os de primeiro n�vel, ent vou ter que armazenar os OG pra dps acessar os filhos
        //e quando estiver acessando os OGs, adicionar o OG e a� os filhos em seguida na lista de objetos global
        //talvez transformar todas as pe�as em prefabs, colocar como props do Arquivo e quando tiver que criar, pedir pra criar uma c�pia 
        //a partir delas
        //para instanciar as pe�as: Instantiate(nomePrefab, new Vector3(0,0,0), Quaternion.identity);
        //ir atualizando a lista Global.listObjectCount, pros nomes ficarem com os n�meros atualizados, pra n gerar conflito


        if (cenaImportada != null && (Global.listaObjetos == null || Global.listaObjetos.Count == 0))
        {
            setImportando(true);
            foreach (KeyValuePair<string, JSONNode> entry in cenaImportada)
            {
                //o problema da rolagem tá no fato de q: eu to colocando as peças 1 no render e deixando as 0 na fabrica
                //o certo é o contrário!!
                var key = entry.Key;
                var value = entry.Value;

                float x = value["posPeca"][0];
                float z = value["posPeca"][2];

                //var contr; //essa linha às vezes dá erro sla pq
                //MissingReferenceException: The object of type 'GameObject' has been destroyed but you are still trying to access it.
                //Your script should either check if it is null or you should not destroy the object.
                //isso acontece pq, msm dps de deletar o objeto, a aba de props deixa as props do bonito lá

                //QUANDO ROLA A TELA, ESSES FICAM NAS POSI��ES!!!!!!!!!!!!!!!!!!!!
                if (key.Contains("CameraP"))
                {
                    //ele s� encaixa a pe�a, mas as funcs n v�o
                    //deu certo a ideia, mas parece q h� um problema com a prefab... ele t� vindo esticado e pequeno
                    //acredito q seja culpa do Quaternion.identity, q � (0.00000, 0.00000, 0.00000, 1.00000)
                    //rota��o � 90, 0, -180
                    //cubo e escalar tem 180 positivo!!!
                    
                    //var vetor = new Vector3(value["posPeca"][0], value["posPeca"][1], value["posPeca"][2]);
                    //GameObject instance = GameObject.Instantiate(pecasPrefabs[0]);
                    //instance.gameObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(90, 0, -180);
                    //instance.gameObject.transform.localScale = new Vector3(7.8f, 2f, 1f);
                    //Instantiate(instance, vetor, Quaternion.identity);
                    //Global.addObject(instance);
                    

                    //IDEIA: CHAMAR FUNC GERAR C�PIA E J� SETAR OS VALORES DA POSI��O, A� VAI SER UMA C�MERA Q FUNCIONA!!
                    //tem q acessar o prefab do objeto q qr gerar uma c�pia, acessar o controller e pegar a func de copiar
                    //FUNCIONOU
                    //FALTA O RETORNO VISUAL NO AMBIENTE GR�FICO E VISUALIZADOR(PROVAVELMENTE)!!

                    var nome = "CameraP"; //problema para importar cena dps de deletar uma... pq a key já foi usada e deletada, ent n existe mais uma "CameraP" em cena...
                    var cam = GameObject.Find(nome);
                    var controller = cam.GetComponent<Controller>();
                    controller.GeraCopiaPeca();

                    //DEU CERTO ASSIM!!! VEIO NO ENCAIXE!!
                    float y = GameObject.Find("CameraSlot").gameObject.transform.position.y;
                    //pegar nome prefab + numero e mudar posicao
                    cam.transform.position = new Vector3(x, y, z);
                    cam.GetComponent<BoxCollider>().enabled = true;
                    if (cam != null) Global.addObject(cam);

                    GameObject.Find("CameraObjetoMain").transform.GetChild(0).gameObject.SetActive(true);
                    //faz o visualizador dar retorno!!
                    Global.cameraAtiva = true;
                    //if (Global.cameraAtiva)
                    //    GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

                    Global.atualizaListaSlot();
                    adicionarEncaixe(cam);
                    setPropsCamera(controller, value, nome);
                }
                if (key.Contains("Objeto"))
                {
                    //ATUALMENTE TÁ QUEBRANDO QND TEM MAIS DE UM OBJT!!!!!!!!!!!!!!!!!!
                    //qnd eu gero a copia pro primeiro, dá certo pq tá naqla posicao na fabrica
                    //mas dps ele pega a posição do renderer, aí complica
                    //talvez se eu armazenar a posicao do prefab ANTES de alterar ele, eu posso setar ela na copia
                    //acredito tbm q esteja dando problema por conta da rolagem


                    //PROS FILHOS:
                    //função separada de foreach numa lista children
                    //acessar o children dentro do value, e qnd tiver algo, chamar a func e acessar e criar os objts filho
                    var nome = "ObjetoGraficoP";
                    var nomeSlot = "ObjGraficoSlot";
                    if (countObjt > 0)
                    { 
                        nomeSlot += countObjt;
                        nome += countObjt;
                    }
                    if (limpou) //tá chamando a aba lateral do render do objeto 2 ????
                    {
                        nomeSlot = GameObject.Find("Render").transform.GetChild(1).gameObject.name;
                        if (countObjt > 0)
                        {
                            string nomeFilho = GameObject.Find("Render").transform.GetChild(1).gameObject.name;
                            int num = int.Parse(nomeFilho[-1].ToString());
                            if (num != countObjt) nomeSlot = "ObjGraficoSlot" + countObjt;
                        }
                    }
                    var objeto = GameObject.Find(nome);
                    var controller = objeto.GetComponent<Controller>();
                    controller.GeraCopiaPeca();

                     float y = GameObject.Find(nomeSlot).gameObject.transform.position.y;

                    objeto.transform.position = new Vector3(x, y, z);
                    objeto.GetComponent<BoxCollider>().enabled = true;
                    if (objeto != null) Global.addObject(objeto);

                    if (DropPeca.countObjetosGraficos == 0)
                    {
                        if (controller.criaFormas == null)
                            controller.criaFormas = new Util_VisEdu();

                        controller.criaFormas.criaFormasVazias();
                    }

                    string countObjGrafico = "";
                    if (DropPeca.countObjetosGraficos > 0)
                        countObjGrafico = Convert.ToString(DropPeca.countObjetosGraficos);

                    if (!Equals(countObjGrafico, string.Empty))
                        controller.createGameObjectTree(DropPeca.countObjetosGraficos);

                    Global.iniciaListaSequenciaSlots(DropPeca.countObjetosGraficos);

                    GameObject t;
                    if (limpou) t = GameObject.Find("Render").transform.GetChild(1).gameObject;
                    else t = GameObject.Find("ObjGraficoSlot" + countObjGrafico);
                    //qnd estiver importando dps da limpeza, pegar o filho index 1 do render para slot

                    GameObject cloneObjGrafico = Instantiate(t, t.transform.position, t.transform.rotation, t.transform.parent);
                    DropPeca.countObjetosGraficos += 2;
                    cloneObjGrafico.name = "ObjGraficoSlot" + Convert.ToString(DropPeca.countObjetosGraficos);
                    cloneObjGrafico.transform.position = new Vector3(t.transform.position.x, t.transform.position.y - 14f, t.transform.position.z);


                    objeto.GetComponent<Controller>().setActiveAndRenameGameObject(t, cloneObjGrafico);
                    controller.posicaoColliderDestino = t;

                    if (controller.renderController == null)
                        controller.renderController = new RenderController();

                    controller.renderController.ResizeBases(t, Consts.ObjetoGrafico, true);

                    objeto.GetComponent<Controller>().adicionaObjetoRender();
                    Global.atualizaListaSlot();

                    adicionarEncaixe(objeto);
                    DropPeca.countObjetosGraficos = countObjt;
                    if (value["children"].Count > 0)
                    {
                        if (countObjt == numObjetoAtual) countAcoes = 0;
                        int[] valores = adicionarCriancas((JSONArray)value["children"], countObjt, countTrans, countRot, countEsc, countAcoes);

                        countTrans = valores[0];
                        countRot = valores[1];
                        countEsc = valores[2];
                        countAcoes = valores[3];
                        numObjetoAtual++;
                    }

                    setPropsObjetoGrafico(controller, value, countObjt, nome);
                    countObjt++;
                }
            }
            cenaJSON.text = "";
            //decompor objeto
            //com base na chave, vou criar objetos 
            //dentro deles, acessar o <Meu...> de cada um e setar as props
            //vou ter que criar slots tbm
        }
        else if (cenaImportada == null)
        {
            mensagens[0].SetActive(true);
            StartCoroutine(TutorialNovo.apagarTela(mensagens[0]));
        }
    }
}
