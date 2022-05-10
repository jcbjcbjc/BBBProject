using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetConn
{
	static NetConn _instance;
	public static NetConn Instance
	{
		get
		{
			if (_instance == null) {
				_instance = new NetConn ();
			}
			return _instance;
		}
	}

	Socket _socket;
	byte[] _buffer = new byte[1024 * 1024];

	public void Connect (string ip, int port)
	{
		_socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		try {
			_socket.Connect (ip, port);
			Game.Instance.ShowMessage ("连接服务器成功", 5);
			Start();
		} catch (Exception ex) {
			Game.Instance.ShowMessage ("无法连接到服务器：" + ex.Message, 5);
		}
	}

	void Start()
	{
		_socket.BeginReceive (_buffer, 0, _buffer.Length, SocketFlags.None, StartReceiveCallback, _socket);
	}

	void StartReceiveCallback(IAsyncResult ar)
	{
		try {
			int length = _socket.EndReceive(ar);
			if (length > 0) {
				string str = Encoding.UTF8.GetString(_buffer,0,length);
				var strs = str.Split('|');
				int requestCode = int.Parse(strs[0]);
				int actionCode = int.Parse(strs[1]);
				string data = strs[2];
				OnResponse(requestCode,actionCode,data);
			}
			Start();
		} catch (Exception ex) {
			if (Connected == false) {
				Game.Instance.HideAllPanel ();
				Game.Instance.ShowMessage ("服务端断开了连接请检查网络是否连接或重启客户端，原因：" + ex.Message, 20);
			}
			else {
				Game.Instance.ShowMessage ("无法接收消息：" + ex.Message, 5);
			}
		}
	}

	void OnResponse(int requestCode, int actionCode, string data)
	{
		Game.Instance.OnResponse (requestCode, actionCode, data);
	}

	public int Send(int requestCode, int actionCode, string data)
	{
		try {
			byte[] buffer = Encoding.UTF8.GetBytes(requestCode + "|" + actionCode  + "|" + data);
			return _socket.Send(buffer);
		} catch (Exception ex) {
			if (Connected == false) {
				Game.Instance.ShowMessage ("无法发送消息：请检查网络连接或重启客户端，原因：" + ex.Message, 20);
			}
			else {
				Game.Instance.ShowMessage ("无法发送消息：" + ex.Message, 5);
			}
		}
		return -1;
	}

	public void Close()
	{
		try {
			_socket.Close();
		} catch (Exception ex) {
			Game.Instance.ShowMessage ("无法关闭连接：" + ex.Message);
		}
	}
	public void Reconnect() { }
	public void OnLoseConnect() { 

	}
	public bool Connected { get { return _socket != null && _socket.Connected == true; } }
}
