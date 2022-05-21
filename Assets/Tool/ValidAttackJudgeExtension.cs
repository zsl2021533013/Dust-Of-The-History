using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValidAttackJudgeExtension
{
    private const float dotThreshold = 0.5f; // ¼´60¶È

    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        Vector3 dir = target.position - transform.position;
        dir.Normalize();

        return Vector3.Dot(dir, transform.forward) > dotThreshold;
    }
}
