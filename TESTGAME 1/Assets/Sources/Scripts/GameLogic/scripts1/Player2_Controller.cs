using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2_Controller : MonoBehaviour
{


    float _jump_time = 0.2f;
    IEnumerator _playermove;


    void Update()
    {
        if (_stage_control.Instance._is_game_pre)
        {
            Set_Player2();
            Ban_Player2();
        }

        if (_stage_control.Instance._is_game && _stage_control.Instance.isPlayer2)
        {
            Move_Player2();
            Brick_Player2();
        }
        if (_stage_control.Instance._is_game)
        {
            Condition_Jugde();
        }
        if (_stage_control.Instance.isBreak2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out RaycastHit _hit))
                {
                    Break_Player_to2(_hit.collider.gameObject);
                }
                //更新行动点
            }
        }
    }
    public void Set_Player2()
    {
        if (_stage_control.Instance.isplayer2_set)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out RaycastHit _hit))
                {
                    Set_Player_to2(_hit.collider.gameObject);
                }
            }

        }
    }
    public void Ban_Player2()
    {
        if (_stage_control.Instance.isplayer2_ban)
        {
            if (_stage_control.Instance.Ban_Point2 > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(_ray, out RaycastHit _hit))
                    {
                        Ban_Player_to2(_hit.collider.gameObject);
                    }
                }
            }
            else
            {
                _stage_control.Instance.isplayer2_ban = false;

                if (_stage_control.Instance.order == 2)
                {
                    _stage_control.Instance.isplayer1_ban = true;
                }
                else
                {
                    _stage_control.Instance._is_game_pre = false;

                    _stage_control.Instance._is_game = true;

                    _stage_control.Instance.isPlayer1 = true;

                    _stage_control.Instance.Start_1();
                }
            }
        }
    }
    public void Move_Player2()
    {
        if (_stage_control.Instance.isMove2)
        {
            if (_stage_control.Instance.MovementPoint2 > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(_ray, out RaycastHit _hit))
                    {
                        Move_Player_to2(_hit.collider.gameObject);
                    }
                }
            }
            else
            {
                _stage_control.Instance.ConvertTo1();
                return;
            }
        }
    }
    public void Brick_Player2()
    {
        if (_stage_control.Instance.isBrick2)
        {
            if (_stage_control.Instance.MovementPoint2 > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(_ray, out RaycastHit _hit))
                    {
                        Brick_Player_to2(_hit.collider.gameObject);
                    }
                    //更新行动点
                }
            }
            else
            {
                _stage_control.Instance.ConvertTo1();
                return;
            }
            //停顿一段时间
            //
            ////////////
        }

    }

    public void Set_Player_to2(GameObject _go)
    {
        if (_go.GetComponent<_platform>()._sprt.color != Color.green)
        {
            if (_go.transform.position != _stage_control.Instance.BeginPoint1.transform.position && _go.transform.position != _stage_control.Instance.BeginPoint2.transform.position)
            {
                _go.GetComponent<_platform>()._sprt.color = Color.green;
                _go.GetComponent<_platform>()._text[1].gameObject.SetActive(true);

                _go.GetComponent<_platform>()._text[1].text = "1";

                _stage_control.retry_queue.Enqueue(_go);

                _stage_control.Instance.SetPoint2 = _go;

                _stage_control.Instance.isplayer2_set = false;

                if (_stage_control.Instance.order == 2)
                {
                    _stage_control.Instance.isplayer1_set = true;
                }
                else
                {
                    _stage_control.Instance.isplayer1_ban = true;
                }
            }

        }
    }
    public void Ban_Player_to2(GameObject _go)
    {
        if (!NavigationClassic.Navigation_Player(_go))
        {
            return;
        }
        if (_go.GetComponent<_platform>()._sprt.color != Color.green && _go.GetComponent<_platform>()._sprt.color != Color.red)
        {
            if (_go.transform.position != _stage_control.Instance.BeginPoint1.transform.position && _go.transform.position != _stage_control.Instance.BeginPoint2.transform.position)
            {

                _go.GetComponent<_platform>()._sprt.color = Color.red;

                _stage_control.retry_queue.Enqueue(_go);

                NavigationClassic.SetStatus(_go);

                _stage_control.Instance.Ban_Point2--;
            }
        }
    }
    public void Move_Player_to2(GameObject _go)
    {
        if (gadget.CalculateDistance(_stage_control.Instance._Player2.transform, _go.transform) > 0.1f && gadget.CalculateDistance(_stage_control.Instance._Player2.transform, _go.transform) < 1.7f)
        {
            if (_go.GetComponent<_platform>()._sprt.color != Color.red && _go.GetComponent<_platform>()._sprt.color != Color.yellow)
            {
                _playermove = _jump(_go.transform.position);
                StartCoroutine(_playermove);
                _stage_control.Instance.MovementPoint2--;

                _stage_control.retry_queue.Enqueue(_go);

                if (_stage_control.Instance._music)
                {
                    _stage_control.Instance._game_configuration._sounds[2].Play();
                }

                GameObject _t = Instantiate(_stage_control.Instance._good_particles);
                _t.GetComponent<ParticleSystem>().startColor = _go.GetComponent<_platform>()._sprt.color;
                _t.transform.position = _go.transform.position;
            }
        }
    }
    public void Brick_Player_to2(GameObject _go)
    {
        if (_go.GetComponent<_platform>()._sprt.color == Color.red || _go.GetComponent<_platform>()._sprt.color == Color.yellow || _go.GetComponent<_platform>()._sprt.color == Color.green)
        {
            return;
        }
        if (_go.transform.position == _stage_control.Instance.BeginPoint1.transform.position || _go.transform.position == _stage_control.Instance.BeginPoint2.transform.position || _go.transform.position == _stage_control.Instance._Player1.transform.position || _go.transform.position == _stage_control.Instance._Player2.transform.position)
        {
            return;
        }
        if (!NavigationClassic.Navigation_Player(_go))
        {
            return;
        }
        _go.GetComponent<_platform>()._sprt.color = Color.yellow;

        _stage_control.retry_queue.Enqueue(_go);

        NavigationClassic.SetStatus(_go);

        _stage_control.Instance.MovementPoint2--;
    }
    public void Break_Player_to2(GameObject _go)
    {
        if (_go.GetComponent<_platform>()._sprt.color == Color.yellow)
        {
            if (_stage_control.Instance._music)
            {
                _stage_control.Instance._sounds[0].Play();
            }
            _go.GetComponent<_platform>()._sprt.color = _stage_control.Instance._game_configuration._color_list[0];
            _stage_control.Instance.MovementPoint2 -= 3;

            NavigationClassic.DeleteStatus(_go);

            _stage_control.Instance.isBreak2 = false;

            if (_stage_control.Instance.MovementPoint2 <= 0)
            {
                _stage_control.Instance.ConvertTo1();
                return;
            }
        }
    }

    public void Condition_Jugde()
    {
        if (_stage_control.Instance._Player2.transform.position == _stage_control.Instance.SetPoint1.transform.position)
        {
            _stage_control.Instance.SetPoint1.GetComponent<_platform>()._text[1].gameObject.SetActive(false);
            _stage_control.Instance.is_Condition_2 = true;
        }
        if (_stage_control.Instance.is_Condition_2 && _stage_control.Instance._Player2.transform.position == _stage_control.Instance.EndPoint2.transform.position)
        {
            _stage_control.Instance._gameover(2);








        }
    }

    IEnumerator _jump(Vector3 _end)
    {
        //----------------------------------------------

        //----------------------------------------------
        Vector3 _start = _stage_control.Instance._Player2.transform.position;
        //----------------------------------------------
        float elapsedTime = 0;
        //----------------------------------------------
        while (elapsedTime < _jump_time)
        {
            //----------------------------------------------
            _stage_control.Instance._Player2.transform.position = Vector3.Lerp(_start, _end, (elapsedTime / _jump_time));
            elapsedTime += Time.deltaTime;
            //----------------------------------------------
            yield return new WaitForEndOfFrame();
        }
        _stage_control.Instance._Player2.transform.position = _end;

    }
};