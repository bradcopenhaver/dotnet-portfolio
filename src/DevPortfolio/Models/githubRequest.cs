using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using DevPortfolio.Models;

namespace DevPortfolio.Models
{
    public class GithubRequest
    {

        public static List<ResponseRepo.RootObject> getRepos(string sortType)
        {
            RestClient client = new RestClient("https://api.github.com");
            RestRequest request = new RestRequest($"/search/repositories?q=user:bradcopenhaver&sort={sortType}", Method.GET);
            request.AddHeader("User-Agent", "bradcopenhaver");
            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            List<ResponseRepo.RootObject> repoResponse = JsonConvert.DeserializeObject<List<ResponseRepo.RootObject>>(jsonResponse["items"].ToString());

            return repoResponse;
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
