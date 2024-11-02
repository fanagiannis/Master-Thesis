using UnityEngine;
using Behavior;
using UnityEngine.AI;
using UnityEngine.Events;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;

namespace Actions
{
    public interface IAction{
        Node.Status Process();
        void Reset(){}
    }
    public class ActionReset : IAction
    {
        private IAction action;

        public ActionReset(IAction action)
        {
            this.action = action;
        }

        public Node.Status Process()
        {
            action.Reset();
            return Node.Status.SUCCESS; 
        }

        public void Reset()
        {
            action.Reset();
        }
    }
    public class Test : IAction
    {
        private string teststring;
        public Test(string test)
        {
            this.teststring = test;
        }
        public Node.Status Process()
        {
            Debug.Log(teststring);
            return Node.Status.SUCCESS;
        }
    }
    public class RandomPatrol : IAction
    {
        private Agent agent;
        private NavMeshAgent navigation;
        private float waitTime = 2f; 
        private float timer; 
        private bool isWaiting = false;

        public RandomPatrol(Agent agent, NavMeshAgent navigation)
        {
            this.agent = agent;
            this.navigation = navigation;
            SetRandomDestination();
        }

        public Node.Status Process()
        {
            if (isWaiting)
            {
                if (Time.time - timer >= waitTime)
                {
                    isWaiting = false; 
                    SetRandomDestination(); 
                }
                return Node.Status.RUNNING; 
            }
            if (navigation.remainingDistance < 0.5f && !navigation.pathPending)
            {
                isWaiting = true;
                timer = Time.time; 
                return Node.Status.RUNNING; 
            }

            return Node.Status.RUNNING;
        }

        private void SetRandomDestination()
        {
            agent.SetRandomDestination();
            navigation.speed = 5f; 
        }

        public void Reset()
        {
            isWaiting = false;
            navigation.ResetPath(); 
        }
    }
    public class Stop : IAction
    {
        private NavMeshAgent navigation;
        private Agent agent;
        private GuardAnimationController animator;

        public Stop(Agent agent,NavMeshAgent navigation,GuardAnimationController animator)
        {
            this.navigation = navigation;
            this.agent = agent;
            this.animator = animator;
        }

        public Node.Status Process()
        {
            animator.Idle();
            animator.ResetAlert();
            navigation.speed = 0f;
            navigation.ResetPath();
            return Node.Status.SUCCESS;
        }

        public void Reset()
        {
            navigation.ResetPath();
            navigation.isStopped = false;  
        }
    }
    public class LookAtTarget : IAction
    {
        private AnimationController animator;
        private Func<Transform> targetposition;
        private NavMeshAgent navigation;
        private bool looksAtTarget;

        public LookAtTarget(NavMeshAgent navigation, AnimationController animator ,Func<Transform> position)
        {
            this.navigation = navigation;
            this.animator = animator;
            this.targetposition = position;
            this.looksAtTarget = false;
        }

        public Node.Status Process()
        {
            if (targetposition() != null)
            {
                Transform target = targetposition();
                Vector3 directionToTarget = (target.position - animator.gameObject.transform.position).normalized;
                float dotProduct = Vector3.Dot(animator.gameObject.transform.forward, directionToTarget);
                
                if (dotProduct < 0.9f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                    animator.gameObject.transform.rotation = Quaternion.RotateTowards(
                        animator.gameObject.transform.rotation, 
                        targetRotation, 
                        navigation.angularSpeed * Time.deltaTime 
                    );
                    
                    this.navigation.ResetPath();
                    looksAtTarget = true;
                }

                return Node.Status.SUCCESS;
            }
            return Node.Status.SUCCESS; 
            
        }

