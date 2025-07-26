using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace UI
{
    public abstract class UIForm : BaseUIForm
    {
        protected override void OnOpen()
        {
            Panel.Show();
        }

        protected override void OnClose()
        {
            Panel.Hide();
        }
    }

    public abstract class UIForm<TData> : BaseUIForm, ICanSetData<TData>
    {
        public TData Data { get; set; }

        void ICanSetData.SetData(object data)
        {
            this.As<ICanSetData>().SetData(data);
        }

        protected sealed override void OnOpen()
        {
            Panel.Show();
        }

        protected override void OnClose()
        {
            Panel.Hide();
        }
    }

    public interface ICanSetData
    {
        void SetData(object data);
    }

    public interface ICanSetData<TData> : ICanSetData
    {
        TData Data { get; set; }
    }
}

