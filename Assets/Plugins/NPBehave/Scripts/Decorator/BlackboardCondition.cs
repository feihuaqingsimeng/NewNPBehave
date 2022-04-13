using UnityEngine;

namespace NPBehave
{

	public class BlackboardCondition : ObservingDecorator
	{
        private string key;
        private NPValueBase value;
        private Operator op;

		public string Key => key;

		public NPValueBase Value
        {
            get
            {
                return value;
            }
        }

		public Operator Operator
        {
            get
            {
                return op;
            }
        }

        public BlackboardCondition(string key, Operator op, NPValueBase value, Stops stopsOnChange, Node decoratee) : base("BlackboardCondition", stopsOnChange, decoratee)
        {
            this.op = op;
            this.key = key;
            this.value = value;
            this.stopsOnChange = stopsOnChange;
        }
        
        public BlackboardCondition(string key, Operator op, Stops stopsOnChange, Node decoratee) : base("BlackboardCondition", stopsOnChange, decoratee)
        {
            this.op = op;
            this.key = key;
            this.stopsOnChange = stopsOnChange;
        }


        override protected void StartObserving()
        {
            this.RootNode.Blackboard.AddObserver(key, onValueChanged);
        }

        override protected void StopObserving()
        {
            this.RootNode.Blackboard.RemoveObserver(key, onValueChanged);
        }

        private void onValueChanged(Blackboard.Type type, NPValueBase key)
        {
            Evaluate();
        }

        override protected bool IsConditionMet()
        {
            if (op == Operator.ALWAYS_TRUE)
            {
                return true;
            }

            if (!this.RootNode.Blackboard.Isset(key))
            {
                return op == Operator.IS_NOT_SET;
            }

            var o = this.RootNode.Blackboard.Get(key);

            switch (this.op)
            {
                case Operator.IS_SET: return true;
                case Operator.IS_EQUAL: return o.Equals(value);
                case Operator.IS_NOT_EQUAL: return !o.Equals(value);

                case Operator.IS_GREATER_OR_EQUAL:
					return o.CompareTo(value) >= 0;
                case Operator.IS_GREATER:
					return o.CompareTo(value) > 0;
				case Operator.IS_SMALLER_OR_EQUAL:
					return o.CompareTo(value) <= 0;

				case Operator.IS_SMALLER:
					return o.CompareTo(value) < 0;

				default: return false;
            }
        }

        override public string ToString()
        {
            return "(" + this.op + ") " + this.key + " ? " + this.value;
        }
    }
}