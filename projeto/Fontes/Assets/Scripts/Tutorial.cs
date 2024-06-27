using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class Tutorial : MonoBehaviour
{

    public enum Passo
    {
        PulouTutorial = 0,
        PrimeiroPasso = 1,
        SegundoPasso = 2,
        TerceiroPasso = 3,
        QuartoPasso = 4,
        QuintoPasso = 5,
        SextoPasso = 6,
        SetimoPasso = 7,
        UltimoPasso = 8,
        FistTime = 99
    };

    public static bool PrimeiroPassoExecutado;
    public static bool SegundoPassoExecutado;
    public static bool TerceiroPassoExecutado;
    public static bool QuartoPassoExecutado;
    public static bool QuitoPassoExecutado;
    public static bool SextoPassoExecutado;
    public static bool SetimoPassoExecutado;

    private const string TituloFisrt = "Bem Vindo ao VisEdu!";
    private const string Titulo = "Tutorial VisEdu...";
    private const string MsgConfirmacao = "Deseja realizar o tutorial para aprender os conceitos básicos da plataforma?";
    private const string MsgPassoUm = "Arraste a peça 'Objeto Gráfico' até o encaixe adequado.";
    private const string MsgPassoDois = "Arraste a peça 'Cubo' até o encaixe adequado.";
    private const string MsgPassoTres = "Clique sobre a peça 'Cubo' e altere o tamanho.";
    private const string MsgPassoQuatro = "Clique sobre o menu 'Fábrica de peça' e arraste a peça 'Câmera' até o encaixe adequado.";
    private const string MsgPassoCinco = "Arraste a peça 'Rotacionar' até o encaixe adequado e troque seu valor no eixo y.";
    private const string MsgPassoSeis = "Arraste a peça 'Iluminação' e veja a visualização da peça no painel 'Visualizador'.";
    private const string MsgPassoSete = "Arraste a peça 'Objeto Gráfico' até a lixeira.";
    private const string MsgUltimoPasso = "Agora você já pode fazer a sua árvore de peças.";
    private const string BtnSim = "Sim";
    private const string BtnProximo = "Próximo";
    private const string BtnPular = "Pular tutorial";

    public static Passo passoTutorial = Passo.FistTime;
    public static GameObject GOTutorial;
    public static bool estaExecutandoTutorial;
    public static GameObject CursorMouse;
    public static string Nivel;
    public static bool JaChamouMensagem;
    public static bool AbriuMessageBox;
    public static int AnswerMsg;

    public static void MensagemTutorial(bool proximo = false)
    {
        estaExecutandoTutorial = true;

        if (passoTutorial != Passo.PulouTutorial)
        {
            if (passoTutorial == Passo.FistTime)
            {
                if (GameObject.Find("PainelConfirmacao") != null)
                {
                    if (!AbriuMessageBox)
                        MessageBoxVisEdu("PainelConfirmacao", true);

                    CursorMouse = GameObject.Find("CursorMouse");

                    if (AnswerMsg != 0)
                    {
                        //if (EditorUtility.DisplayDialog(TituloFisrt, MsgConfirmacao, BtnSim, BtnPular)) 
                        if (AnswerMsg == 1)
                        {                           
                            CursorMouse.GetComponent<RawImage>().enabled = true;

                            GOTutorial = Instantiate(GameObject.Find("ObjetoGraficoP"), GameObject.Find("ObjetoGraficoP").transform.position, GameObject.Find("ObjetoGraficoP").transform.rotation, GameObject.Find("ObjetoGraficoP").transform.parent);
                            GOTutorial.name = "ObjetoGraficoPTutorial";
                            GOTutorial.transform.position = new Vector3(GameObject.Find("ObjetoGraficoP").transform.position.x, GameObject.Find("ObjetoGraficoP").transform.position.y, GameObject.Find("ObjetoGraficoP").transform.position.z);

                            passoTutorial = Passo.PrimeiroPasso;

                            MessageBoxVisEdu("PainelConfirmacao", false);
                        }
                        else
                        {
                            passoTutorial = Passo.PulouTutorial;
                            estaExecutandoTutorial = false;
                            //CursorMouse.GetComponent<RawImage>().enabled = false;
                            MessageBoxVisEdu("PainelConfirmacao", false);
                        }

                        AnswerMsg = 0;
                    }
                }
            }

            JaChamouMensagem = true;

            if (passoTutorial == Passo.PrimeiroPasso)
            {
                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg01", true);

                //if (!EditorUtility.DisplayDialog(Titulo, MsgPassoUm, BtnProximo, BtnPular))
                if (AnswerMsg == 2)
                {
                    new Tutorial().PulouTutorial();
                    passoTutorial = Passo.PulouTutorial;
                }

                AnswerMsg = 0;
            }
            else if (passoTutorial == Passo.SegundoPasso)
            {
                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg02", true);

                AnswerMsg = 0;
                //if (!EditorUtility.DisplayDialog(Titulo, MsgPassoDois, BtnProximo, BtnPular))
                if (AnswerMsg == 2)
                {
                    new Tutorial().PulouTutorial();
                    passoTutorial = Passo.PulouTutorial;
                }
                else 
                {
                    CursorMouse.transform.position = new Vector3(GameObject.Find("Cubo").transform.position.x + 2.5f, GameObject.Find("Cubo").transform.position.y - 1, GameObject.Find("Cubo").transform.position.z - 6);

                    GOTutorial = Instantiate(GameObject.Find("Cubo"), GameObject.Find("Cubo").transform.position, GameObject.Find("Cubo").transform.rotation, GameObject.Find("Cubo").transform.parent);
                    GOTutorial.name = "CuboTutorial";
                    GOTutorial.transform.position = new Vector3(GameObject.Find("Cubo").transform.position.x, GameObject.Find("Cubo").transform.position.y, GameObject.Find("Cubo").transform.position.z);
                }
            }
            else if (passoTutorial == Passo.TerceiroPasso)
            {
                //if (!EditorUtility.DisplayDialog(Titulo, MsgPassoTres, BtnProximo, BtnPular))
                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg03", true);

                AnswerMsg = 0;

                //if (AnswerMsg == 2)
                //{
                //    new Tutorial().PulouTutorial();
                //    passoTutorial = Passo.PulouTutorial;
                //}
            }
            else if (passoTutorial == Passo.QuartoPasso)
            {
                Nivel = "4.1";

                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg04", true);

                AnswerMsg = 0;

                //if (!EditorUtility.DisplayDialog(Titulo, MsgPassoQuatro, BtnProximo, BtnPular))
                //{
                //    new Tutorial().PulouTutorial();
                //    passoTutorial = Passo.PulouTutorial;
                //}
                //else
                //{
                GOTutorial = Instantiate(GameObject.Find("CameraP"), GameObject.Find("CameraP").transform.position, GameObject.Find("CameraP").transform.rotation, GameObject.Find("CameraP").transform.parent);
                GOTutorial.name = "CameraTutorial";
                GOTutorial.transform.position = new Vector3(GameObject.Find("CameraP").transform.position.x, GameObject.Find("CameraP").transform.position.y, GameObject.Find("CameraP").transform.position.z);
                //}
            }
            else if (passoTutorial == Passo.QuintoPasso)
            {
                Nivel = "4.1";

                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg05", true);

                AnswerMsg = 0;

                //if (!EditorUtility.DisplayDialog(Titulo, MsgPassoCinco, BtnProximo, BtnPular))
                //{
                //    new Tutorial().PulouTutorial();
                //    passoTutorial = Passo.PulouTutorial;
                //}
                //else
                //{
                GOTutorial = Instantiate(GameObject.Find("Rotacionar"), GameObject.Find("Rotacionar").transform.position, GameObject.Find("Rotacionar").transform.rotation, GameObject.Find("Rotacionar").transform.parent);
                GOTutorial.name = "RotacionarTutorial";
                GOTutorial.transform.position = new Vector3(GameObject.Find("Rotacionar").transform.position.x, GameObject.Find("Rotacionar").transform.position.y, GameObject.Find("Rotacionar").transform.position.z);
                //}
            }
            else if (passoTutorial == Passo.SextoPasso)
            {
                Nivel = "4.1";

                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg06", true);

                AnswerMsg = 0;

                //if (!EditorUtility.DisplayDialog(Titulo, MsgPassoSeis, BtnProximo, BtnPular))
                //{
                //    new Tutorial().PulouTutorial();
                //    passoTutorial = Passo.PulouTutorial;
                //}
                //else
                //{
                GOTutorial = Instantiate(GameObject.Find("Iluminacao"), GameObject.Find("Iluminacao").transform.position, GameObject.Find("Iluminacao").transform.rotation, GameObject.Find("Iluminacao").transform.parent);
                GOTutorial.name = "IluminacaoTutorial";
                GOTutorial.transform.position = new Vector3(GameObject.Find("Iluminacao").transform.position.x, GameObject.Find("Iluminacao").transform.position.y, GameObject.Find("Iluminacao").transform.position.z);
                //}
            }
            else if (passoTutorial == Passo.SetimoPasso)
            {
                Nivel = "4.1";

                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg07", true);

                AnswerMsg = 0;

                //if (!EditorUtility.DisplayDialog(Titulo, MsgPassoSete, BtnProximo, BtnPular))
                //{
                //    new Tutorial().PulouTutorial();
                //    passoTutorial = Passo.PulouTutorial;
                //}
                //else
                    GOTutorial = GameObject.Find("ObjetoGraficoPTutorial");
            }
            else if (passoTutorial == Passo.UltimoPasso)
            {
                //if (EditorUtility.DisplayDialog(Titulo, MsgUltimoPasso, "Ok"))
                //{
                //    new Tutorial().PulouTutorial();
                //    passoTutorial = Passo.PulouTutorial;
                //}

                if (!AbriuMessageBox)
                    MessageBoxVisEdu("PainelMsg07", true);

                AnswerMsg = 0;
            }
        }
    }

    public void PulouTutorial()
    {
        estaExecutandoTutorial = false;
        CursorMouse.GetComponent<RawImage>().enabled = false;
        passoTutorial = Passo.PulouTutorial;

        Destroy(GameObject.Find("ObjetoGraficoPTutorial"));
        Destroy(GameObject.Find("CuboTutorial"));
        Destroy(GameObject.Find("CameraTutorial"));
        Destroy(GameObject.Find("RotacionarTutorial"));
        Destroy(GameObject.Find("IluminacaoTutorial"));
        Destroy(GameObject.Find("ObjGraficoSlot1"));
        Destroy(GameObject.Find("TransformacoesSlot_1"));

        if (GameObject.Find("FormasSlot") != null)
            GameObject.Find("FormasSlot").SetActive(false);

        if (GameObject.Find("TransformacoesSlot") != null)
            GameObject.Find("TransformacoesSlot").SetActive(false);

        if (GameObject.Find("IluminacaoSlot") != null)
        {
            GameObject ilumunacao = GameObject.Find("IluminacaoSlot");

            if (GameObject.Find("TransformacoesSlot_1") != null)
            {
                Vector3 pos = ilumunacao.transform.position;
                pos.y = pos.y + 3;
                ilumunacao.transform.position = pos;
            }
            
            GameObject.Find("IluminacaoSlot").SetActive(false);
        }            

        if (GameObject.Find("BaseObjetoGrafico") != null)
            GameObject.Find("BaseObjetoGrafico").SetActive(false);

        if (GameObject.Find("BaseRenderLateralGO").transform.GetChild(0) != null)
            GameObject.Find("BaseRenderLateralGO").transform.GetChild(0).gameObject.SetActive(false);

        GameObject.Find("CameraVisInferior").GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Nothing");
        new PropCameraPadrao().demosntraCamera(false);

        GameObject.Find("CuboAmbiente").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("CuboAmbiente").transform.localRotation = Quaternion.Euler(0, 0, 0);
        GameObject.Find("CuboAmbiente").GetComponent<MeshRenderer>().enabled = false;

        BotaoFabPecas btn = new BotaoFabPecas();
        btn.CallOnMouseDown();

        GameObject.Find("Msg01").GetComponent<RawImage>().enabled = false;
        GameObject.Find("Msg02").GetComponent<RawImage>().enabled = false;
        GameObject.Find("Msg03").GetComponent<RawImage>().enabled = false;
        GameObject.Find("Msg04").GetComponent<RawImage>().enabled = false;
        GameObject.Find("Msg05").GetComponent<RawImage>().enabled = false;
        GameObject.Find("Msg06").GetComponent<RawImage>().enabled = false;
        GameObject.Find("Msg07").GetComponent<RawImage>().enabled = false;

        for (int i = 0; i < GameObject.Find("PainelBase").transform.childCount; i++)
            MessageBoxVisEdu(GameObject.Find("PainelBase").transform.GetChild(i).name, false);

        Global.listaSequenciaSlots.Clear();        
    }

    public static float GetDistance()
    {
        if (passoTutorial == Passo.PrimeiroPasso)
        {
            return Vector3.Distance(GOTutorial.transform.position, new Vector3(GameObject.Find("ObjGraficoSlot").transform.position.x + 4.55f, GameObject.Find("ObjGraficoSlot").transform.position.y + 0.2f,
                GameObject.Find("ObjGraficoSlot").transform.position.z - 2));
        }
        else if (passoTutorial == Passo.SegundoPasso)
        {
            return Vector3.Distance(GOTutorial.transform.position, new Vector3(GameObject.Find("FormasSlot").transform.position.x + 3.1f, GameObject.Find("FormasSlot").transform.position.y + 0.2f,
                    GameObject.Find("FormasSlot").transform.position.z - 3));
        }
        else if (passoTutorial == Passo.SextoPasso)
        {
            Nivel = "4.2";

            return Vector3.Distance(GOTutorial.transform.position, new Vector3(GameObject.Find("IluminacaoSlot").transform.position.x + 5.1f, GameObject.Find("IluminacaoSlot").transform.position.y + 0.9f,
                    GameObject.Find("IluminacaoSlot").transform.position.z - 4));
        }

        return 0;
    }

    public static void SetMessageImage(bool status)
    {
        string msg = string.Empty;

        if (passoTutorial == Passo.PrimeiroPasso)
            msg = "Msg01";
        else if (passoTutorial == Passo.SegundoPasso)
            msg = "Msg02";
        else if (passoTutorial == Passo.TerceiroPasso)
            msg = "Msg03";
        else if (passoTutorial == Passo.QuartoPasso)
            msg = "Msg04";
        else if (passoTutorial == Passo.QuintoPasso)
            msg = "Msg05";
        else if (passoTutorial == Passo.SextoPasso)
            msg = "Msg06";
        else if (passoTutorial == Passo.SetimoPasso)
            msg = "Msg07";

        if (msg != string.Empty)
            GameObject.Find(msg).GetComponent<RawImage>().enabled = status;
    }

    public static void MessageBoxVisEdu(string painel, bool active)
    {
        if (GameObject.Find(painel) != null)
        {
            GameObject.Find(painel).GetComponent<RawImage>().enabled = active;            
            GameObject.Find(painel).transform.GetChild(0).gameObject.SetActive(active);

            if (GameObject.Find(painel).transform.childCount > 1)
                GameObject.Find(painel).transform.GetChild(1).gameObject.SetActive(active);

            GameObject.Find(painel).transform.parent.GetComponent<BoxCollider>().enabled = active;

            AbriuMessageBox = active;
        }
    }



}

