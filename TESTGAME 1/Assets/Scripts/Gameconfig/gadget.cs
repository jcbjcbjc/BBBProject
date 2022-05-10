using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class gadget
{
    Dictionary<int,int> _panelDict = new Dictionary<int, int>();
    public static float CalculateDistance(Transform startPoint, Transform endPoint)
    {
        float x = System.Math.Abs(startPoint.position.x - endPoint.position.x);
        float y = System.Math.Abs(startPoint.position.z - endPoint.position.z);

        return Mathf.Sqrt(x * x + y * y);
    }
    public static Vector3 SwitchToPosition(int row,int col)
    {
        float nr = row;
        float nc = col;
        return new Vector3(row, col,0);
    }
    public static MyPoint SwitchToCoord(Transform transform)
    {
        int nr = 1;
        int nc = 1;
        return new MyPoint(nr,nc);
    }
}
