using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Baruch.Util
{
    public static class BaruchUtil
    {
        public static IEnumerable<T> FindInterfacesOfType<T>(bool includeInActive = false)
        {
            return Object.FindObjectsOfType<MonoBehaviour>(includeInActive).OfType<T>();
        }
    }
}