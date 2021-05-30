using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Instance_Debug : MonoBehaviour
{
    public static Tutorial_Instance_Debug instance;

    [SerializeField] bool isTutorial;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        isTutorial = false;
    }

    public void setTutorial(bool setVal)
    {
        isTutorial = setVal;
    }

    public bool getTutorialVal()
    {
        return isTutorial;
    }
}
