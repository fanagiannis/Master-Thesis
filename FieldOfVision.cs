using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FieldOfVision : MonoBehaviour
{
    //[SerializeField]private UnityEvent spotPlayer;
    [Header("FOV Customization")]
    public float viewRadius=50f;
    public float viewAngle=150f;
    [Header("Masks")]
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    [Header("Timers")]
    [SerializeField]private float timer=0f;
    [SerializeField]private float resetTime=10f;
    [Header("Visible Targets")]
    public List<Transform> visibleTargets = new List<Transform>();
    public Agent agent;
    
    void Awake()
    {
        agent = GetComponent<Agent>();
    }
    void Update()
    {
        FindTargets();
    }
    public void FindTargets()
    {
        ClearTargets();
        Collider [] targetsInView = Physics.OverlapSphere(transform.position,viewRadius,targetMask);
        for(int i=0;i<targetsInView.Length;i++)
        {
            Transform target = targetsInView[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask)&&target.gameObject.activeSelf)
                {
                    if(!visibleTargets.Contains(target))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);

        Gizmos.color = Color.blue;
        foreach (Transform visibleTarget in visibleTargets)
        {
            Gizmos.DrawLine(transform.position, visibleTarget.position);
        }
    }

    public void ClearTargets()
    {
        timer+=Time.deltaTime;
        if(timer>=resetTime)
        {
            visibleTargets.Clear();
            timer=0f;
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public bool ActiveVisibleTarget()
    {
        return visibleTargets.Count>0;
    }

    public Transform GetVisibleTarget()
    {
        if (visibleTargets.Count>0)
        {
            return visibleTargets[visibleTargets.Count-1];
        }
        else{
            return null;
        }
    }
    public void Debugger(string ex)
    {
        Debug.Log(ex);
    }
}
