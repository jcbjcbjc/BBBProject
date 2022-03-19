using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace GameServer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			UserDo userDo = new UserDo ();
			ResultDo resultDo = new ResultDo ();
			RoomDo roomDo = new RoomDo ();

			foreach (var item in roomDo.GetRoom(1)) {
				Console.WriteLine (item.UserId);
			}

			NetServer server = new NetServer ("127.0.0.1", 2107);

			Console.Title = "联机对战游戏服务端引擎";
			ConsoleWin32Helper.DisableCloseButton (Console.Title);

			while (true) 
			{
				Application.DoEvents ();
				Console.WriteLine ("输入exit退出程序");
				string input = Console.ReadLine ();
				Console.ReadKey ();
				if (input == "exit") {
					roomDo.Delete (0);
					Thread.CurrentThread.Abort ();
					break;
				}
			}
		}
	}	

	public class ConsoleWin32Helper
	{
		[DllImport("User32.dll", EntryPoint="FindWindow")]
		static extern IntPtr FindWindow (string IpClassName, string IpWindowName);

		[DllImport("user32.dll", EntryPoint="GetSystemMenu")]
		static extern IntPtr GetSystemMenu (IntPtr hWnd, IntPtr bRevert);

		[DllImport("user32.dll", EntryPoint="RemoveMenu")]
		static extern IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

		public static void DisableCloseButton(string title)
		{
			Thread.Sleep (100);
			IntPtr windowHandle = FindWindow (null, title);
			IntPtr closeMenu = GetSystemMenu (windowHandle, IntPtr.Zero);
			uint SC_CLOSE = 0XF060;
			RemoveMenu (closeMenu, SC_CLOSE, 0x0);
		}
	}
}
