using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoFabPecas : MonoBehaviour {

    private BotaoPropPadrao btnPadrao;
    private Util_VisEdu visEdu;

    private void OnMouseDown()
    {       
        btnPadrao = new BotaoPropPadrao();
        btnPadrao.setButton(GameObject.Find("BtnFabPecas"), GameObject.Find("FabricaPecas"), true);

        visEdu = new Util_VisEdu();
        visEdu.enableColliderFabricaPecas(true, true);
    }

    public void CallOnMouseDown()
    {
        if (Tutorial.estaExecutandoTutorial)
            Tutorial.Nivel = "4.2";

        OnMouseDown();       
    }
}
