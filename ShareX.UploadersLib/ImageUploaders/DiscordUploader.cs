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

using ShareX.UploadersLib.Properties;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System;
using Newtonsoft.Json;
using ShareX.HelpersLib;

namespace ShareX.UploadersLib.ImageUploaders
{
    public class DiscordImageUploaderService : ImageUploaderService
    {
        public override ImageDestination EnumValue { get; } = ImageDestination.Discord;

        public override Icon ServiceIcon => Resources.Discord;

        public override bool CheckConfig(UploadersConfig config)
        {
            return false;
            //return config.TinyPicAccountType == AccountType.Anonymous || !string.IsNullOrEmpty(config.TinyPicRegistrationCode);
        }

        public override GenericUploader CreateUploader(UploadersConfig config, TaskReferenceHelper taskInfo)
        {
            return new DiscordUploader();
        }
        public override TabPage GetUploadersConfigTabPage(UploadersConfigForm form) => form.tpDiscord;    }

    public sealed class DiscordUploader : ImageUploader
    {
        string URLAPI = "http://localhost:57331/api/Bot/Upload";
        public override UploadResult Upload(Stream stream, string fileName)
        {
            UploadResult result = null;
            result = SendRequestFile(URLAPI, stream, fileName);

            if (result.IsSuccess)
            {
                result.URL = Helpers.GetXMLValue(result.Response, "fullsize");
                result.ThumbnailURL = Helpers.GetXMLValue(result.Response, "thumbnail");
            }

            return result;
        }
    }
}