using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Apdcomms.CdeApi;
using Com.Apdcomms.CdeApi.Presence;
using Com.Apdcomms.CdeApi.Subscription;
using KOps.Application;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KOps.CdeApi
{
    public class CdeGroups
    {
        private readonly ILogger logger;
        private readonly ICde cde;
        private readonly IMediator mediator;
        private readonly CdeGroupStore groups;

        static readonly object _object = new object();

        public CdeGroups(ILogger logger, ICde cde, IMediator mediator)
        {
            this.logger = logger;
            this.cde = cde;
            this.mediator = mediator;
            groups = new CdeGroupStore();

            cde.StateChanged += Cde_StateChanged;
            cde.GroupManagement.PresenceInfoList += GroupManagement_PresenceInfoList;
        }

        private void GroupManagement_PresenceInfoList(object sender, CdePresenceInfoListEventArgs e)
        {
            foreach (var presenceInfo in e.PresenceInfo)
            {

            }
        }

        private async void Cde_StateChanged(object sender, CdeState e)
        {
            if (e == CdeState.LoggedIn)
            {
                var groupCount = await cde.GroupManagement.GetNumberOfGroups();
                var groupInfos = await cde.GroupManagement.GetAllGroupInformation(groupCount);

                logger.LogInformation("[{EventName}] Fetched {GroupCount} groups", "StateChanged", groupCount);

                foreach (var groupInfo in groupInfos)
                {
                    logger.LogInformation("[{EventName}] {@GroupInfo}", "StateChanged", groupInfo);

                    var group = groups.Get(groupInfo.ID.Uri);
                    group.GroupInfo = groupInfo;

                    var memberInfos = await cde.GroupManagement.GetGroupMemberInformation(groupInfo.ID, groupInfo.TotalGroupMembers);
                    group.MemberInfos = memberInfos;

                    Publish(group);
                }
            }
        }

        private void Publish(CdeGroup group)
        {
            var groupUpdate = new GroupUpdate
            {
                Uri = group.GroupInfo.ID.Uri,
                Name = group.GroupInfo.Name,

                Members = group.MemberInfos.Select(memberInfo => new GroupMemberUpdate
                {
                    Uri = memberInfo.Identity.Uri,
                    Name = memberInfo.Identity.Name,
                    Number = memberInfo.Identity.PhoneNumber,
                    Available = memberInfo.Status != CdePresenceStatus.Available,
                }),
            };

            mediator.Publish(groupUpdate);
        }
    }
}
