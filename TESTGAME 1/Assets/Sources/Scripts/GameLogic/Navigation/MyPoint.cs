using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPoint
{
	public int row;
	public int col;
	public int status;
	public MyPoint()
	{
		this.row = 0;
		this.col = 0;
		status = 0;
	}
	public MyPoint(int row,int col)
	{
		this.row = row;
		this.col = col;
		status = 0;
	}


	//为什么引用不了啊啊啊啊啊
}
