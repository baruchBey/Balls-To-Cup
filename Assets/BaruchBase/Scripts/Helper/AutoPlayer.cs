using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Baruch
{
    //TO DO
    public class AutoPlayer : MonoBehaviour
    {
        [SerializeField] UnityEvent _publicMethods;
       
        


        private void Update()
        {
            _publicMethods.Invoke();

        }
        private void Reset()
        {
            name = nameof(AutoPlayer);
        }


       
    }
}
