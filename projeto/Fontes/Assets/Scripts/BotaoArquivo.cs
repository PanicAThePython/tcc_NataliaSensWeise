using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoArquivo : MonoBehaviour
{    
    public GameObject botaoAjuda;
    public GameObject botaoPropriedadePecas;
    public GameObject botaoFabricaPecas;

    public GameObject painelAjuda;
    public GameObject painelPropriedadePecas;
    public GameObject painelFabricaPecas;
    public GameObject painelArquivo;

    private float x, y, z;

    void Start () {
        x = gameObject.transform.localScale.x;
        y = gameObject.transform.localScale.y;
        z = gameObject.transform.localScale.z;
    }

    private void OnMouseDown()
    {
        //Habilita botão de ajuda e desabilita os demais.
        gameObject.transform.localScale = new Vector3(x, 40, z);
        botaoAjuda.transform.localScale = new Vector3(x, 30, z);
        botaoPropriedadePecas.transform.localScale = new Vector3(x, 30, z);
        botaoFabricaPecas.transform.localScale = new Vector3(x, 30, z);

        //Ativa a tela de ajuda e desativa as demais.
        painelAjuda.SetActive(false);
        //painelPropriedadePecas.SetActive(false);
        painelFabricaPecas.SetActive(false);
        painelArquivo.SetActive(true);
    }
}
