using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tamanho
{
    private float x, y, z;

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public class Posicao
{
    private float x, y, z;

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public class ValorIluminacao
{
    private float x, y, z;

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public class PropriedadePeca {

    private string nome;
    private bool podeAtualizar;
    private bool jaIniciouValores;
    private Tamanho tam;
    private Posicao pos;
    private bool ativo;
    private string nomeCuboAmbiente;
    private string nomeCuboVis;
    private PropriedadePeca forma;
    private bool jaInstanciou;
    private Color cor = Color.white;
    private Texture textura;   

    //Iluminação
    private int tipoLuz;
    private float intensidade;
    private ValorIluminacao valorIluminacao;
    private float distancia;
    private float angulo;
    private float expoente;
    private int ultimoIndexLuz;

    public string Nome { get; set; }
    public bool PodeAtualizar { get; set; }
    public bool JaIniciouValores { get; set; }
    public Tamanho Tam { get; set; }
    public Posicao Pos { get; set; }
    public bool Ativo { get; set; }
    public string NomeCuboAmbiente { get; set; }
    public string NomeCuboVis { get; set; }
    public bool JaInstanciou { get; set; }
    public Color Cor { get; set; }
    public Texture Textura { get; set; }    

    //Iluminação
    public int TipoLuz { get; set; }
    public float Intensidade { get; set; }
    public ValorIluminacao ValorIluminacao { get; set; }
    public float Distancia { get; set; }
    public float Angulo { get; set; }
    public float Expoente { get; set; }
    public int UltimoIndexLuz { get; set; }

    public PropriedadePeca()
    {
        tam = new Tamanho();
        pos = new Posicao();
    }
}

public class PropriedadeCamera
{
    private string nome;
    private float posX, posY, posZ;
    private float lookAtX, lookAtY, lookAtZ;
    private float near;
    private float far;
    private Vector2 fov;
    private bool cameraAtiva;
    private bool jaIniciouValores;
    private bool existeCamera;
    private PropriedadeCameraInicial propInicial;

    public float Nome { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
    public float LookAtX { get; set; }
    public float LookAtY { get; set; }
    public float LookAtZ { get; set; }
    public float Near { get; set; }
    public float Far { get; set; }
    public Vector2 FOV { get; set; }
    public bool CameraAtiva { get; set; }
    public bool JaIniciouValores { get; set; }
    public bool ExisteCamera { get; set; }
    public PropriedadeCameraInicial PropInicial { get; set; }

    public PropriedadeCamera()
    {
        
    }
}

public class PropriedadeCameraInicial
{
    private float posX, posY, posZ;
    private float lookAtX, lookAtY, lookAtZ;
    private float near;
    private float far;
    private Vector2 fov;

    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
    public float LookAtX { get; set; }
    public float LookAtY { get; set; }
    public float LookAtZ { get; set; }
    public float Near { get; set; }
    public float Far { get; set; }
    public Vector2 FOV { get; set; }

    public PropriedadeCameraInicial()
    {

    }
}

public class PropriedadeIluminacao
{

}


