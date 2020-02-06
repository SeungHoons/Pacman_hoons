using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inky : GhostBase
{
    GameObject Blinky;

    // Start is called before the first frame update
    void Start()
    {
        Blinky = GameObject.Find("Blinky");
        debugColor = Color.cyan;
        nowDebug = true;
        isInGhostHouse = true;

        base.Start();
        direction = Vector2.right;
        UpdateAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override Vector2 GetTargetPosition()
    {
        if( ghostSelfReleaseTimer > ghostReleaseTimer)
        {

        }
        Vector2 playerPosition = player.transform.localPosition;
        Vector2 playerDirection = player.GetComponent<Player>().oldDirection;

        playerPosition = new Vector2(playerPosition.x, playerPosition.y);

        Vector2 targetPosition = playerPosition + (2 * playerDirection);

        Vector2 tempBlinkyPosition = Blinky.transform.position;

        float directionX = targetPosition.x - tempBlinkyPosition.x;
        float directionY = targetPosition.y - tempBlinkyPosition.y;
        directionX *= 2f;
        directionY *= 2f;

        targetPosition = new Vector2(tempBlinkyPosition.x + directionX, tempBlinkyPosition.y + directionY);

        return targetPosition;
    }

    public override void Restart()
    {
        base.Restart();


        direction = Vector2.right;
        transform.position = nowWayPoint.transform.position;
        nextWayPoint = GetNextWayPoint();
        oldWayPoint = nowWayPoint;
        UpdateAnimation();
    }
}
