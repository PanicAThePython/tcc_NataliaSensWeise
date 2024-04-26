using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using TMPro;
using System;
public class Arquivo : MonoBehaviour
{
    public static JSONObject cena = new JSONObject();
    public GameObject mensagem;
    public TMP_InputField cenaJSON;
    public GameObject[] pecasPrefabs;

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

    public string GetPath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
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

    public void Importar()
    {
        //TEM Q CHECAR SE A LISTA TA VAZIA, SENÃO DÁ ERRO!!!
        //limparListaObjetos();
        //var countObjt = 0;
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
            if (key == "CameraP") 
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
                var controller = pecasPrefabs[0].GetComponent<Controller>();
                GameObject copia = controller.GeraCopiaPeca();
                //pegar nome prefab + numero e mudar posicao
                pecasPrefabs[0].transform.position = new Vector3(x, y, z);
                Global.addObject(GameObject.Find(pecasPrefabs[0].name));

                GameObject.Find("CameraObjetoMain").transform.GetChild(0).gameObject.SetActive(true);
                //faz o visualizador dar retorno!!
                Global.cameraAtiva = true;
                if (Global.cameraAtiva)
                    GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Formas");

                Global.atualizaListaSlot();
                //TEM Q ADICIONAR NA LISTA DE ENCAIXES!!!!!!!!!!!
                foreach (KeyValuePair<string, float> slot in Global.listaPosicaoSlot)  // Slot / posição no eixo y
                {
                    if (slot.Key.Contains(Global.GetSlot(GameObject.Find(pecasPrefabs[0].name).name)) && !Global.listaEncaixes.ContainsKey(GameObject.Find(pecasPrefabs[0].name).name))
                    {
                        if (GameObject.Find(slot.Key) != null)
                            Global.listaEncaixes.Add(GameObject.Find(pecasPrefabs[0].name).name, slot.Key);
                    }
                }
                //pecasPrefabs[0] = GameObject.Find("CameraP1");
                //--> ia solucionar o problema de varios objtsgrafcs, mas n deixou mais clicar na peca

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
                var controller = pecasPrefabs[1].GetComponent<Controller>();
                GameObject copia = controller.GeraCopiaPeca();
                //GameObject objeto = GameObject.Find(pecasPrefabs[1].name);
                //else objeto = GameObject.Find("ObjetoGraficoP" + countObjt);
                
                pecasPrefabs[1].transform.position = new Vector3(x, y, z);
                Global.addObject(GameObject.Find(pecasPrefabs[1].name));

                var criaFormas = new Util_VisEdu();
                criaFormas.criaFormasVazias();

                string countObjGrafico = "";
                if (DropPeca.countObjetosGraficos > 0)
                    countObjGrafico = Convert.ToString(DropPeca.countObjetosGraficos);

                if (!Equals(countObjGrafico, string.Empty))
                    pecasPrefabs[1].GetComponent<Controller>().createGameObjectTree(DropPeca.countObjetosGraficos);

                Global.iniciaListaSequenciaSlots(DropPeca.countObjetosGraficos);

                GameObject t = GameObject.Find("ObjGraficoSlot" + countObjGrafico);

                GameObject cloneObjGrafico = Instantiate(t, t.transform.position, t.transform.rotation, t.transform.parent);
                cloneObjGrafico.name = "ObjGraficoSlot" + Convert.ToString(++DropPeca.countObjetosGraficos);
                cloneObjGrafico.transform.position = new Vector3(t.transform.position.x, t.transform.position.y - 11f, t.transform.position.z);

                pecasPrefabs[1].GetComponent<Controller>().setActiveAndRenameGameObject(t, cloneObjGrafico);

                var renderController = new RenderController();

                renderController.ResizeBases(t, Consts.ObjetoGrafico, true);
                pecasPrefabs[1].GetComponent<Controller>().adicionaObjetoRender();

                Global.atualizaListaSlot();

                foreach (KeyValuePair<string, float> slot in Global.listaPosicaoSlot)  // Slot / posição no eixo y
                {
                    if (slot.Key.Contains(Global.GetSlot(GameObject.Find(pecasPrefabs[1].name).name)) && !Global.listaEncaixes.ContainsKey(GameObject.Find(pecasPrefabs[1].name).name))
                    {
                        if (GameObject.Find(slot.Key) != null)
                            Global.listaEncaixes.Add(GameObject.Find(pecasPrefabs[1].name).name, slot.Key);
                    }
                }
                print(value["children"]);
                //pecasPrefabs[1] = GameObject.Find("ObjetoGraficoP"+countObjt);
                //PARA ARRUMAR: ADICIONAR FILHOS
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
