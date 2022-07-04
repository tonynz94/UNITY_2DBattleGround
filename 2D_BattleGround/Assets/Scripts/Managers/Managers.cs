using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers s_instance;
    public static ResourceManager s_Resource = new ResourceManager();
    public static SoundManager s_Sound = new SoundManager();
    public static UIManager s_ui = new UIManager();
    public static SceneManagerEx s_Scene = new SceneManagerEx();
    public static DataManager s_Data = new DataManager();
    public static ObjectManager s_Object = new ObjectManager();
    public static PlayerManager s_Player = new PlayerManager();
    public static MapManager s_Map = new MapManager();

    public static Managers Instance { get { return s_instance; } }

    public static DataManager Data { get { Init(); return s_Data; } }
    public static ResourceManager Resource { get { Init();  return s_Resource; } }
    public static SoundManager Sound { get { Init(); return s_Sound; } }
    public static UIManager UI { get { Init(); return s_ui; } }
    public static SceneManagerEx Scene { get { Init(); return s_Scene; } }
    public static PlayerManager Player{ get { Init(); return s_Player; } }
    public static MapManager Map { get { Init(); return s_Map; } }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
                go = new GameObject() { name = "@Managers" };

            s_instance = go.AddComponent<Managers>();
            DontDestroyOnLoad(go);
            s_Resource.Init();
            s_Scene.Init();
            s_Data.Init();
            s_Object.Init();
            s_Map.init();
        }
    }
}
