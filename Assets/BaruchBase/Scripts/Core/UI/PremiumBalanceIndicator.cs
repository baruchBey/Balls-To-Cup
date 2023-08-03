using TMPro;
using UnityEngine;
using Baruch.Extension;
using UnityEngine.Pool;

namespace Baruch.UI
{
    public class PremiumBalanceIndicator : MonoBehaviour, IPremiumBalance
    {
        [SerializeField] TextMeshProUGUI _text;       


        public void OnPremiumBalanceChanged(uint balance)
        {
            _text.text = $"{balance}<sprite=1>";

        }
    }
}
