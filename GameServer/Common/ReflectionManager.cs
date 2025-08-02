using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameServer.Common
{
    public class ReflectionManager : Singleton<ReflectionManager, ReflectionManager>
    {
        Dictionary<Type, object> _map;

        Dictionary<Type, FieldInfo[]> _fieldMap;

        protected override void OnInit()
        {
            base.OnInit();

            _map = new Dictionary<Type, object>();
            _fieldMap = new Dictionary<Type, FieldInfo[]>();
            Collect();
        }

        void Collect()
        {
            HashSet<object> set = new HashSet<object>();

            Type[] types = typeof(ReflectionManager).Assembly.GetTypes();
            foreach (var item in types)
            {
                CollectMaskClass(item, set);
                CollectFields(item);
            }

            foreach (var item in set)
            {
                Inject(item);
            }
        }

        void CollectMaskClass(Type item, HashSet<object> set)
        {
            ReflectionAttribute attribute = item.GetCustomAttribute<ReflectionAttribute>();
            if (attribute == null) return;

            if (attribute.Types == null || attribute.Types.Count == 0)
            {
                object instance = Activator.CreateInstance(item);
                _map.Add(item, instance);
                set.Add(instance);
            }
            else
            {
                object instance = Activator.CreateInstance(item);
                for (int i = 0; i < attribute.Types.Count; i++)
                {
                    _map.Add(attribute.Types[i], instance);
                }
                set.Add(instance);
            }
        }

        void CollectFields(Type type)
        {
            List<FieldInfo> result = new List<FieldInfo>();
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var item in fieldInfos)
            {
                InjectAttribute attribute = item.GetCustomAttribute<InjectAttribute>();
                if (attribute == null) continue;

                result.Add(item);
            }

            if (result.Count != 0)
            {
                _fieldMap.Add(type, result.ToArray());
            }
        }

        public object Get(Type type)
        {
            if (_map.ContainsKey(type))
            {
                return _map[type];
            }
            else
            {
                return null;
            }
        }

        public void Inject(object obj)
        {
            Type type = obj.GetType();

            if (_fieldMap.ContainsKey(type))
            {
                foreach (var item in _fieldMap[type])
                {
                    item.SetValue(obj, Get(item.FieldType));
                }
            }
        }
    }

    public static class Builder
    {
        public static T NewAndInject<T>() where T : new()
        {
            T t = new T();
            ReflectionManager.Instance.Inject(t);

            return t;
        }
    }
}