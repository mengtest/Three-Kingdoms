using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryOnMe : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
