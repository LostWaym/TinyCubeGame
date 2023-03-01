﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EnemyLogic : MonoBehaviour
{
    private Entity entity;

    public float attackStunTime = 1f;
    public float attackStunTimer;
    public float attackDamage = 1f;
    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        attackStunTimer -= Time.deltaTime;
        if (attackStunTimer > 0f)
        {
            entity.SetDesireVelocity(Vector3.zero);
        }
        else
        {
            var target = GameManager.Instance.Player;
            if (target != null)
            {
                Vector3 vec = target.transform.position - transform.position;
                entity.SetDesireRotation(Quaternion.LookRotation(vec));
                entity.SetDesireVelocity(vec.normalized);
            }
            else
            {
                entity.SetDesireVelocity(Vector3.zero);
            }
        }
    }

    public void DoAttack(Entity entity)
    {
        if (attackStunTimer > 0)
            return;

        attackStunTimer = attackStunTime;
        entity.TakeDamage(attackDamage);
    }

    private void OnTriggerStay(Collider other)
    {
        var ent = other.GetComponent<Entity>();
        if (ent != null && ent.entityType == EntityType.Player)
        {
            DoAttack(ent);
        }
    }
}
