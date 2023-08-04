using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch
{
    public class BottleExit : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
