namespace SapAutomation.Managers.JobManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.ApiManager;
    using QA.AutomatedMagic.CommandsMagic;
    using AemUserManager;

    [CommandManager("Aem job manager")]
    public class JobManager : BaseCommandManager
    {
        [Command("Command for run github job", "Run github job")]
        public Response RunGitHubJob(ApiManager aManager, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            var req = new Request()
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.GET,
                PostData = "/bin/sapdx/github/admin?action=force-job"
            };
            var resp = aManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, user.LoginID, user.Password, log);

            return resp;
        }

        [Command("Command for Verify Git Hub Job Response", "Run github job")]
        public void VerifyGitHubJobResponse(Response response, string expectedResponse, ILogger log)
        {
            response.Content.Equals(expectedResponse);
        }
    }
}
