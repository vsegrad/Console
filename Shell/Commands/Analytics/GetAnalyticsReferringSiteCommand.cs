﻿using System.Data.Objects;
using System.Management.Automation;

namespace Cognifide.PowerShell.Shell.Commands.Analytics
{
    [Cmdlet("Get", "AnalyticsReferringSites")]
    public class GetAnalyticsReferringSiteCommand : AnalyticsBaseCommand
    {
        protected override void ProcessRecord()
        {
            ObjectQuery<ReferringSites> referringSites = Context.ReferringSites;
            PipeQuery(referringSites);
        }
    }
}