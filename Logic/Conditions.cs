using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;

namespace Conditions
{
    public interface ICondition
    {
        bool Evaluate();
    }
    
    public class ConditionLeaf : ICondition
    {
        private Func<bool> _condition;

        public ConditionLeaf(Func<bool> condition)
        {
            _condition = condition;
        }

        public bool Evaluate()
        {
            return _condition();
        }
    }

}