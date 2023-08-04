using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch
{
    public class Finish : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.LogWarning(collision);
        }
    }
}
