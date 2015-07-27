﻿using System.Management.Automation;
using Cognifide.PowerShell.Core.Extensions;
using Cognifide.PowerShell.Core.Utility;
using Sitecore.Data.Items;
using Sitecore.Security.AccessControl;
using Sitecore.Security.Accounts;

namespace Cognifide.PowerShell.Commandlets.Security.Items
{
    [Cmdlet(VerbsCommon.Add, "ItemAcl", SupportsShouldProcess = true)]
    [OutputType(typeof (Item))]
    public class AddItemAclCommand : BaseItemAclCommand
    {
        public override string Filter { get; set; }

        [Parameter(ParameterSetName = "Account ID, Item from Path", Mandatory = true)]
        [Parameter(ParameterSetName = "Account ID, Item from ID", Mandatory = true)]
        [Parameter(ParameterSetName = "Account ID, Item from Pipeline", Mandatory = true)]
        public virtual PropagationType PropagationType { get; set; }

        [Parameter(ParameterSetName = "Account ID, Item from Path", Mandatory = true)]
        [Parameter(ParameterSetName = "Account ID, Item from ID", Mandatory = true)]
        [Parameter(ParameterSetName = "Account ID, Item from Pipeline", Mandatory = true)]
        public virtual SecurityPermission SecurityPermission { get; set; }

        [Parameter(ParameterSetName = "Item from Path", Mandatory = true)]
        [Parameter(ParameterSetName = "Item from ID", Mandatory = true)]
        [Parameter(ParameterSetName = "Item from Pipeline", Mandatory = true, ValueFromPipeline = true)]
        public virtual AccessRuleCollection AccessRules { get; set; }

        [Parameter]
        public SwitchParameter PassThru { get; set; }

        protected override void ProcessItem(Item item)
        {
            var accessRules = item.Security.GetAccessRules();

            if (AccessRules == null)
            {
                AccessRight accessRight;

                if (!this.TryGetAccessRight(out accessRight, true)) return;

                Account account = this.GetAccountFromIdentity(Identity);

                var accessRule = AccessRule.Create(account, accessRight, PropagationType, SecurityPermission);
                accessRules.Add(accessRule);

                if (ShouldProcess(item.GetProviderPath(),
                    string.Format(
                        "Add access right '{0}' with PropagationType '{1}', SecurityPermission '{2}' for '{3}'",
                        accessRight.Name, PropagationType, SecurityPermission, Identity.Name)))
                {
                    item.Security.SetAccessRules(accessRules);
                }
            }
            else
            {
                if (ShouldProcess(item.GetProviderPath(),"Add Acl list."))
                {
                    accessRules.AddRange(AccessRules);
                    item.Security.SetAccessRules(accessRules);
                }
            }

            if (PassThru)
            {
                WriteItem(item);
            }
        }

        public AddItemAclCommand()
        {
            AddDynamicParameter<string>("AccessRight", new ParameterAttribute
            {
                ParameterSetName = "Account ID, Item from Path",
                Mandatory = true
            }, new ParameterAttribute
            {
                ParameterSetName = "Account ID, Item from ID",
                Mandatory = true
            }, new ParameterAttribute
            {
                ParameterSetName = "Account ID, Item from Pipeline",
                Mandatory = true
            }, new ValidateSetAttribute(WellKnownRights));
        }
    }
}