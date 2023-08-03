using Baruch.Extension;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

using static Baruch.Util.BaruchUtil;

namespace Baruch
{
    [DisallowMultipleComponent]
    public class CurrencyManager : Singleton<CurrencyManager>, IInit
    {
        [Save] private static double _balance = 6;
        [Save] private static uint _premiumBalance = default;

        public static double Balance { get => _balance; set { _balance = value; OnBalanceChanged(); } }

        public static uint PremiumBalance { get => _premiumBalance; set { _premiumBalance = value; OnPremiumBalanceChanged(); } }

        static readonly HashSet<IBalance> _balanceInterfaces = new();
        static readonly HashSet<IPremiumBalance> _premiumBalanceInterfaces = new();

        public static event Action<double> OnGain;
        public static event Action<double> OnSpend;

        public static event Action<uint> OnPremiumGain;
        public static event Action<uint> OnPremiumSpend;

        public void Init()
        {
            _balanceInterfaces.UnionWith(FindInterfacesOfType<IBalance>(true));
            _premiumBalanceInterfaces.UnionWith(FindInterfacesOfType<IPremiumBalance>(true));

            OnBalanceChanged();
            OnPremiumBalanceChanged();
        }



        public bool HasEnoughBalance(double amount)
        {
            return Balance >= amount;
        }

        public void Gain(double gainedAmount)
        {
            Balance += gainedAmount;
            OnGain?.Invoke(gainedAmount);
        }


        public void Spend(double spentAmount)
        {
            Balance -= spentAmount;
            OnSpend?.Invoke(spentAmount);
        }


        public bool HasEnoughPremiumBalance(uint amount)
        {
            return PremiumBalance >= amount;
        }

        public void PremiumGain(uint gainedAmount)
        {
            PremiumBalance += gainedAmount;
            OnPremiumGain?.Invoke(gainedAmount);
        }

        public void PremiumSpend(uint spentAmount)
        {
            PremiumBalance -= spentAmount;
            OnPremiumSpend?.Invoke(spentAmount);
        }

        private static void OnBalanceChanged()
        {
            _balanceInterfaces.ForEach(x => x.OnBalanceChanged(Balance));
        }

        private static void OnPremiumBalanceChanged()
        {
            _premiumBalanceInterfaces.ForEach(x => x.OnPremiumBalanceChanged(PremiumBalance));
        }


    }

#if UNITY_EDITOR

    [CustomEditor(typeof(CurrencyManager))]
    public class CurrencyManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Display static fields in the Inspector
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Static Fields", EditorStyles.boldLabel);

            // Display the "Balance" field
            CurrencyManager.Balance = EditorGUILayout.DoubleField("Balance", Math.Max(CurrencyManager.Balance, 0));

            // Display the "PremiumBalance" field
            CurrencyManager.PremiumBalance = (uint)EditorGUILayout.LongField("Premium Balance", Math.Min(CurrencyManager.PremiumBalance, uint.MaxValue));


        }
    }
#endif
}