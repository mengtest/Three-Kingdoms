using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private static Dictionary<string, GameState> m_GameStateMap = null;
    private static GameState m_CurState = null;

    void Start()
    {
        m_GameStateMap = new Dictionary<string, GameState>();
        m_CurState = null;
        LoadScene(1);
    }

    private static void SetState(GameState state)
    {
        if (state == null)
        {
            return;
        }
        if (state != m_CurState && m_CurState != null)
        {
            m_CurState.Stop();
        }
        m_CurState = state;
        m_CurState.Start();
    }

    public static void LoadScene(int sceneId)
    {
        SceneData data = DataManager.s_SceneDataManager.GetData(sceneId);

        if (data == null)
        {
            Debug.LogError("Init SceneData is null, id:" + sceneId);
            return;
        }

        GameState state = null;
        if (!m_GameStateMap.TryGetValue(data.GameState, out state))
        {
            state = Assembly.GetExecutingAssembly().CreateInstance(data.GameState) as GameState;
            if (state == null)
            {
                Debug.LogError("Scene state is error" + data.GameState);
                return;
            }
            m_GameStateMap.Add(data.GameState, state);
        }
        SetState(state);

        // 状态设置完毕 开始load场景
        DownloadManager.Instance.LoadScene(data.LevelName, state.LoadComplete);
    }
}
