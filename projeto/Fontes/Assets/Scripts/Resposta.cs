using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resposta : MonoBehaviour
{
    private static string[] respostaOrdem;
    private static Dictionary<string, PropriedadePeca> respostaProps;
    private static PropriedadeCamera[] camProp = new PropriedadeCamera[4];

    public static void setRespostaOrdem(string[] r)
    {
        respostaOrdem = r;
    }

    public static string[] getRespostaOrdem()
    {
        return respostaOrdem;
    }
    public static void setRespostaProps(Dictionary<string, PropriedadePeca> r)
    {
        respostaProps = r;
    }

    public static Dictionary<string, PropriedadePeca> getRespostaProps()
    {
        return respostaProps;
    }

    public static void setCamProps(PropriedadeCamera r, int index)
    {
        camProp[index] = r;
    }

    public static PropriedadeCamera getCamProps(int index)
    {
        return camProp[index];
    }
}
