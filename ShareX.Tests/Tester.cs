using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShareX.UploadersLib.ImageUploaders;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using ShareX.UploadersLib;

namespace ShareX.Tests
{
    [TestClass]
    public class Tester
    {

        private string email;
        private string password;

        [TestMethod]
        public void LoginTest()
        {
            GetInfo();
            DiscordUploader uploader = new DiscordUploader();
            List<DiscordServer> servers = uploader.Login(email, password);
            Assert.IsTrue(servers.Count > 0);
        }

        [TestMethod]
        public void UploadTest()
        {
            GetInfo();
            UploadersLib.UploadersConfig config = new UploadersLib.UploadersConfig();
            config.DiscordUserName = email;
            config.DiscordPassword = password;
            config.DiscordCurrentGuild = new DiscordServer("ChattyMcChatFace", 206226676365918209);
            DiscordUploader uploader = new DiscordUploader(config);

            string fileName = "oldfashioned.jpg";
            Image image = new Bitmap("C:/TEMP/" + fileName);
            UploadResult result;
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, image.RawFormat);
                result = uploader.Upload(stream, fileName);
            }

            Assert.IsTrue(result.IsSuccess);
        }

        private void GetInfo()
        {
            StreamReader reader = new StreamReader(new FileStream("C:/TEMP/info.txt", FileMode.Open));
            email = reader.ReadLine();
            password = reader.ReadLine();
            reader.Close();
        }
    }
}
