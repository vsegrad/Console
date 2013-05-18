﻿using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Jobs.AsyncUI;
using Sitecore.Web;
using Sitecore.Web.Authentication;

namespace Cognifide.PowerShell.Shell.Commands.Interactive
{
    [Cmdlet(VerbsCommon.Show, "ModalDialog", SupportsShouldProcess = true, DefaultParameterSetName = "Name")]
    public class ShowModalDialogCommand : BaseFormCommand
    {
        [Parameter(Mandatory = true)]
        public string Control { get; set; }

        [Parameter]
        public string Url { get; set; }

        [Parameter]
        public string[] Parameters{ get; set; }

        protected override void ProcessRecord()
        {
            LogErrors(() =>
                {
                    string response = null;
                    if (Parameters != null)
                    {

                        var hashParams =
                            new Hashtable(Parameters.ToDictionary(p => p.ToString().Split('|')[0],
                                                                  p => WebUtil.SafeEncode(p.ToString().Split('|')[1])));
                        response = JobContext.ShowModalDialog(hashParams, Control, WidthString, HeightString);
                    }
                    else if (!string.IsNullOrEmpty(Url))
                    {
                        response = JobContext.ShowModalDialog(Url, WidthString, HeightString);
                    }
                    else if (!string.IsNullOrEmpty(Control))
                    {
                        response = JobContext.ShowModalDialog(Title ?? "Sitecore", Control, WidthString, HeightString);
                    }
                    WriteObject(response);
                });
        }
    }
}