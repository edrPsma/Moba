using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindableUI.Runtime
{
    public class BindImpactAttribute : Attribute
    {
        public EImpactGroup GroupType;
        public string ImpactType;

        public BindImpactAttribute(EImpactGroup groupType, string impactType)
        {
            GroupType = groupType;
            ImpactType = impactType;
        }
    }
}