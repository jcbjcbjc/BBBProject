using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Player_Control : MonoBehaviour
{
    float _jump_time = 0.2f;
    IEnumerator _playermove;

    public void Update()
    {
        if (Multiple_Controller.Instance._is_game_pre)
        {
            Set_Player();
            Ban_Player();
        }
        if (Multiple_Controller.Instance._is_game && Multiple_Controller.Instance.isPlayer)
        {
            Move_Player();
            Brick_Player();
        }
        if (Multiple_Controller.Instance._is_game)
        {
            Condition_Jugde();
        }
        if (Multiple_Controller.Instance.isBreak)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out RaycastHit _hit))
                {
                    Break_Player_to(_hit.collider.gameObject);
                }
                //更新行动点
            }
        }
    }
    public void Set_Player()
    {
        if (Multiple_Controller.Instance.isplayer_set)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out RaycastHit _hit))
                {
                    Set_Player_to(_hit.collider.gameObject);
                }
            }
        }
    }
    public void Ban_Player()
    {
        if (Multiple_Controller.Instance.isplayer_ban)
        {
            if (Multiple_Controller.Instance.Ban_Point > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(_ray, out RaycastHit _hit))
                    {
                        Ban_Player_to(_hit.collider.gameObject);
                    }
                }
            }
            else
            {
                Multiple_Controller.Instance.isplayer_ban = false;

                if (Multiple_Controller.Instance.order == Multiple_Controller.Instance.ID)
                {

                    Multiple_Controller.Instance.SendGameMessege(GameCode.ToBan,"");
                    
                }
                else
                {
                    Multiple_Controller.Instance.SendGameMessege(GameCode.EndPre, "");

                    Multiple_Controller.Instance._is_game_pre = false;

                    Multiple_Controller.Instance._is_game = true;
                }
            }
        }
    }
    public void Move_Player()
    {
        if (Multiple_Controller.Instance.isMove)
        {
            if (Multiple_Controller.Instance.MovementPoint > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(_ray, out RaycastHit _hit))
                    {
                        Move_Player_to(_hit.collider.gameObject);
                    }
                }
            }
            else
            {
                Multiple_Controller.Instance.Convert();
                return;
            }
        }
    }
    public void Brick_Player()
    {
        if (Multiple_Controller.Instance.isBrick)
        {
            if (Multiple_Controller.Instance.MovementPoint > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(_ray, out RaycastHit _hit))
                    {
                        Brick_Player_to(_hit.collider.gameObject);
                    }
                }
            }
            else
            {
                Multiple_Controller.Instance.Convert();
                return;
            }
        }
    }


    public void Set_Player_to(GameObject _go)
    {
        if (_go.GetComponent<_platform>().status != MapCode.Default) {
            return;
        }

        Multiple_Controller.Instance.SendGameMessege(GameCode.Set, _go.GetComponent<_platform>().row.ToString() + ";" + _go.GetComponent<_platform>().col.ToString());

        _go.GetComponent<_platform>()._text[1].gameObject.SetActive(true);
        _go.GetComponent<_platform>()._text[1].text = "M";

        _go.GetComponent<_platform>()._sprt.color = Color.green;

        
        _go.GetComponent<_platform>().status = MapCode.Set;
        
        
        Multiple_Controller.Instance.SetPoint[0] = _go;
        
        Multiple_Controller.Instance.isplayer_set = false;

        if (Multiple_Controller.Instance.order == Multiple_Controller.Instance.ID)
        {
            Multiple_Controller.Instance.SendGameMessege(GameCode.ToSet, "");
        }
        else
        {
            Multiple_Controller.Instance.SendGameMessege(GameCode.ToBan, "");
            
        }
    }
    public void Ban_Player_to(GameObject _go)
    {
        if (!Multiple_Controller.Instance.Navigation.Navigation_Player(_go))
        {
            return;
        }
        if (_go.GetComponent<_platform>().status != MapCode.Default) {
            return;
        }
        Multiple_Controller.Instance.SendGameMessege(GameCode.Ban, _go.GetComponent<_platform>().row.ToString() + ";" + _go.GetComponent<_platform>().col.ToString());

        _go.GetComponent<_platform>()._sprt.color = Color.red;

        Multiple_Controller.Instance.Navigation.SetStatusBan(_go);

        Multiple_Controller.Instance.Ban_Point--;
    }
    public void Move_Player_to(GameObject _go)
    {
        if (gadget.CalculateDistance(Multiple_Controller.Instance._Player[0].transform, _go.transform) > 0.1f && gadget.CalculateDistance(Multiple_Controller.Instance._Player[0].transform, _go.transform) < 1.7f)
        {
            if (_go.GetComponent<_platform>().status == MapCode.Brick || _go.GetComponent<_platform>().status == MapCode.Ban) {
                return;
            }

            Multiple_Controller.Instance.SendGameMessege(GameCode.Move, _go.GetComponent<_platform>().row.ToString() + ";" + _go.GetComponent<_platform>().col.ToString());

            _playermove = _jump(_go.transform.position);
            StartCoroutine(_playermove);

            Multiple_Controller.Instance.MovementPoint--;
            Multiple_Controller.Instance. SendGameMessege(GameCode.ConveyPoint, Multiple_Controller.Instance.MovementPoint.ToString() + ";" + "1");
            if (Multiple_Controller.Instance._music)
            {
                //Multiple_Controller.Instance._sounds[2].Play();
            }

            GameObject _t = Instantiate(Multiple_Controller.Instance._good_particles);
            _t.GetComponent<ParticleSystem>().startColor = _go.GetComponent<_platform>()._sprt.color;
            _t.transform.position = _go.transform.position;
        }
    }
    public void Brick_Player_to(GameObject _go)
    {
        if (_go.GetComponent<_platform>().status != MapCode.Default) { return; }
       
        if ( _go.transform.position == Multiple_Controller.Instance._Player[0].transform.position || _go.transform.position == Multiple_Controller.Instance._Player[1].transform.position)
        {
            return;
        }
        if (!Multiple_Controller.Instance.Navigation.Navigation_Player(_go))
        {
            return;
        }

        Multiple_Controller.Instance.SendGameMessege(GameCode.Brick, _go.GetComponent<_platform>().row.ToString() + ";" + _go.GetComponent<_platform>().col.ToString());

        Multiple_Controller.Instance.Navigation.SetStatusBrick(_go);

        Multiple_Controller.retry_queue.Enqueue(_go);

        _go.GetComponent<_platform>()._sprt.color = Color.yellow;

        Multiple_Controller.Instance.MovementPoint--;
        Multiple_Controller.Instance.SendGameMessege(GameCode.ConveyPoint, Multiple_Controller.Instance.MovementPoint.ToString() + ";" + "1");
    }
    public void Break_Player_to(GameObject _go)
    {
        if (_go.GetComponent<_platform>().status == MapCode.Brick)
        {
            if (Multiple_Controller.Instance._music)
            {
                //_g.Instance._sounds[0].Play();
            }

            Multiple_Controller.Instance.SendGameMessege(GameCode.Break, _go.GetComponent<_platform>().row.ToString() + ";" + _go.GetComponent<_platform>().col.ToString());

            _go.GetComponent<_platform>()._sprt.color = Multiple_Controller.Instance._game_configuration._color_list[0];
            Multiple_Controller.Instance.MovementPoint -= 3;
            Multiple_Controller.Instance.SendGameMessege(GameCode.ConveyPoint, Multiple_Controller.Instance.MovementPoint.ToString() + ";" + "1");
            Multiple_Controller.Instance.Navigation.DeleteStatus(_go);

            Multiple_Controller.Instance.isBreak= false;

            if (Multiple_Controller.Instance.MovementPoint<= 0)
            {
                Multiple_Controller.Instance.Convert();
                return;
            }
        }
    }

    public void Condition_Jugde()
    {
        if (Multiple_Controller.Instance._Player[0].transform.position == Multiple_Controller.Instance.SetPoint[1].transform.position)
        {
            Multiple_Controller.Instance.SendGameMessege(GameCode.ConditionSuccess, "");

            Multiple_Controller.Instance.SetPoint[1].GetComponent<_platform>()._text[1].gameObject.SetActive(false);

            Multiple_Controller.Instance.is_Condition[0] = true;
        }
        if (Multiple_Controller.Instance.is_Condition[0] && Multiple_Controller.Instance._Player[0].transform.position == Multiple_Controller.Instance.EndPoint[0].position)
        {
            Multiple_Controller.Instance.SendGameMessege(GameCode.Success, "");

            Multiple_Controller.Instance.SendRoomMessege(ActionCode.StopGame, "");

            Multiple_Controller.Instance.Gameover(Game.Instance.User.Name);
        }
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
