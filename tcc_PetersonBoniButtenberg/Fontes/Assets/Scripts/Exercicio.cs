using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System;
using System.Globalization;
using UnityEngine.UI;

public class Exercicio : MonoBehaviour
{
    public ToggleGroup tg;
    public Toggle nenhum;
    public TMP_Text enunText;

    public GameObject[] texturas;

    private string nomeToggle;
    private int numeroExerc;

    private string[] enunciados = new string[4];
    private string[] respostas = new string[4];
    private List<Dictionary<string, PropriedadePeca>> props = new List<Dictionary<string, PropriedadePeca>>();

    private Arquivo arq = new Arquivo();

    private int errosOrdem = 0;
    private int acertosOrdem = 0;
    private int acertosProps = 0;
    private int errosProps = 0;
    private int[] checarProps = new int[3];

    private int checar = 0;

    void getNovoExercicio()
    {
        nomeToggle = tg.GetFirstActiveToggle().name;
        numeroExerc = int.Parse(nomeToggle[nomeToggle.Length - 1].ToString());
        enableEnunciado();
    }

    void enableEnunciado()
    {
        enunText.text = enunciados[numeroExerc];
        string[] resp = respostas[numeroExerc].Split(", ");
        Resposta.setRespostaOrdem(resp);
        Resposta.setRespostaProps(props[numeroExerc]);
    }

