/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： 消息（传递）中心
 *    Description: 
 *           功能： 负责UI框架中，所有UI窗体中间的数据传值。
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
namespace MessageHandler
{
    public class MessageCenter
    {
        //委托：消息传递
        public delegate void DelMessageDelivery(KeyValuesUpdate kv);

        //消息中心缓存集合
        //<string : 数据大的分类，DelMessageDelivery 数据执行委托>
        public static Dictionary<string, DelMessageDelivery> _dicMessages = new Dictionary<string, DelMessageDelivery>();

        /// <summary>
        /// 增加消息的监听。
        /// </summary>
        /// <param name="messageType">消息分类</param>
        /// <param name="handler">消息委托</param>
        public static void AddMsgListener(string messageType, DelMessageDelivery handler)
        {

            if (!_dicMessages.ContainsKey(messageType))
            {

                _dicMessages.Add(messageType, null);
            }
            _dicMessages[messageType] += handler;
        }

        /// <summary>
        /// 取消消息的监听
        /// </summary>
        /// <param name="messageType">消息分类</param>
        /// <param name="handele">消息委托</param>
        public static void RemoveMsgListener(string messageType, DelMessageDelivery handele)
        {
            if (_dicMessages.ContainsKey(messageType))
            {
                _dicMessages[messageType] -= handele;
            }

        }

        /// <summary>
        /// 取消所有指定消息的监听
        /// </summary>
        public static void ClearALLMsgListener()
        {
            if (_dicMessages != null)
            {
                _dicMessages.Clear();
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messageType">消息的分类</param>
        /// <param name="kv">键值对(对象)</param>
        public static void SendMessage(string messageType, KeyValuesUpdate kv)
        {
            DelMessageDelivery del;                         //委托

            if (_dicMessages.TryGetValue(messageType, out del))
            {

                if (del != null)
                {
                    //调用委托
                    del(kv);
                }
                else
                {
                    Debug.Log("del == null!, Please check the messageType ！ param messageType = " + messageType);
                }


            }
        }
        public static void SendMessage(string messageType, object data)
        {
            KeyValuesUpdate kvs = new KeyValuesUpdate(data);
            SendMessage(ProConst.SHOW_MESSAGE_MESSAGE, kvs);
        }
    }

    /// <summary>
    /// 键值更新对
    /// 功能： 配合委托，实现委托数据传递
    /// </summary>
    public class KeyValuesUpdate
    {
        //值
        private object _Values;


        /*  只读属性  */

        public object Values
        {
            get { return _Values; }
        }

        public KeyValuesUpdate(object valueObj)
        {
            _Values = valueObj;
        }
    }



}


