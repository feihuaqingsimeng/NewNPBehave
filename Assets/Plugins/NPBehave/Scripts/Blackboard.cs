using UnityEngine;
using System.Collections.Generic;

namespace NPBehave
{
    public class Blackboard
    {
        public enum Type
        {
            ADD,
            REMOVE,
            CHANGE
        }
		private struct Notification
		{
			public string key;
			public Type type;
			public NPValueBase value;
			public Notification(string key, Type type, NPValueBase value)
			{
				this.key = key;
				this.type = type;
				this.value = value;
			}
		}
		
        private Clock clock;
        private Dictionary<string, NPValueBase> data = new Dictionary<string, NPValueBase>();
        private Dictionary<string, List<System.Action<Type, NPValueBase>>> observers = new Dictionary<string, List<System.Action<Type, NPValueBase>>>();
        private bool isNotifiyng = false;
        private Dictionary<string, List<System.Action<Type, NPValueBase>>> addObservers = new Dictionary<string, List<System.Action<Type, NPValueBase>>>();
        private Dictionary<string, List<System.Action<Type, NPValueBase>>> removeObservers = new Dictionary<string, List<System.Action<Type, NPValueBase>>>();
        private List<Notification> notifications = new List<Notification>();
        private List<Notification> notificationsDispatch = new List<Notification>();
        private Blackboard parentBlackboard;
        private HashSet<Blackboard> children = new HashSet<Blackboard>();

        public Blackboard(Blackboard parent, Clock clock)
        {
            this.clock = clock;
            this.parentBlackboard = parent;
        }
        public Blackboard(Clock clock)
        {
            this.parentBlackboard = null;
            this.clock = clock;
        }

        public void Enable()
        {
            if (this.parentBlackboard != null)
            {
                this.parentBlackboard.children.Add(this);
            }
        }

        public void Disable()
        {
            if (this.parentBlackboard != null)
            {
                this.parentBlackboard.children.Remove(this);
            }
            if (this.clock != null)
            {
                this.clock.RemoveTimer(this.NotifiyObservers);
            }
        }

		public void Set<T>(string key,T value)
		{

            if (this.parentBlackboard != null && this.parentBlackboard.Isset(key))
            {
                this.parentBlackboard.Set(key, value);
            }
            else
            {
                if (!this.data.ContainsKey(key))
                {
					var npValue = NPValueFactory.Create(value);
					this.data[key] = npValue;
					this.notifications.Add(new Notification(key, Type.ADD, npValue));
                    this.clock.AddTimer(0f, 0, NotifiyObservers);
                }
                else
                {
					var npValue = data[key] as NPValue<T>;
					var v = npValue.Get();
                    if ((v == null && value != null) || (v != null && !v.Equals(value)))
                    {
						npValue.Set(value);
                        this.notifications.Add(new Notification(key, Type.CHANGE, npValue));
                        this.clock.AddTimer(0f, 0, NotifiyObservers);
                    }
                }
            }
        }

        public void Unset<T>(string key)
        {
			if(data.TryGetValue(key,out NPValueBase value))
			{
				this.data.Remove(key);
				this.notifications.Add(new Notification(key, Type.REMOVE, null));
				this.clock.AddTimer(0f, 0, NotifiyObservers);
			}
        }
		public NPValueBase Get(string key)
		{
			if (data.TryGetValue(key, out NPValueBase value))
			{
				return value;
			}
			else if (this.parentBlackboard != null)
			{
				return this.parentBlackboard.Get(key);
			}
			return null;
		}
		public T Get<T>(string key)
		{
			if(data.TryGetValue(key,out NPValueBase value))
			{
				if (value == null)
					return default;
				return ( value as INPValue<T>).Get();
			}
            else if (this.parentBlackboard != null)
            {
                return this.parentBlackboard.Get<T>(key);
            }
            else
            {
                return default;
            }
        }

