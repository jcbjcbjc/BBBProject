/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： 资源加载管理器      
 *    Description: 
 *           功能： 本功能是在Unity的Resources类的基础之上，增加了“缓存”的处理。
 *                  本脚本适用于
 *    Date: 2022
 *    Version: 0.1版本
 *    Modify Recoder: 
 *    
 *   
 */
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


public class ResourcesMgr : MonoBehaviour
{
    /* 字段 */
    private static ResourcesMgr _Instance;              //本脚本私有单例实例
    private Hashtable ht = null;                        //容器键值对集合


    /// <summary>
    /// 得到实例(单例)
    /// </summary>
    /// <returns></returns>
    public static ResourcesMgr GetInstance()
    {
        if (_Instance == null)
        {
            _Instance = new GameObject("_ResourceMgr").AddComponent<ResourcesMgr>();
        }
        return _Instance;
    }

    void Awake()
    {
        ht = new Hashtable();
    }

    /// <summary>
    /// 调用资源（带对象缓冲技术）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="isCatch"></param>
    /// <returns></returns>
    public T LoadResource<T>(string path, bool isCatch) where T : UnityEngine.Object
    {
        if (ht.Contains(path))
        {
            return ht[path] as T;
        }

        T TResource = Resources.Load<T>(path);
        if (TResource == null)
        {
            Debug.LogError(GetType() + "/GetInstance()/TResource 提取的资源找不到，请检查。 path=" + path);
        }
        else if (isCatch)
        {
            ht.Add(path, TResource);
        }

        return TResource;
    }

    /// <summary>
    /// 调用资源（带对象缓冲技术）
    /// </summary>
    /// <param name="path"></param>
    /// <param name="isCatch"></param>
    /// <returns></returns>
    public GameObject LoadAsset(string path, bool isCatch)
    {
        GameObject goObj = LoadResource<GameObject>(path, isCatch);
        GameObject goObjClone = GameObject.Instantiate<GameObject>(goObj);
        if (goObjClone == null)
        {
            Debug.LogError(GetType() + "/LoadAsset()/克隆资源不成功，请检查。 path=" + path);
        }
        //goObj = null;//??????????
        return goObjClone;
    }       
}//Class_end