        public void Reset()
        {
            looksAtTarget = false;
        }
    }
    public class GoTo : IAction
        {
            private AnimationController animator;
            private NavMeshAgent navigation;
            private System.Func<Vector3> getdestination;
            public GoTo (AnimationController animator ,NavMeshAgent navigation,System.Func<Vector3> getdestination)
            {
                this.animator = animator;
                this.navigation=navigation;
                this.getdestination = getdestination;
                navigation.ResetPath();
                    
            }
            public virtual Node.Status Process()
            {
                Vector3 destination = getdestination();
                animator.Run();
                this.navigation.speed = 4f;
                navigation.SetDestination(destination); 
                if(navigation.remainingDistance< 1f && !navigation.pathPending)
                {
                    navigation.ResetPath();
                    return Node.Status.SUCCESS;
                }
                return Node.Status.RUNNING;  
            }
            public virtual void Reset()
            {
                navigation.ResetPath();
            }
        }   
    namespace ZombieActions
    {
        public class ZombieHit : IAction
        {
            private Func<Transform> target;
            private ZombieAnimationController animator;
            private NavMeshAgent navigation;

            public ZombieHit(ZombieAnimationController animator,NavMeshAgent navigation,Func<Transform> target)
            {
                this.target = target;
                this.animator = animator;
                this.navigation = navigation;
            }

            public Node.Status Process()
            {
                Transform player = target();
                PlayerDummy tgt = player.GetComponent<PlayerDummy>();
                if (target != null)
                {
                    navigation.ResetPath();
                    animator.Hit();
                    return Node.Status.SUCCESS;
                }

                return Node.Status.FAILURE;
            }

            public void Reset()
            {
            
            }
        }       

         public class ZombieRandomPatrol : IAction
        {
            private Agent agent;
            private NavMeshAgent navigation;
            private ZombieAnimationController animator;
            private float waitTime = 2f; 
            private float timer; 
            private bool isWaiting = false;

            public ZombieRandomPatrol(Agent agent, NavMeshAgent navigation,ZombieAnimationController animator)
            {
                this.agent = agent;
                this.navigation = navigation;
                this.animator = animator;
                SetRandomDestination();
                
            }

            public Node.Status Process()
            {
                    if (isWaiting)
                    {
                        if (Time.time - timer >= waitTime)
                        {
                            isWaiting = false;
                            SetRandomDestination(); 
                        }
                        return Node.Status.RUNNING;
                    }
                    if (navigation.remainingDistance < 0.5f && !navigation.pathPending)
                    {
                        animator.Idle();
                        isWaiting = true;
                        timer = Time.time; 
                        return Node.Status.RUNNING; 
                    }
                    return Node.Status.RUNNING; 
            }

            private void SetRandomDestination()
            {
                this.navigation.speed = 1f;
                animator.Walk();
                agent.SetRandomDestination();
            }

            public void Reset()
            {
                isWaiting = false;
                navigation.ResetPath(); 
            }
        }

        
    }

    namespace GuardActions
    {
        public class Crouch : IAction
        {
            private GuardAnimationController animator;
            public Crouch(GuardAnimationController animator)
            {
                this.animator = animator;
            }
            public Node.Status Process()
            {
                animator.Crouch(); 
                return Node.Status.SUCCESS; 
            }

            public void Reset()
            {
                animator.ResetAll();
            }

        }

        public class Stand : IAction
        {
            private GuardAnimationController animator;
            public Stand(GuardAnimationController animator)
            {
                this.animator = animator;
            }
            public Node.Status Process()
            {
                animator.Idle();
                animator.Alert(); 
                return Node.Status.SUCCESS; 
            }

            public void Reset()
            {
                animator.ResetAll();
            }

        }

        public class SetDanger : IAction
        {
            private Agent agent;
            private bool value;
            public SetDanger(Agent agent,bool value)
            {
                this.agent = agent;
                this.value = value;
            }
            public Node.Status Process()
            {
                agent.SetDanger(value);
                return Node.Status.SUCCESS;
            }
        }


        public class GuardRandomPatrol : IAction
        {
            private Agent agent;
            private NavMeshAgent navigation;
            private AnimationController animator;
            private float waitTime = 2f; 
            private float timer; 
            private bool isWaiting = false;

            public GuardRandomPatrol(Agent agent, NavMeshAgent navigation,AnimationController animator)
            {
                this.agent = agent;
                this.navigation = navigation;
                this.animator = animator;          
            }

