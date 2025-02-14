using System.Collections;
using Behavior;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    protected NavMeshAgent navigation;
    protected Vector3 destination;
    protected BehaviorTree BT;
    protected AISensors sensors;
    [Header("Booleans")]
    [SerializeField] protected bool InDanger;
    [SerializeField] protected bool Active;
    public virtual void Start()
    {
        navigation = GetComponent<NavMeshAgent>();
        sensors = GetComponent<AISensors>();
    }
    public virtual void Update()
    {
        if(Active){
            BT.Process();
        }
    }
    public virtual void BakeBehavior(){}
    public void ResetBT()=>BT.Reset();
    public void SetDestination(Vector3 des)
    {
        navigation.SetDestination(des);
    }
    public void SetRandomDestination()
    {
        destination=new Vector3(Random.Range(-10f,10f),gameObject.transform.position.y,Random.Range(-10f,10f));
        navigation.SetDestination(destination);
    }
    public void StopAgent()
    {
        navigation.isStopped = true;
        navigation.ResetPath(); 
        navigation.velocity = Vector3.zero;
    }
    public virtual bool TargetInRange(Transform target, float range)
    {
        if(target!=null)
        {
            return Vector3.Distance(transform.position, target.position) <= range;
        }
        else
        {
            return false;
        }      
    }
    public virtual bool CheckTarget(){return false;}
    public void SetDanger(bool value)
    {
        InDanger=value;
    }
}

