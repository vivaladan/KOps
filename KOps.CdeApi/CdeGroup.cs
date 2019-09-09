using System;
using System.Collections.Generic;
using System.Text;
using Com.Apdcomms.CdeApi;
using Com.Apdcomms.CdeApi.GroupManagement;

namespace KOps.CdeApi
{
    public class CdeGroup
    {
        public CdeGroupInfo GroupInfo { get; internal set; }

        public IEnumerable<CdeMemberInfo> MemberInfos { get; internal set; }
    }
}
