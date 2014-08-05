namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Web;
    using Uma.Eservices.Common;
    
    /// <summary>
    /// Represents the functionality for verifying user's response to the recpatcha challenge.
    /// </summary>
    public class RecaptchaVerificationHelper
    {
        /// <summary>
        /// RECaptcha challenge field
        /// </summary>
        private readonly string challenge;

        /// <summary>
        /// Creates an instance of the <see cref="RecaptchaVerificationHelper"/> class.
        /// </summary>
        /// <param name="httpContext">Sets current httpContext from controller</param>
        public RecaptchaVerificationHelper(HttpContextBase httpContext) 
            : this(Config.RECaptchaPrivateKey, httpContext)
        { }

        /// <summary>
        /// Creates an instance of the <see cref="RecaptchaVerificationHelper"/> class.
        /// </summary>
        /// <param name="privateKey">Sets the private key of the RECaptcha verification request.</param>
        /// <param name="httpContext">Sets current httpContext from controller</param>
        internal RecaptchaVerificationHelper(string privateKey, HttpContextBase httpContext)
        {
            if (string.IsNullOrEmpty(privateKey))
            {
                throw new InvalidOperationException("Private key cannot be null or empty.");
            }

            if (httpContext == null || httpContext.Request == null)
            {
                throw new InvalidOperationException("Http request context does not exist.");
            }

            var request = httpContext.Request;

            if (string.IsNullOrEmpty(request.Form["RECaptcha_challenge_field"]))
            {
                throw new InvalidOperationException("RECaptcha challenge field cannot be empty.");
            }

            this.PrivateKey = privateKey;
            this.UserHostAddress = request.UserHostAddress;
            this.challenge = request.Form["RECaptcha_challenge_field"];
            this.Response = request.Form["RECaptcha_response_field"];
        }

        /// <summary>
        /// Gets the privae key of the RECaptcha verification request.
        /// </summary>
        public string PrivateKey
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the user's host address of the RECaptcha verification request.
        /// </summary>
        public string UserHostAddress
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the user's response to the RECaptcha challenge of the RECaptcha verification request.
        /// </summary>
        public string Response
        {
            get;
            private set;
        }

        /// <summary>
        /// Verifies whether the user's response to the RECaptcha request is correct.
        /// </summary>
        /// <returns>Returns the result as a value of the <see cref="RecaptchaVerificationResult"/> enum.</returns>
        public RecaptchaVerificationResult VerifyRecaptchaResponseTask()
        {
            string[] responseTokens = this.GetResponseFromRecaptcha();

            if (responseTokens.Length == 2)
            {
                if (responseTokens[0].Equals("true", StringComparison.CurrentCulture))
                {
                    return RecaptchaVerificationResult.Success;
                }
                
                switch (responseTokens[1])
                {
                    case "incorrect-captcha-sol":
                        return RecaptchaVerificationResult.IncorrectCaptchaSolution;
                    case "invalid-site-private-key":
                        return RecaptchaVerificationResult.InvalidPrivateKey;
                    case "invalid-request-cookie":
                        return RecaptchaVerificationResult.InvalidCookieParameters;
                }
            }

            return RecaptchaVerificationResult.UnknownError;
        }

        /// <summary>
        /// Retreave RECaptcha response from google
        /// </summary>
        public virtual string[] GetResponseFromRecaptcha()
        {
            string privateKey = Config.RECaptchaPrivateKey;
            string postData = string.Format(CultureInfo.InvariantCulture, "privatekey={0}&remoteip={1}&challenge={2}&response={3}", privateKey, this.UserHostAddress, this.challenge, this.Response);
            byte[] postDataBuffer = System.Text.Encoding.ASCII.GetBytes(postData);
            Uri verifyUri = new Uri("http://api-verify.RECaptcha.net/verify", UriKind.Absolute);
            var webRequest = (HttpWebRequest)WebRequest.Create(verifyUri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = postDataBuffer.Length;
            webRequest.Method = "POST";
            var proxy = WebRequest.GetSystemWebProxy();
            proxy.Credentials = CredentialCache.DefaultCredentials;
            webRequest.Proxy = proxy;
            var requestStream = webRequest.GetRequestStream();
            requestStream.Write(postDataBuffer, 0, postDataBuffer.Length);
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            using (var sr = new StreamReader(webResponse.GetResponseStream()))
            {
                return sr.ReadToEnd().Split('\n');
            }
        }
    }
}