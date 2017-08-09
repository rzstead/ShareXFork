#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2017 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using ShareX.HelpersLib;
using ShareX.UploadersLib.Properties;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;
using System;
using Newtonsoft.Json;

namespace ShareX.UploadersLib.ImageUploaders
{
    public class DiscordImageUploaderService : ImageUploaderService
    {
        public override ImageDestination EnumValue { get; } = ImageDestination.Discord;

        public override Icon ServiceIcon => Resources.TinyPic;

        public override bool CheckConfig(UploadersConfig config)
        {
            return false;
            //return config.TinyPicAccountType == AccountType.Anonymous || !string.IsNullOrEmpty(config.TinyPicRegistrationCode);
        }

        public override GenericUploader CreateUploader(UploadersConfig config, TaskReferenceHelper taskInfo)
        {
            return new DiscordUploader(config.DiscordOauth2Info);
        }

        public override TabPage GetUploadersConfigTabPage(UploadersConfigForm form) => form.tpDiscord;
    }

    public sealed class DiscordUploader : ImageUploader, IOAuth2Basic
    {
        public OAuth2Info AuthInfo { get; set; }

        private const string URLAPI = "https://discordapp.com/api/";

        public DiscordUploader(OAuth2Info auth)
        {
            AuthInfo = auth;
        }

        public override UploadResult Upload(Stream stream, string fileName)
        {
            throw new NotImplementedException();
        }

        public string GetAuthorizationURL()
        {
            Dictionary<string, string> args = new Dictionary<string, string>();

            args.Add("client_id", AuthInfo.Client_ID);

            return CreateQuery(URLAPI + "/oauth2/authorize", args);
        }

        public bool GetAccessToken(string code)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("client_id", AuthInfo.Client_ID);

            string response = SendRequestMultiPart(URLAPI + "oauth2/authorize", args);

            if (!string.IsNullOrEmpty(response))
            {
                OAuth2Token token = JsonConvert.DeserializeObject<OAuth2Token>(response);

                if (token != null && !string.IsNullOrEmpty(token.access_token))
                {
                    token.UpdateExpireDate();
                    AuthInfo.Token = token;
                    return true;
                }
            }

            return false;
        }
    }
}