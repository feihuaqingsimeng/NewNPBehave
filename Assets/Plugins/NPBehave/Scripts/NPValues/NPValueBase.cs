
using System;

namespace NPBehave
{
	public abstract class NPValueBase: IEquatable<NPValueBase>,IComparable<NPValueBase>
	{
		public abstract Type ValueType();
		public abstract bool Equals(NPValueBase other);
		public virtual int CompareTo(NPValueBase other)
		{
			throw new NotImplementedException();
		}
		
	}
}
