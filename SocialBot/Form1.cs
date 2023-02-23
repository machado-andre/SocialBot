using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/*using InstagramApiSharp;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Logger;*/
using InstaSharper.API;
using InstaSharper.Classes;
using InstaSharper.API.Builder;
using InstaSharper.Logger;
using InstaSharper.Classes.Models;
using SocialBot.Classes;
//using InstagramApiSharp.API;

namespace SocialBot
{
    public partial class Form1 : Form
    {
        private static UserSessionData user;
        private IInstaApi API;
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (textBoxUsername.Text == "" || textBoxPassword.Text == "")
            {
                MessageBox.Show("Username or Password can't be empty.");
                return;
            }
            user = new UserSessionData();
            user.UserName = textBoxUsername.Text;
            user.Password = textBoxPassword.Text;
            API = InstaApiBuilder.CreateBuilder()
                .SetUser(user)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .SetRequestDelay(RequestDelay.FromSeconds(0,8))
                .Build();

            var isLog = await API.LoginAsync();

            if (isLog.Succeeded)
            {
                MessageBox.Show("Logged in successfully.");
                pictureBoxUserImg.Load(user.LoggedInUder.ProfilePicture);
            }
            else
            {
                MessageBox.Show("Error Logging in!");
            }

        }

        private async void btnSendMsg_Click(object sender, EventArgs e)
        {
            IResult<InstaUser> balau = await API.GetUserAsync("tomas.balau");

            var inboxThreads = await API.GetDirectInboxAsync();
            if (inboxThreads.Succeeded)
            {
                MessageBox.Show("Unable to get inbox");
                return;
            }
            MessageBox.Show($"Got {inboxThreads.Value.Inbox.Threads.Count} inbox threads");
            
            var firstThread = inboxThreads.Value.Inbox.Threads.FirstOrDefault();

            // just send message to user (thread not specified)
            var sendMessageResult = await API.SendDirectMessage($"{firstThread.Users.FirstOrDefault()?.Pk}", string.Empty, textBoxMessage.Text);
            MessageBox.Show(sendMessageResult.Succeeded ? "Message sent" : "Unable to send message");
        }
    }
}
