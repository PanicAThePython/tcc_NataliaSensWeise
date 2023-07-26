using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FabricaPecas : MonoBehaviour {

    private Util_VisEdu visEdu = new Util_VisEdu();
    private static bool fabrica, render = false;
    private bool podeAlterar = true;

    void LateUpdate () {
        Vector3 screenPoint = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(gameObject.transform.position);

        if (Input.mousePosition.y > GameObject.Find("Main Camera").GetComponent<Camera>().pixelRect.height ||
            Input.mousePosition.x > (GameObject.Find("Main Camera").GetComponent<Camera>().pixelRect.width))
            podeAlterar = false;

        if (podeAlterar)
        {
            if (Input.mousePosition.y > (GameObject.Find("Main Camera").GetComponent<Camera>().pixelRect.height / 2) &&
            Input.mousePosition.x < (GameObject.Find("Main Camera").GetComponent<Camera>().pixelRect.width / 2))
            {
                if (!fabrica)
                {
                    visEdu.enableColliderFabricaPecas(false, true, true);
                    fabrica = true;
                    render = false;
                }
            }
            else
            {
                if (!render)
                {
                    visEdu.enableColliderFabricaPecas(true, true, true);
                    render = true;
                    fabrica = false;
                }
            }
        }                 
    }

    
}
