using System;
using UnityEngine;

namespace CabinIcarus.IcSkillSystem.Runtime.xNode_Nodes
{
    [CreateNodeMenu("CabinIcarus/Nodes/UnityEngine/CoreModule/Vector2 Value")]
    public partial class Vector2ValueNode:ValueNode<IcVariableVector2>
    {
        [SerializeField]
        private UnityEngine.Vector2 _value;
   
        private IcVariableVector2 _variableValue = new IcVariableVector2();
   
        protected override IcVariableVector2 GetTValue()
        {
            _variableValue.Value = _value;
            return _variableValue;
        }
    }
}