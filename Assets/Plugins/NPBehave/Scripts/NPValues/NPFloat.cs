
using System;

namespace NPBehave
{
	public class NPFloat : NPValue<float>, IEquatable<NPFloat>
	{
		public override Type ValueType()
		{
			return typeof(float);
		}
		public override int CompareTo(NPValueBase other)
		{
			var b = other as NPFloat;
			if (b != null)
				return value.CompareTo(b.value);
			var b2 = other as NPInt;
			if (b2 != null)
				return value.CompareTo(b2.value);
			var b3 = other as NPLong;
			if (b3 != null)
				return value.CompareTo(b3.value);

			throw new InvalidOperationException($"can not CompareTo {this.GetType()} and {other.GetType()}");
		}
		public override bool Equals(NPValueBase other)
		{
			return Equals(other as NPFloat);
		}
		public bool Equals(NPFloat other)
		{
			if (ReferenceEquals(null, other))
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return value.Equals(other.value);
		}
		public override bool Equals(object obj)
		{
			return Equals(obj as NPFloat);
		}
		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
		public static bool operator ==(NPFloat lhs, NPFloat rhs)
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
		public static bool operator !=(NPFloat lhs, NPFloat rhs)
		{
			return !(lhs == rhs);
		}
		public static bool operator >(NPFloat lhs, NPFloat rhs)
		{
			return lhs.value > rhs.value;
		}
		public static bool operator >=(NPFloat lhs, NPFloat rhs)
		{
			return lhs.value >= rhs.value;
		}
		public static bool operator <(NPFloat lhs, NPFloat rhs)
		{
			return lhs.value < rhs.value;
		}
		public static bool operator <=(NPFloat lhs, NPFloat rhs)
		{
			return lhs.value <= rhs.value;
		}

	}
}
