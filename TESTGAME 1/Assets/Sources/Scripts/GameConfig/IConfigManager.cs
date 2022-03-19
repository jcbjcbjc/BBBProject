/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： 通用配置管理器接口   
 *    Description: 
 *           功能： 
 *                基于“键值对”配置文件的通用解析
 *                  
 *    Date: 2017
 *    Version: 0.1版本
 *    Modify Recoder: 
 *    
 *   
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	public interface IConfigManager  {

        /// <summary>
        /// 只读属性： 应用设置
        /// 功能： 得到键值对集合数据
        /// </summary>
	    Dictionary<string, string> AppSetting { get; }

        /// <summary>
        /// 得到配置文件（AppSeting）最大的数量
        /// </summary>
        /// <returns></returns>
	    int GetAppSettingMaxNumber();

	}

    [Serializable]
    internal class KeyValuesInfo
    {
        //配置信息
        public List<KeyValuesNode> ConfigInfo = null;
    }

    [Serializable]
    internal class KeyValuesNode
    {
        //键
        public string Key = null;
        //值
        public string Value = null;
    }
