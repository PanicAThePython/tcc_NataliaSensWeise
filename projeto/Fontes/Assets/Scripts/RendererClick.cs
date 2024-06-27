using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererClick : MonoBehaviour {

    const string PropRenderer = "PropRenderer";	

    private void OnMouseDown()
    {
        BotaoPropPadrao btn = new BotaoPropPadrao();
        btn.setButton(GameObject.Find("BtnPropPecas"), GameObject.Find(PropRenderer));
    }
}