        public bool Isset(string key)
        {
            return this.data.ContainsKey(key) || (this.parentBlackboard != null && this.parentBlackboard.Isset(key));
        }

        public void AddObserver(string key, System.Action<Type, NPValueBase> observer)
        {
            var observers = GetObserverList(this.observers, key);
            if (!isNotifiyng)
            {
                if (!observers.Contains(observer))
                {
                    observers.Add(observer);
                }
            }
            else
            {
                if (!observers.Contains(observer))
                {
                    var addObservers = GetObserverList(this.addObservers, key);
                    if (!addObservers.Contains(observer))
                    {
                        addObservers.Add(observer);
                    }
                }

                var removeObservers = GetObserverList(this.removeObservers, key);
                if (removeObservers.Contains(observer))
                {
                    removeObservers.Remove(observer);
                }
            }
        }

        public void RemoveObserver(string key, System.Action<Type, NPValueBase> observer)
        {
            var observers = GetObserverList(this.observers, key);
            if (!isNotifiyng)
            {
                if (observers.Contains(observer))
                {
                    observers.Remove(observer);
                }
            }
            else
            {
                var removeObservers = GetObserverList(this.removeObservers, key);
                if (!removeObservers.Contains(observer))
                {
                    if (observers.Contains(observer))
                    {
                        removeObservers.Add(observer);
                    }
                }

                var addObservers = GetObserverList(this.addObservers, key);
                if (addObservers.Contains(observer))
                {
                    addObservers.Remove(observer);
                }
            }
        }


#if UNITY_EDITOR
        public List<string> Keys
        {
            get
            {
                if (this.parentBlackboard != null)
                {
                    List<string> keys = this.parentBlackboard.Keys;
                    keys.AddRange(data.Keys);
                    return keys;
                }
                else
                {
                    return new List<string>(data.Keys);
                }
            }
        }

        public int NumObservers
        {
            get
            {
                int count = 0;
                foreach (string key in observers.Keys)
                {
                    count += observers[key].Count;
                }
                return count;
            }
        }
#endif


        private void NotifiyObservers()
        {
            if (notifications.Count == 0)
            {
                return;
            }

            notificationsDispatch.Clear();
            notificationsDispatch.AddRange(notifications);
            foreach (Blackboard child in children)
            {
                child.notifications.AddRange(notifications);
                child.clock.AddTimer(0f, 0, child.NotifiyObservers);
            }
            notifications.Clear();

            isNotifiyng = true;
            foreach (Notification notification in notificationsDispatch)
            {
				if (!this.observers.ContainsKey(notification.key))
                {
                    //                Debug.Log("1 do not notify for key:" + notification.key + " value: " + notification.value);
                    continue;
                }
                var observers = GetObserverList(this.observers, notification.key);
                foreach (var observer in observers)
                {
                    if (this.removeObservers.ContainsKey(notification.key) && this.removeObservers[notification.key].Contains(observer))
                    {
                        continue;
                    }
                    observer(notification.type, notification.value);
                }
            }

            foreach (string key in this.addObservers.Keys)
            {
                GetObserverList(this.observers, key).AddRange(this.addObservers[key]);
            }
            foreach (string key in this.removeObservers.Keys)
            {
                foreach (var action in removeObservers[key])
                {
                    GetObserverList(this.observers, key).Remove(action);
                }
            }
            this.addObservers.Clear();
            this.removeObservers.Clear();

            isNotifiyng = false;
        }

        private List<System.Action<Type, NPValueBase>> GetObserverList(Dictionary<string, List<System.Action<Type, NPValueBase>>> target, string key)
        {
            List<System.Action<Type, NPValueBase>> observers;
            if (target.ContainsKey(key))
            {
                observers = target[key];
            }
            else
            {
                observers = new List<System.Action<Type, NPValueBase>>();
                target[key] = observers;
            }
            return observers;
        }
    }
}
