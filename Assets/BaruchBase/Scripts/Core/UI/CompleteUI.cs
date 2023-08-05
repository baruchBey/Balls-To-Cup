using UnityEngine;
using UnityEngine.UI;

namespace Baruch.UI
{
    public class CompleteUI : SubUI
    {
        [SerializeField] Button _continueButton;

        

        public override void Init()
        {
            _continueButton.onClick.AddListener(LevelManager.Instance.NextLevel);
        }

        public void OnDisable()
        {
        }

        public override void OnEnable()
        {
        }
    }
}