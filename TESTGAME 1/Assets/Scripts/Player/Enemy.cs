using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Enemy : MonoBehaviour
{
    float _jump_time = 0.2f;
    IEnumerator _playermove;
    public void Set_Player_to(GameObject _go)
    {
        _go.GetComponent<_platform>()._text[1].gameObject.SetActive(true);
        ///////////////////////////////////////////////////////////////////////////////
        _go.GetComponent<_platform>()._text[1].text = "E";
        //////////////////////////////////////////////////////////////////////////
        _go.GetComponent<_platform>()._sprt.color = Color.green;

        _go.GetComponent<_platform>().status = MapCode.Set;


        //////////////////////////////////////////////////////////////////////
        Multiple_Controller.Instance.SetPoint[1] = _go;
        //////////////////////////////////////////////////////////////////////
    }
    public void Ban_Player_to(GameObject _go)
    {
        _go.GetComponent<_platform>()._sprt.color = Color.red;

        Multiple_Controller.Instance.Navigation.SetStatusBan(_go);
    }
    public void Move_Player_to(GameObject _go)
    {
        _playermove = _jump(_go.transform.position);
        StartCoroutine(_playermove);

        GameObject _t = Instantiate(Multiple_Controller.Instance._good_particles);
        _t.GetComponent<ParticleSystem>().startColor = _go.GetComponent<_platform>()._sprt.color;
        _t.transform.position = _go.transform.position;
        
    }
    public void Brick_Player_to(GameObject _go)
    {
        Multiple_Controller.Instance.Navigation.SetStatusBrick(_go);

        _go.GetComponent<_platform>()._sprt.color = Color.yellow;
    }
    public void Break_Player_to(GameObject _go)
    {
        _go.GetComponent<_platform>()._sprt.color = Multiple_Controller.Instance._game_configuration._color_list[0];
        Multiple_Controller.Instance.Navigation.DeleteStatus(_go);

    }
    IEnumerator _jump(Vector3 _end)
    {
        //----------------------------------------------

        //----------------------------------------------
        Vector3 _start = Multiple_Controller.Instance._Player[0].transform.position;
        //----------------------------------------------
        float elapsedTime = 0;
        //----------------------------------------------
        while (elapsedTime < _jump_time)
        {
            //----------------------------------------------
            Multiple_Controller.Instance._Player[0].transform.position = Vector3.Lerp(_start, _end, (elapsedTime / _jump_time));
            elapsedTime += Time.deltaTime;
            //----------------------------------------------
            yield return new WaitForEndOfFrame();
        }
        Multiple_Controller.Instance._Player[0].transform.position = _end;
    }
}
