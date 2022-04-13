
using System;

namespace NPBehave
{
	public class NPInt : NPValue<int>, IEquatable<NPInt>
	{
		public override Type ValueType()
		{
			return typeof(int);
		}
		public override int CompareTo(NPValueBase other)
		{
			var b = other as NPInt;
			if (b != null)
				return value.CompareTo(b.value);
			var b2 = other as NPFloat;
			if (b2 != null)
				return b2.value.CompareTo(value);
			var b3 = other as NPLong;
			if (b3 != null)
				return b3.value.CompareTo(value);

			throw new InvalidOperationException($"can not CompareTo {this.GetType()} and {other.GetType()}");
		}

		public bool Equals(NPInt other)
		{
			if (ReferenceEquals(null, other))
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return value.Equals(other.value);
		}
		public override bool Equals(NPValueBase other)
		{
			return Equals(other as NPInt);
		}
		public override bool Equals(object obj)
		{
			return Equals(obj as NPInt);
		}
		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
		public static bool operator ==(NPInt lhs, NPInt rhs)
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
		public static bool operator !=(NPInt lhs, NPInt rhs)
		{
			return !(lhs == rhs);
		}
		public static bool operator >(NPInt lhs, NPInt rhs)
		{
			return lhs.value > rhs.value;
		}
		public static bool operator >=(NPInt lhs, NPInt rhs)
		{
			return lhs.value >= rhs.value;
		}
		public static bool operator <(NPInt lhs, NPInt rhs)
		{
			return lhs.value < rhs.value;
		}
		public static bool operator <=(NPInt lhs, NPInt rhs)
		{
			return lhs.value <= rhs.value;
		}

		
	}
}
