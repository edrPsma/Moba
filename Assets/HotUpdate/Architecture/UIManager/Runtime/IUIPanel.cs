using UnityEngine;

namespace UI
{
    public interface IUIPanel
    {
        Canvas Canvas { get; }
        CanvasGroup CanvasGroup { get; }
        int OrderLayer { get; }
        bool StartOver { get; }
        IUIForm Form { get; }
        GameObject GameObject { get; }
        internal void SetLayer(int orderLayer);
        void Show();
        void Hide();
    }
}
