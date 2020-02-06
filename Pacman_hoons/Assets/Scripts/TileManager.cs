using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileManager : Singletone<TileManager>
{
    [SerializeField]
    Tilemap myTileMap;

    [SerializeField]
    private GameObject walls;
    [SerializeField]
    private GameObject wall;
    [SerializeField]
    private GameObject pellets = null;
    [SerializeField]
    private GameObject normalPellet = null;
    [SerializeField]
    private GameObject bigPellet = null;

    public MyTile[,] myTiles;

    public int TileWidth{ get; }  = 28;
    public int TileHeight { get; } = 31;

    private void Awake()
    {
        myTiles = new MyTile[TileHeight, TileWidth];
        InitTiles();
        CheckTileWayPoint();
    }

    void InitTiles()
    {
        for (int i = 0; i < TileHeight; i++)
        {
            for (int j = 0; j < TileWidth; j++)
            {
                TileBase nowTile;
                nowTile = myTileMap.GetTile(new Vector3Int(j, i, 0));
                myTiles[i, j] = new MyTile();

                string tempString = nowTile.name;
                if (nowTile.name == "tex_tileset_original_34")
                {
                    myTiles[i, j].tileState = MyTile.TileState.Road;
                    //hardcording 
                    if((i == 7 && j == 1) || (i == 7 && j == 26) || (i == 26 && j == 1) || (i == 26 && j == 26))
                    {
                        GameObject temp;
                        temp = Instantiate(bigPellet, new Vector3(j + 0.5f, i + 0.5f, 0), this.transform.rotation, pellets.transform);
                        GameManager.Inst.totalPellet++;
                        GameManager.Inst.allPellet.Insert(GameManager.Inst.allPellet.Count, temp);
                    }
                    else
                    {
                        if(j >= 9 && i >=13 && j<=18 && i<=19)
                        {

                        }
                        else
                        {
                            GameObject temp;
                            temp = Instantiate(normalPellet, new Vector3(j + 0.5f, i + 0.5f, 0), this.transform.rotation, pellets.transform);
                            GameManager.Inst.totalPellet++;
                            GameManager.Inst.allPellet.Insert(GameManager.Inst.allPellet.Count, temp);

                        }
                    }
                    if ((i == 16 && j == 0) || (i == 16 && j == 27))
                    {
                        myTiles[i, j].isPortal = true;
                    }
                }
                else
                {
                    myTiles[i, j].tileState = MyTile.TileState.Wall;
                    //이걸 만드는 이유가 레이로 쏴서 waypoint가 맞거나 벽에 맞을 때까지 가서 멈춘다. 그것 위한 콜라이더 였음
                    Instantiate(wall, new Vector3(j + 0.5f, i + 0.5f, 0), this.transform.rotation, walls.transform);
                }
            }
        }
    }

    //일단 지금필요한건 웨이포인트 위치. (연결은 아직 레이케스트)
    void CheckTileWayPoint()
    {
        bool upDown = false;
        bool leftRight = false;
        for (int i = 0; i < TileHeight; i++)
        {
            for (int j = 0; j < TileWidth; j++)
            {
                if (myTiles[i, j].tileState == MyTile.TileState.Road)
                {
                    upDown = false;
                    leftRight = false;

                    //상
                    if (i != TileHeight - 1)
                    {
                        if (myTiles[i + 1, j].tileState == MyTile.TileState.Road)
                        {
                            upDown = true;
                        }
                    }
                    //하
                    if (i != 0)
                    {
                        if (myTiles[i - 1, j].tileState == MyTile.TileState.Road)
                        {
                            upDown = true;
                        }
                    }

                    //좌
                    if (j != 0)
                    {
                        if(myTiles[i, j-1].tileState == MyTile.TileState.Road)
                        {
                            leftRight = true;
                        }
                    }

                    //우
                    if (j != TileWidth - 1)
                    {
                        if (myTiles[i,j+1].tileState == MyTile.TileState.Road)
                        {
                            leftRight = true;
                        }
                    }

                    if(leftRight && upDown)
                    {
                        myTiles[i, j].isWayPoint = true;
                        //여기서 웨이포인트까지 생성
                        //인스턴스후
                        //myTiles[i,j].WayPoint = 
                    }
                }
            }
        }
    }

    public Tilemap GetTilemap()
    {
        return myTileMap;
    }
}
