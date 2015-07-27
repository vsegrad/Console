﻿using System;
using System.Linq;
using System.Management.Automation;
using Cognifide.PowerShell.Core.Extensions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Maintenance;

namespace Cognifide.PowerShell.Commandlets.Data
{
    [Cmdlet(VerbsData.Initialize, "SearchIndex", DefaultParameterSetName = "Name")]
    public class InitializeSearchIndexCommand : BaseCommand
    {
        private readonly string[] indexes = ContentSearchManager.Indexes.Select(i => i.Name).ToArray();

        [ValidateSet("*")]
        [Parameter(ValueFromPipeline = true, Position = 0, ParameterSetName = "Name")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, ParameterSetName = "Instance")]
        public ISearchIndex Index { get; set; }

        [Parameter]
        public SwitchParameter IncludeRemoteIndex { get; set; }

        [Parameter]
        public SwitchParameter AsJob { get; set; }

        protected override void ProcessRecord()
        {
            if (ParameterSetName.Is("Name"))
            {
                foreach (var index in ContentSearchManager.Indexes)
                {
                    if (!index.Name.Is(Name)) continue;

                    RebuildIndex(index);
                    if (IncludeRemoteIndex)
                    {
                        RebuildIndex(index, true);
                    }
                }
            }
            else if (ParameterSetName.Is("Instance") && Index != null)
            {
                RebuildIndex(Index);
                if (IncludeRemoteIndex)
                {
                    RebuildIndex(Index, true);
                }
            }
        }

        private void RebuildIndex(ISearchIndex index, bool isRemoteIndex = false)
        {
            if (IndexCustodian.IsRebuilding(index))
            {
                WriteVerbose(String.Format("Skipping full index rebuild for {0} because it's already running.", index.Name));
                return;
            }

            const string message = "Starting full index rebuild for {0}.";
            WriteVerbose(String.Format(message, index.Name));
            var job = (isRemoteIndex) ? IndexCustodian.FullRebuildRemote(index) : IndexCustodian.FullRebuild(index);
            if (job == null) return;

            WriteVerbose(String.Format("Background job created: {0}", job.Name));

            if (AsJob)
            {
                WriteObject(job);
            }
        }

        public override object GetDynamicParameters()
        {
            if (!_reentrancyLock.WaitOne(0))
            {
                _reentrancyLock.Set();

                SetValidationSetValues("Name", indexes);

                _reentrancyLock.Reset();
            }

            return base.GetDynamicParameters();
        }
    }
}