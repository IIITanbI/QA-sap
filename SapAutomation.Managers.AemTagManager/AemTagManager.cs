namespace SapAutomation.Managers.AemTagManager
{
    using System.Text;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;

    using AemUserManager;
    using QA.AutomatedMagic.ApiManager;
    public class AemTagManager : BaseCommandManager
    {
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
            if (!tag.NeedToCreate) return;
            log?.INFO($"Create AEM tag:'{tag.Name}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = BuildCreateTagCmd(tag)
            };

            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, user.LoginID, user.Password, log);

            if (tag.ChildTags != null)
                foreach (var childTag in tag.ChildTags)
                {
                    CreateTag(apiManager, childTag, landscapeConfig, user, log);
                }

            log?.INFO($"Tag with name:' {tag.Name}' successfully created");
        }

        [Command("Delete AEM tag", "DeleteTag")]
        public void DeleteTag(ApiManager apiManager, AemTag tag, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            log?.INFO($"Delete AEM tag:'{tag.Name}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/bin/tagcommand?cmd=deleteTag&path=/etc/tags/{tag.Path}"
            };
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, user.LoginID, user.Password, log);

            log?.INFO($"Tag with name:' {tag.Name}' successfully deleted");
        }

        [Command("Activate AEM tag", "PublishTag")]
        public void ActivateTag(ApiManager apiManager, AemTag tag, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            log?.INFO($"Activate AEM tag:'{tag.Name}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/bin/tagcommand?cmd=activateTag&path=/etc/tags/{tag.Path}"
            };
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, user.LoginID, user.Password, log);

            log?.INFO($"Tag with name:' {tag.Name}' successfully activated");
        }

        [Command("Deactivate AEM tag", "PublishTag")]
        public void DeactivateTag(ApiManager apiManager, AemTag tag, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            log?.INFO($"Deactivate AEM tag:'{tag.Name}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/bin/tagcommand?cmd=deactivateTag&path=/etc/tags/{tag.Path}"
            };
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, user.LoginID, user.Password, log);

            log?.INFO($"Tag with name:' {tag.Name}' successfully deactivated");
        }
    }
}
