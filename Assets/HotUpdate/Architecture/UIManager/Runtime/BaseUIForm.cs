using System;
using UnityEngine;

namespace UI
{
    public abstract class BaseUIForm : IUIForm
    {
        public abstract UIGroup DefultGroup { get; }
        public abstract string Location { get; }
        UIGroup IUIForm.Group { get; set; }
        EPanelState IUIForm.PanelState { get; set; } = EPanelState.Closed;
        IUIPanel IUIForm.Panel { get; set; }
        public GameObject GameObject => (this as IUIForm).Panel?.GameObject;
        protected UIPanel Panel => (this as IUIForm).Panel as UIPanel;

        public object LoadUserData { get; set; }

        void IUIForm.Close()
        {
            EPanelState state = (this as IUIForm).PanelState;
            if (state == EPanelState.Active)
            {
                (this as IUIForm).PanelState = EPanelState.Closed;
                OnClose();
            }
            else
            {
                (this as IUIForm).PanelState = EPanelState.Closed;
            }
        }
        void IUIForm.Destroy() { OnDestroy(); }
        void IUIForm.Open()
        {
            (this as IUIForm).PanelState = EPanelState.Active;
            OnOpen();
        }

        void IUIForm.Start() { OnStart(); }

        protected virtual void OnStart() { }
        protected virtual void OnOpen() { }
        protected virtual void OnClose() { }
        protected virtual void OnDestroy() { }
        protected virtual void OnShow() { }
        protected virtual void OnHide() { }
    }
}