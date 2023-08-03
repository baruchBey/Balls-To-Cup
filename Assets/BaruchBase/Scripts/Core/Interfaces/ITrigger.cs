using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch
{
    public interface ITrigger
    {
        void OnTriggerEnter(Collider collider);
        void OnTriggerExit(Collider collider);
        void OnTriggerStay(Collider collider);
    }
}