﻿using CabinIcarus.IcSkillSystem.Runtime.xNode_NPBehave_Node.Attributes;
using NPBehave;
using UnityEngine;

namespace CabinIcarus.IcSkillSystem.Runtime.xNode_NPBehave_Node.Tasks
{
    [CreateNodeMenu("CabinIcarus/IcSkillSystem/Behave Nodes/Task/Wait/Seconds")]
    [NodeWidth(300)]
    public class WaitNode_Seconds:ANPBehaveNode<Wait>
    {
        [SerializeField,Input(ShowBackingValue.Always,ConnectionType.Override,TypeConstraint.Strict)] 
        private float _seconds;

        protected override Wait GetOutValue()
        {
            return new Wait(_getSeconds);
        }

        private float _getSeconds()
        {
            return GetInputValue(nameof(_seconds),_seconds);
        }
    }
}