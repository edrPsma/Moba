using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup), typeof(GraphicRaycaster))]
    public class UIPanel : MonoBehaviour, IUIPanel
    {
#if UNITY_EDITOR
        [ShowInInspector] public System.Type FormType => Form.GetType();
        [ShowInInspector] public EPanelState PanelState => Form.PanelState;
        [ShowInInspector] public UIGroup Group => Form.Group;
        [ShowInInspector] public bool Initialization => StartOver;
#endif

        public Canvas Canvas { get; private set; }
        public CanvasGroup CanvasGroup { get; private set; }
        public int OrderLayer => Canvas.sortingOrder;
        [ShowInInspector] public IUIForm Form { get; internal set; }
        public GameObject GameObject => gameObject;
        public bool StartOver { get; private set; }
        // public BindComponent BindComponent { get; private set; }

        void Awake()
        {
            // BindComponent = GetComponent<BindComponent>();
            Canvas = GetComponent<Canvas>();
            CanvasGroup = GetComponent<CanvasGroup>();
            // Canvas.vertexColorAlwaysGammaSpace = true;
        }

        void Start()
        {
            Form.Start();
            StartOver = true;
        }

        void OnDestroy()
        {
            Form.Destroy();
        }

        void IUIPanel.SetLayer(int orderLayer)
        {
            Canvas.overrideSorting = true;
            Canvas.sortingOrder = orderLayer;
        }

        public void Show()
        {
            CanvasGroup.alpha = 1;
            CanvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            CanvasGroup.alpha = 0;
            CanvasGroup.blocksRaycasts = false;
        }
    }
}
