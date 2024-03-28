using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThemeChange : MonoBehaviour
{

    private bool control = true;

    public TMP_Text textoModo;

    public TMP_Text[] textosTMP;
    public Text[] textos;
    public GameObject[] objetos;

    public Material claro;
    public Material escuro;

    public void themeChanging()
    {
        if (!control)
        {
            textoModo.text = "Claro";
            changingText();
            changingBackground();
            control = true;
        }
        else
        {
            textoModo.text = "Noturno";
            //textosTMP[0].text = "TESTE";
            //textosTMP[0].color = Color.red;
            changingText();
            changingBackground();
            control = false;
        }
    }

    private void changingBackground()
    {
        if (control)
        {
            for (int i = 0; i < objetos.Length; i++)
            {
                objetos[i].GetComponent<MeshRenderer>().material = escuro;
            }
        }
        else
        {
            for (int i = 0; i < objetos.Length; i++)
            {
                objetos[i].GetComponent<MeshRenderer>().material = claro;
            }
        }
    }

   

    private void changingText()
    {
        if (control)
        {
            for (int i = 0; i < textos.Length; i++)
            {
                textos[i].color = Color.white;
            }
            
            for (int i = 0; i < textosTMP.Length; i++)
            {
                print(textosTMP[i].color);
                textosTMP[i].color = Color.white;
                //textosTMP[i].UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            }
            
        }
        else
        {
            for (int i = 0; i < textos.Length; i++)
            {
                textos[i].color = Color.HSVToRGB(32, 32, 32);
            }
            
            for (int i = 0; i < textosTMP.Length; i++)
            {
                print(textosTMP[i].color);
                textosTMP[i].color = Color.HSVToRGB(32,32,32);
                //textosTMP[i].UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            }
            
        }
    }
    
}
