﻿using System.Data.Objects;
using System.Management.Automation;

namespace Cognifide.PowerShell.Shell.Commands.Analytics
{
    [Cmdlet("Get", "AnalyticsAutomation")]
    public class GetAnalyticsAutomationCommand : AnalyticsBaseCommand
    {
        protected override void ProcessRecord()
        {
            ObjectQuery<Automations> automations = Context.Automations;

            PipeQuery(automations);
        }
    }
}