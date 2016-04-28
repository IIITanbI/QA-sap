namespace SapAutomation.Managers.AnalyticsManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.Managers.FiddlerManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [CommandManager("Analytics manager")]
    public class AnalyticsManager : BaseCommandManager
    {
        [Command("Verify analytics objects")]
        public void VerifyAnalyticsObjects(List<FiddlerRequest> FiddlerRequestList, AnalyticsTest analyticsTest, ILogger log)
        {
            log?.INFO("Verify analytics objects");
            try
            {
                foreach (var fiddlerRequest in FiddlerRequestList)
                {
                    VerifyAnalyticsObject(fiddlerRequest, analyticsTest, log);
                }
                log?.INFO("Verify analytics objects successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Verifying analytics objects completed with error", ex);
                throw new DevelopmentException("Verifying analytics objects completed with error", ex);
            }
        }

        [Command("Verify analytics object")]
        public void VerifyAnalyticsObject(FiddlerRequest fiddlerRequest, AnalyticsTest analyticsTest, ILogger log)
        {
            log?.INFO("Verify analytics object");
            try
            {
                var dict = ParseQueryStringParameters(fiddlerRequest, log);
                foreach (var analyticsObject in analyticsTest.AnalyticsObjectsList)
                {
                    var name = analyticsObject.Name;
                    log?.DEBUG($"Verify analytics object with name: {name}");
                    analyticsObject.ActualValue = dict.ContainsKey(name) ? dict[name] : null;
                    analyticsObject.Check();
                }
                if (analyticsTest.AnalyticsObjectsList.Any(a => a.Reason != null))
                {
                    throw new FunctionalException(analyticsTest.AnalyticsObjectsList.Aggregate(string.Empty, (a, s) => a += s.Reason ?? string.Empty));
                }
                log?.INFO("Verify analytics objects successfully completed");
            }
            catch (FunctionalException fe)
            {
                throw fe;
            }
            catch (Exception ex)
            {
                log?.ERROR("Verifying analytics objects completed with error", ex);
                throw new DevelopmentException("Verifying analytics objects completed with error", ex);
            }
        }

        private static Dictionary<string, string> ParseQueryStringParameters(FiddlerRequest fiddlerRequest, ILogger log)
        {
            log?.TRACE("Parse analytics objects from header");
            try
            {
                var parameters = new Dictionary<string, string>();

                var begin = fiddlerObject.Session.fullUrl.IndexOf('?');
                if (begin != -1)
                {
                    var queryStringParams = fiddlerObject.Session.fullUrl.Substring(begin + 1);

                    var queryParams = queryStringParams.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string queryParam in queryParams)
                    {
                        var keyValuePair = queryParam.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        if (keyValuePair.Length == 1)
                        {
                            parameters[keyValuePair[0]] = string.Empty;
                        }
                        else if(keyValuePair.Length == 2)
                        {
                            parameters[keyValuePair[0]] = keyValuePair[1];
                        }
                        else
                        {
                            log?.WARN($"Find incorrect query param: '{queryParam}'");
                        }
                    }
                }
                log?.TRACE("Parsing analytics objects from header successfully completed");
                return parameters;
            }
            catch (Exception ex)
            {
                log?.ERROR("Parsing analytics objects from header completed with error", ex);
                throw new DevelopmentException("Parsing analytics objects from header completed with error", ex);
            }
        }
    }
}
