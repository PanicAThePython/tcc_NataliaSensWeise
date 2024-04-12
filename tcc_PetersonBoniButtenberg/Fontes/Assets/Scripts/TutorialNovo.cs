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
    public GameObject xEscalar;
    public TMP_InputField escalarTexto;

    private void Update()
    {
        abrirTutorial();
    }
    public IEnumerator apagarTela(GameObject tela)
    {
        yield return new WaitForSeconds(3.0f);
        tela.SetActive(false);
    }
    /*
    public IEnumerator delay()
    {
        yield return new WaitForSeconds(5.0f);
    }
    */

    public void ativarTela()
    {
        painelTutorial.SetActive(true);
        passosTutorial[passo].SetActive(true);
    }
    private void tutorialManager()
    {
        //StartCoroutine(delay());
        passosTutorial[passo].SetActive(false);
        passo++;
        passosTutorial[passo].SetActive(true);
    }

    public void abrirTutorial()
    {

        switch (passo)
        {
            case 0:
                if (colisao.encaixada && colisao.peca == "Camera")
                {
                    tutorialManager();
                }
                break;
            case 1:
                if (colisao.encaixada && colisao.peca == "Objeto")
                {
                    tutorialManager();
                }
                break; 
            case 2:
                if (colisao.encaixada && colisao.peca == "Iluminacao")
                {
                    tutorialManager();
                }
                break;
            case 3:
                if (colisao.encaixada && colisao.peca == "Cubo")
                {
                    tutorialManager();
                }
                break;
            case 4:
                if (colisao.encaixada && colisao.peca == "Escala")
                {
                    tutorialManager();
                }
                break;
            case 5:
                if (xEscalar.activeSelf && escalarTexto.text == "3")
                {
                    tutorialManager();
                }

                break;
            case 6:
                if (Global.listaEncaixes.Count == 0)
                {
                    tutorialManager();
                }
                break;
            case 7:
                StartCoroutine(apagarTela(passosTutorial[passo]));
                StartCoroutine(apagarTela(painelTutorial));
                passo = 0;
                break;
        }
    }
}
