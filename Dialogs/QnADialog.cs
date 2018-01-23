using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using QnAMakerDialog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;


namespace LuisBot.Dialogs
{
    [Serializable]
    [QnAMakerService("9ac0df13b6354d4fafe1583a5bc59b95", "5bd00e82-a8de-4584-aca1-ed0c27e21ae0")]
    public class QnADialog : QnAMakerDialog<object>
    {        
        //public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
        //{
        //    await context.PostAsync(string.Format("Sorry, I couldn't find an answer for **{0}**", originalQueryText));
        //    context.Wait(MessageReceived);
        //}

        //[QnAMakerResponseHandler(50)]
        //public async Task LowScoreHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        //{
        //    await context.PostAsync(string.Format("I found an answer that might help... {0}, {1}", result.Score, result.Answer));
        //    context.Wait(MessageReceived);
        //}

        private class QnAMakerResult
        {
            public IList<Response> answers { get; set; }
        }

        private class Response
        {
            public string answer { get; set; }
            public IList<string> questions { get; set; }
            public double score { get; set; }
        }

        private static string knowledgebaseId = "5bd00e82-a8de-4584-aca1-ed0c27e21ae0"; // Use knowledge base id created.
        private static string qnamakerSubscriptionKey = "9ac0df13b6354d4fafe1583a5bc59b95"; //Use subscription key assigned to you.

        /// <summary>
        /// Try to query a question and get the answer > 50 points
        /// </summary>
        /// <param name="question">The question you want to answer</param>
        /// <param name="answer">The answer you might get</param>
        /// <returns>If get an answer which scores more than 50</returns>
        public bool TryQuery(string question, out string answer)
        {
            string responseString = string.Empty;
            answer = string.Empty;
            var query = question; //User Query

            //Build the URI
            Uri qnamakerUriBase = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v2.0");
            var builder = new UriBuilder($"{qnamakerUriBase}/knowledgebases/{knowledgebaseId}/generateAnswer");

            //Add the question as part of the body
            var postBody = $"{{\"question\": \"{query}\"}}";

            //Send the POST request
            using (WebClient client = new WebClient())
            {
                //Set the encoding to UTF8
                client.Encoding = System.Text.Encoding.UTF8;
                try
                {
                    //Add the subscription key header
                    client.Headers.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);
                    client.Headers.Add("Content-Type", "application/json");
                    
                    responseString = client.UploadString(builder.Uri, postBody);
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        var resp = ex.Response as HttpWebResponse;
                        if (resp != null)
                        {
                            Debug.WriteLine("HTTP Status Code: " + (int)resp.StatusCode);
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }
                    return false;
                }
            }

            QnAMakerResult response;
            try
            {
                response = JsonConvert.DeserializeObject<QnAMakerResult>(responseString);
            }
            catch
            {
                return false;
                throw new Exception("Unable to deserialize QnA Maker response string.");
            }

            try
            {
                Debug.WriteLine(response.answers[0].answer + response.answers[0].score.ToString());
                answer = response.answers[0].answer;
                return response.answers[0].score > 50 ? true : false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }
    }
}