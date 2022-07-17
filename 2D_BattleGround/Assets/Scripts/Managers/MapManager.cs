using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager
{
	Dictionary<int, List<List<int>>> _mapInfoDic = new Dictionary<int, List<List<int>>>();
	Dictionary<Define.MapType, Sprite> _mapSprite = new Dictionary<Define.MapType, Sprite>();
	public Grid CurrentGrid { get; private set; }
	int _currentMapID;

	public int MinX { get; set; }
	public int MaxX { get; set; }
	public int MinY { get; set; }
	public int MaxY { get; set; }

	public void init()
    {
		Sprite treeMap = Managers.Resource.Load<Sprite>("Sprites/Tree 1");
		_mapSprite.Add(Define.MapType.TreeMap, treeMap);
		Sprite lakeMap = Managers.Resource.Load<Sprite>("Sprites/Water 4");
		_mapSprite.Add(Define.MapType.LakeMap, lakeMap);
		Sprite grassMap = Managers.Resource.Load<Sprite>("Sprites/TX Tileset Grass 13");
		_mapSprite.Add(Define.MapType.GrassMap, grassMap);

	}

    public void LoadMap(int mapId)
    {
		_currentMapID = mapId;
		string mapName = "Map_" + mapId.ToString("00"); 
		GameObject go = Managers.Resource.Instantiate($"Map/{mapName}");
		go.name = "Map";
		CurrentGrid = go.GetComponent<Grid>();
		//배경
		Tilemap tileBase = Utils.FindChild(go, "Tilemap_Base", false).GetComponent<Tilemap>();
		//충돌용 
		Tilemap tileCol = Utils.FindChild(go, "Tilemap_Collision", false).GetComponent<Tilemap>();

		if (tileCol != null)
		{
			tileCol.gameObject.SetActive(false);
		}

		MaxX = (tileBase.cellBounds.xMax);
		MinX = (tileBase.cellBounds.xMin);

		MaxY = (tileBase.cellBounds.yMax);
		MinY = (tileBase.cellBounds.yMin);

		List<List<int>> mapBin = new List<List<int>>();

		for(int y = MaxY; y >= MinY; y--)
		{
			List<int> temp = new List<int>();
			for(int x = MinX; x <= MaxX; x++)
			{
				TileBase tile = tileCol.GetTile(new Vector3Int(x,y,0));
				temp.Add(tile == null ? 0 : 1);
			}
			mapBin.Add(temp);
		}

		_mapInfoDic[mapId] = mapBin;
	}

	public bool CanGo(Vector3Int cellPos)
	{
		if (cellPos.x < MinX || cellPos.x > MaxX)
			return false;
		if (cellPos.y < MinY || cellPos.y > MaxY)
			return false;

		int x = cellPos.x - (MinX);
		int y = MaxY - cellPos.y;
		List<List<int>> mapTemp =_mapInfoDic[_currentMapID];
		return mapTemp[y][x]== 0 ? true : false;
	}

	public void DestroyMap()
	{
		GameObject map = GameObject.Find("Map");
		if (map != null)
		{
			GameObject.Destroy(map);
		}
	}

	public Sprite GetMapSprite(Define.MapType maptype)
	{
		Sprite spr = null;
		_mapSprite.TryGetValue(maptype, out spr);
		return spr;
	}
}
