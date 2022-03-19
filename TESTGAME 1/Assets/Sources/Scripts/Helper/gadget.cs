using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class gadget
{

    public static int[] RandomPool = new int[20] { 0,1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 6 };


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
    public static int GenerateRamdomNumber() {
        int RandomPoint = Random.Range(1, 20);
        return RandomPool[RandomPoint];
    }
    //public static List<MyPoint> CreateMultipleMap() {
    //    List < MyPoint > map = new List<MyPoint>();
    //    for (int i = 1; i <= 7; i++)
    //    {
    //        for (int j = 1; j <= 12 - System.Math.Abs(i - 4); j++)
    //        {
    //            MyPoint pt = new MyPoint(i,j);
    //            map.Add(pt);
    //        }
    //    }
    //    return map;
    //}
    //public static List<PlayerData> CreateMultiplePlayer()
    //{
    //    List<PlayerData> Players = new List<PlayerData>();
        
    //    return Players;
    //}
}
