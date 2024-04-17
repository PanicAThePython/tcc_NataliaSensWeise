using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

public class Arquivo : MonoBehaviour
{
    public static JSONObject cena = new JSONObject();
    public MinhaCamera teste;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Exportar()
    {
        teste.addPropsCamera();
        cena.Add("Camera", teste.props);
        string path = Application.persistentDataPath + "/teste.json";
        File.WriteAllText(path, cena.ToString());
    }
}
