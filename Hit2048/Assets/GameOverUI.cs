using MTUnity.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    public GameObject GameOverGameObject;
    public GameObject Form;

    public Text CurrentRoundText;
    public Text RecordRoundText;

    public void SetRound(string current, string Record)
    {
        CurrentRoundText.text = current;
        RecordRoundText.text = Record;
    }

    public Text CurrentScoreText;
    public Text RecordScoreText;

    public void SetScore(string current, string record)
    {
        CurrentCoinText.text = current;
        RecordCoinText.text = record;
    }

    public Text CurrentCoinText;
    public Text RecordCoinText;
    public void SetCoin(string current, string record)
    {
        CurrentBomb.text = current;
        RecordBomb.text = record;
    }

    public Text CurrentBomb;
    public Text RecordBomb;

    public GameObject ResetButton;


    private void OnEnable()
    {
        GameOverGameObject.transform.localScale = Vector3.one * 0.5f;
        ResetButton.transform.localScale = Vector3.one * 0.2f;
        Form.transform.localScale = Vector3.one * 0.5f;

        GameOverGameObject.RunActions(GetScale(2));
        ResetButton.RunActions(GetScale(2));
        Form.RunActions(new MTScaleTo(0.3f, 1f));


    }

    public MTFiniteTimeAction GetScale(float targetScale)
    {
        return new MTSequence(new MTScaleTo(0.15f, 1.2f * targetScale), new MTScaleTo(0.15f, 0.8f * targetScale), new MTScaleTo(0.15f, 1.1f * targetScale),new MTScaleTo(0.15f,1 * targetScale));
    }


	// Update is called once per frame
	void Update () {
		
	}
}
