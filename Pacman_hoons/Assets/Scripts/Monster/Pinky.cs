using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : GhostBase
{
    // Start is called before the first frame update
    void Start()
    {
        debugColor = Color.white;
        nowDebug = true;
        isInGhostHouse = true;

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override Vector2 GetTargetPosition()
    {
        Vector2 playerPosition = player.transform.localPosition;
        Vector2 playerDirection = player.GetComponent<Player>().oldDirection;

        playerPosition = new Vector2(playerPosition.x, playerPosition.y);
        Vector2 targetPosition = playerPosition + (4 * playerDirection);

        return targetPosition;
    }

    public override void Restart()
    {
        base.Restart();


        direction = Vector2.up;
        transform.position = nowWayPoint.transform.position;
        nextWayPoint = GetNextWayPoint();
        oldWayPoint = nowWayPoint;
        UpdateAnimation();
    }
}