    void Start()
    {
        enunciados[0] = "";
        enunciados[1] = "Enunciado 1: Crie uma cena com um cubo com textura de QR Code. Apenas a face frontal do cubo deve estar visível. A visão da câmera deve estar o mais próxima possível sem cortar a visualização do cubo.";
        enunciados[2] = "Enunciado 2: Cria uma cena com um cubo pai e um filho. O cubo pai deve ter textura da FURB e o filho, de madeira, e estar a uma distância de 2 unidades do x do pai. O pai tem que ter uma escala de 2 no eixo Y.";
        enunciados[3] = "Enunciado 3: Crie uma cena com um cubo estampado de FURB. Mude a rotação para 60º no eixo Y. Faça uso da luz spot de forma que um círculo de luz se forme centralizado e cubra o máximo possível do topo do cubo sem tocar a borda.";
        
        respostas[0] = "";
        respostas[1] = "Camera, ObjetoGrafico, Cubo, Iluminacao";
        respostas[2] = "Camera, ObjetoGrafico, Cubo, Escalar, Iluminacao, ObjetoGrafico, Cubo, Transladar";
        respostas[3] = "Camera, ObjetoGrafico, Cubo, Rotacionar, Iluminacao";

        checarProps[0] = 3;
        checarProps[1] = 5;
        checarProps[2] = 4;
        //enunciado 1

        Dictionary<string, PropriedadePeca> dict = new Dictionary<string, PropriedadePeca>();
        
        PropriedadeCamera cam1 = new PropriedadeCamera();

        cam1.PosX = 0;
        cam1.PosY = 0;
        cam1.PosZ = 300;
        cam1.FOV = new Vector2(14f, 14f);
        cam1.Near = 100;
        cam1.Far = 600;
        Resposta.setCamProps(cam1, 1);

        PropriedadePeca objt1 = new PropriedadePeca();
        objt1.Ativo = true;
        dict.Add("ObjetoGrafico", objt1);

        PropriedadePeca cubo1 = new PropriedadePeca();
        cubo1.Ativo = true;
        cubo1.Tam = new Tamanho();
        cubo1.Tam.X = 1;
        cubo1.Tam.Y = 1;
        cubo1.Tam.Z = 1;
        cubo1.Pos = new Posicao();
        cubo1.Pos.X = 0;
        cubo1.Pos.Y = 0;
        cubo1.Pos.Z = 0;
        cubo1.Textura = texturas[0].gameObject.GetComponent<MeshRenderer>().material.mainTexture;
        cubo1.Cor = Color.white;
        dict.Add("Cubo", cubo1);

        PropriedadePeca luz1 = new PropriedadePeca();
        luz1.TipoLuz = 0;
        luz1.Ativo = true;
        luz1.Pos = new Posicao();
        luz1.Pos.X = 100;
        luz1.Pos.Y = 300;
        luz1.Pos.Z = 0;
        dict.Add("Iluminacao", luz1);

        //enunciado 2 "Camera, ObjetoGrafico, Cubo, Rotacionar, Iluminacao, ObjetoGrafico, Cubo, Transladar"
        Dictionary<string, PropriedadePeca> dict2 = new Dictionary<string, PropriedadePeca>();

        PropriedadeCamera cam2 = new PropriedadeCamera();

        cam2.PosX = -100;
        cam2.PosY = 300;
        cam2.PosZ = 300;
        cam2.FOV = new Vector2(45f, 45f);
        cam2.Near = 100;
        cam2.Far = 600;
        Resposta.setCamProps(cam2, 2); //tem q setar em uma lista

        PropriedadePeca objt2 = new PropriedadePeca();
        objt2.Ativo = true;
        dict2.Add("ObjetoGrafico", objt2);

        PropriedadePeca cubo2 = new PropriedadePeca();
        cubo2.Ativo = true;
        cubo2.Tam = new Tamanho();
        cubo2.Tam.X = 1;
        cubo2.Tam.Y = 1;
        cubo2.Tam.Z = 1;
        cubo2.Pos = new Posicao();
        cubo2.Pos.X = 0;
        cubo2.Pos.Y = 0;
        cubo2.Pos.Z = 0;
        cubo2.Textura = texturas[1].gameObject.GetComponent<MeshRenderer>().material.mainTexture; //furb
        cubo2.Cor = Color.white;
        dict2.Add("Cubo", cubo2);

        PropriedadePeca escalar2 = new PropriedadePeca();
        escalar2.Ativo = true;
        escalar2.Tam = new Tamanho();
        escalar2.Tam.X = 1;
        escalar2.Tam.Y = 2;
        escalar2.Tam.Z = 1;
        dict2.Add("Escalar", escalar2);

        PropriedadePeca luz2 = new PropriedadePeca();
        luz2.TipoLuz = 0;
        luz2.Ativo = true;
        luz2.Pos = new Posicao();
        luz2.Pos.X = 100;
        luz2.Pos.Y = 300;
        luz2.Pos.Z = 0;
        dict2.Add("Iluminacao", luz2);

        PropriedadePeca objt2_1 = new PropriedadePeca();
        objt2_1.Ativo = true;
        dict2.Add("ObjetoGraficoo", objt2_1);

        PropriedadePeca cubo2_1 = new PropriedadePeca();
        cubo2_1.Ativo = true;
        cubo2_1.Tam = new Tamanho();
        cubo2_1.Tam.X = 1;
        cubo2_1.Tam.Y = 1;
        cubo2_1.Tam.Z = 1;
        cubo2_1.Pos = new Posicao();
        cubo2_1.Pos.X = 0;
        cubo2_1.Pos.Y = 0;
        cubo2_1.Pos.Z = 0;
        cubo2_1.Textura = texturas[2].gameObject.GetComponent<MeshRenderer>().material.mainTexture; //madeira
        cubo2_1.Cor = Color.white;
        dict2.Add("Cuboo", cubo2_1);

        PropriedadePeca transladar2_1 = new PropriedadePeca();
        transladar2_1.Ativo = true;
        transladar2_1.Pos = new Posicao();
        transladar2_1.Pos.X = 2;
        transladar2_1.Pos.Y = 0;
        transladar2_1.Pos.Z = 0;
        dict2.Add("Transladar", transladar2_1);

        // enunciado 3
        Dictionary<string, PropriedadePeca> dict3 = new Dictionary<string, PropriedadePeca>();

        PropriedadeCamera cam3 = new PropriedadeCamera();

        cam3.PosX = -100;
        cam3.PosY = 300;
        cam3.PosZ = 300;
        cam3.FOV = new Vector2(45f, 45f);
        cam3.Near = 100;
        cam3.Far = 600;
        Resposta.setCamProps(cam3, 3);

        PropriedadePeca objt3 = new PropriedadePeca();
        objt3.Ativo = true;
        dict3.Add("ObjetoGrafico", objt3);

        PropriedadePeca cubo3 = new PropriedadePeca();
        cubo3.Ativo = true;
        cubo3.Tam = new Tamanho();
        cubo3.Tam.X = 1;
        cubo3.Tam.Y = 1;
        cubo3.Tam.Z = 1;
        cubo3.Pos = new Posicao();
        cubo3.Pos.X = 0;
        cubo3.Pos.Y = 0;
        cubo3.Pos.Z = 0;
        cubo3.Textura = texturas[1].gameObject.GetComponent<MeshRenderer>().material.mainTexture;
        cubo3.Cor = Color.white;
        dict3.Add("Cubo", cubo3);

        PropriedadePeca rotacionar3 = new PropriedadePeca();
        rotacionar3.Ativo = true;
        rotacionar3.Pos = new Posicao();
        rotacionar3.Pos.X = 0;
        rotacionar3.Pos.Y = 60;
        rotacionar3.Pos.Z = 0;
        dict3.Add("Rotacionar", rotacionar3);

        PropriedadePeca luz3 = new PropriedadePeca();
        luz3.TipoLuz = 3;
        luz3.Ativo = true;
        luz3.Pos = new Posicao();
        luz3.Pos.X = 0;
        luz3.Pos.Y = 300;
        luz3.Pos.Z = 0;
        luz3.Intensidade = 1.5f;
        luz3.Angulo = 15;
        luz3.Distancia = 1000;
        luz3.Expoente = 10;
        luz3.ValorIluminacao = new ValorIluminacao();
        luz3.ValorIluminacao.X = 0;
        luz3.ValorIluminacao.Y = 0;
        luz3.ValorIluminacao.Z = 0;

        dict3.Add("Iluminacao", luz3);

        props.Add(new Dictionary<string, PropriedadePeca>());
        props.Add(dict);
        props.Add(dict2);
        props.Add(dict3);

        getNovoExercicio();
    }

