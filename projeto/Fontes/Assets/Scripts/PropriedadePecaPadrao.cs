using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Globalization;
using UnityEngine.UI;
using System;

public abstract class PropriedadePecaPadrao : MonoBehaviour {

    public enum typeTransformacao { Transladar, Rotacionar, Escalar};

    public TMP_InputField PosX, TamX;
    public TMP_InputField PosY, TamY;
    public TMP_InputField PosZ, TamZ;
    protected float x, y, z, x1, y1, z1;
    protected bool ativo = true;
    public PropriedadePeca prPeca = new PropriedadePeca();
    public typeTransformacao tipoTransformacao;
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
                prPeca.Ativo = gameObject.transform.GetChild(3).GetComponent<Toggle>().isOn;
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
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = prPeca.Nome;

        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {     
            gameObject.transform.GetChild(3).GetComponent<Toggle>().isOn = prPeca.Ativo;

            if (!Arquivo.importando)
            {
                instanciaTransformacao();

                if (tipoTransformacao == typeTransformacao.Escalar)
                {
                    TamX = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
                    TamY = gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
                    TamZ = gameObject.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();

                    TamX.text = prPeca.Tam.X.ToString();
                    TamY.text = prPeca.Tam.Y.ToString();
                    TamZ.text = prPeca.Tam.Z.ToString();
                }
                else
                {
                    PosX = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
                    PosY = gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
                    PosZ = gameObject.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();

                    PosX.text = prPeca.Pos.X.ToString();
                    PosY.text = prPeca.Pos.Y.ToString();
                    PosZ.text = prPeca.Pos.Z.ToString();
                }
            }

            if (Arquivo.importando) updatePosition();
        }        
    }

    /*

    public void testandoRotacionar(string x1, string y1, string z1)
    {
        prPeca.Pos.X = float.Parse(validaVazio(x1), CultureInfo.InvariantCulture.NumberFormat);
        prPeca.Pos.Y = float.Parse(validaVazio(y1), CultureInfo.InvariantCulture.NumberFormat);
        prPeca.Pos.Z = float.Parse(validaVazio(z1), CultureInfo.InvariantCulture.NumberFormat);

        if (prPeca.Ativo)
        {
            x = prPeca.Pos.X;
            y = prPeca.Pos.Y;
            z = prPeca.Pos.Z;
        }
    }*/
    protected void updatePosition()
    {
        //qnd clica em apagar, o valor some da visualização, mas continua na peça
        //dá pra por qqr valor q o cubo vai responder, mas n terá retorno visual

        if (Global.propriedadePecas.ContainsKey(prPeca.Nome))
        {
            if (tipoTransformacao == typeTransformacao.Escalar)
            {
                TamX = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
                TamY = gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
                TamZ = gameObject.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();

                if (prPeca.Tam == null) prPeca.Tam = new Tamanho();

                prPeca.Tam.X = float.Parse(validaVazio(TamX.text), CultureInfo.InvariantCulture.NumberFormat);
                prPeca.Tam.Y = float.Parse(validaVazio(TamY.text), CultureInfo.InvariantCulture.NumberFormat);
                prPeca.Tam.Z = float.Parse(validaVazio(TamZ.text), CultureInfo.InvariantCulture.NumberFormat);

                x = y = z = 1;

                if (prPeca.Ativo)
                {
                    x = prPeca.Tam.X;
                    y = prPeca.Tam.Y;
                    z = prPeca.Tam.Z;
                }
                //se o slot existir e for slot filho...
                /*
                if (Global.propriedadePecas.ContainsKey(proxSlot) && (number % 2 != 0))
                {
                    PropriedadePeca pp = Global.propriedadePecas[proxSlot];

                    if (pp.Tam == null) pp.Tam = new Tamanho();

                    pp.Tam.X = float.Parse(validaVazio(TamX.text), CultureInfo.InvariantCulture.NumberFormat);
                    pp.Tam.Y = float.Parse(validaVazio(TamY.text), CultureInfo.InvariantCulture.NumberFormat);
                    pp.Tam.Z = float.Parse(validaVazio(TamZ.text), CultureInfo.InvariantCulture.NumberFormat);

                    if (pp.Ativo && prPeca.Ativo)
                    {
                        x1 = pp.Tam.X;
                        y1 = pp.Tam.Y;
                        z1 = pp.Tam.Z;
                    }
                }
                */
            }
            else
            {
                PosX = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
                PosY = gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
                PosZ = gameObject.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_InputField>();

                //fica dando ruim aqui sem motivo, pq existe validação!!!!!!!
                if (prPeca.Pos == null) prPeca.Pos = new Posicao();
               
                prPeca.Pos.X = float.Parse(validaVazio(PosX.text), CultureInfo.InvariantCulture.NumberFormat);
                prPeca.Pos.Y = float.Parse(validaVazio(PosY.text), CultureInfo.InvariantCulture.NumberFormat);
                prPeca.Pos.Z = float.Parse(validaVazio(PosZ.text), CultureInfo.InvariantCulture.NumberFormat);
                
                /*
                if (tipoTransformacao == typeTransformacao.Rotacionar)
                {
                    labelX.text = prPeca.Tam.X.ToString();
                    labelY.text = prPeca.Tam.Y.ToString();
                    labelZ.text = prPeca.Tam.Z.ToString();
                }
                 */  

                x = y = z = 0;

                if (prPeca.Ativo)
                {
                    x = prPeca.Pos.X;
                    y = prPeca.Pos.Y;
                    z = prPeca.Pos.Z;
                }
                //se o slot existir e for slot filho...
                /*
                if (Global.propriedadePecas.ContainsKey(proxSlot) && (number % 2 != 0))
                {
                    PropriedadePeca pp = Global.propriedadePecas[proxSlot];

                    if (pp.Pos == null) pp.Pos = new Posicao();

                    pp.Pos.X = float.Parse(validaVazio(PosX.text), CultureInfo.InvariantCulture.NumberFormat);
                    pp.Pos.Y = float.Parse(validaVazio(PosY.text), CultureInfo.InvariantCulture.NumberFormat);
                    pp.Pos.Z = float.Parse(validaVazio(PosZ.text), CultureInfo.InvariantCulture.NumberFormat);

                    if (pp.Ativo && prPeca.Ativo)
                    {
                        x1 = pp.Pos.X;
                        y1 = pp.Pos.Y;
                        z1 = pp.Pos.Z;
                    }
                }
                */
            }              

            GameObject goTransformacaoAmb = GameObject.Find(prPeca.Nome + "Amb");
            GameObject goTransformacaoVis = GameObject.Find(prPeca.Nome + "Vis");

            //tentando pegar a forma amb e vis do filho
            /*
            GameObject goTransformacaoAmb1 = null;
            GameObject goTransformacaoVis1 = null;

            if (GameObject.Find(proxSlot) != null && (number % 2 != 0))
            {
                foreach(KeyValuePair<string, string> encaixe in Global.listaEncaixes)
                {
                    if (Equals(encaixe.Value, proxSlot))
                    {
                        
                        print(encaixe.Value);
                        print(proxSlot);
                        print(encaixe.Key);
                        
                        
                        goTransformacaoAmb1 = GameObject.Find(encaixe.Key + "Amb");
                        goTransformacaoVis1 = GameObject.Find(encaixe.Key + "Vis");
                        
                        Controller control = GameObject.Find(encaixe.Key).GetComponent<Controller>();

                        //n rolou, deu um erro infinito
                        //o erro cria infinitos TransladarAmb, ent oq eu tenho q fazer é criar um rotacionar no filho (exemplo)
                        control.addGameObjectTree("GameObjectAmb" + control.getNumeroSlotObjetoGrafico(), "Amb", "CuboAmbiente" + control.getNumeroSlotObjetoGrafico());
                        control.addGameObjectTree("CuboVisObject" + control.getNumeroSlotObjetoGrafico(), "Vis", "CuboVis" + control.getNumeroSlotObjetoGrafico());
                        
                        break;
                    }
                }
                
            }
            */
            if (goTransformacaoAmb != null /*&& goTransformacaoVis != null*/)
            {
                if (tipoTransformacao == typeTransformacao.Transladar)
                {
                    goTransformacaoAmb.transform.localPosition = new Vector3(x * -1, y, z);
                    goTransformacaoVis.transform.localPosition = new Vector3(x, y, z);

                    //pra refletir no filho
                    if (prPeca != null)
                    {
                        string slot = Global.listaEncaixes[prPeca.Nome];
                        string num = slot[slot.Length - 1].ToString();
                        string espaco = slot[slot.Length - 2].ToString();
                        if (espaco == "_") num = slot[slot.Length - 3].ToString();
                        if (num == "t") num = "0";
                        int number = int.Parse(num) + 1;

                        GameObject otherCubeAmb = GameObject.Find("CuboAmbiente" + number);
                        GameObject otherCubeVis = GameObject.Find("CuboVis" + number);

                        if(otherCubeAmb != null)
                        {
                            otherCubeAmb.transform.localPosition = new Vector3(x * -1, y, z);
                            otherCubeVis.transform.localPosition = new Vector3(x, y, z);
                        }
                    }
                }
                else if (tipoTransformacao == typeTransformacao.Rotacionar)
                {
                    goTransformacaoAmb.transform.localRotation = Quaternion.Euler(x, y*-1, z*-1);
                    goTransformacaoVis.transform.localRotation = Quaternion.Euler(x, y, z);

                    //pra refletir no filho
                    if (prPeca != null)
                    {
                        string slot = Global.listaEncaixes[prPeca.Nome];
                        string num = slot[slot.Length - 1].ToString();
                        string espaco = slot[slot.Length - 2].ToString();
                        if (espaco == "_") num = slot[slot.Length - 3].ToString();
                        if (num == "t") num = "0";
                        int number = int.Parse(num) + 1;

                        GameObject otherCubeAmb = GameObject.Find("CuboAmbiente" + number);
                        GameObject otherCubeVis = GameObject.Find("CuboVis" + number);

                        if (otherCubeAmb != null)
                        {
                            otherCubeAmb.transform.localRotation = Quaternion.Euler(x, y * -1, z * -1);
                            otherCubeVis.transform.localRotation = Quaternion.Euler(x, y, z);
                        }
                    }
                }
                else
                {
                    goTransformacaoAmb.transform.localScale = new Vector3(x, y, z);
                    goTransformacaoVis.transform.localScale = new Vector3(x, y, z);

                    //pra refletir no filho
                    if (prPeca != null)
                    {
                        string slot = Global.listaEncaixes[prPeca.Nome];
                        string num = slot[slot.Length - 1].ToString();
                        string espaco = slot[slot.Length - 2].ToString();
                        if (espaco == "_") num = slot[slot.Length - 3].ToString();
                        if (num == "t") num = "0";
                        int number = int.Parse(num) + 1;

                        GameObject otherCubeAmb = GameObject.Find("CuboAmbiente" + number);
                        GameObject otherCubeVis = GameObject.Find("CuboVis" + number);

                        if (otherCubeAmb != null)
                        {
                            otherCubeAmb.transform.localScale = new Vector3(x, y, z);
                            otherCubeVis.transform.localScale = new Vector3(x, y, z);
                        }
                        
                    }
                }
            }
            /*
            //refletir no filho
            if (goTransformacaoAmb1 != null) 
            {
                if (tipoTransformacao == typeTransformacao.Transladar)
                {
                    goTransformacaoAmb1.transform.localPosition = new Vector3(x1 * -1, y1, z1);
                    goTransformacaoVis1.transform.localPosition = new Vector3(x1, y1, z1);
                }
                else if (tipoTransformacao == typeTransformacao.Rotacionar)
                {
                    goTransformacaoAmb1.transform.localRotation = Quaternion.Euler(x1, y1 * -1, z1 * -1);
                    goTransformacaoVis1.transform.localRotation = Quaternion.Euler(x1, y1, z1);
                }
                else
                {
                    goTransformacaoAmb1.transform.localScale = new Vector3(x1, y1, z1);
                    goTransformacaoVis1.transform.localScale = new Vector3(x1, y1, z1);
                }
            }
            */
            string forma = Util_VisEdu.getCuboByNomePeca(Global.gameObjectName);

            if (forma != System.String.Empty && Global.propriedadePecas.ContainsKey(forma)) //Se forma for vazio significa que não existe uma forma ainda.
            {
                PropriedadePeca prPecaCubo = Global.propriedadePecas[forma];
                goTransformacaoAmb = GameObject.Find(prPecaCubo.NomeCuboAmbiente);
                goTransformacaoVis = GameObject.Find(prPecaCubo.NomeCuboVis);

                if (goTransformacaoAmb != null && goTransformacaoVis != null)
                {
                    goTransformacaoAmb.transform.localPosition = new Vector3(prPecaCubo.Pos.X * -1, prPecaCubo.Pos.Y, prPecaCubo.Pos.Z);
                    goTransformacaoVis.transform.localPosition = new Vector3(prPecaCubo.Pos.X * -1, prPecaCubo.Pos.Y, prPecaCubo.Pos.Z);
                    goTransformacaoAmb.transform.localScale = new Vector3(prPecaCubo.Tam.X, prPecaCubo.Tam.Y, prPecaCubo.Tam.Z);
                    goTransformacaoVis.transform.localScale = new Vector3(prPecaCubo.Tam.X, prPecaCubo.Tam.Y, prPecaCubo.Tam.Z);
                }
            }   
        }

        atualizaListaProp();
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
        prPeca.Ativo = gameObject.transform.GetChild(3).GetComponent<Toggle>().isOn;
        updatePosition();
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

            prPeca.JaInstanciou = true;            
        }        
    }

    private string validaVazio(string valor)
    {
        if (Equals(valor, String.Empty))
        {
            if (tipoTransformacao == typeTransformacao.Escalar)
                return "1";
            return "0";
        }
        return valor;
    }

}
