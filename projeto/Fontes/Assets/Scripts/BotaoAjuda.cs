using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoAjuda : MonoBehaviour
{
    private BotaoPropPadrao btnPadrao;    

    private void OnMouseDown()
    {        
        btnPadrao = new BotaoPropPadrao();
        btnPadrao.setButton(GameObject.Find("BtnAjuda"), GameObject.Find("Ajuda"), true);
    }
}
