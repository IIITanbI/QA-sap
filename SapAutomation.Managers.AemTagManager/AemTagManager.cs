namespace SapAutomation.Managers.AemTagManager
{
    using System.Text;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using AemUserManager;
    using QA.AutomatedMagic.ApiManager;
    using System;
    public class AemTagManager : BaseCommandManager
    {
        private void CheckAuthorization(Request request, AemUser user)
        {
            user.CheckAuthorization();
            request.Cookie = user.Cookie;
        }

        private string BuildCreateTagCmd(AemTag tag)
        {
            StringBuilder cmd = new StringBuilder();

            cmd.Append("/bin/tagcommand?cmd=createTag");

            if (tag.Parent != null)
                cmd.Append($"&parentTagID={tag.Parent.GetFullName()}");

            cmd.Append($"&jcr:title={tag.Title}");
            cmd.Append($"&tag={tag.Name}");
            cmd.Append($"&jcr:description={tag.Description}");

            return cmd.ToString();
        }

        [Command("Create AEM tag", "CreateTag")]
        public void CreateTag(ApiManager apiManager, AemTag tag, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            try
            {
                log?.INFO($"Create AEM tag:'{tag.Name}'");

                var req = new Request()
                {
                    ContentType = "text/html;charset=UTF-8",
                    Method = Request.Methods.POST,
                    PostData = BuildCreateTagCmd(tag)
                };

                CheckAuthorization(req, user);
                apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, user.Username, user.Password, log);

                log?.INFO($"Tag with name:' {tag.Name}' successfully created");
            }
            catch (Exception ex)
            {
                throw new CommandAbortException($"Error occurred during creating Tag {tag.Name}", ex);
            }
        }

        [Command("Delete AEM tag", "DeleteTag")]
        public void DeleteTag(ApiManager apiManager, AemTag tag, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            try
            {
                log?.INFO($"Delete AEM tag:'{tag.Name}'");

                var req = new Request()
                {
                    ContentType = "text/html;charset=UTF-8",
                    Method = Request.Methods.POST,
                    PostData = $"/bin/tagcommand?cmd=deleteTag&path=/etc/tags/{tag.Path}"
                };

                CheckAuthorization(req, user);
                apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, user.Username, user.Password, log);

                log?.INFO($"Tag with name:' {tag.Name}' successfully deleted");
            }
            catch (Exception ex)
            {
                throw new CommandAbortException($"Error occurred during deleting Tag {tag.Name}", ex);
            }
        }

        [Command("Activate AEM tag", "PublishTag")]
        public void ActivateTag(ApiManager apiManager, AemTag tag, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            try
            {
                log?.INFO($"Activate AEM tag:'{tag.Name}'");

                var req = new Request()
                {
                    ContentType = "text/html;charset=UTF-8",
                    Method = Request.Methods.POST,
                    PostData = $"/bin/tagcommand?cmd=activateTag&path=/etc/tags/{tag.Path}"
                };

                CheckAuthorization(req, user);
                apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, user.Username, user.Password, log);

                log?.INFO($"Tag with name:' {tag.Name}' successfully activated");
            }
            catch (Exception ex)
            {
                throw new CommandAbortException($"Error occurred during activation Tag {tag.Name}", ex);
            }
        }

        [Command("Deactivate AEM tag", "PublishTag")]
        public void DeactivateTag(ApiManager apiManager, AemTag tag, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            try
            {
                log?.INFO($"Deactivate AEM tag:'{tag.Name}'");

                var req = new Request()
                {
                    ContentType = "text/html;charset=UTF-8",
                    Method = Request.Methods.POST,
                    PostData = $"/bin/tagcommand?cmd=deactivateTag&path=/etc/tags/{tag.Path}"
                };

                CheckAuthorization(req, user);
                apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, user.Username, user.Password, log);

                log?.INFO($"Tag with name:' {tag.Name}' successfully deactivated");
            }
            catch (Exception ex)
            {
                throw new CommandAbortException($"Error occurred during deactivation Tag {tag.Name}", ex);
            }

        }
    }
}
