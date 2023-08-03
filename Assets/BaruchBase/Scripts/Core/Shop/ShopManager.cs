using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch
{
    public class ShopManager : Singleton<ShopManager>, IInit
    {

        [SerializeField] Transform _parent;
        [SerializeField] Transform _shopParent;

        public void Init()
        {
            
        }
    }
}
