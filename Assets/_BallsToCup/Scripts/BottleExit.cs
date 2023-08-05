using System;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch
{
    public class BottleExit : MonoBehaviour
    {
        static readonly HashSet<int> _freeMarbles = new();

        public static event Action OnMarbleExit;
        public static int FreeMarbleCount => _freeMarbles.Count;
        private void Start()
        {
            _freeMarbles.Clear();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            collider.gameObject.transform.SetParent(LevelManager.Instance.CurrentLevel.FreeMarbleParent);

            if(_freeMarbles.Add(collider.GetInstanceID()))
                OnMarbleExit.Invoke();

        }
    }
}
