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
    public GameObject mensagem;
    public TMP_InputField cenaJSON;
    public GameObject[] pecasPrefabs;

    public GameObject Ambiente;
    public GameObject Directional;
    public GameObject Point;
    public GameObject Spot;

    public GameObject[] texturas;

    MeuObjetoGrafico objetoAtual;
    string nomeObjetoAtual = "";

    void setImportando(bool val)
    {
        importando = val;
    }

    List<GameObject> ordenarCena(List<GameObject> lista)
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

    void adicionarCameraNoJSON(int l, List<GameObject> listaOrdenada)
    {
        var camera = listaOrdenada[l].GetComponent<MinhaCamera>();
        camera.addProps();
        cena.Add(listaOrdenada[l].name, camera.getProps());
    }

    void adicionarObjetoGraficoNoJSON(int l, List<GameObject> listaOrdenada)
    {
        if (nomeObjetoAtual.Length > 0)
        {
            cena.Add(nomeObjetoAtual, objetoAtual.getProps());
            objetoAtual.setChildren(limpandoListaChildren());
        }
        listaOrdenada[l].GetComponent<MeuObjetoGrafico>().addProps(listaOrdenada[l].name);
        objetoAtual = listaOrdenada[l].GetComponent<MeuObjetoGrafico>();
        nomeObjetoAtual = objetoAtual.ConverterNomes(listaOrdenada[l].name);
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

        //remover chaves repetidas!!!!!!!!!!!!
        objetoAtual.setChildren(limpandoListaChildren());

        //come�ar novo ObjetoGr�fico
        if (nomeObjetoAtual.Length > 0) cena.Add(nomeObjetoAtual, objetoAtual.getProps());

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

    void limparListaObjetos()
    {
        if (Global.listaObjetos != null)
        {
            for (int l = 0; l < Global.listaObjetos.Count;)
            {
                Global.removeObject(Global.listaObjetos[l]);
            }

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

        Global.propCameraGlobal.PosX = float.Parse(values["posicao"][0], CultureInfo.InvariantCulture.NumberFormat);
        Global.propCameraGlobal.PosY = float.Parse(values["posicao"][1], CultureInfo.InvariantCulture.NumberFormat);
        Global.propCameraGlobal.PosZ = float.Parse(values["posicao"][2], CultureInfo.InvariantCulture.NumberFormat);
        Global.propCameraGlobal.FOV = new Vector2(float.Parse(values["fov"], CultureInfo.InvariantCulture.NumberFormat), float.Parse(values["fov"], CultureInfo.InvariantCulture.NumberFormat));
        Global.propCameraGlobal.LookAtX = float.Parse(values["lookAt"][0], CultureInfo.InvariantCulture.NumberFormat);
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

        GameObject.Find("PropObjGrafico").GetComponent<PropObjetoGraficoScript>().AtualizaCubo(values["ativo"], nome);
        GameObject.Find("PropObjGrafico").GetComponent<PropObjetoGraficoScript>().toggleField.isOn = bool.Parse(values["ativo"]);
    }

    void setPropsCubo(Controller controller, JSONNode values, int countObjt, string nome)
    {
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

            //seta a cor no cubo
            //print(Global.propriedadePecas[prPeca.Nome]);
        }

        if (values["textura"])
        {
            for (int i = 0; i < texturas.Length; i++)
            {
                string novoNome = texturas[i].gameObject.GetComponent<MeshRenderer>().material.name;

                if (novoNome.Contains(values["textura"]))
                {
                    prPeca.Textura = texturas[i].gameObject.GetComponent<MeshRenderer>().material.mainTexture;
                }
            }

            //Texturiza os cubos
            GameObject.Find(prPeca.NomeCuboAmbiente).GetComponent<MeshRenderer>().materials[0].mainTexture = prPeca.Textura;
            GameObject.Find(prPeca.NomeCuboVis).GetComponent<MeshRenderer>().materials[0].mainTexture = prPeca.Textura;
        }

        Global.propriedadePecas.Add(nome, prPeca);

        //por alguma razão, n tá sendo criado em cena com o nome certo... aí tem dois "CuboAmbiente" e "CuboVis"... n aparece o num qnd eu faço a conversão dos nomes
        //print(prPeca.NomeCuboAmbiente);
        //print(prPeca.NomeCuboVis);

        GameObject.Find(Global.propriedadePecas[prPeca.Nome].NomeCuboAmbiente).GetComponent<MeshRenderer>().materials[0].color = prPeca.Cor;
        GameObject.Find(Global.propriedadePecas[prPeca.Nome].NomeCuboVis).GetComponent<MeshRenderer>().materials[0].color = prPeca.Cor;
    }

    void setPropsAcoes(Controller controller, JSONNode values, int countObjt, string nome)
    {
        controller.abrePropriedade.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["nome"];

        controller.abrePropriedade.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["valores"][0];
        controller.abrePropriedade.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = values["valores"][1];
        controller.abrePropriedade.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = values["valores"][2];

        controller.abrePropriedade.transform.GetChild(3).GetComponent<Toggle>().isOn = values["ativo"];

        PropriedadePeca prPeca = new PropriedadePeca();
        prPeca.Nome = values["nome"];
        prPeca.PodeAtualizar = true;
        var nomeAmb = values["nome"] + "Amb";
        if (countObjt > 0) nomeAmb += countObjt;
        prPeca.NomeCuboAmbiente = nomeAmb;
        var nomeVis = values["nome"] + "Vis";
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
        }
        else
        {
            prPeca.Pos = new Posicao();
            prPeca.Pos.X = float.Parse(values["valores"][0]);
            prPeca.Pos.Y = float.Parse(values["valores"][1]);
            prPeca.Pos.Z = float.Parse(values["valores"][2]);

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
        prPeca.Nome = values["nome"];
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
            if (countAcoes > 0) key += "_" + countAcoes;

            if (slot.Key.Contains(Global.GetSlot(peca.name)) && !Global.listaEncaixes.ContainsKey(peca.name))
            {
                if (GameObject.Find(slot.Key) != null)
                    Global.listaEncaixes.Add(GameObject.Find(peca.name).name, key);
                else if (Global.listaEncaixes.ContainsKey(peca.name)
                        && Global.listaEncaixes[peca.name] != key)
                {
                    control.reposicionaSlots(Global.listaEncaixes[peca.name], key);
                }
            }

        }
    }
    int[] adicionarCriancas(JSONArray children, int countObjt, int countCubo, int countAcoes, int countTrans, int countRot, int countEsc, int countLuz)
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
                    var nomeSlot = "FormasSlot";
                    if (countCubo > 0)
                    {
                        nome += countCubo;
                        nomeSlot += countCubo;
                    }
                    var cubo = GameObject.Find(nome);
                    var controller = cubo.GetComponent<Controller>();
                    controller.GeraCopiaPeca();

                    float y = GameObject.Find(nomeSlot).gameObject.transform.position.y;

                    //pegar nome prefab + numero e mudar posicao
                    cubo.transform.position = new Vector3(x, y, z);
                    cubo.GetComponent<BoxCollider>().enabled = true;
                    Global.addObject(cubo);

                    //Global.atualizaListaSlot();
                    //TEM Q ADICIONAR NA LISTA DE ENCAIXES!!!!!!!!!!!
                    adicionarEncaixe(cubo);

                    var cuboAmb = "CuboAmbiente";
                    if (countObjt > 0) cuboAmb += countObjt;
                    if (GameObject.Find(cuboAmb) != null)
                    {
                        //numFormas = getNumeroSlotObjetoGrafico();
                        var formasSlot = "FormasSlot";
                        if (countCubo > 0) formasSlot += countCubo;
                        GameObject t = GameObject.Find("FormasSlot" + formasSlot);

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

                            MeshRenderer mr = GameObject.Find(cuboAmb).GetComponent<MeshRenderer>();

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

                            if (statusCubo || Global.propriedadePecas.Count == 0) mr.enabled = true;
                        }
                        else GameObject.Find(cuboAmb).GetComponent<MeshRenderer>().enabled = true;
                    }

                    setPropsCubo(controller, value, countObjt, nome);
                    countCubo++;
                }
                if (key.Contains("Ilumi"))
                {
                    var nome = "Iluminacao";
                    var nomeSlot = "IluminacaoSlot";
                    if (countLuz > 0)
                    {
                        nome += countLuz;
                        nomeSlot += countLuz;
                    }

                    var luz = GameObject.Find(nome);
                    var controller = luz.GetComponent<Controller>();
                    controller.GeraCopiaPeca();

                    float y = GameObject.Find(nomeSlot).gameObject.transform.position.y;
                    //pegar nome prefab + numero e mudar posicao
                    luz.transform.position = new Vector3(x, y, z); //TÁ COM PROBLEMAS NO Y!!!!!!!!!
                    luz.GetComponent<BoxCollider>().enabled = true;
                    Global.addObject(luz);

                    //Global.atualizaListaSlot();
                    //TEM Q ADICIONAR NA LISTA DE ENCAIXES!!!!!!!!!!!
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
                    if (countObjt > 0)
                    {
                        prPeca.NomeCuboAmbiente = "CuboAmbiente" + Controller.getNumObjeto(countObjt.ToString());
                        prPeca.NomeCuboVis = "CuboVis" + Controller.getNumObjeto(countObjt.ToString());
                    }

                    prPeca.TipoLuz = 0;
                    Global.propriedadePecas.Add(gameObject.name, prPeca);

                    if (gameObject.name.Length > "Iluminacao".Length)
                        controller.CriaLightObject(gameObject.name.Substring("Iluminacao".Length, 1));

                    setPropsIluminacao(controller, value, nome);
                    countLuz++;
                }
                if (key.Contains("Trans") || key.Contains("Rot") || key.Contains("Esc"))
                {
                    
                    var nome = key;
                    var nomeSlot = "TransformacoesSlot";
                    if (countAcoes > 0) nomeSlot += "_" + countAcoes;

                    //if (countRot > 0 && nome.Contains("Rot")) nome += countRot;
                    //if (countTrans > 0 && nome.Contains("Trans")) nome += countTrans;
                    //if (countEsc > 0 && nome.Contains("Esc")) nome += countEsc;

                    var acao = GameObject.Find(nome);
                    var controller = acao.GetComponent<Controller>();
                    controller.GeraCopiaPeca();

                    float y = GameObject.Find(nomeSlot).gameObject.transform.position.y;
                    acao.transform.position = new Vector3(x, y, z);
                    acao.GetComponent<BoxCollider>().enabled = true;
                    Global.addObject(acao);
                    adicionarEncaixe(acao, countAcoes);
                    /*
                    string numObjetoGrafico = controller.getNumeroSlotObjetoGrafico();
                    GameObject ObjGrafSlot = GameObject.Find("ObjGraficoSlot" + numObjetoGrafico);
                    */
                    string numObjetoGrafico = "";
                    if (countObjt > 0) numObjetoGrafico = countObjt.ToString();
                    GameObject ObjGrafSlot = GameObject.Find("ObjGraficoSlot" + numObjetoGrafico);
                    //Retorna  de TransformacoesSlot
                    string slot = "";

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

                    controller.posicaoColliderDestino = t;

                    print(cloneTrans.name);

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
                    setPropsAcoes(controller, value, countObjt, nome);
                    countAcoes++;
                    /*
                    var nome = key;
                    var nomeSlot = "TransformacoesSlot";
                    if (countAcoes > 0) nomeSlot += "_" + countAcoes;
                    
                    //if (countRot > 0 && nome.Contains("Rot")) nome += countRot;
                    //if (countTrans > 0 && nome.Contains("Trans")) nome += countTrans;
                    //if (countEsc > 0 && nome.Contains("Esc")) nome += countEsc;
                    
                    var acao = GameObject.Find(nome);
                    var controller = acao.GetComponent<Controller>();
                    controller.GeraCopiaPeca();

                    float y = GameObject.Find(nomeSlot).gameObject.transform.position.y;
                    acao.transform.position = new Vector3(x, y, z);
                    acao.GetComponent<BoxCollider>().enabled = true;

                    string numObjetoGrafico = "";
                    if (countObjt > 0) numObjetoGrafico = countObjt.ToString();
                    GameObject ObjGrafSlot = GameObject.Find("ObjGraficoSlot" + numObjetoGrafico);


                    //Retorna  de TransformacoesSlot
                    string slot = "";

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

                    int val = 0;
                    string countTransformacoes = "";
                    Int32.TryParse(slot.Substring(slot.IndexOf("_") + 1), out val);

                    if (countAcoes > 0)
                        countTransformacoes = Convert.ToString(countAcoes + 1);
                    else
                        countTransformacoes = "1";
                    //-------------

                    print(nomeSlot);
                    GameObject t = GameObject.Find(nomeSlot);
                    GameObject cloneTrans = Instantiate(t, t.transform.position, t.transform.rotation, t.transform.parent);
                    cloneTrans.name = "TransformacoesSlot" + numObjetoGrafico + "_" + countTransformacoes;
                    cloneTrans.transform.position = new Vector3(t.transform.position.x, t.transform.position.y - 3f, t.transform.position.z);

                    //Global.listaPosicaoSlot.Add(cloneTrans.name, cloneTrans.transform.position.y);

                    controller.addTransformacoeSequenciaSlots(cloneTrans.name);
                    //Global.listaEncaixes.Add(nome, cloneTrans.name);
                    adicionarEncaixe(acao);

                    controller.posicaoColliderDestino = t;

                    if (controller.renderController == null)
                        controller.renderController = new RenderController();


                    //aqui ó
                    if (nome.Contains("Trans")) controller.renderController.ResizeBases(t, Consts.Transladar, true); // o Segundo parâmetro pode ser qualquer tranformação 
                    else if (nome.Contains("Rot")) controller.renderController.ResizeBases(t, Consts.Rotacionar, true);
                    else if (nome.Contains("Esc")) controller.renderController.ResizeBases(t, Consts.Escalar, true);

                    controller.concatNumber = numObjetoGrafico;

                    controller.addGameObjectTree("GameObjectAmb" + numObjetoGrafico, "Amb", "CuboAmbiente" + numObjetoGrafico);
                    controller.addGameObjectTree("CuboVisObject" + numObjetoGrafico, "Vis", "CuboVis" + numObjetoGrafico);

                    controller.configuraIluminacao("-");

                    controller.reorganizaObjetos(numObjetoGrafico);
                    setPropsAcoes(controller, value, countObjt, nome);
                    //Global.atualizaListaSlot();
                    countAcoes++;
                   */
                    /*
                    if (nome.Contains("Rot")) countRot++;
                    else if (nome.Contains("Trans")) countTrans++;
                    else if (nome.Contains("Esc")) countEsc++;
                    */
                }
                //Global.atualizaListaSlot();
            }
        }

        int[] contadores = new int[6];
        contadores[0] = countCubo;
        contadores[1] = countTrans;
        contadores[2] = countRot;
        contadores[3] = countEsc;

        contadores[4] = countAcoes;
        contadores[5] = countLuz;
        return contadores;
    }

    public void Importar()
    {
        setImportando(true);
        //TEM Q CHECAR SE A LISTA TA VAZIA, SENÃO DÁ ERRO!!!
        //limparListaObjetos();
        var countObjt = 0;
        var countCam = 0;
        var countCubo = 0;
        var countTrans = 0;
        var countRot = 0;
        var countEsc = 0;
        var countLuz = 0;
        var countAcoes = 0;
        JSONObject cenaImportada = (JSONObject)JSONObject.Parse(cenaJSON.text);
        var count = 0;
        //string chaves = Object.(cenaImportada);
        //fazer um foreach, pegar a key, e j� ir criando os objetos
        //ele s� vai conseguir acessar os de primeiro n�vel, ent vou ter que armazenar os OG pra dps acessar os filhos
        //e quando estiver acessando os OGs, adicionar o OG e a� os filhos em seguida na lista de objetos global
        //talvez transformar todas as pe�as em prefabs, colocar como props do Arquivo e quando tiver que criar, pedir pra criar uma c�pia 
        //a partir delas
        //para instanciar as pe�as: Instantiate(nomePrefab, new Vector3(0,0,0), Quaternion.identity);
        //ir atualizando a lista Global.listObjectCount, pros nomes ficarem com os n�meros atualizados, pra n gerar conflito
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
                /*
                var vetor = new Vector3(value["posPeca"][0], value["posPeca"][1], value["posPeca"][2]);
                GameObject instance = GameObject.Instantiate(pecasPrefabs[0]);
                instance.gameObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(90, 0, -180);
                instance.gameObject.transform.localScale = new Vector3(7.8f, 2f, 1f);
                Instantiate(instance, vetor, Quaternion.identity);
                Global.addObject(instance);
                */

                //IDEIA: CHAMAR FUNC GERAR C�PIA E J� SETAR OS VALORES DA POSI��O, A� VAI SER UMA C�MERA Q FUNCIONA!!
                //tem q acessar o prefab do objeto q qr gerar uma c�pia, acessar o controller e pegar a func de copiar
                //FUNCIONOU
                //FALTA O RETORNO VISUAL NO AMBIENTE GR�FICO E VISUALIZADOR(PROVAVELMENTE)!!

                var nome = "CameraP";
                if (countCam > 0) nome += countCam;
                var cam = GameObject.Find(nome);
                var controller = cam.GetComponent<Controller>();
                controller.GeraCopiaPeca();

                //DEU CERTO ASSIM!!! VEIO NO ENCAIXE!!
                float y = GameObject.Find("CameraSlot").gameObject.transform.position.y;
                //pegar nome prefab + numero e mudar posicao
                cam.transform.position = new Vector3(x, y, z);
                cam.GetComponent<BoxCollider>().enabled = true;
                Global.addObject(cam);

                GameObject.Find("CameraObjetoMain").transform.GetChild(0).gameObject.SetActive(true);
                //faz o visualizador dar retorno!!
                Global.cameraAtiva = true;
                //if (Global.cameraAtiva)
                //    GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

                Global.atualizaListaSlot();
                //TEM Q ADICIONAR NA LISTA DE ENCAIXES!!!!!!!!!!!
                adicionarEncaixe(cam);
                //pecasPrefabs[0] = GameObject.Find("CameraP1");
                //--> ia solucionar o problema de varios objtsgrafcs, mas n deixou mais clicar na peca
                setPropsCamera(controller, value, nome);
                countCam++;
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
                    nome += countObjt;
                    nomeSlot += countObjt;
                }
                var objeto = GameObject.Find(nome);
                var controller = objeto.GetComponent<Controller>();
                controller.GeraCopiaPeca();

                float y = GameObject.Find(nomeSlot).gameObject.transform.position.y;

                objeto.transform.position = new Vector3(x, y, z);
                objeto.GetComponent<BoxCollider>().enabled = true;
                Global.addObject(objeto);

                var criaFormas = new Util_VisEdu();
                criaFormas.criaFormasVazias();

                string countObjGrafico = "";
                if (DropPeca.countObjetosGraficos > 0)
                    countObjGrafico = Convert.ToString(DropPeca.countObjetosGraficos);

                if (!Equals(countObjGrafico, string.Empty))
                    objeto.GetComponent<Controller>().createGameObjectTree(DropPeca.countObjetosGraficos);

                Global.iniciaListaSequenciaSlots(DropPeca.countObjetosGraficos);

                GameObject t = GameObject.Find("ObjGraficoSlot" + countObjGrafico);

                GameObject cloneObjGrafico = Instantiate(t, t.transform.position, t.transform.rotation, t.transform.parent);
                cloneObjGrafico.name = "ObjGraficoSlot" + Convert.ToString(++DropPeca.countObjetosGraficos);
                cloneObjGrafico.transform.position = new Vector3(t.transform.position.x, t.transform.position.y - 11f, t.transform.position.z);

                objeto.GetComponent<Controller>().setActiveAndRenameGameObject(t, cloneObjGrafico);

                var renderController = new RenderController();

                renderController.ResizeBases(t, Consts.ObjetoGrafico, true);
                objeto.GetComponent<Controller>().adicionaObjetoRender();

                Global.atualizaListaSlot();

                /*
                foreach (KeyValuePair<string, float> slot in Global.listaPosicaoSlot)  // Slot / posição no eixo y
                {
                    if (slot.Key.Contains(Global.GetSlot(GameObject.Find(objeto.name).name)) && !Global.listaEncaixes.ContainsKey(GameObject.Find(objeto.name).name))
                    {
                        if (GameObject.Find(slot.Key) != null)
                            Global.listaEncaixes.Add(GameObject.Find(objeto.name).name, slot.Key);
                    }
                }
                */
                adicionarEncaixe(objeto);
                if (value["children"].Count > 0)
                {
                    int[] contadores = adicionarCriancas((JSONArray)value["children"], countObjt, countCubo, countAcoes, countTrans, countRot, countEsc, countLuz);
                    countCubo = contadores[0];
                    countTrans = contadores[1];
                    countRot = contadores[2];
                    countEsc = contadores[3];
                    countAcoes = contadores[4];
                    countLuz = contadores[5];
                }
                setPropsObjetoGrafico(controller, value, countObjt, nome);
                countObjt++;

            }
            count++;
        }
        cenaJSON.text = "";
        //decompor objeto
        //com base na chave, vou criar objetos 
        //dentro deles, acessar o <Meu...> de cada um e setar as props
        //vou ter que criar slots tbm
    }
}
