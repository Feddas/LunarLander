using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// enum Flags ref: http://stackoverflow.com/questions/8447/enum-flags-attribute
/// </summary>
[Flags]
public enum Setting
{
    None = 0,
    IsSoundOn = 1,
}
