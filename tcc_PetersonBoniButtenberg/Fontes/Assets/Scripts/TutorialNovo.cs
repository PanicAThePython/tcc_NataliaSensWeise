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
    public TMP_InputField escalarTexto;
    public GameObject escala;
    public Toggle grade;
    public GameObject render;

    private void Update()
    {
        abrirTutorial();
    }
    public static IEnumerator apagarTela(GameObject tela)
    {
        yield return new WaitForSeconds(3.0f);
        tela.SetActive(false);
    }

    public void ativarTela()
    {
        painelTutorial.SetActive(true);
        passosTutorial[passo].SetActive(true);
    }
    private void tutorialManager()
    {
        passosTutorial[passo].SetActive(false);
        passo++;
        passosTutorial[passo].SetActive(true);
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
                tutorialManager();
                break;
            case 8:
                StartCoroutine(apagarTela(passosTutorial[passo]));
                StartCoroutine(apagarTela(painelTutorial));
                passo = 0;
                grade.isOn = true;
                break;
        }
    }
}
