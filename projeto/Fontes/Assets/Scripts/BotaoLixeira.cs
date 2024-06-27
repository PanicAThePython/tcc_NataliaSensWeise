using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotaoLixeira : MonoBehaviour
{

	public PropCameraPadrao teste;

	public void OnMouseDown()
	{
		var inputSelected = teste.GetInputSelected(gameObject.name);
		Debug.Log(inputSelected);
	}
}
