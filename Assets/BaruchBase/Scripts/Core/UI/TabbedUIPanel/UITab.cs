using System;

namespace Baruch.UI
{
    [Serializable]
    public class UITab
    {
        protected TabbedUIPanel tabbedUIPanel = default;
        protected TabButton button = default;
        protected UIPanel panel = default;

        public UIPanel Panel => panel;
        public TabButton Button => button;
        
        private UITab(){}
        
        public UITab(TabbedUIPanel tabbedUIPanel, TabButton button, UIPanel panel)
        {
            this.tabbedUIPanel = tabbedUIPanel;
            this.button = button;
            this.panel = panel;
        }

        public virtual void Init()
        {
            button.Init("New Tab", null);
            panel.Init();
            button.Clicked += OnButtonClicked;
        }

        public virtual void OnSelected()
        {
            panel.Activate();
            button.OnSelected();
        }

        public virtual void OnDeselected()
        {
            panel.Deactivate();
            button.OnDeselected();
        }
        
        protected virtual void OnButtonClicked()
        {
            int index = button.transform.GetSiblingIndex();
            tabbedUIPanel.SetSelectedTab(index);
        }
    }
}