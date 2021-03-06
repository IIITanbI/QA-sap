﻿namespace SapAutomation.Managers.AemTagManager
{
    using System.Text;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using AemUserManager;
    using QA.AutomatedMagic.Managers.ApiManager;
    using System;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;

    [CommandManager("Aem tag manager")]
    public class AemTagManager : BaseCommandManager
    {
        private void CheckAuthorization(Request request, AemUser user)
        {
            user.CheckAuthorization();
            request.Cookie = user.Cookie;
        }

        [Command("Get AEM tag")]
        public AemTag GetTag(ApiManager apiManager, AemUser user, LandscapeConfig landscapeConfig, string tagString, ILogger log)
        {
            string root = null;
            string path = null;
            AemTag tag = null;
            try
            {
                log?.DEBUG($"Start construct tag from string: {tagString}");


                root = tagString.Split(':')[0];
                path = tagString.Split(':')[1];

                var req = new Request()
                {
                    ContentType = "application/json;charset=UTF-8",
                    Method = Request.Methods.GET,
                    PostData = $"/etc/tags/{root}/{path}.json"
                };

                CheckAuthorization(req, user);
                var response = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, user.Username, user.Password, log);

                var tmp = JObject.Parse(response.Content);

                tag = new AemTag();
                tag.Title = tmp["jcr:title"].ToString();
                tag.Description = tmp["jcr:description"].ToString();
                tag.TagID = tagString;

                log?.DEBUG($"Tag '{tag.Title}' constructing completed");
                log?.USEFULL($"Tag ID : '{tag.TagID}'");
                return tag;
            }
            catch (Exception ex)
            {
                log?.ERROR($"Can't construct tag from string: {tagString}");
                throw new DevelopmentException($"Can't construct tag from string '{tagString}' during exception", ex,
                    $"Root : '{root}'",
                    $"Path : '{path}'",
                    $"Tag: {tag}");
            }
        }

        [Command("Get list of AEM tags")]
        public List<AemTag> GetTags(ApiManager apiManager, AemUser user, LandscapeConfig landscapeConfig, List<string> tagStrings, ILogger log)
        {
            List<AemTag> tags = null;
            try
            {
                log?.DEBUG($"Start construct tags from strings");

                tags = new List<AemTag>();
                foreach (var tagString in tagStrings)
                {
                    tags.Add(GetTag(apiManager, user, landscapeConfig, tagString, log));
                }

                log?.DEBUG("Tags constructing completed");
                log?.USEFULL($"Tags count: '{tags.Count}'");
                return tags;
            }
            catch (Exception ex)
            {
                log?.ERROR("Can't construct tags from strings");
                throw new DevelopmentException("Can't construct tags from strings during exception", ex,
                    $"tagStrings count: {tagStrings.Count}",
                    $"tags count (already obtained): {tags.Count}");
            }
        }

        public List<AemTag> GetChildAemTags(ApiManager apiManager, AemUser user, AemTag aemTag, LandscapeConfig landscapeConfig, string tagString, ILogger log)
        {
            List<AemTag> children = new List<AemTag>();

            try
            {
                log?.INFO($"Get childs of tag: '{aemTag.Title}'");

                var request = new Request
                {
                    ContentType = "text/html;charset=UTF-8",
                    Method = Request.Methods.GET,
                    PostData = $"{aemTag.Path}.tags.json"
                };

                var response = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, user.Username, user.Password, log);
                var tmp = JObject.Parse(response.Content);
                var pageJsons = (JArray)tmp["tags"];

                var index = 0;
                foreach (var child in pageJsons)
                {
                    if (index++ == 0)
                    {
                        continue;
                    }

                    var tag = new AemTag();
                    tag.Title = child["title"].ToString();
                    tag.Path = child["path"].ToString();
                    tag.Description = child["description"].ToString();
                    tag.TagID = child["tagID"].ToString();

                    if (child["replication"].Children().Count() > 1)
                        tag.Status = child["replication"]["action"].ToString();

                    children.Add(tag);
                }

                log?.INFO($"Getting children of tag:' {aemTag.Title}' successfully completed");
                log?.USEFULL($"Children tags count : '{children.Count}'");

                return children;
            }
            catch (Exception ex)
            {
                log?.ERROR("Can't construct tags from strings");
                throw new DevelopmentException("Can't construct tags from strings during exception", ex,
                    $"Username: '{user.Username}'",
                    $"Password: '{user.Password}'",
                    $"Author host url: '{landscapeConfig.AuthorHostUrl}'",
                    $"Tag: '{aemTag}'",
                    $"Children count (already obtained): {children.Count}");
            }

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
                    PostData = $"/bin/tagcommand?cmd=createTag&parentTagID={tag.Path}&jcr:title={tag.Title}&tag{tag.Name}"
                };

                CheckAuthorization(req, user);
                apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, user.Username, user.Password, log);

                log?.INFO($"Tag with name:' {tag.Name}' successfully created");
                log?.USEFULL($"Tag: '{tag}'");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during creating Tag {tag.Name}");
                throw new DevelopmentException($"Error occurred during creating Tag {tag.Name}", ex,
                    $"Username: '{user.Username}'",
                    $"Password: '{user.Password}'",
                    $"Author host url: '{landscapeConfig.AuthorHostUrl}'",
                    $"Tag: '{tag}'");
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

                log?.INFO($"Tag with name:' {tag.Name}' successfully deleted"); log?.USEFULL($"Tag: '{tag}'");
                log?.USEFULL($"Tag: '{tag}'");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during deleting Tag {tag.Name}");
                throw new DevelopmentException($"Error occurred during deleting Tag {tag.Name}", ex,
                    $"Username: '{user.Username}'",
                    $"Password: '{user.Password}'",
                    $"Author host url: '{landscapeConfig.AuthorHostUrl}'",
                    $"Tag: '{tag}'");
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
                log?.USEFULL($"Tag: '{tag}'");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during activating Tag {tag.Name}");
                throw new DevelopmentException($"Error occurred during activating Tag {tag.Name}", ex,
                    $"Username: '{user.Username}'",
                    $"Password: '{user.Password}'",
                    $"Author host url: '{landscapeConfig.AuthorHostUrl}'",
                    $"Tag: '{tag}'");
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
                log?.USEFULL($"Tag: '{tag}'");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during deactivating Tag {tag.Name}");
                throw new DevelopmentException($"Error occurred during deactivating Tag {tag.Name}", ex,
                    $"Username: '{user.Username}'",
                    $"Password: '{user.Password}'",
                    $"Author host url: '{landscapeConfig.AuthorHostUrl}'",
                    $"Tag: '{tag}'");
            }

        }
    }
}
