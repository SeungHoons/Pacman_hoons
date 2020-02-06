using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 oldDirection;
    [SerializeField]
    private Vector2 direction;
	[SerializeField]
	private Vector2 nextDirection;

	[SerializeField]
	private WayPoint nextWayPoint;
	[SerializeField]
	private WayPoint nowWayPoint;
	[SerializeField]
	private WayPoint oldWayPoint;

    private WayPoint startWayPoint;

	[SerializeField]
	private float speed = 6.0f;

	private Animator animator;

    public bool isCanMove;

	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
        //isCanMove = true;

		SetStartPacmanPosition();

		direction = Vector2.left;
		ChangeDirection(direction);
		UpdateDirection();

        SetDifficultyForStage(GameManager.stage);
	}


	// Update is called once per frame
	void Update()
	{
        if(isCanMove)
        {
            InputKeyboard();
            //processMobileInput();
            Move();
            UpdateDirection();
            UpdateAnimationState();
        }
	}


    //바꿀껏 타일에 대한 정보로 수정
	void SetStartPacmanPosition()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 0.2f, 1 << 8);

		if (hit != false)
		{
			if (hit.transform.CompareTag("WayPoint"))
			{
				nowWayPoint = hit.transform.GetComponent<WayPoint>();
                startWayPoint = nowWayPoint;
            }
		}
		else
		{
			Debug.Log("SetFirstPacmanPosition() Error");
		}
	}

    public void TranslateStartPosition()
    {
        transform.position = startWayPoint.transform.position;
    }

    public void SetDifficultyForStage (int _stage)
    {
        if(_stage==2)
        {
            speed = 7;
        }
    }

    public void UpdateAnimationState()
	{
		if (direction == Vector2.zero)
		{
			animator.SetBool("Idle", true);
		}
		else
		{
			animator.SetBool("Idle", false);
		}
	}

	public void ReStartGame()
	{
        isCanMove = true;

        nowWayPoint = startWayPoint;

        direction = Vector2.left;
        oldDirection = Vector2.left;
        nextDirection = Vector2.left;

        animator.enabled = true;
        ChangeDirection(direction);
        UpdateDirection();
    }

	void Move()
	{
		if (nextWayPoint != nowWayPoint && nextWayPoint != null)
		{
			if (nextDirection == -direction)
			{
				direction *= -1;
				WayPoint tempPoint = nextWayPoint;
				nextWayPoint = oldWayPoint;
				oldWayPoint = tempPoint;
			}

			if (PassedByNextWayPoint())
			{
				nowWayPoint = nextWayPoint;
				transform.position = nowWayPoint.transform.position;

				if (nowWayPoint.isPortal)
				{
					transform.position = nowWayPoint.connectPortal.transform.position;
					nowWayPoint = nowWayPoint.connectPortal;
				}

				WayPoint moveToWayPoint = CheckCanMove(nextDirection);

				if (moveToWayPoint != null)
					direction = nextDirection;

				if (moveToWayPoint == null)
					moveToWayPoint = CheckCanMove(direction);

				if (moveToWayPoint != null)
				{
					nextWayPoint = moveToWayPoint;
					oldWayPoint = nowWayPoint;
					nowWayPoint = null;
				}
				else
				{
					direction = Vector2.zero;
				}
			}
			else
			{
				transform.localPosition += (Vector3)direction * speed * Time.deltaTime;
			}
		}
	}

	WayPoint CheckCanMove(Vector2 _dir)
	{
		WayPoint moveToWayPoint = null;
		for (int i = 0; i < nowWayPoint.validDirections.Count; i++)
		{
			if (nowWayPoint.validDirections[i] == _dir)
			{
				moveToWayPoint = nowWayPoint.neighborsList[i];
				break;
			}
		}
		return moveToWayPoint;
	}

	void InputKeyboard()
	{
		if (Input.GetKeyDown("up"))
		{
			ChangeDirection(Vector2.up);
		}
		else if (Input.GetKeyDown("down"))
		{
			ChangeDirection(Vector2.down);
		}
		else if (Input.GetKeyDown("left"))
		{
			ChangeDirection(Vector2.left);
		}
		else if (Input.GetKeyDown("right"))
		{
			ChangeDirection(Vector2.right);
		}
		else if(Input.GetKeyDown("q"))
		{
			speed = 16;
		}
	}


    public void PressKey(int nKey)
    {
        switch (nKey)
        {
            case 1: //up
                ChangeDirection(Vector2.up);

                break;
            case 2: //down
                ChangeDirection(Vector2.down);

                break;
            case 3: //left
                ChangeDirection(Vector2.left);

                break;
            case 4: //rigth
                ChangeDirection(Vector2.right);

                break;

        }

    }
    //Vector2 touchStart;
    //Vector2 touchend;
    //bool nowtouch= false;
    //Vector2 touchDir;

    //void touchTest()
    //{
    //    if(Input.touchCount>0)
    //    {
    //        touchStart = Input.GetTouch(0).position;
    //        nowtouch = true;
    //    }

    //    if(nowtouch && Input.touchCount == 0)
    //    {
    //        nowtouch = false;
    //        touchend = Input.GetTouch(0).deltaPosition;
    //        touchDir = (touchend - touchStart).normalized;
    //    }
    //    if(touchDir.x)

    //}



    void ChangeDirection(Vector2 _dir)
	{
		if (_dir != direction)
			nextDirection = _dir;

		if (nowWayPoint != null)
		{
			WayPoint moveToWayPoint = CheckCanMove(_dir);

			if (moveToWayPoint != null)
			{
				direction = _dir;
				nextWayPoint = moveToWayPoint;
				oldWayPoint = nowWayPoint;
				nowWayPoint = null;
			}
		}
	}

	void UpdateDirection()
	{
		if (direction == Vector2.up)
		{
            oldDirection = Vector2.up;
			transform.localRotation = Quaternion.Euler(0, 0, 270);
		}
		else if (direction == Vector2.down)
		{
            oldDirection = Vector2.down;
            transform.localRotation = Quaternion.Euler(0, 0, 90);
		}
		else if (direction == Vector2.left)
		{
            oldDirection = Vector2.left;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
		}
		else if (direction == Vector2.right)
		{
            oldDirection = Vector2.right;
            transform.localRotation = Quaternion.Euler(0, 0, 180);
		}
	}

	bool PassedByNextWayPoint()
	{
		float oldToNext = LengthFromOldWayPoint(nextWayPoint.transform.position);
		float oldToPlayer = LengthFromOldWayPoint(transform.position);

		return oldToPlayer > oldToNext;
	}

	float LengthFromOldWayPoint(Vector2 _target)
	{
		Vector2 result = _target - (Vector2)oldWayPoint.transform.position;
		return result.sqrMagnitude;
	}
}
