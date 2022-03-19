using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class MapEditor
{
    GameObject[,] _map;
    GameObject parent;

    GameObject MapPrefab;
    public MapEditor(List<MyPoint> MapQueue) {
        _map = new GameObject[22,22];
        Create_Map( MapQueue);
    }

    private GameObject generate_platform(int i, int j)
    {
        var parent = GameObject.Find("All_Parent").transform;

        var obj = GameObject.Instantiate<GameObject>(MapPrefab, parent);
        obj.name = "_Platform";
        obj.GetComponent<_platform>().row = i;
        obj.GetComponent<_platform>().col = j;
        return obj;
    }



    public void Create_Map(List<MyPoint> MapQueue)
    {
        parent=_create_all_parent();
        MapPrefab = Resources.Load<GameObject>("Platforms/" + "_Platform");
        if (MapPrefab == null) { Debug.Log("MapPrefab==null");return; }
        for (int i = 0; i < MapQueue.Count; i++)
        {
            int nr = MapQueue[i].row;
            int nc = MapQueue[i].col;
            _map[nr,nc] = generate_platform(nr, nc);
             ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _map[nr,nc].transform.position = new Vector3((float)(1.5f * nc - 3.75f + 0.75 * System.Math.Abs(4 - nr)), 0, 1.3f * (nr - 1));
        }
    }
    private GameObject _create_all_parent()
    {
        GameObject _all_parent=new GameObject();
        //----------------------------------------------
        _all_parent.transform.position = new Vector3(0, 0, 0);
        _all_parent.name = "All_Parent";
        //----------------------------------------------
        return _all_parent;
    }
    public void ChangeMapStatus(int row, int col,int status) {
        _map[row,col].GetComponent<_platform>().status = status;
    }
    public GameObject GetMap(int row,int col) { 
        return _map[row,col];
    }
    public void DestroyMap() {
        GameObject.Destroy(parent);
        for (int i = 0; i < 22; i++) {
            for (int j = 0; j < 22; j++) {
                GameObject.Destroy(_map[i, j]);
            }
        }
    }
}
