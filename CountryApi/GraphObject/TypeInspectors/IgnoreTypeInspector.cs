using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HotChocolate.Types.Descriptors;

namespace MarketingApi.GraphObject.TypeInspectors
{
    public class IgnoreTypeInspector : DefaultTypeInspector
    {
        public override IEnumerable<MemberInfo> GetMembers(Type type, bool includeIgnored)
        {
            return base.GetMembers(type, includeIgnored)
                .Where(e => e.Name != "DomainEvents");
        }
    }
}