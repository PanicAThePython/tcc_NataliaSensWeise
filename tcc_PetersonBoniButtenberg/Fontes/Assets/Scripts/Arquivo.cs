using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using TMPro;
using System;
using UnityEngine.UI;

public class Arquivo : MonoBehaviour
{
    public static JSONObject cena = new JSONObject();
    public GameObject mensagem;
    public TMP_InputField cenaJSON;
    public GameObject[] pecasPrefabs;

    MeuObjetoGrafico objetoAtual = new MeuObjetoGrafico();
    string nomeObjetoAtual = "";

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
        Global.listaObjetos[l].GetComponent<MeuObjetoGrafico>().addProps("Objeto Gr�fico");
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
            props.Add("nome", "Ilumina��o");
            props.Add("tipoLuz", "Ambiente");

            JSONArray posicao = new JSONArray();
            posicao.Add("x", "100");
            posicao.Add("y", "300");
            posicao.Add("z", "0");
            props.Add("posicao", posicao);

            props.Add("ativo", true);

            JSONArray posPeca = new JSONArray();
            posPeca.Add("x", this.transform.position.x);
            posPeca.Add("y", this.transform.position.y);
            posPeca.Add("z", this.transform.position.z);
            props.Add("posPeca", posPeca);
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

            JSONArray posPeca = new JSONArray();
            posPeca.Add("x", this.transform.position.x);
            posPeca.Add("y", this.transform.position.y);
            posPeca.Add("z", this.transform.position.z);
            props.Add("posPeca", posPeca);
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

            JSONArray posPeca = new JSONArray();
            posPeca.Add("x", this.transform.position.x);
            posPeca.Add("y", this.transform.position.y);
            posPeca.Add("z", this.transform.position.z);
            props.Add("posPeca", posPeca);
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

