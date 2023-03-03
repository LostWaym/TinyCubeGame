using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class LaunchEnemyLogic : MonoBehaviour
{
    private Entity entity;
    [Header("跟随")]
    public float followDistance = 10f;
    [Header("发射")]
    public Transform launchPrefab;
    public float launchAmount;
    public float launchCounter;
    public float launchPerItval;
    public float launchPerTimer;
    public float launchFirstDelay;
    public float launchItval;
    public float launchTimer;
    public float launchSpeed;
    public float launchAngleRangeDelta;
    public float launchDamage;
    public bool launching;
    public Vector3 launchOffset;
    private Vector3 launchOriPos;
    private Vector3 launchVec;

    private float stunTime;
    private Vector3 stunDirection;


    private void Awake()
    {
        entity = GetComponent<Entity>();
        entity.OnTakeDamage += OnStun;
    }

    private void OnStun(float damage, Vector3 direction)
    {
        stunTime = 1f;
        stunDirection = direction;
        LaunchEnd();
    }

    // Update is called once per frame
    void Update()
    {
        if (stunTime > 0)
        {
            entity.SetDesireVelocity(stunDirection * stunTime);
            stunTime -= Time.deltaTime;
            return;
        }

        launchTimer -= Time.deltaTime;
        Entity player;
        if (launching)
        {
            if (launchCounter == 0)
            {
                LaunchEnd();
            }
            else
            {
                launchPerTimer -= Time.deltaTime;
                if (launchPerTimer <= 0)
                {
                    launchPerTimer = launchPerItval;
                    launchCounter--;
                    var newPos = Quaternion.AngleAxis(Random.Range(-launchAngleRangeDelta, launchAngleRangeDelta), Vector3.up) * launchVec;
                    InternalLaunch(launchOriPos, launchOriPos + newPos);
                }
            }
        }
        else if ((player = GameManager.Instance.Player) != null)
        {
            float dis = (player.transform.position - transform.position).magnitude;
            var forward = player.transform.position - transform.position;
            forward.y = 0;
            entity.SetDesireRotation(Quaternion.LookRotation(forward));
            if (dis > followDistance)
            {
                entity.SetDesireVelocity(forward.normalized);
            }
            else
            {
                entity.SetDesireVelocity(Vector3.zero);
                TryLaunch(player.transform.position);
            }
        }
        else
        {
            entity.SetDesireVelocity(Vector3.zero);
        }
    }

    public void TryLaunch(Vector3 targetPos)
    {
        if (launching || launchTimer > 0)
            return;

        Launch(targetPos);
    }

    public void Launch(Vector3 targetPos)
    {
        entity.SetDesireVelocity(Vector3.zero);
        var forward = targetPos - transform.position;
        forward.y = 0f;
        entity.SetDesireRotation(Quaternion.LookRotation(forward));
        launchOriPos = transform.TransformPoint(launchOffset);
        launchVec = targetPos - launchOriPos;
        launching = true;
        launchCounter = launchAmount;
        launchPerTimer = launchFirstDelay;
    }

    public void LaunchEnd()
    {
        launching = false;
        launchTimer = launchItval;
    }

    private void InternalLaunch(Vector3 oriPos, Vector3 targetPos)
    {
        var forward = (targetPos - oriPos).normalized;
        var bullet = Instantiate(launchPrefab);
        bullet.transform.position = transform.position;
        var logic = bullet.GetComponent<BulletLogic>();
        logic.Init(launchDamage, forward * launchSpeed, entity);
    }
}
