using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Template_BaseScript : MonoBehaviour
{
    #region References
    [BoxGroup("References", centerLabel: true)] [SerializeField] private Scriptmanager scriptmanager = null;
    #endregion

    #region Properties
    
    #endregion

    #region Events

    #endregion

    #region Debug

    #endregion

    #region Limits

    #endregion

    #region Private References
    private Transform m_Transform = null;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Functions

    private void Awake()
    {
        //Set up scriptmanager events.
        scriptmanager.Event_Start += LocalStart;
        scriptmanager.Event_Update += LocalUpdate;

        //Set up references.
        m_Transform = this.transform;
    }

    #endregion

    #region Functions

    private void LocalStart()
    {

    }

    private void LocalUpdate()
    {

    }

    #endregion

    #region Accessors

    #endregion

    #region Enumerators

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {

        if (!Application.isPlaying || !Application.isEditor) return;

    }

    #endregion
}
