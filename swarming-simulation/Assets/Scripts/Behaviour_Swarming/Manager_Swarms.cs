using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Manager_Swarms : MonoBehaviour
{
    #region References
    [BoxGroup("References", centerLabel: true)] [SerializeField] private Scriptmanager scriptmanager = null;

    [BoxGroup("Prefabs", centerLabel: true)] [SerializeField] private GameObject boidPrefab = null;
    #endregion

    #region Properties
    [BoxGroup("Properties", centerLabel: true)] [SerializeField] private int amount = 0;
    [BoxGroup("Properties", centerLabel: true)] [SerializeField] private Vector3 spawnVolume = new Vector3();
    [BoxGroup("Properties", centerLabel: true)] [SerializeField] private Vector3 boundingVolume = new Vector3();

    [BoxGroup("Boid Behaviour", centerLabel: true)] [MinMaxSlider(0, 30)] public Vector2 speedRange = new Vector2();
    [BoxGroup("Boid Behaviour", centerLabel: true)] [MinMaxSlider(0, 50)] public Vector2 neighbourRange = new Vector2();
    [BoxGroup("Boid Behaviour", centerLabel: true)] public float rotationSpeed = 0.0f;
    [BoxGroup("Boid Behaviour", centerLabel: true)] public int swarmThreshold = 0;
    [BoxGroup("Boid Behaviour", centerLabel: true)] public bool boundSwarm = true;

    [BoxGroup("Targets", centerLabel: true)] public Transform target = null;
    [BoxGroup("Targets", centerLabel: true)] public Transform boundingBox_Origin = null;
    #endregion

    #region Events

    #endregion

    #region Debug

    #endregion

    #region Limits

    #endregion

    #region Private References
    private Transform m_Transform = null;

    private GameObject[] swarm;
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

        swarm = new GameObject[amount];

        for (int i = 0; i < amount; i++)
        {
            float x = Random.Range(-spawnVolume.x, spawnVolume.x);
            float y = Random.Range(-spawnVolume.y, spawnVolume.y);
            float z = Random.Range(-spawnVolume.z, spawnVolume.z);

            Vector3 spawnPosition = new Vector3(x, y, z);

            swarm[i] = (GameObject) Instantiate(boidPrefab, spawnPosition, Quaternion.identity);

            swarm[i].GetComponent<Swarm>().accessScriptmanager = scriptmanager;
            swarm[i].GetComponent<Swarm>().accessManager = this;
        }
    }

    private void LocalUpdate()
    {

        /*if (Random.Range(0, 1) < 0.01f)
        {
            float x = Random.Range(-boundingVolume.x, boundingVolume.x);
            float y = Random.Range(-boundingVolume.y, boundingVolume.y);
            float z = Random.Range(-boundingVolume.z, boundingVolume.z);

            target.position = new Vector3(x, y, z);
        }*/

    }

    #endregion

    #region Accessors

    public GameObject[] access_Swarm
    {
        get { return swarm; }
        set { swarm = value; }
    }

    public Vector3 access_BoundingVolume
    {
        get { return boundingVolume; }
        set { boundingVolume = value; }
    }

    #endregion

    #region Enumerators

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {

        if (!Application.isPlaying || !Application.isEditor) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(m_Transform.position, boundingVolume * 2);

        if (target != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(m_Transform.position, .25f);
        }

    }

    #endregion
}
