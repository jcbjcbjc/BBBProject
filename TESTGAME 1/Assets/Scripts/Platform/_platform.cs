using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class _platform : MonoBehaviour {

    public int row;
    public int col;
    public int status;

    private void Awake()
    {
        row = 0;
        col = 0;
        status = MapCode.Default;
    }


    public TextMesh[] _text;

    public int _int_time = 0;
    public SpriteRenderer _sprt;
    
    IEnumerator _fade_in(Color _nc)
    {
        //----------------------------------------------
        Color _base = Color.white;
        float ElapsedTime = 0.0f;
        float TotalTime = 0.5f;
        //----------------------------------------------
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            _sprt.color = Color.Lerp(_base, _nc, (ElapsedTime / TotalTime));
            yield return null;
        }
        //----------------------------------------------
    }
	
	IEnumerator _fade_out(Color _nc)
    {
        //----------------------------------------------
        Color _base = _sprt.color;
        float ElapsedTime = 0.0f;
        float TotalTime = 0.5f;
        //----------------------------------------------
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            _sprt.color = Color.Lerp(_base, _nc, (ElapsedTime / TotalTime));
            yield return null;
        }
        //----------------------------------------------
    }
    //----------------------------------------------
};
