
using System;

namespace NPBehave
{
	public class NPLong : NPValue<long>, IEquatable<NPLong>
	{
		public override Type ValueType()
		{
			return typeof(long);
		}
		public override int CompareTo(NPValueBase other)
		{
			var b = other as NPLong;
			if (b != null)
				return value.CompareTo(b.value);
			var b2 = other as NPFloat;
			if (b2 != null)
				return b2.value.CompareTo(value);
			var b3 = other as NPInt;
			if (b3 != null)
				return value.CompareTo(b3.value);

			throw new InvalidOperationException($"can not CompareTo {this.GetType()} and {other.GetType()}");
		}
		public override bool Equals(NPValueBase other)
		{
			return Equals(other as NPLong);
		}
		public bool Equals(NPLong other)
		{
			if (ReferenceEquals(null, other))
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return value.Equals(other.value);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as NPLong);
		}
		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
		public static bool operator ==(NPLong lhs, NPLong rhs)
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
		public static bool operator !=(NPLong lhs, NPLong rhs)
		{
			return !(lhs == rhs);
		}
		public static bool operator >(NPLong lhs, NPLong rhs)
		{
			return lhs.value > rhs.value;
		}
		public static bool operator >=(NPLong lhs, NPLong rhs)
		{
			return lhs.value >= rhs.value;
		}
		public static bool operator <(NPLong lhs, NPLong rhs)
		{
			return lhs.value < rhs.value;
		}
		public static bool operator <=(NPLong lhs, NPLong rhs)
		{
			return lhs.value <= rhs.value;
		}


	}
}
