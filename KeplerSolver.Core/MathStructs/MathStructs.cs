using System;

namespace MathStructs
{

	public static class VectorMath
	{
		/// <summary>
		///	Vector3 but double for better accuracy
		/// </summary>
		public struct Vector3d
		{
			public double X { get; }
			public double Y { get; }
			public double Z { get; }

			public Vector3d(double x, double y, double z)
			{
				X = x;
				Y = y;
				Z = z;
			}
			
			public static Vector3d FromOrbitalPlane(double xOrb,double yOrb,double i,double Omega, double omega )
			{
					double cosO = Math.Cos(Omega);
					double sinO = Math.Sin(Omega);
					double cosi = Math.Cos(i);
					double sini = Math.Sin(i);
					double cosw = Math.Cos(omega);
					double sinw = Math.Sin(omega);
					
					double r11 = cosO * cosw - sinO * sinw * cosi;
					double r12 = -cosO * sinw - sinO * cosw * cosi;
					
					double r21 = sinO * cosw + cosO * sinw * cosi;
					double r22 = -sinO * sinw + cosO * cosw * cosi;
					
					double r31 = sinw * sini;
					double r32 = cosw * sini;
					
					double x = xOrb * r11 + yOrb * r12;
					double y = xOrb * r21 + yOrb * r22;
					double z = xOrb * r31 + yOrb * r32;
					
					return new Vector3d(x, y, z);
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

			public static Vector3d Cross(Vector3d a, Vector3d b)
				=> new Vector3d(
					a.Y*b.Z - a.Z*b.Y,
					a.Z*b.X - a.X*b.Z,
					a.X*b.Y - a.Y*b.X
					);
		}
	}
}