            JSONArray posPeca = new JSONArray();
            posPeca.Add("x", this.transform.position.x);
            posPeca.Add("y", this.transform.position.y);
            posPeca.Add("z", this.transform.position.z);
            props.Add("posPeca", posPeca);
            propsAcao = props;
        }
        JSONObject filho = new JSONObject();
        filho.Add(Global.listaObjetos[l].name, propsAcao);
        objetoAtual.addChildren(filho);
    }

    public string GetPath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    //faltou exportar a textura!!!!!!!!!
    public void Exportar()
    {
        List<GameObject> ordenada = ordenarCena(Global.listaObjetos);
        for (int l = 0; l < ordenada.Count; l++)
        {
            if (ordenada[l].name.Contains("Camera"))
            {
                adicionarCameraNoJSON(l);
            }
            else if (ordenada[l].name.Contains("Objeto"))
            {
                adicionarObjetoGraficoNoJSON(l);
            }
            else if (ordenada[l].name.Contains("Iluminacao"))
            {
                adicionarIluminacaoNoJSON(l);
            }
            else if (ordenada[l].name.Contains("Cubo"))
            {
                adicionarCuboNoJSON(l);
            }
            else if (ordenada[l].name.Contains("Escala"))
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

    void setPropsCamera(Controller controller, JSONNode values)
    {
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
    }

    void setPropsObjetoGrafico(Controller controller, JSONNode values)
    {
        controller.abrePropriedade.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["nome"];
        controller.abrePropriedade.transform.GetChild(2).GetComponent<Toggle>().enabled = values["ativo"];
    }

    void setPropsCubo(Controller controller, JSONNode values)
    {
        controller.abrePropriedade.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["nome"];

        controller.abrePropriedade.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["tamanho"][0];
        controller.abrePropriedade.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = values["tamanho"][1];
        controller.abrePropriedade.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = values["tamanho"][2];

        controller.abrePropriedade.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][0];
        controller.abrePropriedade.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][1];
        controller.abrePropriedade.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][2];
        
        //como importar cor e textura???

        controller.abrePropriedade.transform.GetChild(6).GetComponent<Toggle>().enabled = values["ativo"];
    }

    void setPropsAcoes(Controller controller, JSONNode values)
    {
        controller.abrePropriedade.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["nome"];

        controller.abrePropriedade.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["valores"][0];
        controller.abrePropriedade.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = values["valores"][1];
        controller.abrePropriedade.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = values["valores"][2];

        controller.abrePropriedade.transform.GetChild(3).GetComponent<Toggle>().enabled = values["ativo"];
    }

    void setPropsIluminacao(Controller controller, JSONNode values)
    {
        controller.abrePropriedade.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["nome"];
       // controller.abrePropriedade.transform.GetChild(1).GetChild(0).GetComponent<Dropdown>().value = values["tipoLuz"];

        var tipoLuz = values["tipoLuz"];
        switch (tipoLuz.ToString())
        {
            case "Ambiente": //2000x0
                controller.abrePropriedade.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][0];
                controller.abrePropriedade.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][1];
                controller.abrePropriedade.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][2];
                
                //como importar cor?????

                controller.abrePropriedade.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(4).GetComponent<Toggle>().enabled = values["ativo"];
                break;

            case "Directional": //2100x0
                controller.abrePropriedade.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][0];
                controller.abrePropriedade.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][1];
                controller.abrePropriedade.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][2];
                //como importar cor?????
                controller.abrePropriedade.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(4).GetComponent<Toggle>().enabled = values["ativo"];

                //direcional
                controller.abrePropriedade.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["intensidade"];
                controller.abrePropriedade.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["valores"][0];
                controller.abrePropriedade.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = values["valores"][1];
                controller.abrePropriedade.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = values["valores"][2];
                break;

            case "Point": //2200x0
                controller.abrePropriedade.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][0];
                controller.abrePropriedade.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][1];
                controller.abrePropriedade.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][2];
                //como importar cor?????
                controller.abrePropriedade.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(4).GetComponent<Toggle>().enabled = values["ativo"];

                //point
                controller.abrePropriedade.transform.GetChild(2).GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["intensidade"];
                controller.abrePropriedade.transform.GetChild(2).GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = values["distancia"];
                break;
            case "Spot":
                controller.abrePropriedade.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][0];
                controller.abrePropriedade.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][1];
                controller.abrePropriedade.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = values["posicao"][2];
                //como importar cor?????
                controller.abrePropriedade.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(4).GetComponent<Toggle>().enabled = values["ativo"];

                //spot
                controller.abrePropriedade.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["intensidade"];
                controller.abrePropriedade.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = values["distancia"];
                controller.abrePropriedade.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = values["angulo"];
                controller.abrePropriedade.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(3).GetChild(0).GetComponent<TMP_InputField>().text = values["expoente"];

                controller.abrePropriedade.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(4).GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = values["valores"][0];
                controller.abrePropriedade.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(4).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text = values["valores"][1];
                controller.abrePropriedade.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(4).GetChild(2).GetChild(0).GetComponent<TMP_InputField>().text = values["valores"][2];
                break;
        }

    }

    void adicionarEncaixe(GameObject peca)
    {
        foreach (KeyValuePair<string, float> slot in Global.listaPosicaoSlot)  // Slot / posição no eixo y
        {
            if (slot.Key.Contains(Global.GetSlot(GameObject.Find(peca.name).name)) && !Global.listaEncaixes.ContainsKey(GameObject.Find(peca.name).name))
            {
                if (GameObject.Find(slot.Key) != null)
                    Global.listaEncaixes.Add(GameObject.Find(peca.name).name, slot.Key);
            }
        }
    }

    int[] adicionarCriancas(JSONArray children, int countObjt, int countCubo, int countTrans, int countRot, int countEsc, int countLuz)
    {
        foreach (KeyValuePair<string, JSONNode> son in children)
        {
            foreach (KeyValuePair<string, JSONNode> props in son.Value)
            {
                var key = props.Key;
                var value = props.Value;
                float x = value["posPeca"][0];
                float y = value["posPeca"][1];
                float z = value["posPeca"][2];

                if (key.Contains("Cubo"))
                {
                    var nome = "Cubo";
                    if (countCubo > 0) nome += countCubo;
                    var cubo = GameObject.Find(nome);
                    var controller = cubo.GetComponent<Controller>();
                    controller.GeraCopiaPeca();

                    //pegar nome prefab + numero e mudar posicao
                    cubo.transform.position = new Vector3(x, y, z);
                    cubo.GetComponent<BoxCollider>().enabled = true;
                    Global.addObject(cubo);

                    Global.atualizaListaSlot();
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

                    setPropsCubo(controller, value);
                    countCubo++;
                }
                if (key.Contains("Ilumi"))
                {
                    var nome = "Iluminacao";
                    if (countLuz > 0) nome += countLuz;
                    var luz = GameObject.Find(nome);
                    var controller = luz.GetComponent<Controller>();
                    controller.GeraCopiaPeca();

                    //pegar nome prefab + numero e mudar posicao
                    luz.transform.position = new Vector3(x, y, z); //TÁ COM PROBLEMAS NO Y!!!!!!!!!
                    luz.GetComponent<BoxCollider>().enabled = true;
                    Global.addObject(luz);

                    Global.atualizaListaSlot();
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
                        prPeca.NomeCuboAmbiente = "CuboAmbiente" + Controller.getNumObjeto(Global.listaEncaixes[gameObject.name]);
                        prPeca.NomeCuboVis = "CuboVis" + Controller.getNumObjeto(Global.listaEncaixes[gameObject.name]);
                    }

                    prPeca.TipoLuz = 0;
                    Global.propriedadePecas.Add(gameObject.name, prPeca);

                    if (gameObject.name.Length > "Iluminacao".Length)
                        controller.CriaLightObject(gameObject.name.Substring("Iluminacao".Length, 1));

                    setPropsIluminacao(controller, value);
                    countLuz++;
                }
                if (key.Contains("Trans"))
                {
                    var nome = "Transladar";
                    if (countTrans > 0) nome += countTrans;
                    var transladar = GameObject.Find(nome);
                    var controller = transladar.GetComponent<Controller>();
                    controller.GeraCopiaPeca();

                    //pegar nome prefab + numero e mudar posicao
                    transladar.transform.position = new Vector3(x, y, z); //TÁ COM PROBLEMAS NO Y!!!!!!!!!
                    transladar.GetComponent<BoxCollider>().enabled = true;
                    Global.addObject(transladar);

                    Global.atualizaListaSlot();
                    //TEM Q ADICIONAR NA LISTA DE ENCAIXES!!!!!!!!!!!
                    adicionarEncaixe(transladar);

                    var slotNome = "ObjGraficoSlot";
                    if (countObjt > 0) slotNome += countObjt;
                    GameObject ObjGrafSlot = GameObject.Find(slotNome);

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

                    if (val > 0)
                        countTransformacoes = Convert.ToString(val + 1);
                    else
                        countTransformacoes = "1";
                    //-------------

                    GameObject t = GameObject.Find(slot);
                    GameObject cloneTrans = Instantiate(t, t.transform.position, t.transform.rotation, t.transform.parent);
                    if (countObjt > 0) cloneTrans.name = "TransformacoesSlot" + countObjt + "_" + countTransformacoes;
                    else cloneTrans.name = "TransformacoesSlot_" + countTransformacoes;
                    cloneTrans.transform.position = new Vector3(t.transform.position.x, t.transform.position.y - 3f, t.transform.position.z);

                    controller.addTransformacoeSequenciaSlots(cloneTrans.name);

                    controller.posicaoColliderDestino = t;

                    if (controller.renderController == null)
                        controller.renderController = new RenderController();

                    controller.renderController.ResizeBases(t, Consts.Transladar, true); // o Segundo parâmetro pode ser qualquer tranformação 

                    controller.configuraIluminacao("-");

                    if (countObjt == 0) controller.reorganizaObjetos("");
                    else controller.reorganizaObjetos(countObjt.ToString());

                    setPropsAcoes(controller, value);
                    countTrans++;
                }
                if (key.Contains("Rotac"))
                {
                    var nome = "Rotacionar";
                    if (countRot > 0) nome += countRot;
                    var rotacionar = GameObject.Find(nome);
                    var controller = rotacionar.GetComponent<Controller>();
                    controller.GeraCopiaPeca();

                    //pegar nome prefab + numero e mudar posicao
                    rotacionar.transform.position = new Vector3(x, y, z); //TÁ COM PROBLEMAS NO Y!!!!!!!!!
                    rotacionar.GetComponent<BoxCollider>().enabled = true;
                    Global.addObject(rotacionar);

                    Global.atualizaListaSlot();
                    //TEM Q ADICIONAR NA LISTA DE ENCAIXES!!!!!!!!!!!
                    adicionarEncaixe(rotacionar);

                    setPropsAcoes(controller, value);
                    countRot++;
                }
                if (key.Contains("Escal"))
                {
                    var nome = "Escalar";
                    if (countEsc > 0) nome += countEsc;
                    var escalar = GameObject.Find(nome);
                    var controller = escalar.GetComponent<Controller>();
                    controller.GeraCopiaPeca();

                    //pegar nome prefab + numero e mudar posicao
                    escalar.transform.position = new Vector3(x, y, z); //TÁ COM PROBLEMAS NO Y!!!!!!!!!
                    escalar.GetComponent<BoxCollider>().enabled = true;
                    Global.addObject(escalar);

                    Global.atualizaListaSlot();
                    //TEM Q ADICIONAR NA LISTA DE ENCAIXES!!!!!!!!!!!
                    adicionarEncaixe(escalar);

                    setPropsAcoes(controller, value);
                    countEsc++;
                }
            }
        }

        int[] contadores = new int[5];
        contadores[0] = countCubo;
        contadores[1] = countTrans;
        contadores[2] = countRot;
        contadores[3] = countEsc;
        contadores[4] = countEsc;
        return contadores;
    }

    public void Importar()
    {
        //TEM Q CHECAR SE A LISTA TA VAZIA, SENÃO DÁ ERRO!!!
        //limparListaObjetos();
        var countObjt = 0;
        var countCam = 0;
        var countCubo = 0;
        var countTrans = 0;
        var countRot = 0;
        var countEsc = 0;
        var countLuz = 0;
        JSONObject cenaImportada = (JSONObject) JSONObject.Parse(cenaJSON.text);
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
            float y = value["posPeca"][1];
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
                setPropsCamera(controller, value);
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
                if (countObjt > 0) nome += countObjt;
                var objeto = GameObject.Find(nome);
                var controller = objeto.GetComponent<Controller>();
                controller.GeraCopiaPeca();

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

                foreach (KeyValuePair<string, float> slot in Global.listaPosicaoSlot)  // Slot / posição no eixo y
                {
                    if (slot.Key.Contains(Global.GetSlot(GameObject.Find(objeto.name).name)) && !Global.listaEncaixes.ContainsKey(GameObject.Find(objeto.name).name))
                    {
                        if (GameObject.Find(slot.Key) != null)
                            Global.listaEncaixes.Add(GameObject.Find(objeto.name).name, slot.Key);
                    }
                }
                if (value["children"].Count > 0)
                {
                    int[] contadores = adicionarCriancas((JSONArray)value["children"], countObjt, countCubo, countTrans, countRot, countEsc, countLuz);
                    countCubo = contadores[0];
                    countTrans = contadores[1];
                    countRot = contadores[2];
                    countEsc = contadores[3];
                    countLuz = contadores[4];
                }

                setPropsObjetoGrafico(controller, value);
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
