using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float hp;
    public float moveSpeed;
    public Vector3 desireVelocity;
    public Quaternion desireRotation;
    public EntityType entityType = EntityType.Other;
    public int dropCoin;

    private Vector3 acceVelocity;

    public delegate void TakeDamageDelegate(float damage, Vector3 direction);
    public event TakeDamageDelegate OnTakeDamage;

    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    protected virtual void FixedUpdate()
    {
        var speed = rb.velocity;
        speed = Vector3.SmoothDamp(speed, desireVelocity * moveSpeed, ref acceVelocity, 0.1f);
        rb.velocity = speed;

        var rotation = transform.rotation;
        transform.rotation = Quaternion.Slerp(rotation, desireRotation, 0.4f);
    }

    public void SetDesireRotation(Quaternion quaternion)
    {
        desireRotation = quaternion;
    }

    public void SetDesireVelocity(Vector3 velocity)
    {
        desireVelocity = velocity;
    }

    public void TakeDamage(float damage, Vector3 direction = default)
    {
        hp -= damage;
        OnTakeDamage?.Invoke(damage, direction);
        if (hp <= 0)
        {
            GameManager.Instance.AnyEntityDeath(this);
            Destroy(gameObject);
        }
    }
}
