using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotaoTutorial : MonoBehaviour
{

    public Button botao;
    public TutorialNovo tn;

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
            new Tutorial().PulouTutorial();
            Tutorial.passoTutorial = Tutorial.Passo.PulouTutorial;
            tn.abrirTutorial();
        //Tutorial.AnswerMsg = 1;

        Tutorial.MessageBoxVisEdu(gameObject.transform.parent.name, false);
    }
}
