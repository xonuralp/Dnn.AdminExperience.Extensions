﻿using Dnn.PersonaBar.Library.Prompt;
using Dnn.PersonaBar.Library.Prompt.Attributes;
using Dnn.PersonaBar.Library.Prompt.Models;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;

namespace Dnn.PersonaBar.Recyclebin.Components.Prompt.Commands
{
    [ConsoleCommand("restore-user", Constants.RecylcleBinCategory, "Prompt_RestoreUser_Description")]
    public class RestoreUser : ConsoleCommandBase
    {
        public override string LocalResourceFile => Constants.LocalResourcesFile;

        [FlagParameter("id", "Prompt_RestoreUser_FlagId", "Integer", true)]
        private const string FlagId = "id";

        private int UserId { get; set; }

        public override void Init(string[] args, PortalSettings portalSettings, UserInfo userInfo, int activeTabId)
        {
            
            UserId = GetFlagValue(FlagId, "User Id", -1, true, true, true);
        }

        public override ConsoleResultModel Run()
        {
            var userInfo = UserController.Instance.GetUser(PortalId, UserId);
            if (userInfo == null)
                return new ConsoleErrorResultModel(string.Format(LocalizeString("UserNotFound"), UserId));

            if (!userInfo.IsDeleted)
                return new ConsoleErrorResultModel(LocalizeString("Prompt_RestoreNotRequired"));

            string message;
            var restoredUser = RecyclebinController.Instance.RestoreUser(userInfo, out message);
            return restoredUser
                ? new ConsoleResultModel(LocalizeString("UserRestored")) { Records = 1 }
                : new ConsoleErrorResultModel(message);
        }
    }
}