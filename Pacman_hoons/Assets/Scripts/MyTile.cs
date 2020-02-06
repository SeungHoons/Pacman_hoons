using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MyTile 
{

	public enum TileState
	{
		Wall,
		OutSideWall,
		Road
	}

	public bool isPortal = false;
    public bool isWayPoint = false;
    //웨이폰인트로 옮겼는데 다시보고 지울것
    public bool isEntranceToGhostHouse = false;

    public GameObject WayPoint;

	public TileState tileState { get; set; }

	public void Init()
	{
		if (tileState == TileState.Road)
		{

		}
	}
}
