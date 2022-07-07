using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
	[SerializeField]
	Grid _currentGrid;

    public void init()
    {

    }

    public void LoadMap(int mapId)
    {
		string mapName = "Map_" + mapId.ToString("00"); 
		GameObject go = Managers.Resource.Instantiate($"Map/{mapName}");
		go.name = "Map";

		GameObject col = Utils.FindChild(go, "Tilemap_Collision", false);
		if (col != null)
		{
			col.SetActive(false);
		}
    }

	public void DestroyMap()
	{
		GameObject map = GameObject.Find("Map");
		if (map != null)
		{
			GameObject.Destroy(map);
			_currentGrid = null;
		}
	}
}
