
using System.Collections;
// using Leaves;
using Behavior;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    protected NavMeshAgent navigation;
    protected Vector3 destination;
    protected bool IsWaiting=false;
    protected BehaviorTree BT;
    protected FieldOfVision lineOfSight;
    protected bool foodOnSight;
    [SerializeField]protected Transform targetPosition;

    [SerializeField] protected bool InDanger;
    public virtual void Start()
    {
        navigation = GetComponent<NavMeshAgent>();
        lineOfSight = GetComponent<FieldOfVision>();
    }
    public virtual void Update()
    {
        BT.Process();
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

    public Vector3 FoodPosition()
    {
        Transform food = lineOfSight.GetVisibleTarget();
        return food != null ? food.position : Vector3.zero;
    }

    public void SetTarget(Transform target)
    {
        targetPosition = target;
    }

    public virtual bool CheckTarget(){return false;}
}