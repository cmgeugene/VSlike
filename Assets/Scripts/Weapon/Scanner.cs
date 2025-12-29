using System;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    void FixedUpdate() 
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float minDiff = float.MaxValue;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;

            // myPos 와 targetPos 의 거리 계산
            float diff = (target.transform.position - transform.position).sqrMagnitude;

            if (diff < minDiff)
            {
                minDiff = diff;
                result = target.transform;
            }
        }

        return result;
    }
}
