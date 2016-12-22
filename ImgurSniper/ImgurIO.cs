﻿using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ImgurSniper {
    public class ImgurIO {
        public static string ClientID {
            get {
                return "766263aaa4c9882";
            }
        }
        public static string ClientSecret {
            get {
                return "1f16f21e51e499422fb90ae670a1974e88f7c6ae";
            }
        }

        private ImgurClient _client;

        /// <summary>
        /// Login to Imgur with OAuth2
        /// </summary>
        public ImgurIO() {
            _client = new ImgurClient(ClientID, ClientSecret);

            string url = _client.EndpointUrl;

            Login();

            //TODO: Remove this
            Thread.Sleep(100000);
        }


        private async void Login() {
            //TODO: make this functional
            OAuth2Endpoint endpoint = new OAuth2Endpoint(_client);
            string redirectUrl = endpoint.GetAuthorizationUrl(Imgur.API.Enums.OAuth2ResponseType.Token);
            var token = await endpoint.GetTokenByRefreshTokenAsync(redirectUrl);
            System.Console.WriteLine(token.AccessToken);
        }

        /// <summary>
        /// Upload Image to Imgur
        /// </summary>
        /// <param name="image">The Image as byte[]</param>
        /// <returns>The Link to the uploaded Image</returns>
        public async Task<string> Upload(byte[] bimage) {
            var endpoint = new ImageEndpoint(_client);
            IImage image;
            using(MemoryStream stream = new MemoryStream(bimage)) {
                image = await endpoint.UploadImageStreamAsync(stream);
            }
            return image.Link;
        }

        public class ImgurModel {
            public ImgurModel(string AccessToken, string RefreshToken) {
                this.AccessToken = AccessToken;
                this.RefreshToken = RefreshToken;
            }

            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }
    }
}
