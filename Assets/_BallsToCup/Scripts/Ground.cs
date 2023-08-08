using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch
{
    public class Ground : MonoBehaviour
    {
        public static event Action OnMarbleWasted;
        static readonly HashSet<int> _wastedMarbles = new();
        public static int WastedMarbleCount => _wastedMarbles.Count;

        private void Start()
        {
            _wastedMarbles.Clear();
        }
     
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (_wastedMarbles.Add(collider.gameObject.GetInstanceID()))
            {
                OnMarbleWasted.Invoke();
                collider.GetComponent<Marble>().Wasted();//Decolor the marbles

            }

        }
    }

}
