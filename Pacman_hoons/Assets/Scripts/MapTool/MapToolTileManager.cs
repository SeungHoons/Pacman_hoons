using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class MapToolTileManager : Singletone<MapToolTileManager>
{
    public Tilemap tileMap;
    public RuleTile _ruleTile;
    public RuleTileEditor _ruleTileEditor;

    private Dictionary<string,int> _mosnterHousePosition;

    private void OnEnable()
    {
        MonsterHousePositionInit();
    }


    private void MonsterHousePositionInit()
    {
        _mosnterHousePosition.Add("MinX", 10);
        _mosnterHousePosition.Add("MinY", 14);
        _mosnterHousePosition.Add("MaxX", 17);
        _mosnterHousePosition.Add("MaxY", 18);
    }

    private void NewMap()
    {
        TileBase nowTile;
        for (int i = -1; i<TileManager.Inst.TileHeight + 1; i++)
        {
            nowTile = tileMap.GetTile(new Vector3Int(0, i, 0));
            //_ruleTileEditor.ContainsMousePosition
            //nowTile.GetTileData
        }
    }
    
}
