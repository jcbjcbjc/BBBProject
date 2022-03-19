/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： 语言国际化 
 *    Description: 
 *           功能： 使得我们发布的游戏，可以根据不同的国家，显示不同的语言信息。
 *                  
 *    Date: 2017
 *    Version: 0.1版本
 *    Modify Recoder: 
 *    
 *   
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW
{
	public class LauguageMgr {
        //本类实例
	    public static LauguageMgr _Instance;
        //语言翻译的缓存集合
	    private Dictionary<string, string> _DicLauguageCache;



	    private LauguageMgr()
	    {
	         _DicLauguageCache=new Dictionary<string, string>();
             //初始化语言缓存集合
	         InitLauguageCache();
	    }

        /// <summary>
        /// 得到本类实例
        /// </summary>
        /// <returns></returns>
	    public static LauguageMgr GetInstance()
	    {
	        if(_Instance==null)
            {
                _Instance=new LauguageMgr();
            }
	        return _Instance;
	    }

        /// <summary>
        /// 到显示文本信息
        /// </summary>
        /// <param name="lauguageID">语言的ID</param>
        /// <returns></returns>
	    public string ShowText(string lauguageID)
	    {
	        string strQueryResult = string.Empty;           //查询结果

            //参数检查
	        if (string.IsNullOrEmpty(lauguageID)) return null;

            //查询处理
            if (_DicLauguageCache != null && _DicLauguageCache.Count>=1)
            {
                _DicLauguageCache.TryGetValue(lauguageID, out strQueryResult);
                if (!string.IsNullOrEmpty(strQueryResult))
                {
                    return strQueryResult;
                }
            }

            Debug.Log(GetType() + "/ShowText()/ Query is Null!  Parameter lauguageID: " + lauguageID);
	        return null;
	    }

	    /// <summary>
        /// 初始化语言缓存集合
        /// </summary>
	    private void InitLauguageCache()
	    {
            IConfigManager config = new ConfigManagerByJson("LauguageJSONConfig");
	        if (config!=null)
	        {
	            _DicLauguageCache = config.AppSetting;
	        }
	    }


	}
}