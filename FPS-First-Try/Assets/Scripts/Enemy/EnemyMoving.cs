using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] internal float onHitIncrease;
    [SerializeField] [Range(1.0f, 6.0f)] internal float moveSpeed;
    [SerializeField] [Range(1.0f, 10.0f)] internal float turnSpeed;

    internal Vector3 GetLookDir(GameObject target)
    {
        var look_dir = target.transform.position - transform.position; look_dir.y = 0;
        return look_dir;
    }
    internal void Moving(Vector3 look_dir)
    {
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(look_dir), turnSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    internal void OnHitIncrease()
    {
        moveSpeed += onHitIncrease;
        turnSpeed += onHitIncrease;
    }
}
