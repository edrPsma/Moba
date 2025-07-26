using System;
using System.Collections.Generic;
using Observable;
using Pool;
using Template;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UIManager : MonoSingleton<IUIManager, UIManager>, IUIManager
    {
        public Camera UICamera { get; private set; }
        public Canvas Canvas { get; private set; }
        public EventSystem UnityEventSystem { get; private set; }
        Dictionary<UIGroup, RectTransform> _groupNodeDic;
        Dictionary<Type, IUIForm> _cacheFormDic;// 表单缓存字典
        Dictionary<UIGroup, LinkedList<IUIPanel>> _openPanelDic;// 打开的ui堆栈
        Queue<IUIForm> _openQueue;// ui打开队列
        Dictionary<Type, LoadUIData> _loadUIDataDic;//面板加载字典
        ObjectPool<LoadUIData> _loadUIDataPool;
        List<Type> _removeLoadUIDataCacheList;// 缓存要删除的ui加载数据
        IUILoader _uiLoader;
        TypeEventSource _eventSource;

        protected override void OnInit()
        {
            gameObject.hideFlags = HideFlags.HideInHierarchy;
            DontDestroyOnLoad(gameObject);
        }

        public void Initialize(IUILoader uILoader)
        {
            GameObject uiRoot = GameObject.Instantiate(Resources.Load<GameObject>("[UIRoot]"));
            uiRoot.name = "[UIRoot]";
            GameObject.DontDestroyOnLoad(uiRoot);
            UICamera = uiRoot.GetComponentInChildren<Camera>();
            Canvas = uiRoot.GetComponentInChildren<Canvas>();
            UnityEventSystem = uiRoot.GetComponentInChildren<EventSystem>();
            _uiLoader = uILoader;

            _eventSource = new TypeEventSource();
            _groupNodeDic = new Dictionary<UIGroup, RectTransform>();
            _cacheFormDic = new Dictionary<Type, IUIForm>();
            _openPanelDic = new Dictionary<UIGroup, LinkedList<IUIPanel>>();
            _loadUIDataDic = new Dictionary<Type, LoadUIData>();
            _removeLoadUIDataCacheList = new List<Type>();
            _openQueue = new Queue<IUIForm>();
            _loadUIDataPool = new ObjectPool<LoadUIData>(EPoolType.Scalable, 10);
            _loadUIDataPool.OnReleaseEvent += OnLoadUIDataRelease;
            CreateUIGroup();
        }

        private void OnLoadUIDataRelease(LoadUIData obj)
        {
            obj.Cancel = false;
            obj.UIForm = null;
            obj.Destroy = false;
            obj.AddPanelComponent = false;
            obj.GameObject = null;
        }

        void CreateUIGroup()
        {
            foreach (UIGroup group in Enum.GetValues(typeof(UIGroup)))
            {
                GameObject temp = new GameObject(group.ToString());
                var rect = temp.AddComponent<RectTransform>();
                temp.transform.SetParent(Canvas.transform);
                rect.offsetMin = new Vector2(0, 0);
                rect.offsetMax = new Vector2(0, 0);
                rect.anchorMax = new Vector2(1, 1);
                rect.anchorMin = new Vector2(0, 0);
                rect.localPosition = Vector3.zero;
                rect.localScale = Vector3.one;
                _groupNodeDic.Add(group, rect);
                _openPanelDic.Add(group, new LinkedList<IUIPanel>());
            }
        }

        public void Pop(Type type, bool destroy = true)
        {
            if (_cacheFormDic.ContainsKey(type))
            {
                IUIForm form = _cacheFormDic[type];

                if (form.PanelState == EPanelState.Loading)
                {
                    if (_loadUIDataDic.ContainsKey(type))
                    {
                        LoadUIData loadUIData = _loadUIDataDic[type];
                        loadUIData.Cancel = true;
                        loadUIData.Destroy = destroy;
                        CloseForm(form);
                    }
                }
                else if (form.PanelState != EPanelState.Closed)
                {
                    Pop(form, destroy);
                }
            }
        }

        void Pop(IUIForm form, bool destroy)
        {
            if (form == null) return;

            LinkedList<IUIPanel> links = _openPanelDic[form.Group];
            IUIForm preForm = null;
            if (links.Last.Value.Form == form)
            {
                preForm = links.Last.Previous?.Value.Form;
            }

            links.Remove(form.Panel);
            CloseForm(form);
            if (destroy)
            {
                _cacheFormDic.Remove(form.GetType());
                _uiLoader.UnLoad(form.Panel.GameObject, form.LoadUserData);
            }
            ResetStack(form.Group);
        }

        void ResetStack(UIGroup group)
        {
            LinkedList<IUIPanel> links = _openPanelDic[group];
            LinkedListNode<IUIPanel> curNode = links.First;
            int orderLayer = GetOrderLayer(group);
            while (curNode != null)
            {
                curNode.Value.SetLayer(orderLayer);
                orderLayer += UIConfig.PanelInterval;
                curNode = curNode.Next;
            }
        }

        public void Pop<T>(bool destroy = true) where T : class, IUIForm, new()
        {
            Type type = typeof(T);
            Pop(type, destroy);
        }

        public void PopAll(UIGroup group, bool destroy = true)
        {
            PopAll(group, true, destroy);
        }

        void PopAll(UIGroup group, bool cancelLoading, bool destroy = true)
        {
            if (cancelLoading)
            {
                foreach (var item in _loadUIDataDic)
                {
                    if (item.Value.UIForm.Group == group)
                    {
                        item.Value.Cancel = true;
                        item.Value.Destroy = destroy;
                        CloseForm(item.Value.UIForm);
                    }
                }
            }

            LinkedList<IUIPanel> links = _openPanelDic[group];
            LinkedListNode<IUIPanel> curNode = links.Last;

            while (curNode != null)
            {
                IUIForm form = curNode.Value.Form;
                CloseForm(form);
                if (destroy)
                {
                    _cacheFormDic.Remove(form.GetType());
                    _uiLoader.UnLoad(form.Panel.GameObject, form.LoadUserData);
                }
                curNode = curNode.Previous;
                links.RemoveLast();
            }
        }

        void CloseForm(IUIForm form)
        {
            form.Close();
            TriggerCloseEvent(form);
        }

        public void PopAllLower<T>(bool destroy = true) where T : class, IUIForm, new()
        {
            Type type = typeof(T);
            if (!_cacheFormDic.ContainsKey(type)) return;

            IUIForm form = _cacheFormDic[type];
            if (form.PanelState == EPanelState.Loading)
            {
                foreach (var item in _loadUIDataDic)
                {
                    if (item.Value.UIForm.Group != form.Group) continue;

                    if (item.Value.UIForm != form)
                    {
                        item.Value.Cancel = true;
                        item.Value.Destroy = destroy;
                        CloseForm(item.Value.UIForm);
                    }
                    else
                    {
                        break;
                    }
                }

                PopAll(form.Group, false, destroy);
            }
            else if (form.PanelState != EPanelState.Closed)
            {
                LinkedList<IUIPanel> links = _openPanelDic[form.Group];
                LinkedListNode<IUIPanel> curNode = links.First;

                while (curNode != null && curNode.Value.Form != form)
                {
                    IUIForm curForm = curNode.Value.Form;
                    CloseForm(curForm);
                    if (destroy)
                    {
                        _cacheFormDic.Remove(curForm.GetType());
                        _uiLoader.UnLoad(curForm.Panel.GameObject, curForm.LoadUserData);
                    }
                    curNode = curNode.Next;
                    links.RemoveFirst();
                }
            }
        }

        public void PopAllUpper<T>(bool destroy = true) where T : class, IUIForm, new()
        {
            Type type = typeof(T);
            if (!_cacheFormDic.ContainsKey(type)) return;

            IUIForm form = _cacheFormDic[type];
            if (form.PanelState == EPanelState.Loading)
            {
                bool upper = false;
                foreach (var item in _loadUIDataDic)
                {
                    if (item.Value.UIForm.Group != form.Group) continue;

                    if (item.Value.UIForm != form)
                    {
                        if (upper)
                        {
                            item.Value.Cancel = true;
                            item.Value.Destroy = destroy;
                            CloseForm(item.Value.UIForm);
                        }
                    }
                    else
                    {
                        upper = true;
                    }
                }
            }
            else if (form.PanelState != EPanelState.Closed)
            {
                foreach (var item in _loadUIDataDic)
                {
                    if (item.Value.UIForm.Group != form.Group) continue;

                    item.Value.Cancel = true;
                    item.Value.Destroy = destroy;
                    CloseForm(item.Value.UIForm);
                }

                LinkedList<IUIPanel> links = _openPanelDic[form.Group];
                LinkedListNode<IUIPanel> curNode = links.Last;

                while (curNode != null && curNode.Value.Form != form)
                {
                    IUIForm curForm = curNode.Value.Form;
                    CloseForm(curForm);
                    if (destroy)
                    {
                        _cacheFormDic.Remove(curForm.GetType());
                        _uiLoader.UnLoad(curForm.Panel.GameObject, curForm.LoadUserData);
                    }
                    curNode = curNode.Previous;
                    links.RemoveLast();
                }
            }
        }

        public void Push<T>() where T : UIForm, new()
        {
            IUIForm form = GetOrCreateForm<T>();
            Push(form, form.DefultGroup, false);
        }

        public void Push<T>(UIGroup group) where T : UIForm, new()
        {
            IUIForm form = GetOrCreateForm<T>();
            Push(form, group, false);
        }

        IUIForm Push(IUIForm form, UIGroup group, bool async)
        {
            if (IsPushed(form)) return form;

            form.Group = group;
            form.PanelState = EPanelState.Loading;

            Type type = form.GetType();
            if (_loadUIDataDic.ContainsKey(type))
            {
                if (_loadUIDataDic[type].Cancel)
                {
                    _loadUIDataDic[type].Cancel = false;
                    _loadUIDataDic[type].Destroy = false;
                }
                else
                {
                    Debug.LogError("Repeated loading, there should be some problems here.");
                }
            }
            else
            {
                LoadUIData loadUIData = _loadUIDataPool.SpawnByType();
                loadUIData.UIForm = form;
                loadUIData.Cancel = false;
                loadUIData.Destroy = false;
                loadUIData.GameObject = form.GameObject;
                loadUIData.AddPanelComponent = form.Panel != null;
                _loadUIDataDic.Add(type, loadUIData);
                _openQueue.Enqueue(form);
                if (form.Panel == null)
                {
                    if (async)
                    {
                        _uiLoader.LoadAsync(form.Location, out object userData, panelObj => OnLoadOver(form, panelObj));
                        form.LoadUserData = userData;
                    }
                    else
                    {
                        _uiLoader.Load(form.Location, out object userData, panelObj => OnLoadOver(form, panelObj));
                        form.LoadUserData = userData;
                    }
                }
            }

            return form;
        }

        public void Update()
        {
            ProcessLoadUIDataDic();
            ProcessOpenQueue();
        }

        void ProcessLoadUIDataDic()
        {
            foreach (var item in _loadUIDataDic)
            {
                LoadUIData temp = item.Value;
                if (temp.GameObject == null) continue;

                if (temp.Cancel)
                {
                    if (temp.Destroy)
                    {
                        _uiLoader.UnLoad(temp.GameObject, temp.UIForm.LoadUserData);
                        _cacheFormDic.Remove(temp.UIForm.GetType());
                    }
                    else
                    {
                        AddUIPanelComponent(temp);
                    }

                    _removeLoadUIDataCacheList.Add(temp.UIForm.GetType());
                }
                else
                {
                    AddUIPanelComponent(temp);
                }
            }
            for (int i = 0; i < _removeLoadUIDataCacheList.Count; i++)
            {
                _loadUIDataPool.Release(_loadUIDataDic[_removeLoadUIDataCacheList[i]]);
                _loadUIDataDic.Remove(_removeLoadUIDataCacheList[i]);
            }
            _removeLoadUIDataCacheList.Clear();
        }

        void AddUIPanelComponent(LoadUIData loadUIData)
        {
            if (!loadUIData.AddPanelComponent)
            {
                loadUIData.AddPanelComponent = true;
                loadUIData.GameObject.transform.SetParent(GetGroupNode(loadUIData.UIForm.Group).transform, false);
                UIPanel panel = loadUIData.GameObject.AddComponent<UIPanel>();
                panel.Form = loadUIData.UIForm;
                loadUIData.UIForm.Panel = panel;
                panel.Hide();
            }
        }

        void ProcessOpenQueue()
        {
            if (_openQueue.Count > 0)
            {
                IUIForm form = _openQueue.Peek();

                // 取消打开
                if (form.PanelState == EPanelState.Closed)
                {
                    _openQueue.Dequeue();
                }
                else
                {
                    // 面板已初始化
                    if (form.Panel != null && form.Panel.StartOver)
                    {
                        try
                        {
                            OpenPanel(form);
                        }
                        catch (Exception e) { Debug.LogError(e); }
                        finally
                        {
                            _loadUIDataPool.Release(_loadUIDataDic[form.GetType()]);
                            _loadUIDataDic.Remove(form.GetType());

                            _openQueue.Dequeue();
                        }
                    }
                }
            }
        }

        void OpenPanel(IUIForm form)
        {
            IUIPanel panel = form.Panel;
            int targetOrder = GetOrderLayer(form.Group);

            LinkedList<IUIPanel> link = _openPanelDic[form.Group];
            if (link.Count > 0)
            {
                targetOrder = link.Last.Value.OrderLayer + UIConfig.PanelInterval;
            }
            link.AddLast(panel);
            panel.SetLayer(targetOrder);
            form.Open();
            TriggerOpenEvent(form);
#if UNITY_EDITOR
            (link.Last.Value as UIPanel).GameObject.transform.SetAsLastSibling();
#endif
        }

        private void OnLoadOver(IUIForm form, GameObject gameObject)
        {
            Type type = form.GetType();
            if (_loadUIDataDic.ContainsKey(type))
            {
                LoadUIData loadUIData = _loadUIDataDic[type];
                loadUIData.GameObject = gameObject;
            }
            else
            {
                Debug.LogError("There may be some problems here that cause repeated loading.");
            }
        }

        bool IsPushed(IUIForm form)
        {
            if (form == null) return false;

            return form.PanelState != EPanelState.Closed;
        }

        IUIForm GetOrCreateForm<T>() where T : class, IUIForm, new()
        {
            Type type = typeof(T);
            if (_cacheFormDic.ContainsKey(type))
            {
                return _cacheFormDic[type];
            }
            else
            {
                IUIForm form = new T();
                _cacheFormDic.Add(type, form);
                return form;
            }
        }

        public RectTransform GetGroupNode(UIGroup group)
        {
            return _groupNodeDic[group];
        }

        public int GetOrderLayer(UIGroup group)
        {
            return (int)group * UIConfig.GroupInterval;
        }

        public void Destroy<T>() where T : class, IUIForm, new()
        {
            Type type = typeof(T);
            if (_cacheFormDic.ContainsKey(type))
            {
                IUIForm form = _cacheFormDic[type];
                if (IsPushed(form))
                {
                    Pop(form, true);
                }
                else
                {
                    _cacheFormDic.Remove(type);
                    _uiLoader.UnLoad(form.GameObject, form.LoadUserData);
                }
            }
        }

        public void PushAsync<T>() where T : UIForm, new()
        {
            IUIForm form = GetOrCreateForm<T>();
            Push(form, form.DefultGroup, true);
        }

        public void PushAsync<T>(UIGroup group) where T : UIForm, new()
        {
            IUIForm form = GetOrCreateForm<T>();
            Push(form, group, true);
        }

        public bool IsOpened<T>() where T : class, IUIForm, new()
        {
            Type type = typeof(T);
            return IsOpened(type);
        }

        public bool IsOpened(Type type)
        {
            if (_cacheFormDic.ContainsKey(type))
            {
                return _cacheFormDic[type].PanelState == EPanelState.Active;
            }
            else
            {
                return false;
            }
        }

        public T SearchForm<T>() where T : class, IUIForm, new()
        {
            Type type = typeof(T);
            return SearchForm(type) as T;
        }

        public IUIForm SearchForm(Type type)
        {
            if (_cacheFormDic.ContainsKey(type))
            {
                return _cacheFormDic[type];
            }
            else
            {
                return null;
            }
        }

        public GameObject SearchPanel(string name)
        {
            foreach (var item in _cacheFormDic)
            {
                IUIForm form = item.Value;
                if (form.GameObject != null && form.GameObject.name.Replace("(Clone)", "") == name)
                {
                    return form.GameObject;
                }
            }
            return null;
        }

        public void Push<T, TData>(TData data) where T : UIForm<TData>, new()
        {
            IUIForm form = GetOrCreateForm<T>();

            (form as UIForm<TData>).Data = data;
            form.As<ICanSetData<TData>>().Data = data;
            Push(form, form.DefultGroup, false);
        }

        public void Push<T, TData>(UIGroup group, TData data) where T : UIForm<TData>, new()
        {
            IUIForm form = GetOrCreateForm<T>();
            form.As<ICanSetData<TData>>().Data = data;
            Push(form, group, false);
        }

        public void PushAsync<T, TData>(TData data) where T : UIForm<TData>, new()
        {
            IUIForm form = GetOrCreateForm<T>();
            form.As<ICanSetData<TData>>().Data = data;
            Push(form, form.DefultGroup, true);
        }

        public void PushAsync<T, TData>(UIGroup group, TData data) where T : UIForm<TData>, new()
        {
            IUIForm form = GetOrCreateForm<T>();
            form.As<ICanSetData<TData>>().Data = data;
            Push(form, group, true);
        }

        public void HideGroup(UIGroup uiGroup)
        {
            _groupNodeDic[uiGroup].localScale = Vector3.zero;
        }

        public void ShowGroup(UIGroup uiGroup)
        {
            _groupNodeDic[uiGroup].localScale = Vector3.one;
        }

        void TriggerOpenEvent(IUIForm form)
        {
            _eventSource.Trigger($"{form.GetType()}_Open", form);
            _eventSource.Trigger($"Open", form);
        }

        void TriggerCloseEvent(IUIForm form)
        {
            _eventSource.Trigger($"{form.GetType()}_Close", form);
            _eventSource.Trigger($"Close", form);
        }

        public IUnRegister SubscribeOnOpen<T>(Action<IUIForm> onOpen) where T : IUIForm
        {
            Type type = typeof(T);
            return _eventSource.Register($"{type}_Open", onOpen);
        }

        public void UnsubscribeOnOpen<T>(Action<IUIForm> onOpen) where T : IUIForm
        {
            Type type = typeof(T);
            _eventSource.UnRegister($"{type}_Open", onOpen);
        }

        public IUnRegister SubscribeOnClose<T>(Action<IUIForm> onClose) where T : IUIForm
        {
            Type type = typeof(T);
            return _eventSource.Register($"{type}_Close", onClose);
        }

        public void UnsubscribeOnClose<T>(Action<IUIForm> onClose) where T : IUIForm
        {
            Type type = typeof(T);
            _eventSource.UnRegister($"{type}_Close", onClose);
        }

        public IUnRegister SubscribeOnOpen(Action<IUIForm> onOpen)
        {
            return _eventSource.Register($"Open", onOpen);
        }

        public void UnsubscribeOnOpen(Action<IUIForm> onOpen)
        {
            _eventSource.UnRegister($"Open", onOpen);
        }

        public IUnRegister SubscribeOnClose(Action<IUIForm> onClose)
        {
            return _eventSource.Register($"Close", onClose);
        }

        public void UnsubscribeOnClose(Action<IUIForm> onClose)
        {
            _eventSource.UnRegister($"Close", onClose);
        }

        class LoadUIData
        {
            public IUIForm UIForm;// ui表单
            public bool Cancel = false;// 是否取消打开
            public bool Destroy = false;// 是否删除物体
            public GameObject GameObject;
            public bool AddPanelComponent = false;//是否添加了面板组件
        }
    }
}
