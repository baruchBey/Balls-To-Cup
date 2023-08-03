using TMPro;
using UnityEngine;
using Baruch.Extension;
using UnityEngine.Pool;

namespace Baruch.UI
{
    public class BalanceIndicator : MonoBehaviour,IBalance
    {
        [SerializeField] TextMeshProUGUI _text;


        public void OnBalanceChanged(double balance)
        {
            _text.text = balance.Format(suffix: "<sprite=0>");
        }
    }
}
