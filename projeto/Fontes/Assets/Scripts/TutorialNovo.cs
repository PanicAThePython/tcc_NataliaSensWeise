using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialNovo : MonoBehaviour
{

    [SerializeField] private int passo = 0;
    [SerializeField] private ChecarColisao colisao;
    public GameObject painelTutorial;
    public GameObject[] passosTutorial;
    public GameObject[] passosTutorialExtra;
    public TMP_InputField escalarTexto;
    public TMP_InputField transladarTextoFilho;
    public TMP_InputField transladarTextoPai;
    public GameObject escala;
    public GameObject transladar;
    public Toggle grade;
    public GameObject render;
    private bool extra = false;

    private void Update()
    {
        if (!extra) abrirTutorial();
        if (extra) abrirTutorialExtra();
    }
    public static IEnumerator apagarTela(GameObject tela)
    {
        yield return new WaitForSeconds(3.0f);
        tela.SetActive(false);
    }

    public void ativarTela()
    {
        extra = false;
        passo = 0;
        painelTutorial.SetActive(true);
        passosTutorial[passo].SetActive(true);
    }
    public void ativarTelaExtra()
    {
        extra = true;
        passo = 0;
        painelTutorial.SetActive(true);
        //passosTutorial[passo].SetActive(false);
        passosTutorialExtra[passo].SetActive(true);
    }
    private void tutorialManager(GameObject[] tutorial)
    {
        tutorial[passo].SetActive(false);
        passo++;
        tutorial[passo].SetActive(true);
    }

    public void abrirTutorial()
    {
        switch (passo)
        {
            case 0 when render.activeSelf && !grade.isOn:
            case 1 when colisao.encaixada && colisao.peca == "Camera":
            case 2 when colisao.encaixada && colisao.peca == "Objeto":
            case 3 when colisao.encaixada && colisao.peca == "Iluminacao":
            case 4 when colisao.encaixada && colisao.peca == "Cubo":
            case 5 when colisao.encaixada && colisao.peca == "Escala":
            case 6 when escala.activeSelf && escalarTexto.text == "3":
            case 7 when Global.listaEncaixes.Count == 0:
                tutorialManager(passosTutorial);
                break;
            case 8:
                StartCoroutine(apagarTela(passosTutorial[passo]));
                StartCoroutine(apagarTela(painelTutorial));
                passo = 0;
                grade.isOn = true;
                break;
        }
    }

    public void abrirTutorialExtra()
    {
        switch (passo)
        {
            case 0 when colisao.encaixada && colisao.peca == "Camera":
            case 1 when colisao.encaixada && colisao.peca == "Objeto":
            case 2 when colisao.encaixada && colisao.peca == "Iluminacao":
            case 3 when colisao.encaixada && colisao.peca == "Objeto":
            case 4 when colisao.encaixada && colisao.peca == "Translacao" && transladar.activeSelf && transladarTextoFilho.text == "2":
            case 5 when colisao.encaixada && colisao.peca == "Cubo":
            case 7 when colisao.encaixada && colisao.peca == "Cubo":
            case 6 when colisao.encaixada && colisao.peca == "Translacao" && transladar.activeSelf && transladarTextoPai.text == "2":
                tutorialManager(passosTutorialExtra);
                break;
            case 8:
                StartCoroutine(apagarTela(passosTutorialExtra[passo]));
                StartCoroutine(apagarTela(painelTutorial));
                passo = 0;
                grade.isOn = true;
                break;
        }
    }
}
