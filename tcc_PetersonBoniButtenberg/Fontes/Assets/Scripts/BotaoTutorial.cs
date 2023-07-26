using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotaoTutorial : MonoBehaviour {

    public Button botao;

    public void Start()
    {
        if (botao != null)
            botao.onClick.AddListener(delegate { SetAnswerMsg(); });
    }

    private void SetAnswerMsg()
    {
        if (botao.name.Contains("Pular"))
        {
            new Tutorial().PulouTutorial();
            Tutorial.passoTutorial = Tutorial.Passo.PulouTutorial;
        }            
        else
            Tutorial.AnswerMsg = 1;

        Tutorial.MessageBoxVisEdu(gameObject.transform.parent.name, false);
    }
}
