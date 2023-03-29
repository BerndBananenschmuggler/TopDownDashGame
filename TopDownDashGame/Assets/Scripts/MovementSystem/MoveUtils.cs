using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveUtils
{
    private static Matrix4x4 m_isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => m_isoMatrix.MultiplyPoint3x4(input);
}