            public Node.Status Process()
            {
                    if (isWaiting)
                    {
                        if (Time.time - timer >= waitTime)
                        {
                            isWaiting = false;
                            SetRandomDestination(); 
                        }
                        return Node.Status.RUNNING;
                    }
                    if (navigation.remainingDistance < 0.5f && !navigation.pathPending)
                    {
                        animator.Idle();
                        isWaiting = true;
                        timer = Time.time; 
                        return Node.Status.RUNNING; 
                    }
                    return Node.Status.RUNNING; 
            }

            private void SetRandomDestination()
            {
                this.navigation.speed = 2f;
                animator.Walk();
                agent.SetRandomDestination();
                
            }

            public void Reset()
            {
                isWaiting = false;
                navigation.ResetPath(); 
            }
        }

        public class GuardPatrol : IAction
        {
            private Agent agent;
            private NavMeshAgent navigation;
            private AnimationController animator;
            private List<Transform> patrolPoints;
            private float waitTime = 2f; 
            private float timer; 
            private bool isWaiting = false;
            private int currentPatrolIndex = 0; 

            public GuardPatrol(Agent agent, NavMeshAgent navigation, AnimationController animator, List<Transform> patrolPoints)
            {
                this.agent = agent;
                this.navigation = navigation;
                this.animator = animator;
                this.patrolPoints = patrolPoints;
            }

            public Node.Status Process()
            {
                if (isWaiting)
                {
                    if (Time.time - timer >= waitTime)
                    {
                        isWaiting = false;
                        SetNextDestination(); 
                    }
                    return Node.Status.RUNNING;
                }
                
                if (HasReachedDestination())
                {
                    animator.Idle();
                    isWaiting = true;
                    timer = Time.time; 
                    return Node.Status.RUNNING; 
                }

                return Node.Status.RUNNING;
            }

            private bool HasReachedDestination()
            {
                return !navigation.pathPending && navigation.remainingDistance < 0.5f;
            }

            private void SetNextDestination()
            {
                navigation.speed = 2f;
                animator.Walk();
                if (patrolPoints.Count > 0)
                {
                    navigation.SetDestination(patrolPoints[currentPatrolIndex].position);
                    currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count; 
                }
            }

            public void Reset()
            {
                isWaiting = false;
                currentPatrolIndex = 0; 
                navigation.ResetPath(); 
            }
        }


        public class ShootAction : IAction
        {
            private Func<Transform> target;
            private AnimationController animator;
            private UnityEvent shootEvent;
            private float damage;

            public ShootAction(AnimationController animator, UnityEvent shoot,float damage, Func<Transform> target)
            {
                this.target = target;
                this.animator = animator;
                this.damage = damage;
                this.shootEvent = shoot;
                
            }

            public Node.Status Process()
            {
                Transform target = this.target.Invoke();
                if (target != null)
                {
                    Entity tg = target.gameObject.GetComponent<Entity>();
                    Debug.Log("BANG");
                    shootEvent.Invoke();
                    
                    
                    if (UnityEngine.Random.Range(0,5) > 1)
                    {
                        tg.TakeDamage((int)damage);
                        return Node.Status.SUCCESS;; 
                    }
                    else
                    {
                        Debug.Log("Missed!");
                        return Node.Status.SUCCESS;
                    }
                } 
                return Node.Status.RUNNING; 
            }

            public void Reset()
            {
            
            }
        }

        public class Aim : IAction
        {
            private GuardAnimationController animator;
            public Aim(GuardAnimationController animator)
            {
                this.animator = animator;
            }

            public Node.Status Process()
            {       
                animator.Idle();
                animator.Alert();
                return Node.Status.SUCCESS; 
            }

            public void Reset()
            {
            
            }
        }

        public class GuardGoTo : IAction
        {
            private GuardAnimationController animator;
            private NavMeshAgent navigation;
            private System.Func<Vector3> getdestination;
            public GuardGoTo (GuardAnimationController animator ,NavMeshAgent navigation,System.Func<Vector3> getdestination)
            {
                this.animator = animator;
                this.navigation=navigation;
                this.getdestination = getdestination;
                navigation.ResetPath();
                    
            }
            public virtual Node.Status Process()
            {
                Vector3 destination = getdestination();
                animator.Run();
                this.navigation.speed = 4f;
                navigation.SetDestination(destination); 
                if(navigation.remainingDistance< 1f && !navigation.pathPending)
                {
                    navigation.ResetPath();
                    return Node.Status.SUCCESS;
                }
               return Node.Status.RUNNING;  
            }
            public virtual void Reset()
            {
                //navigation.ResetPath();
            }
        }

