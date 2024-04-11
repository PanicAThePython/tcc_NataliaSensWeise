using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialNovo : MonoBehaviour
{

    public bool comecar = false;
    public int passo = 1;
    public ChecarColisao colisao;
    public GameObject painelTutorial;
    public TMP_Text titulo;
    public TMP_Text info;

    private void Update()
    {
        abrirTutorial();
    }
    public IEnumerator apagarTela()
    {
        yield return new WaitForSeconds(3.0f);
        painelTutorial.SetActive(false);
    }

    public void ativarTela()
    {
        painelTutorial.SetActive(true);
    }
    public void abrirTutorial()
    {
        if (passo == 1 && colisao.encaixada && colisao.peca == "Camera")
        {
            passo = 2;
            titulo.text = "Passo 2";
            info.text = "Arraste a peça 'Objeto Gráfico' até o encaixe de seta no Renderer.";
        }
        else if (passo == 2 && colisao.encaixada && colisao.peca == "Objeto")
        {
            passo = 3;
            titulo.text = "Passo 3";
            info.text = "Arraste a peça 'Iluminação' até o encaixe de seta no Objeto Gráfico.";
        }
        else if (passo == 3 && colisao.encaixada && colisao.peca == "Iluminacao")
        {
            passo = 4;
            titulo.text = "Passo 4";
            info.text = "Arraste a peça 'Cubo' até o encaixe de quadrado no Objeto Gráfico.";

        }
        else if (passo == 4 && colisao.encaixada && colisao.peca == "Cubo")
        {
            passo = 5;
            titulo.text = "Passo 5";
            info.text = "Arraste a peça 'Escalar' até o encaixe de losango no Objeto Gráfico.";
        }
        else if (passo == 5 && colisao.encaixada && colisao.peca == "Escala")
        {
            passo = 6;
            titulo.text = "Passo 6";
            info.text = "Clique no 'Escalar' posicionado e mude o valor de x para 3.";
            print(gameObject);
        }
    }
}
