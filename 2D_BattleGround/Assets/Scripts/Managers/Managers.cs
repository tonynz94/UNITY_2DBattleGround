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

    public static Managers Instance { get { return s_instance; } }
    public static ResourceManager Resource { get { Init();  return s_Resource; } }
    public static SoundManager Sound { get { Init(); return s_Sound; } }
    public static UIManager UI { get { Init(); return s_ui; } }
    public static SceneManagerEx Scene { get { Init(); return s_Scene; } }



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
        }
    }
}
