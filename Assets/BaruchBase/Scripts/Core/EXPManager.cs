using Baruch.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch
{
    public class EXPManager : Singleton<EXPManager>, IInit
    {
        public float EXP = float.Epsilon;
        EXPBar _expBar;
        

        public void Init()
        {
            //_expBar = GameUI.Instance.EXPBar;
            _expBar.SetLevel(EXP);
        }



    }
}
