/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： 演示Unity 对Json 解析API    
 
 *    Description: 
 *           功能：
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
	public class TestUnityJson : MonoBehaviour {


		void Start () {
			Hero heroObj=new Hero();
		    heroObj.Name = "郭靖";
		    heroObj.MyLevel = new Level() {HeroLevel = 800};
            //相当于如下写法
            //Level lev=new Level();
            //lev.HeroLevel = 800;
            //heroObj.MyLevel = lev;

		    //方法1： Json 序列化工作(对象--> 文件)
		    string strHeroInfo = JsonUtility.ToJson(heroObj);
            Debug.Log("测试1： 得到的序列化后的字符串="+strHeroInfo);

		    //方法2： 反序列化(Json文件--> 对象)
		    Hero heroInfo2 = JsonUtility.FromJson<Hero>(strHeroInfo);
            Debug.Log("测试2：得到反序列化对象数值，名称:  "+heroInfo2.Name+" 等级:  "+heroInfo2.MyLevel.HeroLevel);

		    //方法3： 测试覆盖反序列化。
            Hero hero=new Hero();
		    hero.Name = "杨过";
		    hero.MyLevel = new Level() {HeroLevel = 500};
            
            //Json 序列化
		    string heroInfo3 = JsonUtility.ToJson(hero);
            //测试覆盖反序列化
            JsonUtility.FromJsonOverwrite(heroInfo3, heroObj);
            Debug.Log("测试3, 得到再次反序列化覆盖的对象信息，名称:  "+heroObj.Name+"  等级： "+heroObj.MyLevel.HeroLevel);


		}
		
	}
}