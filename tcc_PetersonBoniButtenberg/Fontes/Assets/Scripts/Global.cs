using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TamanhoGlobal
{
    public static float x, y, z;
}

public static class PosicaoGlobal
{
    public static float x, y, z;
}

public class Global : MonoBehaviour {     

    public static List<GameObject> listaObjetos;
    public static List<string> listaSequenciaSlots = new List<string>();
    public static List<string> cuboVisComIluminacao = new List<string>();
    
    public static Dictionary<string, int> listObjectCount = new Dictionary<string, int>();
    //public static Dictionary<string, ArrayList> propriedadePecas = new Dictionary<string, ArrayList>();
    public static Dictionary<string, PropriedadePeca> propriedadePecas = new Dictionary<string, PropriedadePeca>();
    public static Dictionary<string, GameObject> listLastGOTransformacoesSlot = new Dictionary<string, GameObject>();
    public static Dictionary<string, List<GameObject>> listaPosicaoObjetosRender = new Dictionary<string, List<GameObject>>();
    public static Dictionary<string, string> listaEncaixes = new Dictionary<string, string>(); // Peça / Slot
    public static Dictionary<string, int> listUltimoTransladar = new Dictionary<string, int>();
    public static Dictionary<string, float> listaPosicaoSlot = new Dictionary<string, float>(); // Slot / Posição de y
    public static Dictionary<string, PropriedadePeca[]> propriedadeIluminacao = new Dictionary<string, PropriedadePeca[]>();
    public static ArrayList lightObjectList = new ArrayList();

    public static GameObject lastPressedButton;
    public static string gameObjectName;
    public static bool podeAtualizar;
    public static string lastObjetoGraficoName;
    public static string slotName;
    public static string nomeObjetoDestuido;
    public static bool podeInciarLista = true;
    public static bool podeExcruiObjeto;
    public static GameObject cuboVis;
    public static GameObject posAmb;
    public static bool cameraAtiva;
    public static PropriedadeCamera propCameraGlobal = new PropriedadeCamera();
    public static bool Grafico2D;    

    public static void addObject(GameObject go)
    {
        
        if (listaObjetos == null)
            listaObjetos = new List<GameObject>();

        if (go != null && !listaObjetos.Contains(go))
            listaObjetos.Add(go);
    }

    // se não passar o objt q é pra deletar, remove o último adicionado se a lista n estiver vazia
    public static void removeObject(GameObject go)
    {
        print("eita");
        if (go != null && listaObjetos.Contains(go))
            listaObjetos.Remove(go);
        else if (go == null && listaObjetos.Count > 0) listaObjetos.RemoveAt(-1);
    }

    public static bool containsByName(string goName)
    {
        bool result = false;

        if (goName != "")
        {
            GameObject go = GameObject.Find(goName);
            result = listaObjetos.Contains(go);
        }

        return result;            
    }

    public static string[] GetPeca(string slotName)
    {
        string[] nomeObj = new string[3];

        if (slotName.Contains("CameraSlot"))
            nomeObj[0] = "CameraP";
        else if (slotName.Contains("ObjGraficoSlot"))
            nomeObj[0] = "ObjetoGraficoP";
        else if (slotName.Contains("FormasSlot"))
            nomeObj[0] = "Cubo";
        else if (slotName.Contains("TransformacoesSlot"))
        {
            nomeObj[0] = "Transladar";
            nomeObj[1] = "Rotacionar";
            nomeObj[2] = "Escalar";
        }
        else if (slotName.Contains("IluminacaoSlot"))
            nomeObj[0] = "Iluminacao";
        
        return nomeObj;
    }

    public static string GetSlot(string pecaName)
    {
        string nomeObj = "";

        if (pecaName.Contains("Camera"))
            nomeObj = "CameraSlot";
        else if (pecaName.Contains("ObjetoGraficoP"))
            nomeObj = "ObjGraficoSlot";
        else if (pecaName.Contains("Cubo"))
            nomeObj = "FormasSlot";
        else if (pecaName.Contains("Transladar") || pecaName.Contains("Rotacionar") || pecaName.Contains("Escalar"))
            nomeObj = "TransformacoesSlot";
        else if (pecaName.Contains("Iluminacao"))
            nomeObj = "IluminacaoSlot"; 

        return nomeObj;
    }

    public static void atualizaListaSlot()
    {
        listaPosicaoSlot.Clear();

        GameObject render = GameObject.Find("Render");

        for(int i = 0; i < render.transform.childCount; i++)
        {
            if (render.transform.GetChild(i).name.Contains("Slot"))
            {
                listaPosicaoSlot.Add(render.transform.GetChild(i).name, render.transform.GetChild(i).position.y);

                if (render.transform.GetChild(i).name.Contains("ObjGraficoSlot"))
                {
                    GameObject objGrafico = GameObject.Find(render.transform.GetChild(i).name);

                    for (int j = 0; j < objGrafico.transform.childCount; j++)
                    {
                        if (objGrafico.transform.GetChild(j).name.Contains("Slot"))
                        {
                            listaPosicaoSlot.Add(objGrafico.transform.GetChild(j).name, objGrafico.transform.GetChild(j).position.y);
                        }
                    }
                }
            }
        }
    }

    public static void iniciaListaSequenciaSlots(int numObjeto)
    {
        string numObjStr = Convert.ToString(numObjeto);

        if (numObjeto == 0)
        {
            listaSequenciaSlots.Add("CameraSlot");
            numObjStr = "";
        }      

        listaSequenciaSlots.Add("ObjGraficoSlot" + numObjStr);
        listaSequenciaSlots.Add("FormasSlot" + numObjStr);
        listaSequenciaSlots.Add("TransformacoesSlot" + numObjStr);
        listaSequenciaSlots.Add("IluminacaoSlot" + numObjStr);        
    }

    public static string Tsadas(string tranf)
    {
        return "";
    }

}
