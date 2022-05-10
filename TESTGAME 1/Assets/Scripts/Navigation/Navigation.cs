using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class Navigation
{
    private MyPoint CurrentPoint;
    private bool isFind = false;
    //构建小地图
    private MyPoint[,] map;
    //构建探索数组
    private Queue<MyPoint> ExploreList;

    public Navigation(List<MyPoint> MapQueue) {
        Init(MapQueue);
    }
    void Init(List<MyPoint> MapQueue)
    {
        map = new MyPoint[22, 22];
        for (int i = 1; i <= 21; i++)
            for (int j = 1; j <= 21; j++)
            {
                map[i, j] = new MyPoint
                {
                    row = i,
                    col = j,
                    status = 1
                };
            }
        for (int i = 0; i < MapQueue.Count; i++) {
            map[MapQueue[i].row, MapQueue[i].col].status = 0;
        }
        ExploreList = new Queue<MyPoint>();
    }
    public bool IsNavigation(MyPoint BeginPoint, MyPoint EndPoint)
    {
        map[EndPoint.row , EndPoint.col ].status = 4;
        CurrentPoint = (MyPoint)BeginPoint;
        while (true)
        {
            if (11 - CurrentPoint.row > 0)
            {
                for (int i = CurrentPoint.row - 1; i <= System.Math.Min(CurrentPoint.row + 1, 21); i++)
                {
                    if (i <= 0) { continue; }
                    for (int j = CurrentPoint.col - 1; j <= System.Math.Min(CurrentPoint.col + 1, 21 - System.Math.Abs(i - 11)); j++)
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
                        switch (map[i, j ].status)
                        {
                            //如果没探索过则初始化这个节点的信息
                            case 0:
                                map[i , j ].status = 2;
                                ExploreList.Enqueue(map[i , j ]);
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
                for (int i = CurrentPoint.row - 1; i <= System.Math.Min(CurrentPoint.row + 1, 21); i++)
                {
                    if (i <= 0) { continue; }
                    for (int j = CurrentPoint.col - 1; j <= System.Math.Min(CurrentPoint.col + 1, 21 - System.Math.Abs(i - 11)); j++)
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
                        switch (map[i , j ].status)
                        {
                            //如果没探索过则初始化这个节点的信息
                            case 0:
                                map[i , j ].status = 2;
                                ExploreList.Enqueue(map[i , j ]);
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
                for (int i = CurrentPoint.row - 1; i <= System.Math.Min(CurrentPoint.row + 1, 21); i++)
                {
                    if (i <= 0) { continue; }
                    for (int j = CurrentPoint.col - 1; j <= System.Math.Min(CurrentPoint.col + 1, 21 - System.Math.Abs(i - 11)); j++)
                    {
                        if (isFind)
                        {
                            Clearmap();
                            return true;
                        }
                        if (j <= 0) { continue; }
                        if (j == CurrentPoint.col + 1 && i != CurrentPoint.row) { continue; }
                        switch (map[i , j ].status)
                        {
                            //如果没探索过则初始化这个节点的信息
                            case 0:
                                map[i , j ].status = 2;
                                ExploreList.Enqueue(map[i , j ]);
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
            map[CurrentPoint.row , CurrentPoint.col ].status = 2;
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
        for (int i = 1; i <= 21; i++)
        {
            for (int j = 1; j <= 21; j++)
            {
                if (map[i , j ].status == 2 || map[i , j ].status == 4)
                {
                    map[i , j ].status = 0;
                }
            }
        }
        isFind = false;
        ExploreList.Clear();
    }
    public void ChangeMapStatus(int row,int col, int status) {
        map[row , col ].status = status;
    }
}
