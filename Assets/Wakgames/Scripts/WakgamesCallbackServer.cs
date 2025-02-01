using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using WakSDK.ApiRequest;

namespace WakSDK
{
    public class WakgamesCallbackServer : MonoBehaviour
    {
        private HttpListener _listener;

        public string ClientId { get; set; }
        public string CsrfState { get; set; }
        public string CodeVerifier { get; set; }

        public bool IsRunning { get; private set; } = false;
        public WakgamesToken UserWakgamesToken { get; private set; }

        public void StartServer(int listenPort)
        {
            if (IsRunning)
            {
                throw new InvalidOperationException();
            }

            var listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{listenPort}/");
            listener.Start();
            _listener = listener;

            IsRunning = true;

            ThreadPool.QueueUserWorkItem(HandleRequests);
        }

        private void HandleRequests(object _)
        {
            while (_listener.IsListening)
            {
                try
                {
                    var context = _listener.GetContext();
                    var request = context.Request;
                    var response = context.Response;

                    // callback 쪽 요청이 아니면 404 응답 주고 무시.
                    if (request.Url.LocalPath != "/callback")
                    {
                        response.StatusCode = 404;

                        byte[] buffer = Encoding.UTF8.GetBytes("not found");
                        response.ContentLength64 = buffer.Length;
                        Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                        continue;
                    }

                    var queryParams = ParseQueryString(request.Url.Query);
                    queryParams.TryGetValue("code", out string code);
                    queryParams.TryGetValue("state", out string state);
                    queryParams.TryGetValue("error", out string error);
                    queryParams.TryGetValue("message", out string message);

                    bool success = string.IsNullOrEmpty(error) && state == CsrfState;

                    if (success)
                    {
                        try
                        {
                            UserWakgamesToken = GetToken(code);

                            response.Redirect($"{Wakgames.Host}/oauth/authorize?success=1");
                            response.Close();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);

                            error = e.Message;
                            message = e.ToString();

                            success = false;
                        }
                    }

                    if (!success)
                    {
                        string responseText;
                        if (string.IsNullOrEmpty(error) && string.IsNullOrEmpty(message))
                        {
                            responseText = "error: invalid state";
                        }
                        else
                        {
                            responseText = $"error: {error}\n{message}";
                        }

                        byte[] buffer = Encoding.UTF8.GetBytes(responseText);
                        response.ContentLength64 = buffer.Length;
                        Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }

                    break;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    break;
                }
            }

            _listener.Stop();
            _listener.Close();

            IsRunning = false;
        }

        private WakgamesToken GetToken(string code)
        {
            string callbackUri = Uri.EscapeDataString(_listener.Prefixes.First() + "callback");
            string getTokenUri = $"{Wakgames.Host}/api/oauth/token?grantType=authorization_code&clientId={ClientId}&code={code}&verifier={CodeVerifier}&callbackUri={callbackUri}";

            var request = (HttpWebRequest)WebRequest.Create(getTokenUri);
            request.Method = "POST";
            request.ContentLength = 0;
            request.UserAgent = $"WakGames_Game/{ClientId}";

            string responseContent;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                responseContent = reader.ReadToEnd();
            }

            return JsonUtility.FromJson<WakgamesToken>(responseContent);
        }

        private Dictionary<string, string> ParseQueryString(string queryString)
        {
            var result = new Dictionary<string, string>();
            if (queryString.StartsWith("?"))
            {
                queryString = queryString.Substring(1);
            }
            foreach (string segment in queryString.Split('&'))
            {
                int index = segment.IndexOf('=');
                if (index >= 0)
                {
                    string key = Uri.UnescapeDataString(segment.Substring(0, index));
                    string value = Uri.UnescapeDataString(segment.Substring(index + 1));
                    result[key] = value;
                }
            }
            return result;
        }

        private void OnDestroy()
        {
            if (_listener != null && _listener.IsListening)
            {
                _listener.Stop();
                _listener.Close();
            }
        }
    }
}
