using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class BasePlayer : MonoBehaviour
{
    float _jump_time = 0.2f;
    IEnumerator _playermove;

    Transform _transform;

    public virtual void Move_Player_to(GameObject _go)
    {
       

    }
    public virtual void Brick_Player_to(GameObject _go)
    {
        
    }
    public virtual void Break_Player_to(GameObject _go)
    {
        

    }
    IEnumerator _jump(Vector3 _end)
    {
        //----------------------------------------------

        //----------------------------------------------
        Vector3 _start = _transform.position;
        //----------------------------------------------
        float elapsedTime = 0;
        //----------------------------------------------
        while (elapsedTime < _jump_time)
        {
            //----------------------------------------------
            _transform.position = Vector3.Lerp(_start, _end, (elapsedTime / _jump_time));
            elapsedTime += Time.deltaTime;
            //----------------------------------------------
            yield return new WaitForEndOfFrame();
        }
        _transform.position = _end;
    }
}
