using UnityEngine;
using UnityEngine.UI;

namespace Baruch.UI
{
    public class FailUI : SubUI
    {
        [SerializeField] Button _restartButton;


        public override void Init()
        {
            _restartButton.onClick.AddListener(LevelManager.Instance.RestartLevel);
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