using MTUnity.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Praise : MonoBehaviour {

    // Use this for initialization

    const float TIME = 1f;


    private void OnEnable()
    {
        var image = GetComponent<Image>();
        var newColor = image.color;
        newColor.a = 1;
        image.color = newColor;
        transform.localPosition = Vector3.zero;
        RunAction();
    }
    public void RunAction()
    {
        StartCoroutine(Hide());
        this.RunActions(new MTSpawn(new MTSequence(new MTScaleTo(TIME / 2, 4f), new MTScaleTo(TIME / 2, 3f)), new MTMoveBy(TIME, Vector3.up * 250),
                   new MTImageFadeTo(TIME,0f)));

    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(TIME + 0.1f);
        gameObject.SetActive(false);
    }
}
