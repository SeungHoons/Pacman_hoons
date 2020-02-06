using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBase : MonoBehaviour
{
    //Debug
    protected Vector2 debugTarget;
    public bool nowDebug = false;
    protected Color debugColor;

    private void OnDrawGizmos()
    {
        Gizmos.color = debugColor;
        if (nowDebug)
            Gizmos.DrawSphere(debugTarget, 0.4f);
    }
    /////////

    #region 선언부분
    [SerializeField]
    protected WayPoint nextWayPoint;
    [SerializeField]
    protected WayPoint nowWayPoint;
    [SerializeField]
    protected WayPoint oldWayPoint;
    [SerializeField]
    protected WayPoint startWayPoint;
    [SerializeField]
    protected WayPoint scatterWayPoint;
    [SerializeField]
    protected WayPoint ghostHouseWayPoint;

    [SerializeField]
    protected Vector2 direction;
    Animator animator;

    [SerializeField]
    protected float moveSpeed = 5.9f;
    private float oldMoveSpeed;
    private float frightenedMoveSpeed = 3f;
    private float eatenMoveSpeed = 15f;
    private float normalMoveSpeed = 5.9f;

    [SerializeField]
    protected float ghostReleaseTimer = 0f;
    [SerializeField]
    protected float ghostSelfReleaseTimer = 0;


    public bool isCanMove = true;
    [SerializeField]
    protected bool isInGhostHouse;

    private int scatterModeTime1 = 7;
    private int chaseModeTime1 = 20;
    private int scatterModeTime2 = 10;
    private int chaseModeTime2 = 20;
    private int scatterModeTime3 = 5;

    private float frightenedModeTimer = 0;
    private int frightenedModeTime = 10;
    private int startBlinkTime = 7;

    public static int frightenedGhosts;


    public enum Mode
    {
        Scatter,
        Chase,
        Frightened,
        Eaten
    }

    //처음에 스케터로 바꿔줘야함
    [SerializeField]
    protected Mode nowMode = Mode.Scatter;
    protected Mode oldMode;
    [SerializeField]
    private float modeChangeTimer = 0;
    private int modeChangeNum = 1;

    protected GameObject player;

    #endregion

    // Start is called before the first frame update
    protected void Start()
    {
        
        SetDifficultyForStage(GameManager.stage);

        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        nowMode = Mode.Scatter;

        SetStartPosition();

        startWayPoint = nowWayPoint;

        if (isInGhostHouse)
        {
            direction = Vector2.up;
            nextWayPoint = nowWayPoint.neighborsList[0];
        }
        else
        {
            direction = Vector2.left;
            nextWayPoint = GetNextWayPoint();
        }

        oldWayPoint = nowWayPoint;

        UpdateAnimation();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (isCanMove)
        {
            ModeUpdate();

            Move();

            ReleaseGhost();

        }
    }

    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.CompareTag("Player"))
        {
            if (nowMode == Mode.Frightened)
            {
                Eaten();
            }
            else
            {
                if (nowMode != Mode.Eaten)
                {
                    GameManager.Inst.StartDeath();

                }
            }
        }
    }

    public void SetDifficultyForStage(int level)
    {
        
        GameSaveData.StageData tempStageData = StageDataManager.Inst.GetStageData(level);

        scatterModeTime1 = tempStageData.scatterModeTime1;
        scatterModeTime2 = tempStageData.scatterModeTime2;
        scatterModeTime3 = tempStageData.scatterModeTime3;

        chaseModeTime1 = tempStageData.chaseModeTime1;
        chaseModeTime2 = tempStageData.chaseModeTime2;

        frightenedModeTime = tempStageData.frightenedModeTimer;
        startBlinkTime = tempStageData.startBlinkingTime;

        moveSpeed = tempStageData.moveSpeed;
        normalMoveSpeed = tempStageData.normalMoveSpeed;
        frightenedMoveSpeed = tempStageData.frightenedMoveSpeed;
        eatenMoveSpeed = tempStageData.eatenMoveSpeed;

        //if (level == 1)
        //{
        //    scatterModeTime1 = 7;
        //    scatterModeTime2 = 7;
        //    scatterModeTime3 = 5;

        //    chaseModeTime1 = 20;
        //    chaseModeTime2 = 20;

        //    frightenedModeTime = 10;
        //    startBlinkTime = 7;

        //    moveSpeed = 5.9f;
        //    normalMoveSpeed = 5.9f;
        //    frightenedMoveSpeed = 2.9f;
        //    eatenMoveSpeed = 15f;
        //}
        //else if (level == 2)
        //{
        //    scatterModeTime1 = 7;
        //    scatterModeTime2 = 7;
        //    scatterModeTime3 = 5;

        //    chaseModeTime1 = 20;
        //    chaseModeTime2 = 1000;

        //    frightenedModeTime = 9;
        //    startBlinkTime = 6;

        //    moveSpeed = 6.9f;
        //    normalMoveSpeed = 6.9f;
        //    frightenedMoveSpeed = 3.9f;
        //    eatenMoveSpeed = 17f;
        //}
    }

    public void TranslateStartPosition()
    {
        transform.position = startWayPoint.transform.position;
    }

    public virtual void Restart()
    {
        isCanMove = true;
        SetDifficultyForStage(GameManager.stage);

        nowMode = Mode.Scatter;

        moveSpeed = normalMoveSpeed;
        oldMode = 0;


        ghostReleaseTimer = 0;
        modeChangeNum = 1;
        modeChangeTimer = 0;
        animator.SetBool("Frightened", false);
        animator.SetBool("Blink", false);
        animator.SetBool("Eaten", false);
        animator.enabled = true;

        Blinky tempBlinky = GetComponent<Blinky>();
        if (tempBlinky == null)
        {
            isInGhostHouse = true;
        }

        nowWayPoint = startWayPoint;
    }

    void CheckIsInGhostHouse()
    {
        if (nowMode == Mode.Eaten)
        {
            if (nowWayPoint.isGhostHouse)
            {
                moveSpeed = normalMoveSpeed;
                direction = Vector2.up;
                nextWayPoint = nowWayPoint.neighborsList[0];

                oldWayPoint = nowWayPoint;

                nowMode = Mode.Chase;


                animator.SetBool("Eaten", false);

                ghostReleaseTimer = ghostSelfReleaseTimer - 3;
                isInGhostHouse = true;
                UpdateAnimation();
            }
        }
    }

    void Move()
    {
        if (nextWayPoint != nowWayPoint && nextWayPoint != null && !isInGhostHouse)
        {
            if (PassedByNextWayPoint())
            {
                nowWayPoint = nextWayPoint;
                transform.localPosition = nowWayPoint.transform.position;

                CheckIsInGhostHouse();


                if (nowWayPoint.isPortal)
                {
                    transform.position = nowWayPoint.connectPortal.transform.position;
                    nowWayPoint = nowWayPoint.connectPortal;
                }
                if (nowWayPoint.isEntranceToGhostHouse && nowMode == Mode.Eaten)
                {
                    nextWayPoint = nowWayPoint.connectPortal;
                    direction = Vector2.down;
                    oldWayPoint = nowWayPoint;
                    nowWayPoint = null;
                }
                else if (nowWayPoint.isGhostHouse && nowMode != Mode.Eaten)
                {
                    nextWayPoint = nowWayPoint.neighborsList[0];
                    direction = nowWayPoint.validDirections[0];
                    oldWayPoint = nowWayPoint;
                    nowWayPoint = null;
                }
                else
                {
                    nextWayPoint = GetNextWayPoint();
                    oldWayPoint = nowWayPoint;
                    nowWayPoint = null;
                }
                UpdateAnimation();
            }
            else
            {
                transform.localPosition += (Vector3)direction * moveSpeed * Time.deltaTime;
            }
        }


    }

    void ReleaseGhost()
    {
        ghostReleaseTimer += Time.deltaTime;
        if (ghostReleaseTimer > ghostSelfReleaseTimer && isInGhostHouse)
        {
            isInGhostHouse = false;
        }
    }

    public void StartFrightenedMode()
    {
        if (nowMode != Mode.Eaten)
        {
            GameManager.Inst.ghostEatenScore = 200;

            frightenedModeTimer = 0;
            animator.SetBool("Blink", false);
            ChangeMode(Mode.Frightened);
        }
    }

    void ChangeMode(Mode _mode)
    {
        if (nowMode == Mode.Frightened)
        {
            moveSpeed = oldMoveSpeed;
        }

        if (_mode == Mode.Frightened)
        {
            oldMoveSpeed = moveSpeed;
            moveSpeed = frightenedMoveSpeed;
        }

        if (nowMode != _mode)
        {
            oldMode = nowMode;
            nowMode = _mode;
        }

        UpdateAnimation();

    }

    void Eaten()
    {
        GameManager.Inst.addScore(GameManager.Inst.ghostEatenScore);

        nowMode = Mode.Eaten;
        oldMoveSpeed = moveSpeed;
        moveSpeed = eatenMoveSpeed;
        animator.SetBool("Frightened", false);
        animator.SetBool("Blink", false);
        UpdateAnimation();

        GameManager.Inst.StartEaten(this);

        GameManager.Inst.ghostEatenScore *= 2;
        frightenedGhosts--;

        if (frightenedGhosts == 0)
            EndFrightenedMode();
    }

    void ModeUpdate()
    {
        if (nowMode != Mode.Frightened)
        {
            modeChangeTimer += Time.deltaTime;

            if (modeChangeNum == 1)
            {
                if (nowMode == Mode.Scatter && modeChangeTimer > scatterModeTime1)
                {
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }
                if (nowMode == Mode.Chase && modeChangeTimer > chaseModeTime1)
                {
                    modeChangeNum = 2;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                }
            }
            else if (modeChangeNum == 2)
            {
                if (nowMode == Mode.Scatter && modeChangeTimer > scatterModeTime2)
                {
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }
                if (nowMode == Mode.Chase && modeChangeTimer > chaseModeTime2)
                {
                    modeChangeNum = 3;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                }
            }
            else if (modeChangeNum == 3)
            {
                if (nowMode == Mode.Scatter && modeChangeTimer > scatterModeTime3)
                {
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }
            }
        }
        else if (nowMode == Mode.Frightened)
        {
            animator.SetBool("Frightened", true);
            frightenedModeTimer += Time.deltaTime;

            if (frightenedModeTimer >= startBlinkTime)
            {
                animator.SetBool("Blink", true);
            }

            if (frightenedModeTimer >= frightenedModeTime)
            {
                EndFrightenedMode();
                ChangeMode(oldMode);
            }
        }
    }

    void EndFrightenedMode()
    {
        frightenedGhosts = 0;

        animator.SetBool("Frightened", false);
        animator.SetBool("Blink", false);
        frightenedModeTimer = 0;

        GameManager.Inst.ghostEatenScore = 200;
    }

    protected WayPoint GetNextWayPoint()
    {
        Vector2 targetPosition = Vector2.zero;

        if (nowMode == Mode.Chase)
        {
            targetPosition = GetTargetPosition();
        }
        else if (nowMode == Mode.Scatter)
        {
            targetPosition = scatterWayPoint.transform.position;
        }
        else if (nowMode == Mode.Frightened)
        {
            targetPosition = GetRandomPosition();
        }
        else if (nowMode == Mode.Eaten)
        {
            targetPosition = ghostHouseWayPoint.transform.position;
        }

        //Debug
        debugTarget = targetPosition;

        WayPoint moveToNode = null;

        WayPoint[] foundWayPoints = new WayPoint[4];
        Vector2[] foundWayPointsDirection = new Vector2[4];

        int waypointCount = 0;

        for (int i = 0; i < nowWayPoint.neighborsList.Count; i++)
        {
            if (nowWayPoint.validDirections[i] != -direction)
            {
                if (nowMode != Mode.Eaten)
                {

                    foundWayPoints[waypointCount] = nowWayPoint.neighborsList[i];
                    foundWayPointsDirection[waypointCount] = nowWayPoint.validDirections[i];
                    waypointCount++;
                }
                else
                {
                    foundWayPoints[waypointCount] = nowWayPoint.neighborsList[i];
                    foundWayPointsDirection[waypointCount] = nowWayPoint.validDirections[i];
                    waypointCount++;
                }
            }
        }

        if (foundWayPoints.Length == 1)
        {
            moveToNode = foundWayPoints[0];
            direction = foundWayPointsDirection[0];
        }
        if (foundWayPoints.Length > 1)
        {
            float leastDistance = 100000f;

            for (int i = 0; i < foundWayPoints.Length; i++)
            {
                if (foundWayPointsDirection[i] != Vector2.zero)
                {
                    float distance = GetDistance(foundWayPoints[i].transform.position, targetPosition);

                    if (distance <= leastDistance)
                    {
                        leastDistance = distance;
                        moveToNode = foundWayPoints[i];
                        direction = foundWayPointsDirection[i];
                    }
                }
            }
        }
        return moveToNode;
    }

    virtual protected Vector2 GetTargetPosition()
    {
        return Vector2.zero;
    }

    MyTile GetTileAtPosition(Vector2 pos)
    {
        int tileX = Mathf.RoundToInt(pos.x - 0.5f);
        int tileY = Mathf.RoundToInt(pos.y - 0.5f);

        MyTile tile = TileManager.Inst.myTiles[tileY, tileX];
        if (tile != null)
        {
            return tile;
        }
        return null;
    }

    Vector2 GetRandomPosition()
    {
        int x = Random.Range(0, TileManager.Inst.TileWidth);
        int y = Random.Range(0, TileManager.Inst.TileHeight);

        return new Vector2(x + 0.5f, y + 0.5f);
    }

    protected void UpdateAnimation()
    {
        animator.SetInteger("DirY", (int)direction.y);
        animator.SetInteger("DirX", (int)direction.x);

        if (nowMode == Mode.Eaten)
        {
            animator.SetBool("Eaten", true);
        }
    }

    protected void SetStartPosition()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 0.2f, 1 << 8);

        if (hit != false)
        {
            if (hit.transform.CompareTag("WayPoint"))
            {
                nowWayPoint = hit.transform.GetComponent<WayPoint>();
            }
        }
        else
        {
            Debug.Log("SetFirstPacmanPosition() Error");
        }
        //맵
        //TileManager.Inst.myTiles[(int)transform.position.y, (int)transform.position.x]
        //transform.position
    }

    bool PassedByNextWayPoint()
    {
        float oldToNext = LengthFromOldWayPoint(nextWayPoint.transform.position);
        float oldToMonster = LengthFromOldWayPoint(transform.position);

        return oldToMonster > oldToNext;
    }

    float LengthFromOldWayPoint(Vector2 _target)
    {
        Vector2 result = _target - (Vector2)oldWayPoint.transform.position;
        return result.sqrMagnitude;
    }

    protected float GetDistance(Vector2 _posA, Vector2 _posB)
    {
        float dx = _posA.x - _posB.x;
        float dy = _posA.y - _posB.y;

        float distance = Mathf.Sqrt(dx * dx + dy * dy);
        return distance;
    }
}