        public class Inspect : IAction
        {
            private GuardAnimationController animator;
            private NavMeshAgent navigation;
            private AISensors sensors;
            private System.Func<Vector3> getdestination;
            public Inspect (GuardAnimationController animator ,NavMeshAgent navigation ,AISensors sensors ,System.Func<Vector3> getdestination)
            {
                this.animator = animator;
                this.navigation=navigation;
                this.sensors=sensors;
                this.getdestination = getdestination;
                navigation.ResetPath();
                    
            }
            public virtual Node.Status Process()
            {
                Vector3 destination = getdestination();
                animator.Walk();
                this.navigation.speed = 2f;
                navigation.SetDestination(destination); 
                if(navigation.remainingDistance< 2f && !navigation.pathPending)
                {
                    this.navigation.speed = 0f;
                    animator.Idle();
                    sensors.ClearSource();
                    animator.ResetAlert();
                    navigation.ResetPath();
                    return Node.Status.SUCCESS;
                }
               return Node.Status.RUNNING;  
            }
            public virtual void Reset()
            {
                //navigation.ResetPath();
            }
        }
        
        public class GuardSearch : IAction
        {
            private Transform guardTransform;
            private float rotationSpeed;
            private float minAngle;
            private float maxAngle;
            private float currentAngle;
            private bool rotatingRight;

            public GuardSearch(Transform guardTransform, float rotationSpeed, float minAngle, float maxAngle)
            {
                this.guardTransform = guardTransform;
                this.rotationSpeed = rotationSpeed;
                this.minAngle = minAngle;
                this.maxAngle = maxAngle;
                this.currentAngle = guardTransform.localEulerAngles.y;
                this.rotatingRight = true;
            }

            public Node.Status Process()
            {
                LineRenderer lineRenderer = guardTransform.GetComponent<LineRenderer>();
                lineRenderer.enabled = false;
                if (rotatingRight)
                {
                    currentAngle += rotationSpeed * Time.deltaTime;
                    if (currentAngle >= maxAngle) rotatingRight = false;
                }
                else
                {
                    currentAngle -= rotationSpeed * Time.deltaTime;
                    if (currentAngle <= minAngle) rotatingRight = true;
                }
                currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
                guardTransform.localEulerAngles = new Vector3(guardTransform.localEulerAngles.x, currentAngle, guardTransform.localEulerAngles.z);

                return Node.Status.RUNNING;
            }

            public void Reset()
            {
                rotatingRight = true;
                currentAngle = guardTransform.localEulerAngles.y;
            }
        }

        public class AimTarget : IAction
        {
            private Transform guardTransform;
            private Func<Transform> getTarget;
            private float rotationSpeed;

            public AimTarget(Transform guardTransform, Func<Transform> getTarget, float rotationSpeed )
            {
                this.guardTransform = guardTransform;
                this.getTarget = getTarget;
                this.rotationSpeed = rotationSpeed;
            }

            public Node.Status Process()
            {
                Transform target = getTarget();
                if (target == null)
                {
                    return Node.Status.FAILURE; 
                }

                Vector3 directionToTarget = (target.position - guardTransform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                guardTransform.rotation = Quaternion.RotateTowards(guardTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                float angleToTarget = Quaternion.Angle(guardTransform.rotation, targetRotation);

                LineRenderer lineRenderer = guardTransform.GetComponent<LineRenderer>();
                lineRenderer.enabled = true;
                if (lineRenderer != null)
                {
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, guardTransform.position+new Vector3(0,1f,0)); 
                    lineRenderer.SetPosition(1, target.position+new Vector3(0,1f,0));        
                    lineRenderer.startColor = Color.red;                
                    lineRenderer.endColor = Color.red;
                }
                if (angleToTarget < 1f) 
                {
                    return Node.Status.SUCCESS; 
                }

                return Node.Status.SUCCESS; 
            }
            public void Reset()
            {
                
            }
        }
    }  
}
