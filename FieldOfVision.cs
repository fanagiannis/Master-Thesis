using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FieldOfVision : MonoBehaviour
{
    [SerializeField]private UnityEvent spotPlayer;
    public float viewRadius=50f;
    public float viewAngle=150f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public List<Transform> visibleTargets = new List<Transform>();
    void Update()
    {
        FindTargets();
    }
    public void FindTargets()
    {
        visibleTargets.Clear();
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
                    spotPlayer.Invoke();
                    visibleTargets.Add(target);  
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

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public Transform GetVisibleTarget(string searchTag)
    {
        return null;
        // // Transform visibleFood = null;
        // Transform visibleTarget = null;
        // foreach(Transform target in visibleTargets)
        // {
        //     if(target.tag == "Player")
        //     {
                
        //     }
            
            
        //     // if(target.tag == "Threat")
        //     // {
        //     //     visibleThreat=target;
        //     // }
        //     // else if(target.tag == "Food")
        //     // {
        //     //     visibleFood=target;
        //     // }
            
        // }
        // return visibleTarget;
        
        // //return visibleTarget!= null ? visibleThreat : visibleFood;
        
    }
    public void Debugger(string ex)
    {
        Debug.Log(ex);
    }
}
