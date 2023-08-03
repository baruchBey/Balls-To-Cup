using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Baruch.UI
{
    [DisallowMultipleComponent]
    public class TabbedUIPanel : UIPanel
    {
        [Header("Buttons")]
        [SerializeField] protected Button exitButton = default;
        
        [Header("Transforms")]
        [SerializeField] protected RectTransform tabButtonContainer = default;
        [SerializeField] protected RectTransform tabPanelContainer = default;

        [Header("Components")]
        [SerializeField] protected CanvasGroup canvasGroup = default;

        [Header("Dependencies")]
        [SerializeField] protected TabButton tabButtonPrefab = default;
        [SerializeField] protected UIPanel tabPanelPrefab = default;

        protected List<UITab> tabs = default;
        protected UITab selectedTab = default;
        
        protected Tween canvasGroupFadeInTween = default;
        protected Tween canvasGroupFadeOutTween = default;

        public List<UITab> Tabs => tabs;
        
        public override void Init()
        {
            base.Init();
            CreateTabs(3);
            SetSelectedTab(0);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        public override void Activate()
        {
            canvasGroup.alpha = 0f;
            canvasGroupFadeInTween?.Kill();
            canvasGroupFadeOutTween?.Kill();
            canvasGroupFadeInTween = canvasGroup.DOFade(1f, 0.5f);
            SetSelectedTab(0);
            base.Activate();
        }

        public override void Deactivate()
        {
            canvasGroupFadeInTween?.Kill();
            canvasGroupFadeOutTween?.Kill();
            canvasGroupFadeOutTween = canvasGroup.DOFade(0f, 0.5f)
                .OnComplete
                (
                    base.Deactivate
                );
            base.Deactivate();
        }

        protected virtual void OnExitButtonClicked()
        {
            Deactivate();
        }
        
        protected virtual void CreateTabs(int count)
        {
            tabs = new List<UITab>();
            for (int i = 0; i < count; i++)
            {
                TabButton tabButton = Instantiate(tabButtonPrefab, tabButtonContainer);
                UIPanel tabPanel = Instantiate(tabPanelPrefab, tabPanelContainer);
                UITab tab = new UITab(this, tabButton, tabPanel);
                tab.Init();
                tabs.Add(tab);
            }
        }

        public virtual void SetSelectedTab(int index)
        {
            selectedTab?.OnDeselected();
            selectedTab = tabs[index];
            selectedTab.OnSelected();
        }
    }
}