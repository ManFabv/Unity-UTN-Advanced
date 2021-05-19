using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static string NextLevel = string.Empty;
    public static string CurrentLevel = string.Empty;

    private static string GameObjectName = "LevelManager";

    private static LevelManager m_Instance;
    public static LevelManager Instance 
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject manager = GameObject.Find(GameObjectName);
                if (manager != null)
                    m_Instance = manager.GetComponent<LevelManager>();
                
                if(m_Instance == null)
                    m_Instance = new GameObject("LevelManager").AddComponent<LevelManager>();
            }
            DontDestroyOnLoad(m_Instance);
            return m_Instance;
        }
    }
}