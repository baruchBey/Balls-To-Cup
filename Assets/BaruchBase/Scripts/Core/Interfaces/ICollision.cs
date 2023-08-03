using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch
{
    public interface ICollision
    {
        void OnCollisionEnter(Collision collision);
        void OnCollisionExit(Collision collision);
        void OnCollisionStay(Collision collision);
    }
}
