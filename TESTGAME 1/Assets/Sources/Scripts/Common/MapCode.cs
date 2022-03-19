using System;
using UnityEngine;

namespace Common
{
	public class MapCode
	{
		public const int Default = 0;
		public const int Set = 1;
		public const int Ban = 2;
		public const int Brick = 3;
		public const int StartorEndPoint = 4;
		public const int Player = 5;
	}
	public class Vec3
	{
		public float x;
		public float y;
		public float z;
		public Vec3(float x, float y, float z) { 
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}
}
