using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DownloadManager : MonoBehaviour
{
    private static DownloadManager _Instance;

    public static DownloadManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType(typeof(DownloadManager)) as DownloadManager;
            }
            return _Instance;
        }
    }

    public delegate void LoadCallback(params object[] args);
    public void LoadScene(string name, LoadCallback loadHandler, params object[] args)
    {
        StartCoroutine(LoadSceneBundle(name, loadHandler, args));
    }

    private IEnumerator LoadSceneBundle(string name, LoadCallback loadHandler, params object[] args)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(name);
        yield return async;

        Resources.UnloadUnusedAssets();
        GC.Collect();

        if (loadHandler != null)
        {
            loadHandler(args);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
