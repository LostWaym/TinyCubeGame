using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class PlayerLogic : MonoBehaviour
{
    private Entity entity;
    public Transform bulletPrefab;

    [Header("能量")]
    public float maxEnergy = 5;
    public float energy = 5;
    public float energyRegenDelay = 2;
    public float energyRegenDelayTimer = 0;
    public float energyRegenAmount = 5;

    [Header("发射")]
    public float launchItval = 0.5f;
    public float launchItvalTimer = 0;
    public float launchCost = 1f;
    public float launchAngleRangeDelta = 0f;
    public float bulletDamage = 1;
    public float bulletMoveSpeed = 8;

    [Header("冲刺")]
    public float dashCost = 2f;
    public float dashMultiply = 10f;
    public float dashLength = 5f;
    public Vector3 dashingTargetPoint;
    public bool dashing = false;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    private void Update()
    {
        energyRegenDelayTimer -= Time.deltaTime;
        launchItvalTimer -= Time.deltaTime;

        if (energyRegenDelayTimer <= 0)
        {
            energy = Mathf.Min(maxEnergy, energy + energyRegenAmount * Time.deltaTime);
        }

        if (dashing)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, dashingTargetPoint, entity.moveSpeed * dashMultiply * Time.deltaTime);
            transform.position = newPos;
            if ((dashingTargetPoint - newPos).magnitude < 1e-5f)
            {
                dashing = false;
            }
        }
    }

    public void LaunchBullet(Vector3 oriPos, Vector3 targetPos)
    {
        if (energy < launchCost)
            return;

        if (launchItvalTimer > 0)
            return;

        energy -= launchCost;
        energyRegenDelayTimer = energyRegenDelay;
        launchItvalTimer = launchItval;

        var vec = targetPos - oriPos;
        var newPos = Quaternion.AngleAxis(UnityEngine.Random.Range(-launchAngleRangeDelta, launchAngleRangeDelta), Vector3.up) * vec;
        InternalLaunchBullet(oriPos, oriPos + newPos);
    }

    private void InternalLaunchBullet(Vector3 oriPos, Vector3 targetPos)
    {
        Transform bullet = Instantiate(bulletPrefab);
        BulletLogic logic = bullet.GetComponent<BulletLogic>();
        Vector3 velocity = (targetPos - oriPos).normalized * bulletMoveSpeed;
        bullet.position = oriPos;
        bullet.localScale *= 1.5f;
        logic.Init(bulletDamage, velocity, entity);
    }

    public void Dash(Vector3 targetPos)
    {
        if (energy < dashCost)
            return;

        if (dashing)
            return;

        dashing = true;
        energy -= dashCost;
        energyRegenDelayTimer = energyRegenDelay;
        Vector3 vec = targetPos - transform.position;
        vec = Vector3.ClampMagnitude(vec, dashLength);
        dashingTargetPoint = transform.position + vec;
    }
}
