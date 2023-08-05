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

        public void OnDisable()
        {

        }

        public override void OnEnable()
        {
        }
    }
}