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
        [Command("Verify analytics query parameters")]
        public void VerifyAnalyticsQueryParameters(List<FiddlerRequest> FiddlerRequestList, AnalyticsTest analyticsTest, ILogger log)
        {
            log?.INFO("Verify analytics query parameters");
            try
            {
                if (FiddlerRequestList.Count == 0)
                {
                    log?.INFO("FiddlerRequestList contains 0 requests");
                    throw new DevelopmentException("FiddlerRequestList contains 0 requests", null);
                }

                List<Exception> exceptions = new List<Exception>();
                FiddlerRequestList = FiddlerRequestList.Take(1).ToList();
                foreach (var fiddlerRequest in FiddlerRequestList)
                {
                    try
                    {
                        VerifyAnalyticsQueryParameter(fiddlerRequest, analyticsTest, log);
                    }
                    catch(Exception ex)
                    {
                        exceptions.Add(ex);
                    }

                    if (exceptions.Count == FiddlerRequestList.Count)
                    {
                        string res = exceptions.Aggregate(string.Empty, (s, e) => s += e.Message + "\n\n\n");
                        throw new FunctionalException(res, null);
                    }
                }
                log?.INFO("Verify analytics query parameters successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Verifying analytics query parameters completed with error", ex);
                throw new DevelopmentException("Verifying analytics query parameters completed with error", ex);
            }
        }

        [Command("Verify analytics query parameter")]
        public void VerifyAnalyticsQueryParameter(FiddlerRequest fiddlerRequest, AnalyticsTest analyticsTest, ILogger log)
        {
            log?.INFO("Verify analytics query parameter");
            try
            {
                var dict = ParseQueryStringParameters(fiddlerRequest, log);
                foreach (var analyticsObject in analyticsTest.AnalyticsQueryParametersList)
                {
                    var name = analyticsObject.Name;
                    log?.DEBUG($"Verify analytics object with name: {name}");
                    analyticsObject.ActualValue = dict.ContainsKey(name) ? dict[name] : null;

                    if (analyticsObject.ActualValue != null)
                    {
                        analyticsObject.ActualValue = Uri.UnescapeDataString(analyticsObject.ActualValue);
                    }

                    analyticsObject.Check();
                }
                if (analyticsTest.AnalyticsQueryParametersList.Any(a => a.Reason != null))
                {
                    throw new FunctionalException(analyticsTest.AnalyticsQueryParametersList.Aggregate(string.Empty, (a, s) => a += s.Reason + "\n"));
                }
                log?.INFO("Verify analytics query parameter successfully completed");
            }
            catch (FunctionalException fe)
            {
                throw fe;
            }
            catch (Exception ex)
            {
                log?.ERROR("Verifying analytics query parameter completed with error", ex);
                throw new DevelopmentException("Verifying analytics query parameter completed with error", ex);
            }
        }

        private static Dictionary<string, string> ParseQueryStringParameters(FiddlerRequest fiddlerRequest, ILogger log)
        {
            log?.TRACE("Parse analytics query string parameter from header");
            try
            {
                var parameters = new Dictionary<string, string>();

                var begin = fiddlerRequest.FullUrl.IndexOf('?');
                if (begin != -1)
                {
                    var queryStringParams = fiddlerRequest.FullUrl.Substring(begin + 1);

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
                            log?.WARN($"Find incorrect query parameter: '{queryParam}'");
                        }
                    }
                }
                log?.TRACE("Parsing analytics query string parameter from header successfully completed");
                return parameters;
            }
            catch (Exception ex)
            {
                log?.ERROR("Parsing analytics query string parameters from header completed with error", ex);
                throw new DevelopmentException("Parsing analytics query string parameters from header completed with error", ex);
            }
        }
    }
}
