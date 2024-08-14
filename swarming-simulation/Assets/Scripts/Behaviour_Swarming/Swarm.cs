using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Swarm : MonoBehaviour
{
    #region References
    [BoxGroup("References", centerLabel: true)] [SerializeField] private Scriptmanager scriptmanager = null;

    [BoxGroup("References", centerLabel: true)] [SerializeField] private Manager_Swarms manager = null;
    #endregion

    #region Properties
    [BoxGroup("Properties", centerLabel: true)] [SerializeField] private float speed = 0;
    [BoxGroup("Properties", centerLabel: true)] [SerializeField] private float lineOfSight = 1;
    #endregion

    #region Events

    #endregion

    #region Debug

    #endregion

    #region Limits

    #endregion

    #region Private References
    private Transform m_Transform = null;
    private List<Transform> swarmNeighbours = new List<Transform>();
    private Transform m_Target = null;
    private Transform m_boundingBoxOrigin = null;
    #endregion

    #region Private Variables

    #endregion

    #region Unity Functions

    private void Start()
    {
        //Set up scriptmanager events.
        scriptmanager.Event_Update += LocalUpdate;

        //Set up references.
        m_Transform = this.transform;
        m_Target = manager.target;
        m_boundingBoxOrigin = manager.boundingBox_Origin;

        speed = Random.Range(manager.speedRange.x, manager.speedRange.y);
    }

    #endregion

    #region Functions

    private void LocalUpdate()
    {

        Bounds bounds = new Bounds(m_boundingBoxOrigin.position, manager.access_BoundingVolume * 2);
        RaycastHit hit = new RaycastHit();
        Vector3 swarm_Direction = Vector3.zero;

        swarm_Direction = Physics.Raycast(m_Transform.position, m_Transform.forward, out hit, lineOfSight) ? Vector3.Reflect(m_Transform.forward, hit.normal) : m_boundingBoxOrigin.position - m_Transform.position;

        if (Physics.Raycast(m_Transform.position, m_Transform.forward, out hit, lineOfSight))
        {
            swarm_Direction = Vector3.Reflect(m_Transform.forward, hit.normal);
            m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, Quaternion.LookRotation(swarm_Direction), manager.rotationSpeed * Time.deltaTime);
        }
        else if(!bounds.Contains(m_Transform.position) && manager.boundSwarm)
        {
            swarm_Direction = m_boundingBoxOrigin.position - m_Transform.position;
            m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, Quaternion.LookRotation(swarm_Direction), manager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            SwarmingBehaviour_Default();
        }

        if(Random.Range(0, 1) < 0.1f)
            speed = Random.Range(manager.speedRange.x, manager.speedRange.y);

        m_Transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void SwarmingBehaviour_Default()
    {
        GameObject[] m_Swarm = manager.access_Swarm;
        swarmNeighbours = new List<Transform>();

        Vector3 swarm_VectorCentre = Vector3.zero;
        Vector3 swarm_VectorAvoid = Vector3.zero;

        float swarm_Speed = 0.1f;
        float swarm_Distance = 0.0f;
        int swarm_Size = 0;

        foreach (GameObject boid in m_Swarm)
        {

            if (boid == this.gameObject) continue;

            // Manual distance calculation, for some reason.
            swarm_Distance = Mathf.Abs( Mathf.Sqrt( 
                Mathf.Pow(boid.transform.position.x - m_Transform.position.x, 2 ) +
                Mathf.Pow(boid.transform.position.y - m_Transform.position.y, 2 ) +
                Mathf.Pow(boid.transform.position.z - m_Transform.position.z, 2 )
             ));

            bool m_Swarm_Belonging = swarm_Distance < manager.neighbourRange.y;

            if (!m_Swarm_Belonging) continue;

            swarmNeighbours.Add(boid.transform);

            swarm_VectorCentre += boid.transform.position;
            swarm_Size++;

            bool m_Swarm_Avoidance = swarm_Distance < manager.neighbourRange.x;

            if (m_Swarm_Avoidance)
            {
                swarm_VectorAvoid += (m_Transform.position - boid.transform.position);
            }

            swarm_Speed += boid.GetComponent<Swarm>().getSpeed;

        }

        if (swarm_Size < manager.swarmThreshold) return;

        swarm_VectorCentre /= swarm_Size;
        swarm_VectorCentre += (m_Target.position - m_Transform.position);

        swarm_Speed /= swarm_Size;

        Vector3 swarm_Direction = (swarm_VectorCentre + swarm_VectorAvoid) - m_Transform.position;

        if (swarm_Direction != Vector3.zero)
        {
            m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, Quaternion.LookRotation(swarm_Direction), manager.rotationSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region Accessors

    public float getSpeed{ get{ return speed; }}

    public Manager_Swarms accessManager
    { 
        get { return manager; }
        set { manager = value; }
    }

    public Scriptmanager accessScriptmanager
    { 
        get { return scriptmanager; }
        set { scriptmanager = value; }
    }

    #endregion

    #region Enumerators

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {

        if (!Application.isPlaying || !Application.isEditor) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_Transform.position, manager.neighbourRange.y);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_Transform.position, manager.neighbourRange.x);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_Transform.position, m_Transform.position + m_Transform.forward * lineOfSight);
        Gizmos.DrawLine(m_Transform.position, m_Target.position);

        Gizmos.DrawWireCube(m_boundingBoxOrigin.position, manager.access_BoundingVolume * 2);

        Gizmos.color = Color.green;
        foreach(Transform boid in swarmNeighbours)
        {
            Gizmos.DrawLine(m_Transform.position, boid.position);
        }
    }

    #endregion
}
