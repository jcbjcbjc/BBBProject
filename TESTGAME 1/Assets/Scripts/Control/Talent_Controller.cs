using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class Talent_Controller :BaseControl
{
    

    public int[] RandomPool;

    protected override void Update()
    {
        base.Update();
    }
    protected override void execute()
    {

    }
    public override bool Navigation_Obstacle(GameObject _go)
    {
        SetStatusBrick(_go);
        for (int i = 0; i < MaxCount; i++) {
            if (!_navigation.IsNavigation(gadget.SwitchToCoord(_Players[i].transform),_PlayerData[i].EndPoint))
            {
                DeleteStatus(_go);
                return false;
            }
        }
        DeleteStatus(_go);
        return true;
    }
    protected override void InitGame()
    {
        RandomPool = new int[20];
        for (int i = 1; i <= 4; i++)
        {
            RandomPool[i] = 1;
        }
        for (int i = 5; i <= 8; i++)
        {
            RandomPool[i] = 2;
        }
        for (int i = 9; i <= 12; i++)
        {
            RandomPool[i] = 3;
        }
        for (int i = 13; i <= 16; i++)
        {
            RandomPool[i] = 4;
        }
        for (int i = 17; i <= 18; i++)
        {
            RandomPool[i] = 5;
        }
        RandomPool[19] = 6;
    }
    protected override void Camera_Change()
    {

    }
    protected override void Onquest(int GaCode, string data)
    {

    }
    protected override void SendSelectOrderMessenge()
    {

    }
    protected override void Convert()
    {

    }
    // Start is called before the first frame update
    //---------------------------

}
