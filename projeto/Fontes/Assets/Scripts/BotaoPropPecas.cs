using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoPropPecas : MonoBehaviour
{
    private BotaoPropPadrao btnPadrao; 

    private void OnMouseDown()
    {
        btnPadrao = new BotaoPropPadrao();
        btnPadrao.setButton(GameObject.Find("BtnPropPecas"), null, true);
    }

}
