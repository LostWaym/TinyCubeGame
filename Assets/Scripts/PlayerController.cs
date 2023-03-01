using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class PlayerController : MonoBehaviour
{
    private Entity entity;
    private PlayerLogic playerLogic;
    private void Awake()
    {
        entity = GetComponent<Entity>();
        playerLogic = GetComponent<PlayerLogic>();
    }
    // Update is called once per frame
    void Update()
    {
        float h, v;
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        var desireVelocity = new Vector3(h, 0, v).normalized;
        entity.SetDesireVelocity(desireVelocity);

        DoMouseUpdate();
    }

    private void DoMouseUpdate()
    {
        Vector3 pos = GetMousePoint();
        if (pos.sqrMagnitude == 0)
            return;

        pos.y = transform.position.y;
        var desireRotation = Quaternion.LookRotation(pos - transform.position);
        entity.SetDesireRotation(desireRotation);

        if (Input.GetMouseButton(0))
        {
            playerLogic.LaunchBullet(transform.position, pos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            playerLogic.Dash(pos);
        }
    }

    private Vector3 GetMousePoint()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 pos = ray.GetPoint(enter);
            return pos;
        }

        return Vector3.zero;
    }
}
