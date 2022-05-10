/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： 对于Unity中Resource 目录下的Json 文件的解析Demo
 *    Description: 
 *           功能： yyy
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

namespace Test
{
	public class TestUnityJson2 : MonoBehaviour {


		void Start ()
		{
             //提取文件，得到字符串数据
		     TextAsset TaObj=Resources.Load<TextAsset>("People");
             //反序列化  文件-->对象 
             PersonInfo perInfo=JsonUtility.FromJson<PersonInfo>(TaObj.text);
             //显示对象中数据
             foreach (People per in perInfo.People)
		     {
		        Debug.Log(" ");
                Debug.Log(string.Format("name={0},Age={1}",per.Name,per.Age));
		     }
		}
		
	}
}