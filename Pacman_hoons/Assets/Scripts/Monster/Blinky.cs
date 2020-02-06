using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : GhostBase
{
    // Start is called before the first frame update
    void Start()
    {
        debugColor = Color.red;
        nowDebug = true;

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override Vector2 GetTargetPosition()
    {
        Vector2 targetPosition = Vector2.zero;

        Vector2 pacmanPosition = player.transform.localPosition;
        targetPosition = new Vector2(pacmanPosition.x, pacmanPosition.y  );
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
