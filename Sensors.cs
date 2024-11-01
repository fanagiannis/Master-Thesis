using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class AISensors : MonoBehaviour
{
    //[SerializeField]private UnityEvent spotPlayer;
    [Header("Gizmo Colors")]
    [SerializeField]private Color fieldOfVisionColor;
    [SerializeField]private Color hearingRadiusColor;
    [Header("Vision Cone")]
    public float viewRadius=50f;
    public float viewAngle=150f;

    [Header("Sound Cone")]
    public float hearingRadius;
    //public float hearingAngle=150f;
    [Header("Masks")]
    public LayerMask visibleTargetMask;
    public LayerMask soundSourceMask;
    public LayerMask obstacleMask;
    [Header("Timers")]
    [SerializeField]private float timer=0f;
    [SerializeField]private float resetTime=10f;
    [Header("Visible Targets")]
    public List<Transform> visibleTargets = new List<Transform>();
    public List<Transform> detectedSoundSources = new List<Transform>();
    public Transform test;
    public Agent agent;
    
    void Awake()
    {
        agent = GetComponent<Agent>();
        
    }
    void Update()
    {
        AudioCone();
        VisionCone();
        if(detectedSoundSources.Count>0)
        {
            test=FindSoundSource();
        }
        
    }
    public void VisionCone()
    {
        ClearTargets(visibleTargets);
        Collider [] targetsInView = Physics.OverlapSphere(transform.position,viewRadius,visibleTargetMask);
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

    public void AudioCone()
    {
        ClearTargets(detectedSoundSources);   
        Collider[] sourcesHeared = Physics.OverlapSphere(transform.position, hearingRadius, soundSourceMask);

        for (int i = 0; i < sourcesHeared.Length; i++)
        {
            Transform target = sourcesHeared[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask) && target.gameObject.activeSelf)
            {
                if (!detectedSoundSources.Contains(target))
                {
                    detectedSoundSources.Add(target);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawWireSphere(transform.position, hearingRadius);

        Handles.color = hearingRadiusColor;
        Handles.DrawSolidDisc(transform.position,Vector3.up, hearingRadius);
        Handles.color = fieldOfVisionColor;
        Handles.DrawSolidArc(transform.position, Vector3.up,DirFromAngle(-viewAngle / 2, false), viewAngle, viewRadius);
        Handles.Label(transform.position+new Vector3(0,0,-7),"FieldOfHearing");
        Handles.Label(transform.position+new Vector3(0,0,3),"FieldOfVision");

        Gizmos.color = Color.blue;
        foreach (Transform visibleTarget in visibleTargets)
        {
            Gizmos.DrawLine(transform.position, visibleTarget.position);
        }
        Gizmos.color = Color.white;
        foreach (Transform hearedSource in detectedSoundSources)
        {
            Gizmos.DrawLine(transform.position, hearedSource.position);
        }
    }

    public void ClearTargets(List<Transform> list)
    {
        timer+=Time.deltaTime;
        if(timer>=resetTime)
        {
            list.Clear();
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
    public Transform FindSoundSource()
    {
        if (detectedSoundSources.Count == 0)
        {
            return null; 
        }

        Transform closestSource = detectedSoundSources[0];
        float closestDistance = Vector3.Distance(transform.position, closestSource.position);

        for (int i = 1; i < detectedSoundSources.Count; i++)
        {
            Transform currentSource = detectedSoundSources[i];
            float currentDistance = Vector3.Distance(transform.position, currentSource.position);

            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestSource = currentSource;
            }
        }

        return closestSource; // Return the closest sound source
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
