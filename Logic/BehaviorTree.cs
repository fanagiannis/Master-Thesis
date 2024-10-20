using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Actions;
using Conditions;
using Unity.IO.LowLevel.Unsafe;
using System.Linq;
using System.Text;

namespace Behavior
{
    public class Node
    {
        public enum Status{SUCCESS,FAILURE,RUNNING}
        protected string Name;
        public List<Node> children = new List<Node>();
        protected int currentchild;
        public int priority;
        public Node(string name,int priority=0)
        {
            this.Name = name;
            this.priority = priority;
        }
        public virtual Status Process()
        {
            return children[currentchild].Process();
        }
        public virtual void Reset()
        {
            currentchild=0;
            foreach(Node child in children)
            {
                child.Reset();
            }
        }
        public void AddChild(Node child)
        {
            children.Add(child);
        }
        public string nodeName()
        {
            return Name;
        }
    }

    public class BehaviorTree : Node
    {
        public BehaviorTree(string name) : base(name){}
        public override Status Process()
        {
           while(currentchild<children.Count)
           {
                if(children[currentchild].Process()!=Status.SUCCESS){
                    
                    return children[currentchild].Process();
                }
                currentchild++;
           }
           base.Reset();
           return Status.SUCCESS;
        }
        public void PrintTree() {
            StringBuilder sb = new StringBuilder();
            PrintNode(this, 0, sb);
            Debug.Log(sb.ToString());
        }
        static void PrintNode(Node node, int indentLevel, StringBuilder sb) {
            sb.Append(' ', indentLevel * 2).AppendLine(node.nodeName());
            foreach (Node child in node.children) {
                PrintNode(child, indentLevel + 1, sb);
            }
        }
    }

    public class Action : Node 
    {
        private IAction action;
        public Action(string name,IAction action,int priority=0) : base(name,priority)
        {
            this.Name=name;
            this.priority = priority;
            this.action = action;
        }
        public override Status Process()
        {
            return action.Process();
        }
        public override void Reset()
        {
            action.Reset();
        }
        public IAction GetAction()
        {
            return action;
        }
    }

    public class Condition : Node
    {
        private ICondition condition;
        public Condition(string name,ICondition condition,int priority=0) : base(name,priority)
        {
            this.Name=name;
            this.condition = condition;
            this.priority=priority;
        }
        public override Status Process()
        {
            if(condition.Evaluate())
            {
                return Status.SUCCESS;
            }
            else
            {
                return Status.FAILURE;
            }
        }
    }

    public class Sequence : Node
    {
        public Sequence(string name,int priority=0) : base(name,priority){}
        public override Status Process()
        {
            for (currentchild = 0; currentchild < children.Count; currentchild++)
            {
                Status status = children[currentchild].Process();
                switch (status)
                {
                    case Status.SUCCESS:
                        continue; 
                    case Status.RUNNING:
                        return Status.RUNNING;
                    case Status.FAILURE:
                        return Status.FAILURE;       
                }
            }
            return Status.SUCCESS;
        }
        public override void Reset()
        {
            base.Reset();
            foreach (var child in children)
            {
                child.Reset();
            }
        }
    }

    public class Fallback : Node
    {
        public Fallback(string name, int priority = 0) : base(name, priority) {}

        public override Status Process()
        {
            for (currentchild = 0; currentchild < children.Count; currentchild++)
            {
                Status status = children[currentchild].Process();
                switch (status)
                {
                    case Status.SUCCESS:
                        return Status.SUCCESS;
                    case Status.RUNNING:
                        return Status.RUNNING;
                    case Status.FAILURE:
                        continue;
                }
            }
            return Status.FAILURE;
        }

        public override void Reset()
        {
            base.Reset();
            foreach (var child in children)
            {
                child.Reset();
            }
        }
    }

}
