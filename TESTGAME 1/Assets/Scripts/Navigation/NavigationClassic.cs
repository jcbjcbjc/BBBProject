using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationClassic : MonoBehaviour
{
    private static NavigationClassic _instance;
    public static NavigationClassic Instance { get { if (!_instance) { _instance = GameObject.FindObjectOfType(typeof(NavigationClassic)) as NavigationClassic; } return _instance; } }
    private MyPoint CurrentPoint;
    public bool isFind = false;
    //构建小地图
    public MyPoint[,] map;
    //构建探索数组
    private Queue<MyPoint> ExploreList;
    public void Awake()
    {
        map = new MyPoint[7, 12];
        for (int i = 0; i < 7; i++)
            for (int j = 0; j < 12; j++)
            {
                map[i, j] = new MyPoint
                {
                    row = i + 1,
                    col = j + 1,
                    status = 0
                };
                if (map[i, j].col > 12 - System.Math.Abs(map[i, j].row - 4))
                {
                    map[i, j].status = 1;
                }
            }
        ExploreList = new Queue<MyPoint>();
    }
    public bool isNavigation(MyPoint BeginPoint, MyPoint EndPoint)
    {
        map[EndPoint.row - 1, EndPoint.col - 1].status = 4;
        CurrentPoint = (MyPoint)BeginPoint;
        while (true)
        {
            if (4 - CurrentPoint.row > 0)
            {
                for (int i = CurrentPoint.row - 1; i <= System.Math.Min(CurrentPoint.row + 1, 7); i++)
                {
                    if (i <= 0) { continue; }
                    for (int j = CurrentPoint.col - 1; j <= System.Math.Min(CurrentPoint.col + 1, 12 - System.Math.Abs(i - 4)); j++)
                    {
                        if (isFind)
                        {
                            Clearmap();
                            return true;
                        }
                        if (j <= 0) { continue; }
                        if (i == CurrentPoint.row && j == CurrentPoint.col) { continue; }
                        if (i == CurrentPoint.row - 1 && j == CurrentPoint.col + 1) { continue; }
                        if (i == CurrentPoint.row + 1 && j == CurrentPoint.col - 1) { continue; }
                        switch (map[i - 1, j - 1].status)
                        {
                            //如果没探索过则初始化这个节点的信息
                            case 0:
                                map[i - 1, j - 1].status = 2;
                                ExploreList.Enqueue(map[i - 1, j - 1]);
                                break;
                            //探索过则更新这个节点信息
                            case 1:
                                break;
                            case 2:
                                break;
                            case 4:
                                //找到终点
                                isFind = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else if (4 - CurrentPoint.row < 0)
            {
                for (int i = CurrentPoint.row - 1; i <= System.Math.Min(CurrentPoint.row + 1, 7); i++)
                {
                    if (i <= 0) { continue; }
                    for (int j = CurrentPoint.col - 1; j <= System.Math.Min(CurrentPoint.col + 1, 12 - System.Math.Abs(i - 4)); j++)
                    {
                        if (isFind)
                        {
                            Clearmap();
                            return true;
                        }
                        if (j <= 0) { continue; }

                        if (i == CurrentPoint.row && j == CurrentPoint.col) { continue; }
                        if (i == CurrentPoint.row - 1 && j == CurrentPoint.col - 1) { continue; }
                        if (i == CurrentPoint.row + 1 && j == CurrentPoint.col + 1) { continue; }
                        switch (map[i - 1, j - 1].status)
                        {
                            //如果没探索过则初始化这个节点的信息
                            case 0:
                                map[i - 1, j - 1].status = 2;
                                ExploreList.Enqueue(map[i - 1, j - 1]);
                                break;
                            //探索过则更新这个节点信息
                            case 1:
                                break;
                            case 2:
                                break;
                            case 4:
                                //找到终点
                                isFind = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else
            {
                for (int i = CurrentPoint.row - 1; i <= System.Math.Min(CurrentPoint.row + 1, 7); i++)
                {
                    if (i <= 0) { continue; }
                    for (int j = CurrentPoint.col - 1; j <= System.Math.Min(CurrentPoint.col + 1, 12 - System.Math.Abs(i - 4)); j++)
                    {
                        if (isFind)
                        {
                            Clearmap();
                            return true;
                        }
                        if (j <= 0) { continue; }
                        if (j == CurrentPoint.col + 1 && i != CurrentPoint.row) { continue; }
                        switch (map[i - 1, j - 1].status)
                        {
                            //如果没探索过则初始化这个节点的信息
                            case 0:
                                map[i - 1, j - 1].status = 2;
                                ExploreList.Enqueue(map[i - 1, j - 1]);
                                break;
                            //探索过则更新这个节点信息
                            case 1:
                                break;
                            case 2:
                                break;
                            case 4:
                                //找到终点
                                isFind = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            map[CurrentPoint.row - 1, CurrentPoint.col - 1].status = 2;
            if (isFind == true)
            {
                Clearmap();
                return true;
            }
            //选出下一个成为最短路径的节点
            if (ExploreList.Count == 0)
            {
                Clearmap();
                return false;
            }
            else
            {
                CurrentPoint = ExploreList.Dequeue();
            }
        }
    }
    public void Clearmap()
    {
        for (int i = 1; i <= 7; i++)
        {
            for (int j = 1; j <= 12; j++)
            {
                if (map[i - 1, j - 1].status == 2 || map[i - 1, j - 1].status == 4)
                {
                    map[i - 1, j - 1].status = 0;
                }
            }
        }
        isFind = false;
        ExploreList.Clear();
    }
    public static bool Navigation_Player(GameObject _go)
    {
        SetStatus(_go);

        if (_stage_control.Instance.is_Condition_1)
        {
            if (!NavigationClassic.Instance.isNavigation(Switch(_stage_control.Instance._Player1.transform), Switch(_stage_control.Instance.EndPoint1.transform)))
            {
               DeleteStatus(_go);
                return false;
            }
        }
        else
        {
            if (!NavigationClassic.Instance.isNavigation(Switch(_stage_control.Instance._Player1.transform), Switch(_stage_control.Instance.SetPoint2.transform)) || !NavigationClassic.Instance.isNavigation(Switch(_stage_control.Instance.SetPoint2.transform), Switch(_stage_control.Instance.EndPoint1.transform)))
            {
                DeleteStatus(_go);
                return false;
            }
        }
        if (_stage_control.Instance.is_Condition_2)
        {
            if (!NavigationClassic.Instance.isNavigation(Switch(_stage_control.Instance._Player2.transform), Switch(_stage_control.Instance.EndPoint2.transform)))
            {
                DeleteStatus(_go);
                return false;
            }
        }
        else
        {
            if (!NavigationClassic.Instance.isNavigation(Switch(_stage_control.Instance._Player2.transform),Switch(_stage_control.Instance.SetPoint1.transform)) || !NavigationClassic.Instance.isNavigation(Switch(_stage_control.Instance.SetPoint1.transform), Switch(_stage_control.Instance.EndPoint2.transform)))
            {
                DeleteStatus(_go);
                return false;
            }
        }
        return true;
    }
    public static MyPoint Switch(Transform Point)
    {
        MyPoint vector2 = new();
        float row = Point.position.z / 1.3f + 1;
        float col = (Point.position.x - 0.75f * System.Math.Abs(4 - row) + 3.75f) / 1.5f;
        vector2.row = Mathf.RoundToInt(row);
        vector2.col = Mathf.RoundToInt(col);
        vector2.status = 0;
        return vector2;
    }
    public static void SetStatus(GameObject _go)
    {
        MyPoint myPoint = Switch(_go.transform);

        NavigationClassic.Instance.map[myPoint.row - 1, myPoint.col - 1].status = 1;
    }
    public static void DeleteStatus(GameObject _go)
    {
        MyPoint myPoint = Switch(_go.transform);
        NavigationClassic.Instance.map[myPoint.row - 1, myPoint.col - 1].status = 0;
    }
    public void ClearGrid()
    {
        for (int i = 1; i <= 7; i++)
        {
            for (int j = 1; j <= 12 - System.Math.Abs(i - 4); j++)
            {
                map[i - 1, j - 1].status = 0;
            }
        }
    }
}
