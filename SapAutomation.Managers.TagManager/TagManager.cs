﻿namespace SapAutomation.Managers.TagManager
{
    using System.Text;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.ApiManager;

    public class TagManager : BaseCommandManager
    {
        private string BuildCreateTagCmd(Tag tag)
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
        public void CreateTag(ApiManager apiManager, Tag tag, LandscapeConfig landscapeConfig, ILogger log)
        {
            if (!tag.NeedToCreate) return;
            log?.INFO($"Create AEM tag:'{tag.Name}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = BuildCreateTagCmd(tag)
            };

            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, log);

            if (tag.ChildTags != null)
                foreach (var childTag in tag.ChildTags)
                {
                    CreateTag(apiManager, childTag, landscapeConfig, log);
                }

            log?.INFO($"Tag with name:' {tag.Name}' successfully created");
        }

        [Command("Delete AEM tag", "DeleteTag")]
        public void DeleteTag(ApiManager apiManager, Tag tag, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Delete AEM tag:'{tag.Name}'");

            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = $"/bin/tagcommand?cmd=deleteTag&path={tag.Path}"
            };
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, log);

            log?.INFO($"Tag with name:' {tag.Name}' successfully deleted");
        }
    }
}