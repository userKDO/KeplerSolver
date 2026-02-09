using System;

namespace MathStructs
{

	public class VectorMath
	{
		/// <summary>
		///	Vector3 but double for better accuracy
		/// </summary>
		public struct Vector3d
		{
			public double X;
			public double Y;
			public double Z;

			public Vector3d(double x, double y, double z)
			{
				X = x;
				Y = y;
				Z = z;
			}

			public double Length()
				=> Math.Sqrt(X*X + Y*Y + Z*Z);

			public double LengthSquared()
				=> X*X + Y*Y + Z*Z;

			public Vector3d Normalized()
			{
				double l = Length();
				return new Vector3d(X/l, Y/l, Z/l);
			}

			public static Vector3d operator +(Vector3d a, Vector3d b)
				=> new(a.X+b.X, a.Y+b.Y, a.Z+b.Z);

			public static Vector3d operator -(Vector3d a, Vector3d b)
				=> new(a.X-b.X, a.Y-b.Y, a.Z-b.Z);

			public static Vector3d operator *(Vector3d v, double k)
				=> new(v.X*k, v.Y*k, v.Z*k);

			public static Vector3d operator /(Vector3d v, double k)
				=> new(v.X/k, v.Y/k, v.Z/k);

			public static double Dot(Vector3d a, Vector3d b)
				=> a.X*b.X + a.Y*b.Y + a.Z*b.Z;

			public static double Cross(Vector3d a, Vector3d b)
				=> new{
					a.Y*b.Z - a.Z*b.Y,
					a.Z*b.X - a.X*b.Z,
					a.X*b.Y - a.Y*b.X
				};
		}
	}
}
