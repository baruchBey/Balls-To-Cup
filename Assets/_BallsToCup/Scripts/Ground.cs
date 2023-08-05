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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_wastedMarbles.Add(collision.gameObject.GetInstanceID()))
                OnMarbleWasted.Invoke();

            //Decolor the marbles mb 
        }
    }

}
