using UnityEngine;
using Behavior;
using UnityEngine.AI;
using UnityEngine.Events;
using System;
using Unity.VisualScripting;

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
            if(targetposition()!=null)
            {
                Transform target = targetposition();
                Vector3 directionToTarget = (target.position - animator.gameObject.transform.position).normalized;
                float dotProduct = Vector3.Dot(animator.gameObject.transform.forward, directionToTarget);
                if (dotProduct < 0.9f)  
                {
                    animator.gameObject.transform.LookAt(target.position);
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

        public class ShootAction : IAction
        {
            private Func<Transform> target;
            private AnimationController animator;
            private UnityEvent shootEvent;

            public ShootAction(AnimationController animator, UnityEvent shoot, Func<Transform> target)
            {
                this.target = target;
                this.animator = animator;
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
                        tg.TakeDamage(45);
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
            private AnimationController animator;
            private NavMeshAgent navigation;
            private System.Func<Vector3> getdestination;
            public GuardGoTo (AnimationController animator ,NavMeshAgent navigation,System.Func<Vector3> getdestination)
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


    }
    
}
