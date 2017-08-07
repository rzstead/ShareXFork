﻿#region License Information (GPL v3)

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
            return new DiscordUploader(APIKeys.DiscordClientId, APIKeys.DiscordClientSecret);
        }

        //REMEMBER TO MAKE A FORM FOR DISCORD USER LOGIN DETAILS STUFF THINGS
        public override TabPage GetUploadersConfigTabPage(UploadersConfigForm form) => form.tpTinyPic;
    }

    public sealed class DiscordUploader : ImageUploader
    {
        public string DiscordID { get; set; }
        public string DiscordSecret { get; set; }

        private const string URLAPI = "https://discordapp.com/api/";

        public DiscordUploader(string id, string secret)
        {
            DiscordID = id;
            DiscordSecret = secret;
        }

        public override UploadResult Upload(Stream stream, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}