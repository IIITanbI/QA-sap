namespace SapAutomation.Managers.AemUserManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.ApiManager;
    using QA.AutomatedMagic.CommandsMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [CommandManager(typeof(AemUserManagerConfig), "Manager for users")]
    public class AemUserManager : BaseCommandManager
    {
        public ThreadLocal<AemUserManagerConfig> Config;

        public AemUserManager(AemUserManagerConfig config)
        {
            Config = new ThreadLocal<AemUserManagerConfig>(() => config);
        }

        private void AuthorizeUser(ApiManager apiManager, AemUser user, string host, ILogger log)
        {
            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/libs/granite/core/content/login.html/j_security_check?j_username={user.Username}&j_password={user.Password}"
            };
            req.Cookie = new CookieContainer();

            var responce = apiManager.PerformRequest(host, req, log);
            user.Cookie = req.Cookie;
        }

        private void CheckAuthorization(Request request)
        {
            Config.Value.Admin.CheckAuthorization();
            request.Cookie = Config.Value.Admin.Cookie;
        }

        [Command("Authorize Admin in Aem Publish")]
        public void AuthorizeAdmin(ApiManager apiManager, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Authorize Admin in AEM Publish: '{Config.Value.Admin.Username}'");

            AuthorizeUser(apiManager, Config.Value.Admin, landscapeConfig.PublishHostUrl, log);

            log?.INFO($"User with ID:' {Config.Value.Admin.Username}' successfully authorized");
        }

        [Command("Authorize user in Aem Publish")]
        public void AuthorizeUserInPublish(ApiManager apiManager, AemUser user, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Authorize user in AEM Publish: '{user.Username}'");

            AuthorizeUser(apiManager, user, landscapeConfig.PublishHostUrl, log);

            log?.INFO($"User with ID:' {user.Username}' successfully authorized");
        }

        [Command("Authorize user in Aem Author")]
        public void AuthorizeUserInAuthor(ApiManager apiManager, AemUser user, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Authorize user in AEM Author: '{user.Username}'");

            AuthorizeUser(apiManager, user, landscapeConfig.AuthorHostUrl, log);

            log?.INFO($"User with ID:' {user.Username}' successfully authorized");
        }

        [Command("Create Aem user")]
        public void CreateUser(ApiManager apiManager, AemUser user, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Create AEM user with ID:'{user.Username}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/libs/cq/security/authorizables/POST?rep:userId={user.Username}&givenName={user.FirstName}&familyName={user.LastName}&email={user.Mail}&rep:password={user.Password}&rep:password={user.Password}&intermediatePath={Config.Value.UserPath}"
            };

            CheckAuthorization(req);
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, Config.Value.Admin.Username, Config.Value.Admin.Password, log);

            log?.INFO($"User with ID:' {user.Username}' successfully created");
        }

        [Command("Delete Aem user")]
        public void DeleteUser(ApiManager apiManager, AemUser user, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Delete AEM user with ID:'{user.Username}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"{Config.Value.UserPath}/{user.Username}?deleteAuthorizable={user.Username}"
            };

            CheckAuthorization(req);
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, Config.Value.Admin.Username, Config.Value.Admin.Password, log);

            log?.INFO($"User with ID:' {user.Username}' successfully deleted");
        }

        [Command("Activate Aem user")]
        public void ActivateUser(ApiManager apiManager, AemUser user, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Activate AEM user with ID:'{user.Username}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/bin/replicate.json?cmd=Activate&path{Config.Value.UserPath}/{user.Username}"
            };

            CheckAuthorization(req);
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, Config.Value.Admin.Username, Config.Value.Admin.Password, log);

            log?.INFO($"User with ID:' {user.Username}' successfully activated");
        }

        [Command("Deactivate Aem user")]
        public void DeactivateUser(ApiManager apiManager, AemUser user, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Deactivate AEM user with ID:'{user.Username}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/bin/replicate.json?cmd=DeActivate&path{Config.Value.UserPath}/{user.Username}"
            };

            CheckAuthorization(req);
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, Config.Value.Admin.Username, Config.Value.Admin.Password, log);

            log?.INFO($"User with ID:' {user.Username}' successfully deactivated");
        }

        [Command("Set aem user to group")]
        public void SetUserToGroup(ApiManager apiManager, AemUser user, AemGroup group, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Set aem user '{user.Username}' to group '{group.GroupID}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"{Config.Value.UserPath}/{user.Username}?memberAction=memberOf&memberEntry={group.GroupID}"
            };

            CheckAuthorization(req);
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, Config.Value.Admin.Username, Config.Value.Admin.Password, log);

            log?.INFO($"Setting aem user '{user.Username}' to group '{group.GroupID}' completed");
        }

        [Command("Set aem user to groups")]
        public void SetUserToGroups(ApiManager apiManager, AemUser user, List<AemGroup> groups, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Set aem user '{user.Username}' to groups");

            var cmd = new StringBuilder();
            cmd.Append($"{Config.Value.UserPath}/{user.Username}?memberAction=memberOf");
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

            CheckAuthorization(req);
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, Config.Value.Admin.Username, Config.Value.Admin.Password, log);

            log?.INFO($"Setting aem user '{user.Username}' to groups completed");
        }

        [Command("Create Aem user group")]
        public void CreateGroup(ApiManager apiManager, AemGroup group, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Create AEM user group with ID:'{group.GroupID}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/libs/cq/security/authorizables/POST?groupName={group.GroupID}&givenName={group.GroupName}&aboutMe={group.Description}&intermediatePath={Config.Value.GroupPath}"
            };

            CheckAuthorization(req);
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, Config.Value.Admin.Username, Config.Value.Admin.Password, log);

            log?.INFO($"Group with ID:' {group.GroupID}' successfully created");
        }

        [Command("Delete Aem user group")]
        public void DeleteGroup(ApiManager apiManager, AemGroup group, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Delete AEM user group with ID:'{group.GroupID}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"{Config.Value.GroupPath}/{group.GroupID}?deleteAuthorizable={group.GroupID}"
            };

            CheckAuthorization(req);
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, Config.Value.Admin.Username, Config.Value.Admin.Password, log);

            log?.INFO($"Group with ID:' {group.GroupID}' successfully deleted");
        }

        [Command("Activate Aem user group")]
        public void ActivateGroup(ApiManager apiManager, AemGroup group, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Activate AEM user group with ID:'{group.GroupID}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/bin/replicate.json?cmd=Activate&path{Config.Value.GroupPath}/{group.GroupID}"
            };

            CheckAuthorization(req);
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, Config.Value.Admin.Username, Config.Value.Admin.Password, log);

            log?.INFO($"Group with ID:' {group.GroupID}' successfully activated");
        }

        [Command("Deactivate Aem user group")]
        public void DeactivateGroup(ApiManager apiManager, AemGroup group, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Deactivate AEM user group with ID:'{group.GroupID}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/bin/replicate.json?cmd=DeActivate&path{Config.Value.GroupPath}/{group.GroupID}"
            };

            CheckAuthorization(req);
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, Config.Value.Admin.Username, Config.Value.Admin.Password, log);

            log?.INFO($"Group with ID:' {group.GroupID}' successfully deactivated");
        }
    }
}
