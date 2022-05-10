using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenu : BasePanel
{
    public GameObject[] _UI_HowToPlaylittle;
    public GameObject[] _UI_HowToPlay;

    public int order = 0;

    public bool isHowToPlay=false;
    // Start is called before the first frame update
    public void Button1() {
        if (order < 3)
        {
            _UI_HowToPlaylittle[order].SetActive(false);
            _UI_HowToPlaylittle[++order].SetActive(true);
        }
        else {

            _UI_HowToPlaylittle[3].SetActive(false);

            _UI_HowToPlay[0].SetActive(false);

            _UI_HowToPlay[1].SetActive(true);

            order++;

        }
    }
    public void Button2() {
        if (order < 9)
        {
            _UI_HowToPlaylittle[order].SetActive(false);

            _UI_HowToPlaylittle[++order].SetActive(true);
        }
        else {
            close();
        }
    }
    public void close (){

        Game.Instance.ShowPanel("StartPanel");
        Game.Instance.HidePanel("TutorPanel");

        _UI_HowToPlaylittle[0].SetActive(true);

        _UI_HowToPlaylittle[4].SetActive(true);

        _UI_HowToPlay[1].SetActive(false);

        _UI_HowToPlay[0].SetActive(true);

        isHowToPlay = false;

        order = 0;
    }
}
