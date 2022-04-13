
using System;

namespace NPBehave
{
	public class NPString : NPValue<string>, IEquatable<NPString>
	{
		public override Type ValueType()
		{
			return typeof(string);
		}
		public override int CompareTo(NPValueBase other)
		{
			var b = other as NPString;
			return value.CompareTo(b.value);
		}
		public bool Equals(NPString other)
		{
			if (ReferenceEquals(null, other))
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return value.Equals(other.value);
		}
		public override bool Equals(NPValueBase other)
		{
			return Equals(other as NPString);
		}
		public override bool Equals(object obj)
		{
			return Equals(obj as NPString);
		}
		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
		public static bool operator ==(NPString lhs, NPString rhs)
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
		public static bool operator !=(NPString lhs, NPString rhs)
		{
			return !(lhs == rhs);
		}
	}
}

