namespace Baruch
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Must be on Fields, Must be Static
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SaveAttribute : Attribute
    {

    }

}
