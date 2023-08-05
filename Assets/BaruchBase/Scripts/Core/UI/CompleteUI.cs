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

       
        public override void OnDisable()
        {
            throw new System.NotImplementedException();
        }

        public override void OnEnable()
        {
        }
    }
}