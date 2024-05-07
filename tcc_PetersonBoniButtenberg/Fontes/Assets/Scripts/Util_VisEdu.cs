using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util_VisEdu : MonoBehaviour {

    private GameObject cuboVisObjectMain;
    private GameObject posicaoAmb;

    public GameObject CuboVisObjectMain { get; set; }
    public GameObject PosicaoAmb { get; set; }

    public void criaFormasVazias()
    { 
        CuboVisObjectMain = GameObject.Find("CuboVisObjectMain");
        PosicaoAmb = GameObject.Find("PosicaoAmb");

        CuboVisObjectMain = Instantiate(CuboVisObjectMain, CuboVisObjectMain.transform.position, CuboVisObjectMain.transform.rotation, CuboVisObjectMain.transform.parent);
        PosicaoAmb = Instantiate(PosicaoAmb, PosicaoAmb.transform.position, PosicaoAmb.transform.rotation, PosicaoAmb.transform.parent);

        Global.cuboVis = CuboVisObjectMain;
        Global.posAmb = PosicaoAmb;
    }

    public static string getCuboByNomePeca(string nome)
    {
        string cubo = "";
        Controller c = new Controller();

        if (nome != string.Empty && Global.listaEncaixes.ContainsKey(nome))
        {
            for (int i = 0; i < GameObject.Find(Global.listaEncaixes[nome]).transform.parent.childCount; i++)
            {
                if (GameObject.Find(Global.listaEncaixes[nome]).transform.parent.GetChild(i).name.Contains("FormasSlot"))
                {
                    foreach (KeyValuePair<string, string> pair in Global.listaEncaixes)
                    {
                        if (pair.Value == GameObject.Find(Global.listaEncaixes[nome]).transform.parent.GetChild(i).name)
                            return pair.Key;
                    }
                }
            }            
        }           

        return cubo;        
    }

    public void enableColliderFabricaPecas(bool enable, bool enableChildren, bool render = false)
    {
        GameObject enabledObject = GameObject.Find("FabricaPecas");

        if (enableChildren)
        {
            for (int i = 0; i < enabledObject.transform.childCount; i++)
            {
                if (!enabledObject.transform.GetChild(i).name.Contains("Spline") && !enabledObject.transform.GetChild(i).name.Contains("Poligono") &&
                    !enabledObject.transform.GetChild(i).name.Contains("GO_Pecas") && !enabledObject.transform.GetChild(i).name.Contains("GO_ObjetosGraficos"))
                {
                    // Se render for false é porque é pra alterar os colliders da fabrica de peças.
                    if (!render)
                    {
                        if (!Global.listaEncaixes.ContainsKey(enabledObject.transform.GetChild(i).name))
                            enabledObject.transform.GetChild(i).GetComponent<Collider>().enabled = enable;
                    }
                    else
                    {
                        // Senão dever alterar os colliders do render.
                        if (Global.listaEncaixes.ContainsKey(enabledObject.transform.GetChild(i).name))
                            enabledObject.transform.GetChild(i).GetComponent<Collider>().enabled = enable;
                    }                    
                }                
            }                
        }

        enabledObject.GetComponent<Collider>().enabled = enable;
    }    

}
