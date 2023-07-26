using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Globalization;
using UnityEngine.UI;

public class PropCuboPadrao : MonoBehaviour
{
    protected enum typeTransf { Pos, Tam};

    protected TMP_InputField TamX;
    protected TMP_InputField TamY;
    protected TMP_InputField TamZ;

    protected TMP_InputField PosX;
    protected TMP_InputField PosY;
    protected TMP_InputField PosZ;

    protected Color Cor;

    protected float x, y, z;
    protected bool ativo = true;
    protected PropriedadePeca prPeca = new PropriedadePeca();
    protected string nomePeca;
    private bool dadosIniciais = false;

    protected void mainMethod()
    {
        if (prPeca.PodeAtualizar)
        {
            preencheCampos();
            prPeca.PodeAtualizar = false;
        }
    }

    protected bool inicializou()
    {
        bool inicializou = false;

        if (!Global.propriedadePecas.ContainsKey(Global.gameObjectName))
            inicializou = true;
        else
        {
            prPeca = Global.propriedadePecas[Global.gameObjectName];

            if (prPeca.JaIniciouValores)
            {
                dadosIniciais = false;
                return true;
            }
            else
            {
                prPeca.Ativo = true;
                prPeca.PodeAtualizar = true;
                prPeca.JaIniciouValores = true;
                nomePeca = prPeca.Nome;

                inicializou = false;
                dadosIniciais = true;

                atualizaListaProp();
            }
        }        

        return inicializou;
    }

    protected void preencheCampos()
    {
        // Pode ser ToggleCubo, por isso a validação.
        if (Equals(gameObject.name, "PropCubo"))
        {
            gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = prPeca.Nome;

            if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
            {
                TamX = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
                TamY = gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
                TamZ = gameObject.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();

                PosX = gameObject.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
                PosY = gameObject.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
                PosZ = gameObject.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();

                gameObject.transform.GetChild(6).GetComponent<Toggle>().isOn = prPeca.Ativo;

                instanciaTransformacao();

                TamX.text = prPeca.Tam.X.ToString();
                TamY.text = prPeca.Tam.Y.ToString();
                TamZ.text = prPeca.Tam.Z.ToString();

                PosX.text = prPeca.Pos.X.ToString();
                PosY.text = prPeca.Pos.Y.ToString();
                PosZ.text = prPeca.Pos.Z.ToString();

                GameObject.Find("CorSelecionada").GetComponent<MeshRenderer>().materials[0].color = prPeca.Cor;
                GameObject.Find("TexturaSelecionada").GetComponent<MeshRenderer>().materials[0].mainTexture = prPeca.Textura;
            }
        }        
    }

    protected void updatePosition()
    {        
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            TamX = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
            TamY = gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
            TamZ = gameObject.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();

            PosX = gameObject.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
            PosY = gameObject.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
            PosZ = gameObject.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();

            prPeca.Tam.X = float.Parse(validaVazio(TamX.text, typeTransf.Tam), CultureInfo.InvariantCulture.NumberFormat);
            prPeca.Tam.Y = float.Parse(validaVazio(TamY.text, typeTransf.Tam), CultureInfo.InvariantCulture.NumberFormat);
            prPeca.Tam.Z = float.Parse(validaVazio(TamZ.text, typeTransf.Tam), CultureInfo.InvariantCulture.NumberFormat);

            prPeca.Pos.X = float.Parse(validaVazio(PosX.text, typeTransf.Pos), CultureInfo.InvariantCulture.NumberFormat);
            prPeca.Pos.Y = float.Parse(validaVazio(PosY.text, typeTransf.Pos), CultureInfo.InvariantCulture.NumberFormat);
            prPeca.Pos.Z = float.Parse(validaVazio(PosZ.text, typeTransf.Pos), CultureInfo.InvariantCulture.NumberFormat);

            GameObject goTransformacaoAmb = GameObject.Find(prPeca.NomeCuboAmbiente);
            GameObject goTransformacaoVis = GameObject.Find(prPeca.NomeCuboVis);

            if (goTransformacaoAmb != null && goTransformacaoVis != null)
            {       
                float x = prPeca.Pos.X * -1;

                goTransformacaoAmb.transform.localPosition = new Vector3(x, prPeca.Pos.Y, prPeca.Pos.Z);
                goTransformacaoVis.transform.localPosition = new Vector3(x, prPeca.Pos.Y, prPeca.Pos.Z);
                goTransformacaoAmb.transform.localScale = new Vector3(prPeca.Tam.X, prPeca.Tam.Y, prPeca.Tam.Z);
                goTransformacaoVis.transform.localScale = new Vector3(prPeca.Tam.X, prPeca.Tam.Y, prPeca.Tam.Z);
            }
        }

        //atualizaListaProp();

    }

    private void atualizaListaProp()
    {
        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            Global.propriedadePecas.Remove(prPeca.Nome);
            Global.propriedadePecas.Add(prPeca.Nome, prPeca);
        }
    }

    protected void toggleChanged()
    {
        if (gameObject.name == "ToggleCubo")
        {
            prPeca.Ativo = gameObject.GetComponent<Toggle>().isOn;
            //updatePosition();

            string goObjGraficoSlotPai = GameObject.Find(GameObject.Find(Global.listaEncaixes[Global.gameObjectName]).transform.parent.name).name;

            string pecaObjGrafico = string.Empty;

            // Pega o nome da peça conectada ao "goObjGraficoSlotPai"
            foreach (KeyValuePair<string, string> peca in Global.listaEncaixes)
            {
                if (Equals(peca.Value, goObjGraficoSlotPai))
                {
                    pecaObjGrafico = peca.Key;
                    break;
                }                    
            }

            bool existeProp = false;

            foreach (KeyValuePair<string, PropriedadePeca> pec in Global.propriedadePecas)
            {
                if (pec.Key == pecaObjGrafico)
                    existeProp = true;
            }
            
            if (existeProp)
            {
                if (Global.propriedadePecas[pecaObjGrafico].Ativo)
                    GameObject.Find(prPeca.NomeCuboAmbiente).GetComponent<MeshRenderer>().enabled = prPeca.Ativo;
                else
                    GameObject.Find(prPeca.NomeCuboAmbiente).GetComponent<MeshRenderer>().enabled = false;
            }
            else
                GameObject.Find(prPeca.NomeCuboAmbiente).GetComponent<MeshRenderer>().enabled = prPeca.Ativo;
        }        
    }

    protected bool pieceChanged()
    {
        bool result = nomePeca != prPeca.Nome;
        nomePeca = prPeca.Nome;
        prPeca.PodeAtualizar = result;
        return result;
    }

    protected bool jaClicouEmAlgumObjeto()
    {
        return Global.gameObjectName != null;
    }

    private void instanciaTransformacao()
    {
        if (!prPeca.JaInstanciou)
        {
            if (prPeca.Pos == null)
                prPeca.Pos = new Posicao();

            prPeca.Pos.X = 0;
            prPeca.Pos.Y = 0;
            prPeca.Pos.Z = 0;

            if (prPeca.Tam == null)
                prPeca.Tam = new Tamanho();

            prPeca.Tam.X = 1;
            prPeca.Tam.Y = 1;
            prPeca.Tam.Z = 1;

            prPeca.Cor = Color.white;

            prPeca.Ativo = true;

            prPeca.JaInstanciou = true;
        }
    }

    private string validaVazio(string valor, typeTransf tipo)
    {
        if (Equals(valor, System.String.Empty))
        {
            if (tipo == typeTransf.Tam)
                return "1";
            return "0";
        }
        return valor;
    }
}
