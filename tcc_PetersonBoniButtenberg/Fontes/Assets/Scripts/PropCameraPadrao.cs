using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class PropCameraPadrao : MonoBehaviour {

    public enum InputSelected { InputPosX, InputPosY, InputPosZ, InputLookAtX, InputLookAtY, InputLookAtZ, InputFOV, InputEmpty, InputFar, InputNear };

    private TMP_InputField PosX;
    private TMP_InputField PosY;
    private TMP_InputField PosZ;
    private TMP_InputField LookAtX;
    private TMP_InputField LookAtY;
    private TMP_InputField LookAtZ;
    private TMP_InputField FOV;
    private TMP_InputField Far;
    private TMP_InputField Near;

    private const float POS_X_INICIAL = -100;
    private const float POS_Y_INICIAL = 300;
    private const float POS_Z_INICIAL = 300;
    private const float FOV_INICIAL = 45;
    private const float NEAR_INICIAL = 100;
    private const float FAR_INICIAL = 600;

    public InputSelected inputSelected;
    protected string inputValue;
    protected bool podeAtualizarCamera;

    public void demosntraCamera(bool demonstra)
    {
        GameObject cam = GameObject.Find("CameraObjetoMain"); 

        for (int i = 0; i < cam.transform.childCount; i++)
            cam.transform.GetChild(i).transform.gameObject.SetActive(demonstra);

        Global.cameraAtiva = demonstra;
    } 

    protected void preencheCampos()
    {
        Global.propCameraGlobal.JaIniciouValores = true;

        gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = "Câmera";
        
        PosX = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
        PosY = gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
        PosZ = gameObject.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();

        LookAtX = gameObject.transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<TMP_InputField>();
        LookAtY = gameObject.transform.GetChild(2).GetChild(1).GetChild(1).GetComponent<TMP_InputField>();
        LookAtZ = gameObject.transform.GetChild(2).GetChild(2).GetChild(1).GetComponent<TMP_InputField>();

        FOV = gameObject.transform.GetChild(3).GetChild(1).GetComponent<TMP_InputField>();
        Near = gameObject.transform.GetChild(4).GetChild(1).GetComponent<TMP_InputField>();
        Far = gameObject.transform.GetChild(5).GetChild(1).GetComponent<TMP_InputField>();

        PosX.text = "100";
        PosY.text = "300";
        PosZ.text = "300";

        LookAtX.text = "0";
        LookAtY.text = "0";
        LookAtZ.text = "0";

        FOV.text = "45";
        Near.text = "100";
        Far.text = "600";

        Global.propCameraGlobal.PropInicial = new PropriedadeCameraInicial();
        GameObject goCameraObj = GameObject.Find("CameraObjetoMain");

        Global.propCameraGlobal.PosX = POS_X_INICIAL;
        Global.propCameraGlobal.PosY = POS_Y_INICIAL;
        Global.propCameraGlobal.PosZ = POS_Z_INICIAL;
        Global.propCameraGlobal.FOV = new Vector2(FOV_INICIAL, FOV_INICIAL) ;
        Global.propCameraGlobal.LookAtX = 0;
        Global.propCameraGlobal.LookAtY = 0;
        Global.propCameraGlobal.LookAtZ = 0;
        Global.propCameraGlobal.Near = NEAR_INICIAL;
        Global.propCameraGlobal.Far = FAR_INICIAL;

        Global.propCameraGlobal.PropInicial.PosX = goCameraObj.transform.position.x;
        Global.propCameraGlobal.PropInicial.PosY = goCameraObj.transform.position.y;
        Global.propCameraGlobal.PropInicial.PosZ = goCameraObj.transform.position.z;

        Global.propCameraGlobal.PropInicial.LookAtX = goCameraObj.transform.GetChild(0).transform.rotation.x;
        Global.propCameraGlobal.PropInicial.LookAtY = goCameraObj.transform.GetChild(0).transform.rotation.y;
        Global.propCameraGlobal.PropInicial.LookAtZ = goCameraObj.transform.GetChild(0).transform.rotation.z;

        Global.propCameraGlobal.PropInicial.FOV = new Vector2(goCameraObj.transform.localScale.x / FOV_INICIAL, goCameraObj.transform.localScale.y / FOV_INICIAL);
        Global.propCameraGlobal.PropInicial.Near = 100;
        Global.propCameraGlobal.PropInicial.Far = 600;


        goCameraObj = GameObject.Find("CameraObjetoMain");
        goCameraObj.transform.position =
            new Vector3(Global.propCameraGlobal.PropInicial.PosX + Global.propCameraGlobal.PosX,
                        Global.propCameraGlobal.PropInicial.PosY + Global.propCameraGlobal.PosY,
                        Global.propCameraGlobal.PropInicial.PosZ + Global.propCameraGlobal.PosZ);

        GameObject goCameraPos = GameObject.Find("CameraObjectPos");
        goCameraPos.transform.localPosition =
            new Vector3(-Global.propCameraGlobal.PosX * 14,
                        Global.propCameraGlobal.PosY * 13.333333f,
                        -Global.propCameraGlobal.PosZ * 16);

        GameObject.Find("CameraVisInferior").GetComponent<Camera>().nearClipPlane = Global.propCameraGlobal.PropInicial.Near;
        GameObject.Find("CameraVisInferior").GetComponent<Camera>().farClipPlane = Global.propCameraGlobal.PropInicial.Far;
        //print(new Vector3(Global.propCameraGlobal.LookAtX, Global.propCameraGlobal.LookAtY, Global.propCameraGlobal.LookAtZ));
        //goCameraObj.transform.GetChild(0).transform.LookAt(GameObject.Find("CuboAmb").transform, new Vector3(0,0,0));

    }

    protected void updatePosition(Camera cam)
    {
        inputSelected = GetInputSelected(gameObject.name);
        inputValue = gameObject.GetComponent<TMP_InputField>().text;

        if (inputSelected == InputSelected.InputPosX)
            Global.propCameraGlobal.PosX = - float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
        else if (inputSelected == InputSelected.InputPosY)
            Global.propCameraGlobal.PosY = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
        else if (inputSelected == InputSelected.InputPosZ)
        {
            Global.propCameraGlobal.PosZ = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
            podeAtualizarCamera = true;
        }
        if (inputSelected == InputSelected.InputLookAtX)
        {
            Global.propCameraGlobal.LookAtX = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
        }
        else if (inputSelected == InputSelected.InputLookAtY)
        {
            Global.propCameraGlobal.LookAtY = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);

        }
        else if (inputSelected == InputSelected.InputLookAtZ)
        {
            Global.propCameraGlobal.LookAtZ = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
        }

        if (inputSelected == InputSelected.InputFOV)
        {
            Global.propCameraGlobal.FOV = new Vector2(float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat), float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat));

            //Altera FoV (Field of View)
            GameObject.Find("CameraVisInferior").GetComponent<Camera>().fieldOfView = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
        }
        if (inputSelected == InputSelected.InputFar)
        {
            Global.propCameraGlobal.Far = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);
            GameObject.Find("CameraVisInferior").GetComponent<Camera>().farClipPlane = Global.propCameraGlobal.Far;
        }
        if (inputSelected == InputSelected.InputNear)
        {
            Global.propCameraGlobal.Near = float.Parse(validaVazio(inputValue), CultureInfo.InvariantCulture.NumberFormat);

            GameObject.Find("CameraVisInferior").GetComponent<Camera>().nearClipPlane = Global.propCameraGlobal.Near;
            //GameObject.Find("CameraObjetoMain").transform.GetChild(0).GetComponent<Camera>().nearClipPlane = Global.propCameraGlobal.Near;
        }

        //Atualiza posição da camera
        GameObject goCameraObj = GameObject.Find("CameraObjetoMain");
        goCameraObj.transform.position =
            new Vector3(Global.propCameraGlobal.PropInicial.PosX + Global.propCameraGlobal.PosX,
                        Global.propCameraGlobal.PropInicial.PosY + Global.propCameraGlobal.PosY,
                        Global.propCameraGlobal.PropInicial.PosZ + Global.propCameraGlobal.PosZ);

        GameObject goCameraPos = GameObject.Find("CameraObjectPos");
        goCameraPos.transform.localPosition =
            new Vector3(-Global.propCameraGlobal.PosX * 14,
                        Global.propCameraGlobal.PosY * 13.333333f,
                        -Global.propCameraGlobal.PosZ * 16);

        //Atualiza Look At - *********** Não está funcionando ***********
        if (inputSelected == InputSelected.InputLookAtX || inputSelected == InputSelected.InputLookAtY || inputSelected == InputSelected.InputLookAtZ )
        {
            //ACHO Q CONSEGUII

            //goCameraObj.transform.LookAt(GameObject.Find("CuboAmb").transform, new Vector3(10000, 0, 0));
            //  Quaternion.Euler(Global.propCameraGlobal.PropInicial.LookAtX + Global.propCameraGlobal.LookAtX,
            //                 Global.propCameraGlobal.PropInicial.LookAtY + Global.propCameraGlobal.LookAtY,
            //               Global.propCameraGlobal.PropInicial.LookAtZ + Global.propCameraGlobal.LookAtZ);
            goCameraObj.transform.GetChild(0).transform.LookAt(GameObject.Find("CuboAmb").transform, new Vector3( Global.propCameraGlobal.LookAtX, Global.propCameraGlobal.LookAtY, Global.propCameraGlobal.LookAtZ));
        }
            

        //Atualiza FOV da camera (Scale)
        goCameraObj.transform.localScale =
            new Vector3(Global.propCameraGlobal.PropInicial.FOV.x * Global.propCameraGlobal.FOV.x,
                        Global.propCameraGlobal.PropInicial.FOV.y * Global.propCameraGlobal.FOV.y,
                        goCameraObj.transform.localScale.z);
    } 

    protected bool jaClicouEmAlgumObjeto()
    {
        return Global.gameObjectName != null;
    }    

    private string validaVazio(string valor)
    {
        if (Equals(valor, System.String.Empty))
        {            
            return "0";
        }
        return valor;
    }

    private float multM(float transform)
    {
        return transform * 1000;
    }

    public InputSelected GetInputSelected(string inputName)
    {
        if (inputName.Contains("PosicaoX"))
            return InputSelected.InputPosX;
        else if (inputName.Contains("PosicaoY"))
            return InputSelected.InputPosY;
        else if (inputName.Contains("PosicaoZ"))
            return InputSelected.InputPosZ;
        else if (inputName.Contains("LookAtX"))
            return InputSelected.InputLookAtX;
        else if (inputName.Contains("LookAtY"))
            return InputSelected.InputLookAtY;
        else if (inputName.Contains("LookAtZ"))
            return InputSelected.InputLookAtZ;
        else if (inputName.Contains("FOV"))
            return InputSelected.InputFOV;
        else if (inputName.Contains("Near"))
            return InputSelected.InputNear;
        else if (inputName.Contains("Far"))
            return InputSelected.InputFar;

        return InputSelected.InputEmpty;
    }

    public void AjustaCameraEmX()
    {
        GameObject go = GameObject.Find("CameraObjetoMain");
        go.transform.GetComponent<RectTransform>().transform.position = 
            new Vector3(go.transform.GetComponent<RectTransform>().transform.position.x + 1,
                        go.transform.GetComponent<RectTransform>().transform.position.y,
                        go.transform.GetComponent<RectTransform>().transform.position.z);
    }
}
