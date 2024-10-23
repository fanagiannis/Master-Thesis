using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEditor.PackageManager.Requests;
using Unity.VisualScripting;
using System.Linq.Expressions;
using UnityEngine.Events;

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

    public class GoTo : IAction
    {
        private Animator animator;
        private NavMeshAgent navigation;
        private System.Func<Vector3> getdestination;
        public GoTo (Animator animator,NavMeshAgent navigation,System.Func<Vector3> getdestination)
        {
            this.animator = animator;
            this.navigation=navigation;
            this.getdestination = getdestination;
            navigation.ResetPath();
                
        }
        public virtual Node.Status Process()
        {
            Vector3 destination = getdestination();
            animator.SetBool("IsWalking",true);
            this.navigation.speed = 4f;
            animator.SetBool("IsRunning",true);
            navigation.SetDestination(destination); 
            if(navigation.remainingDistance< 0.5f && !navigation.pathPending)
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

    public class Stop : IAction
    {
        private NavMeshAgent navigation;
        private Agent agent;

        public Stop(Agent agent,NavMeshAgent navigation)
        {
            this.navigation = navigation;
            this.agent = agent;
        }

        public Node.Status Process()
        {
            navigation.isStopped = true;
            agent.StopAgent();
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
        private Animator animator;
        private Transform targetposition;
        private NavMeshAgent navigation;
        private bool looksAtTarget;

        public LookAtTarget(NavMeshAgent navigation, Animator animator, Transform position)
        {
            this.navigation = navigation;
            this.animator = animator;
            this.targetposition = position;
            this.looksAtTarget = false;
        }

        public Node.Status Process()
        {
            Vector3 directionToTarget = (targetposition.position - animator.gameObject.transform.position).normalized;
            float dotProduct = Vector3.Dot(animator.gameObject.transform.forward, directionToTarget);
            if (dotProduct < 0.9f)  
            {
                animator.gameObject.transform.LookAt(targetposition);
                animator.SetBool("Alert", true);
                this.navigation.ResetPath();
                animator.SetBool("IsWalking", false);
                looksAtTarget = true;
            }
            
            return Node.Status.SUCCESS;
        }

        public void Reset()
        {
            looksAtTarget = false;
        }
    }

    public class ZombieLookAtTarget : IAction
    {
        private Animator animator;
        private Transform targetposition;
        private NavMeshAgent navigation;
        private bool looksAtTarget;

        public ZombieLookAtTarget(NavMeshAgent navigation, Animator animator, Transform position)
        {
            this.navigation = navigation;
            this.animator = animator;
            this.targetposition = position;
            this.looksAtTarget = false;
        }

        public Node.Status Process()
        {
            Vector3 directionToTarget = (targetposition.position - animator.gameObject.transform.position).normalized;
            float dotProduct = Vector3.Dot(animator.gameObject.transform.forward, directionToTarget);
            if (dotProduct < 0.9f)  
            {
                animator.gameObject.transform.LookAt(targetposition);
                this.navigation.ResetPath();
                animator.SetBool("IsWalking", false);
                looksAtTarget = true;
            }
            
            return Node.Status.SUCCESS;
        }

        public void Reset()
        {
            looksAtTarget = false;
        }
    }

    public class Crouch : IAction
    {
        private Animator animator;
        public Crouch(Animator animator)
        {
            this.animator = animator;
        }
        public Node.Status Process()
        {
            animator.SetBool("IsWalking",false);
            animator.SetBool("IsRunning",false);
            animator.SetBool("IsCrouching",true);  
            //animator.SetBool("IsAlert",true); 
            return Node.Status.SUCCESS; 
        }

        public void Reset()
        {
            animator.SetBool("IsCrouching",false);  
        }

    }

    public class GuardRandomPatrol : IAction
    {
        private Agent agent;
        private NavMeshAgent navigation;
        private Animator animator;
        private float waitTime = 2f; 
        private float timer; 
        private bool isWaiting = false;

        public GuardRandomPatrol(Agent agent, NavMeshAgent navigation,Animator animator)
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
                    animator.SetBool("IsWalking",false);
                    isWaiting = true;
                    timer = Time.time; 
                    return Node.Status.RUNNING; 
                }
                return Node.Status.RUNNING; 
        }

        private void SetRandomDestination()
        {
            animator.SetBool("IsWalking",true);
            agent.SetRandomDestination();
        }

        public void Reset()
        {
            isWaiting = false;
            navigation.ResetPath(); 
        }
    }

    public class ShootAction : IAction
    {
        private Transform player;
        private Animator animator;
        private UnityEvent shootEvent;

        public ShootAction(Transform player, Animator animator, UnityEvent shoot)
        {
            this.player = player;
            this.animator = animator;
            this.shootEvent = shoot;
        }

        public Node.Status Process()
        {
            PlayerDummy target = player.GetComponent<PlayerDummy>();
            if (target != null)
            {
                animator.SetTrigger("Shoot");
                Debug.Log("BANG");
                shootEvent.Invoke();
                int random = Random.Range(0, 10);
                
                if (random > 4)
                {
                    target.TakeDamage(1f);
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
}
