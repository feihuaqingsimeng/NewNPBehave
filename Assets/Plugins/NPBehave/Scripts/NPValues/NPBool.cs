
using System;

namespace NPBehave
{
	public class NPBool : NPValue<bool>, IEquatable<NPBool>
	{
		public override Type ValueType()
		{
			return typeof(bool);
		}
		public override int CompareTo(NPValueBase other)
		{
			var b = other as NPBool;
			return value.CompareTo(b.value);
		}
		public bool Equals(NPBool other)
		{
			if (ReferenceEquals(null, other))
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return value.Equals(other.value);
		}
		public override bool Equals(NPValueBase other)
		{
			return Equals(other as NPBool);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as NPBool);
		}
		public override int GetHashCode()
		{
			return value.GetHashCode();
		}

		

		public static bool operator ==(NPBool lhs, NPBool rhs)
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
		public static bool operator !=(NPBool lhs, NPBool rhs)
		{
			return !(lhs == rhs);
		}
	}
}
