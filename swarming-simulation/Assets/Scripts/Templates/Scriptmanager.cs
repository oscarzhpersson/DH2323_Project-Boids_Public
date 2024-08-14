using System.Collections;
using UnityEngine;

public class Scriptmanager : MonoBehaviour {

    #region Events

    public delegate void ScriptManager_EventHandler();

    public event ScriptManager_EventHandler Event_Awake;
    public event ScriptManager_EventHandler Event_Start;
    public event ScriptManager_EventHandler Event_Update;
    public event ScriptManager_EventHandler Event_LateUpdate;
    public event ScriptManager_EventHandler Event_FixedUpdate;
    public event ScriptManager_EventHandler Event_OnDestroy;

    #endregion

    #region Unity Functions

    private void Awake()
    {
        if (Event_Awake != null)
        {
            Event_Awake();
        }
    }

    private void Start()
    {
        if (Event_Start != null)
        {
            Event_Start();
        }
    }

    private void Update()
    {
        if (Event_Update != null)
        {
            Event_Update();
        }
    }

    private void LateUpdate()
    {
        if (Event_LateUpdate != null)
        {
            Event_LateUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (Event_FixedUpdate != null)
        {
            Event_FixedUpdate();
        }
    }

    private void OnDestroy()
    {
        if (Event_OnDestroy != null)
        {
            Event_OnDestroy();
        }
    }

    #endregion

}
