using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resposta : MonoBehaviour
{
    private static string[] respostaOrdem;
    private static Dictionary<string, PropriedadePeca> respostaProps;
    private static PropriedadeCamera[] camProp = new PropriedadeCamera[4];

    //eu vou ter q ter, pra cada exerc, uma lista com todos os objetos q a cena deve ter
    //em cada objt, vou ter q checar se a peça está com as propriedades corretas
    //se a ordem estiver errada ou alguma prop n estiver de acordo, errou; senão, acertou

    //mas fazer novas listas de objetos pode ser complicado... seria mlr se eu tivesse uma lista com os nomes dos objetos
    //e dps uma lista com as props por escrito

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
