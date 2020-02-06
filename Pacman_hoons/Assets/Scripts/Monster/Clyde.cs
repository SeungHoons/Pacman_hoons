using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : GhostBase
{
    // Start is called before the first frame update
    void Start()
    {
        debugColor = Color.yellow;
        nowDebug = true;
        isInGhostHouse = true;


        base.Start();
        direction = Vector2.left;
        UpdateAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override Vector2 GetTargetPosition()
    {
        Vector2 playerPosition = player.transform.position;

        float distance = GetDistance(transform.position, playerPosition);
        Vector2 targetPosition = Vector2.zero;

        if (distance >= 8)
        {
            targetPosition = new Vector2(playerPosition.x, playerPosition.y);
        }
        else if (distance < 8)
        {
            targetPosition = scatterWayPoint.transform.position;
        }
        return targetPosition;
    }
    public override void Restart()
    {
        base.Restart();


        direction = Vector2.left;
        transform.position = nowWayPoint.transform.position;
        nextWayPoint = GetNextWayPoint();
        oldWayPoint = nowWayPoint;
        UpdateAnimation();
    }
}
