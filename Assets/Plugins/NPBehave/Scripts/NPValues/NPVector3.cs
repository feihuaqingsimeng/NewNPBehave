
using System;
using UnityEngine;

namespace NPBehave
{
	public class NPVector3 : NPValue<Vector3>, IEquatable<NPVector3>
	{

		public override Type ValueType()
		{
			return typeof(Vector3);
		}
		public bool Equals(NPVector3 other)
		{
			if (ReferenceEquals(null, other))
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return value.Equals(other.value);
		}
		public override bool Equals(NPValueBase other)
		{
			return Equals(other as NPVector3);
		}
		public override bool Equals(object obj)
		{
			return Equals(obj as NPVector3);
		}
		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
		public static bool operator ==(NPVector3 lhs, NPVector3 rhs)
		{
			if (ReferenceEquals(lhs, null))
			{
				if (ReferenceEquals(rhs, null))
				{
					return true;
				}
				return false;
			}
			return lhs.Equals(rhs);
		}
		public static bool operator !=(NPVector3 lhs, NPVector3 rhs)
		{
			return !(lhs == rhs);
		}
	}
}
