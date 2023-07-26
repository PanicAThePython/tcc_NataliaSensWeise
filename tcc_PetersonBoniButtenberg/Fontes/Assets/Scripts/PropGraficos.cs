using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropGraficos : MonoBehaviour {

    public TMP_Dropdown mainDropdown;
    public Toggle toggleField;
    public Camera cam;
    public GameObject camObjeto;
    public Camera camVis;
    public GameObject camVisPai;

    private GameObject gizmoPivot;

    void Start()
    {
        if (mainDropdown != null)
            mainDropdown.onValueChanged.AddListener(delegate { AdicionaValorPropriedade(); });

        if (toggleField != null)
            toggleField.onValueChanged.AddListener(delegate { AdicionaValorPropriedadeToggle(); });
    }

    private void AdicionaValorPropriedade()
    {
        if (mainDropdown.GetComponent<TMP_Dropdown>().value == 0) // 3D
        {
            cam.orthographic = false;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, 125, cam.transform.localPosition.z);
            cam.transform.localRotation = Quaternion.Euler(33.75f, -180, 0);
            cam.GetComponent<MoveAmbiente>().enabled = true;

            camVis.orthographic = false;

            camVisPai.transform.localPosition = new Vector3(1400f, 4000f, -4800f);
            camVisPai.transform.localRotation = Quaternion.Euler(37.28f, -15.229f, 0);

            camObjeto.transform.localPosition = new Vector3(966f, 0, -19962.67f);
            camObjeto.transform.localRotation = Quaternion.Euler(38.701f, -195.786f, -100.024f);

            Global.Grafico2D = false;
        }
        else // 2D
        {
            cam.orthographic = true;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, -200, cam.transform.localPosition.z);
            cam.transform.localRotation = Quaternion.Euler(0, -180, 0);
            cam.orthographicSize = 300f;
            cam.GetComponent<MoveAmbiente>().enabled = false;
            
            camVis.orthographic = true;
            camVis.orthographicSize = 150f;

            camVisPai.transform.localPosition = new Vector3(31, 49, -4256);
            camVisPai.transform.localRotation = Quaternion.Euler(0.215f, 0, 0);

            camObjeto.transform.localPosition = new Vector3(1057f, -296, -19920);
            camObjeto.transform.localRotation = Quaternion.Euler(0.475f, -181.515f, -90.013f);

            Global.Grafico2D = true;
        }
    }

    private void AdicionaValorPropriedadeToggle()
    {
        if (!toggleField.GetComponent<Toggle>().isOn)
            EnableGizmo(false);
        else
            EnableGizmo(true);
    }


    private void EnableGizmo(bool status)
    {
        gizmoPivot = GameObject.Find("GizmoPivot");

        for (int i = 0; i < gizmoPivot.transform.GetChild(0).childCount; i++)
            gizmoPivot.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().enabled = status;
    }  

    
}
