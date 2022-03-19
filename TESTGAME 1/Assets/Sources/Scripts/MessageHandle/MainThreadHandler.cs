using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Net;

namespace MessageHandler
{
	public class MainThreadHandler : MonoBehaviour
	{
		List<TcpDataFromServer> tcpRecvDatas;

		private static MainThreadHandler _Instance = null;
		public static MainThreadHandler GetInstance()
		{
			if (_Instance == null)
			{
				_Instance = new GameObject("MainThreadHandler").AddComponent<MainThreadHandler>();
			}
			return _Instance;
		}

		private void Awake()
		{
			tcpRecvDatas = new List<TcpDataFromServer>();

		}
		private void Update()
		{
			List<TcpDataFromServer> Datas = null;
			if (tcpRecvDatas.Count != 0)
			{
				Datas = tcpRecvDatas;
				tcpRecvDatas.Clear();
			}
			if (Datas == null)
			{
				return;
			}
			foreach (TcpDataFromServer Data in Datas)
			{
				KeyValuesUpdate kvs = new KeyValuesUpdate(Data.Data);
				MessageCenter.SendMessage(Data.MessageType, kvs);
			}
		}
		public void AddRecvMessage(TcpDataFromServer tcpRecvData)
		{
			tcpRecvDatas.Add(tcpRecvData);
		}
	}

}