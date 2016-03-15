﻿namespace SapAutomation.Managers.AemUserManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.ApiManager;
    using QA.AutomatedMagic.CommandsMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [CommandManager(typeof(UserManagerConfig), "Manager for users")]
    public class UserManager : BaseCommandManager
    {
        public UserManagerConfig Config;

        public UserManager(UserManagerConfig config)
        {
            Config = config;
        }

        [Command("Create Aem user")]
        public void CreateUser(ApiManager apiManager, User user, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Create AEM user with ID:'{user.LoginID}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/libs/cq/security/authorizables/POST?rep:userId={user.LoginID}&givenName={user.FirstName}&familyName={user.LastName}&email={user.Mail}&rep:password={user.Password}&rep:password={user.Password}&intermediatePath={Config.UserPath}"
            };

            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, log);

            log?.INFO($"User with ID:' {user.LoginID}' successfully created");
        }

        [Command("Delete Aem user")]
        public void DeleteUser(ApiManager apiManager, User user, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Delete AEM user with ID:'{user.LoginID}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"{Config.UserPath}/{user.LoginID}?deleteAuthorizable={user.LoginID}"
            };

            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, log);

            log?.INFO($"User with ID:' {user.LoginID}' successfully deleted");
        }

        [Command("Activate Aem user")]
        public void ActivateUser(ApiManager apiManager, User user, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Activate AEM user with ID:'{user.LoginID}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/bin/replicate.json?cmd=Activate&path{Config.UserPath}/{user.LoginID}"
            };

            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, log);

            log?.INFO($"User with ID:' {user.LoginID}' successfully activated");
        }

        [Command("Deactivate Aem user")]
        public void DeactivateUser(ApiManager apiManager, User user, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Deactivate AEM user with ID:'{user.LoginID}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/bin/replicate.json?cmd=DeActivate&path{Config.UserPath}/{user.LoginID}"
            };

            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, log);

            log?.INFO($"User with ID:' {user.LoginID}' successfully deactivated");
        }

        [Command("Set aem user to group")]
        public void SetUserToGroup(ApiManager apiManager, User user, Group group, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Set aem user '{user.LoginID}' to group '{group.GroupID}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"{Config.UserPath}/{user.LoginID}?memberAction=memberOf&memberEntry={group.GroupID}"
            };

            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, log);

            log?.INFO($"Setting aem user '{user.LoginID}' to group '{group.GroupID}' completed");
        }

        [Command("Set aem user to groups")]
        public void SetUserToGroups(ApiManager apiManager, User user, List<Group> groups, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Set aem user '{user.LoginID}' to groups");

            var cmd = new StringBuilder();
            cmd.Append($"{Config.UserPath}/{user.LoginID}?memberAction=memberOf");
            foreach (var group in groups)
            {
                log?.TRACE($"Set group '{group.GroupID}'");
                cmd.Append($"&memberEntry ={ group.GroupID}");
            }

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = cmd.ToString()
            };

            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, log);

            log?.INFO($"Setting aem user '{user.LoginID}' to groups completed");
        }

        [Command("Create Aem user group")]
        public void CreateGroup(ApiManager apiManager, Group group, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Create AEM user group with ID:'{group.GroupID}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/libs/cq/security/authorizables/POST?groupName={group.GroupID}&givenName={group.GroupName}&aboutMe={group.Description}&intermediatePath={Config.GroupPath}"
            };

            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, log);

            log?.INFO($"Group with ID:' {group.GroupID}' successfully created");
        }

        [Command("Delete Aem user group")]
        public void DeleteGroup(ApiManager apiManager, Group group, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Delete AEM user group with ID:'{group.GroupID}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"{Config.GroupPath}/{group.GroupID}?deleteAuthorizable={group.GroupID}"
            };

            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, log);

            log?.INFO($"Group with ID:' {group.GroupID}' successfully deleted");
        }

        [Command("Activate Aem user group")]
        public void ActivateGroup(ApiManager apiManager, Group group, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Activate AEM user group with ID:'{group.GroupID}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/bin/replicate.json?cmd=Activate&path{Config.GroupPath}/{group.GroupID}"
            };

            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, log);

            log?.INFO($"Group with ID:' {group.GroupID}' successfully activated");
        }

        [Command("Deactivate Aem user group")]
        public void DeactivateGroup(ApiManager apiManager, Group group, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Deactivate AEM user group with ID:'{group.GroupID}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/bin/replicate.json?cmd=DeActivate&path{Config.GroupPath}/{group.GroupID}"
            };

            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, log);

            log?.INFO($"Group with ID:' {group.GroupID}' successfully deactivated");
        }
    }
}