    void Update()
    {
        if (tg.GetFirstActiveToggle().name != nomeToggle)
        {
            getNovoExercicio();
        }
    }

    void contabilizarAcertosOrdem()
    {
        acertosOrdem++;
    }

    void contabilizarErrosOrdem()
    {
        errosOrdem++;
    }

    void contabilizarAcertosProps()
    {
        acertosProps++;
    }

    void contabilizarErrosProps()
    {
        errosProps++;
    }

    void contarChecagem()
    {
        checar++;
    }

    void setChecar(int c)
    {
        checar = c;
    }

    void zerarContadores()
    {
        checar = 0;
        acertosProps = 0;
        acertosOrdem = 0;
        errosOrdem = 0;
        errosProps = 0;
    }

    public void compararRespostas()
    {
        if (Global.listaObjetos == null || Global.listaObjetos.Count == 0 || numeroExerc == 0)
        {
            enunText.text = "Verifique se a lista de objetos está vazia ou se um exercício foi selecionado.";
            StartCoroutine(zerarToggle());
        }
        else
        {
            List<GameObject> ordenada = arq.ordenarCena(Global.listaObjetos);
            string[] gabarito = Resposta.getRespostaOrdem();
            Dictionary<string, PropriedadePeca> propriedades = Resposta.getRespostaProps();
            PropriedadeCamera camProps = Resposta.getCamProps(numeroExerc);
            //tem q comparar a camera separadamente
            if (Global.propCameraGlobal.FOV == camProps.FOV) contabilizarAcertosProps();
            else contabilizarErrosProps();

            if (Global.propCameraGlobal.Near == camProps.Near) contabilizarAcertosProps();
            else contabilizarErrosProps();

            if (Global.propCameraGlobal.Far == camProps.Far) contabilizarAcertosProps();
            else contabilizarErrosProps();

            if (Global.propCameraGlobal.PosX == camProps.PosX) contabilizarAcertosProps();
            else contabilizarErrosProps();

            if (Global.propCameraGlobal.PosY == camProps.PosY) contabilizarAcertosProps();
            else contabilizarErrosProps();

            if (Global.propCameraGlobal.PosZ == camProps.PosZ) contabilizarAcertosProps();
            else contabilizarErrosProps();
            contarChecagem();
            string chave = "";
            if (ordenada.Count > gabarito.Length)
            {
                enunText.text = "Há peças demais na cena, favor deixar apenas os necessários.";
                StartCoroutine(zerarToggle());
            }
            else
            {
                for (int i = 0; i < ordenada.Count; i++)
                {
                    if (ordenada[i].name.Contains(gabarito[i]))
                    {
                        contabilizarAcertosOrdem();
                        foreach (KeyValuePair<string, PropriedadePeca> pp in Global.propriedadePecas)
                        {
                            foreach (KeyValuePair<string, PropriedadePeca> prop in propriedades)
                            {
                                if (pp.Key.Contains(prop.Key) && pp.Key.Contains(ordenada[i].name))
                                {
                                    contarChecagem();
                                    if (pp.Key.Contains("Obj"))
                                    {
                                        //compara props objt
                                        if (pp.Value.Ativo == prop.Value.Ativo) contabilizarAcertosProps();
                                        else contabilizarErrosProps();
                                    }
                                    else if (pp.Key.Contains("Cubo"))
                                    {
                                        //compara cubo
                                        if (pp.Value.Ativo == prop.Value.Ativo) contabilizarAcertosProps();
                                        else contabilizarErrosProps();

                                        if (pp.Value.Pos.X == prop.Value.Pos.X) contabilizarAcertosProps();
                                        else contabilizarErrosProps();

                                        if (pp.Value.Pos.Y == prop.Value.Pos.Y) contabilizarAcertosProps();
                                        else contabilizarErrosProps();

                                        if (pp.Value.Pos.Z == prop.Value.Pos.Z) contabilizarAcertosProps();
                                        else contabilizarErrosProps();

                                        if (pp.Value.Tam.X == prop.Value.Tam.X) contabilizarAcertosProps();
                                        else contabilizarErrosProps();

                                        if (pp.Value.Tam.Y == prop.Value.Tam.Y) contabilizarAcertosProps();
                                        else contabilizarErrosProps();

                                        if (pp.Value.Tam.Z == prop.Value.Tam.Z) contabilizarAcertosProps();
                                        else contabilizarErrosProps();

                                        if (pp.Value.Cor != null && prop.Value.Cor != null)
                                        {
                                            if (pp.Value.Cor == prop.Value.Cor) contabilizarAcertosProps();
                                            else contabilizarErrosProps();
                                        }

                                        if (pp.Value.Textura != null && prop.Value.Textura != null)
                                        {
                                            if (pp.Value.Textura.name == prop.Value.Textura.name) contabilizarAcertosProps();
                                            else contabilizarErrosProps();
                                        }

                                    }
                                    else if (pp.Key.Contains("Ilumi"))
                                    {
                                        //compara luz
                                        if (pp.Value.TipoLuz == prop.Value.TipoLuz) contabilizarAcertosProps();
                                        else contabilizarErrosProps();

                                        PropriedadePeca luzinha = pp.Value;
                                        if (Global.propriedadeIluminacao.ContainsKey(pp.Key))
                                        {
                                            PropriedadePeca[] luzes = Global.propriedadeIluminacao[pp.Key];
                                            luzinha = luzes[pp.Value.TipoLuz];
                                        }

                                        if (prop.Value.TipoLuz > 0)
                                        {
                                            if (prop.Value.TipoLuz == 1)
                                            {
                                                if (luzinha.Intensidade == prop.Value.Intensidade) contabilizarAcertosProps();
                                                else contabilizarErrosProps();

                                                if (luzinha.ValorIluminacao.X == prop.Value.ValorIluminacao.X) contabilizarAcertosProps();
                                                else contabilizarErrosProps();

                                                if (luzinha.ValorIluminacao.Y == prop.Value.ValorIluminacao.Y) contabilizarAcertosProps();
                                                else contabilizarErrosProps();

                                                if (luzinha.ValorIluminacao.Z == prop.Value.ValorIluminacao.Z) contabilizarAcertosProps();
                                                else contabilizarErrosProps();
                                            }
                                            else if (prop.Value.TipoLuz == 2)
                                            {
                                                if (luzinha.Intensidade == prop.Value.Intensidade) contabilizarAcertosProps();
                                                else contabilizarErrosProps();

                                                if (luzinha.Distancia == prop.Value.Distancia) contabilizarAcertosProps();
                                                else contabilizarErrosProps();
                                            }
                                            else
                                            {
                                                if (luzinha.Angulo == prop.Value.Angulo) contabilizarAcertosProps();
                                                else contabilizarErrosProps();

                                                if (luzinha.Expoente == prop.Value.Expoente) contabilizarAcertosProps();
                                                else contabilizarErrosProps();

                                                if (luzinha.Intensidade == prop.Value.Intensidade) contabilizarAcertosProps();
                                                else contabilizarErrosProps();

                                                if (luzinha.Distancia == prop.Value.Distancia) contabilizarAcertosProps();
                                                else contabilizarErrosProps();

                                                if (luzinha.ValorIluminacao != null)
                                                {
                                                    if (luzinha.ValorIluminacao.X == prop.Value.ValorIluminacao.X) contabilizarAcertosProps();
                                                    else contabilizarErrosProps();

                                                    if (luzinha.ValorIluminacao.Y == prop.Value.ValorIluminacao.Y) contabilizarAcertosProps();
                                                    else contabilizarErrosProps();

                                                    if (luzinha.ValorIluminacao.Z == prop.Value.ValorIluminacao.Z) contabilizarAcertosProps();
                                                    else contabilizarErrosProps();
                                                }
                                            }
                                        }
                                        if (Global.propriedadeIluminacao.ContainsKey(pp.Key))
                                        {
                                            if (luzinha.Ativo == prop.Value.Ativo) contabilizarAcertosProps();
                                            else contabilizarErrosProps();
                                        }

                                        if (pp.Value.Cor != null && prop.Value.Cor != null)
                                        {
                                            if (pp.Value.Cor == prop.Value.Cor) contabilizarAcertosProps();
                                            else contabilizarErrosProps();
                                        }

                                        if (luzinha.Pos != null && prop.Value.Pos != null)
                                        {
                                            if (luzinha.Pos.X == prop.Value.Pos.X) contabilizarAcertosProps();
                                            else contabilizarErrosProps();

                                            if (luzinha.Pos.Y == prop.Value.Pos.Y) contabilizarAcertosProps();
                                            else contabilizarErrosProps();

                                            if (luzinha.Pos.Z == prop.Value.Pos.Z) contabilizarAcertosProps();
                                            else contabilizarErrosProps();
                                        }
                                    }
                                    else if (pp.Key.Contains("Esc"))
                                    {
                                        //compara escala
                                        if (pp.Value.Tam.X == prop.Value.Tam.X) contabilizarAcertosProps();
                                        else contabilizarErrosProps();

                                        if (pp.Value.Tam.Y == prop.Value.Tam.Y) contabilizarAcertosProps();
                                        else contabilizarErrosProps();

                                        if (pp.Value.Tam.Z == prop.Value.Tam.Z) contabilizarAcertosProps();
                                        else contabilizarErrosProps();

                                        if (pp.Value.Ativo == prop.Value.Ativo) contabilizarAcertosProps();
                                        else contabilizarErrosProps();
                                    }
                                    else if (pp.Key.Contains("Rot") || pp.Key.Contains("Trans"))
                                    {
                                        //compara outras ações
                                        if (pp.Value.Ativo == prop.Value.Ativo) contabilizarAcertosProps();
                                        else contabilizarErrosProps();

                                        if (pp.Value.Pos.X == prop.Value.Pos.X) contabilizarAcertosProps();
                                        else contabilizarErrosProps();

                                        if (pp.Value.Pos.Y == prop.Value.Pos.Y) contabilizarAcertosProps();
                                        else contabilizarErrosProps();

                                        if (pp.Value.Pos.Z == prop.Value.Pos.Z) contabilizarAcertosProps();
                                        else contabilizarErrosProps();
                                    }

                                    chave = prop.Key;
                                }
                            }
                            propriedades.Remove(chave);

                        }
                    }
                    else contabilizarErrosOrdem();
                }
                mostrarResultado();
            }
        }
        setChecar(checar);
    }
    private IEnumerator zerarToggle()
    {
        yield return new WaitForSeconds(3.0f);
        tg.GetFirstActiveToggle().isOn = false;
        nenhum.isOn = true;
    }
    private void mostrarResultado()
    {
        int checagem = checarProps[numeroExerc - 1];
        float total = acertosOrdem + acertosProps + errosOrdem + errosProps;
        float acertos = (acertosProps + acertosOrdem);
        float porc = (acertos / total) * 100f;
        string mensagem = "Você acertou " + porc.ToString("F") + "% do exercício!";

        if (porc != 100f)
        {
            if (acertosOrdem < acertosOrdem + errosOrdem) mensagem += "\nVerifique a ordem das peças em cena ou se alguma está faltando.";
            else if (acertosProps < acertosProps + errosProps) mensagem += "\nVerifique se as propriedades mencionadas no enunciado foram alteradas corretamente.";
            else if (checar != checagem) mensagem += "\nVerifique se não esqueceu de alterar as propriedade de alguma peça.";
        }

        enunText.text = mensagem;
        StartCoroutine(zerarToggle());
        zerarContadores();
    }
}
