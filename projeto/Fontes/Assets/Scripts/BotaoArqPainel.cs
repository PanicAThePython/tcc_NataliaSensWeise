using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoArqPainel : MonoBehaviour
{
    private BotaoPropPadrao btnPadrao;

    private void OnMouseDown()
    {
        btnPadrao = new BotaoPropPadrao();
        btnPadrao.setButton(GameObject.Find("BtnArquivo"), GameObject.Find("Arquivo"), true);
    }
}
