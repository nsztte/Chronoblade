using UnityEngine;
using System;

public static class TransformUtils
{
    /// <summary>
    /// 조건에 맞는 자식 트랜스폼 탐색
    /// </summary>
    public static Transform FindChildRecursive(Transform parent, Predicate<Transform> match)
    {
        foreach (Transform child in parent)
        {
            if (match(child))
                return child;

            var found = FindChildRecursive(child, match);
            if (found != null)
                return found;
        }
        return null;
    }
}
