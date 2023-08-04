using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baruch
{
    public class GravitySetter : MonoBehaviour
    {
        const float GRAVITY = 9.8f;

        // Update is called once per frame
        void Update()
        {
            var angle = transform.eulerAngles.z;
            Physics2D.gravity = GRAVITY * new Vector2(Mathf.Sin(angle), -Mathf.Cos(angle));
        }
    }
}
