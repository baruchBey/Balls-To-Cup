namespace Baruch
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Must be on Fields, Must be Static
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DeprecatedSaveAttribute : PropertyAttribute
    {

    }
}
