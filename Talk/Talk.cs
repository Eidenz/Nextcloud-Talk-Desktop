using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Xml.Linq;
using CefSharp.WinForms;
using CefSharp.Handler;
using CefSharp;
using System.Net.Http;
using System.Resources;
using System.Collections;
using Microsoft.VisualBasic;
using System.Globalization;
using System.IO;

namespace Talk
{
    public partial class Talk : Form
    {
        public static String website = null;
        public static String user = null;
        public static String pass = null;
        public static String lang = "fr-FR";
        public static IEnumerable<XElement> convos;
        public static IEnumerable<XElement> oldconvos;
        public static Boolean error = false;

        public Talk()
        {
            CefSharp.WinForms.CefSettings settings = new CefSharp.WinForms.CefSettings();
            settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
            settings.CefCommandLineArgs.Add("enable-media-stream", "1");
            CefSharp.Cef.Initialize(settings);
            InitializeComponent();
        }

        private void Talk_Load(object sender, EventArgs e)
        {
            notifications.Icon = Properties.Resources.logo;

            CultureInfo ci = CultureInfo.InstalledUICulture;
            lang = ci.Name;

            //connect to Talk
            try
            {
                IResourceReader reader = new ResourceReader("myResources.resources");
                IDictionaryEnumerator dict = reader.GetEnumerator();
                dict.MoveNext();
                website = dict.Value.ToString();
                dict.MoveNext();
                user = dict.Value.ToString();
                dict.MoveNext();
                pass = dict.Value.ToString();
                reader.Close();
            }
            catch(Exception ex)
            {
                //file does not exist
            }


            if (user == null || user == "")
            {
                String setupMessage = "Veuillez indiquer l'adresse du serveur nextcloud (https://nextcloud.myserver.com)";
                if (lang != "fr-FR")
                {
                    setupMessage = "Please enter the Nextcloud server address (https://nextcloud.myserver.com)";
                }
                website = Interaction.InputBox(setupMessage, "Nextcloud Talk");

                if (website == null || website == "")
                {
                    //user cancelled
                    MessageBox.Show("User cancelled operation, exiting.", "Nextcloud Talk");
                    Application.Exit();
                }
                else
                {
                    if (website.Substring(website.Length - 1) == "/")
                    {
                        website = website.Substring(0, website.Length - 1);
                    }
                    Talk_Connect();
                }
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                //user connected, showing conversations
                getConversationsAsync();
                checkMessages.Enabled = true;
                login.Load(website + "/apps/spreed/");
                login.DownloadHandler = new MyCustomDownloadHandler();
                login.Visible = true;
            }
        }

        private async Task getConversationsAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    //calling API
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), website + "/ocs/v2.php/apps/spreed/api/v4/room"))
                    {
                        request.Headers.TryAddWithoutValidation("OCS-APIRequest", "true");

                        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + pass));
                        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                        var response = await httpClient.SendAsync(request);
                        String responseText = await response.Content.ReadAsStringAsync();

                        if (responseText != "Update in process.")
                        {
                            XDocument parsedResponse = XDocument.Parse(responseText);

                            oldconvos = convos;
                            convos = parsedResponse.Descendants("element");

                            //reading XML and displaying result
                            foreach (XElement item in convos)
                            {
                                //TODO
                                //item.Element("displayName").Value
                            }
                        }
                    }
                }

                error = false;
            }
            catch (Exception ex)
            {
                if (!error)
                {
                    error = true;
                    String errorMessage = "Impossible de récuperer les conversations !";
                    if (lang != "fr-FR")
                    {
                        errorMessage = "Could not retreive conversations!";
                    }
                    //MessageBox.Show(errorMessage, "Nextcloud Talk"); //removed for production
                }
            }
        }

        private void Talk_Connect()
        {
            login.RequestHandler = new CustomRequestHandler();
            login.Load(website + "/index.php/login/flow");

            login.Visible = true;
        }

        public static void getTokenAsync(String address)
        {
            try
            {
                String temp = address;
                user = temp.Substring(temp.IndexOf("&user:") + 6);
                user = user.Substring(0, user.IndexOf("&password:"));
                user = user.Replace("%40", "@");
                pass = temp.Substring(temp.IndexOf("&password:") + 10);

                IResourceWriter writer = new ResourceWriter("myResources.resources");
                writer.AddResource("website", website);
                writer.AddResource("user", user);
                writer.AddResource("pass", pass);
                writer.Close();

                String setupMessage = "Configuration terminé. Veuillez relancer le programme.";
                if (lang != "fr-FR")
                {
                    setupMessage = "Finished setting up. Please restart the program.";
                }
                MessageBox.Show(setupMessage, "Nextcloud Talk");
                Application.Exit();
            }
            catch (Exception ex)
            {
                String errorMessage = "Impossible de sauvegarder les identifiants ! Veuillez autoriser les droits d'écriture au programme.";
                if (lang != "fr-FR")
                {
                    errorMessage = "Could not store user credentials! Please allow write access to the program.";
                }
                MessageBox.Show(errorMessage, "Nextcloud Talk");
            }
        }

        private async void checkMessages_TickAsync(object sender, EventArgs e)
        {
            //check for new messages
            await getConversationsAsync();

            Boolean changed = false;
            String who = null;
            String message = null;
            if (oldconvos != null && convos != null)
            {
                if (convos.Count() == oldconvos.Count())
                {
                    for (int i = 0; i < convos.Count(); i++)
                    {
                        if ((user != convos.ElementAt(i).Element("lastMessage").Element("actorId").Value) && (oldconvos.ElementAt(i).Element("lastMessage").Element("message").Value != convos.ElementAt(i).Element("lastMessage").Element("message").Value))
                        {
                            changed = true;
                            if (convos.ElementAt(i).Element("type").Value == "2")
                            {
                                who = "[" + convos.ElementAt(i).Element("displayName").Value + "] " + convos.ElementAt(i).Element("lastMessage").Element("actorDisplayName").Value;
                            }
                            else
                            {
                                who = convos.ElementAt(i).Element("displayName").Value;
                            }
                            message = convos.ElementAt(i).Element("lastMessage").Element("message").Value;
                        }
                    }
                }
                else
                {
                    changed = true;
                }
            }

            if (changed)
            {
                String balloonTitle = who;
                String balloonText = message;

                if (who == null || message == null)
                {
                    balloonTitle = "Nouvelle conversation";
                    balloonText = "Une nouvelle conversation viens d'être créée.";
                    if (lang != "fr-FR")
                    {
                        balloonTitle = "New conversation";
                        balloonText = "A new conversation was made.";
                    }
                }

                if (!this.Visible)
                {
                    notifications.BalloonTipTitle = balloonTitle;
                    notifications.BalloonTipText = balloonText;
                    notifications.Icon = Properties.Resources.notif;
                    notifications.ShowBalloonTip(60000); //for old Windows versions. Lucky...
                }
            }
        }

        private void Talk_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                this.ShowInTaskbar = false;
                login.Load(website + "/apps/spreed/");
            }
        }

        private void Talk_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                this.ShowInTaskbar = false;
                login.Load(website + "/apps/spreed/");
            }
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifications.Visible = false;
            notifications.Dispose();
            Application.Exit();
        }

        private void notifications_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                notifications.Icon = Properties.Resources.logo;

                //System.Diagnostics.Process.Start(website + "/apps/spreed/");
                Show();
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                this.BringToFront();
            }
        }

        private void notifications_BalloonTipClicked(object sender, EventArgs e)
        {
            notifications.Icon = Properties.Resources.logo;

            //System.Diagnostics.Process.Start(website + "/apps/spreed/");
            Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.BringToFront();
        }

        private void Talk_Activated(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void resetSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String message = "Etes vous sûr de réinitialiser ?";
            if (lang != "fr-FR")
            {
                message = "Are you sure you want to reset?";
            }

            DialogResult dialogResult = MessageBox.Show(message, "Nextcloud Talk", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                File.Delete("myResources.resources");

                message = "Paramètres réinitialisés. Relancer le programme pour démarrer le paramétrage.";
                if (lang != "fr-FR")
                {
                    message = "Settings reset. Restart the program to launch setup.";
                }
                MessageBox.Show(message, "Nextcloud Talk");

                notifications.Visible = false;
                notifications.Dispose();
                Application.Exit();
            }
        }
    }

    public class CustomResourceRequestHandler : ResourceRequestHandler
    {
        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            var headers = request.Headers;
            headers["OCS-APIREQUEST"] = "true";
            request.Headers = headers;

            return CefReturnValue.Continue;
        }

        protected override void OnResourceRedirect(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {
            // We need value of this redirect url - newUrl
            // Check the value of the redirect and handle it
            if (newUrl.StartsWith("nc://login/server:"))
            {
                Talk.getTokenAsync(newUrl);
            }

            return;
        }
    }

    public class CustomRequestHandler : RequestHandler
    {
        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return new CustomResourceRequestHandler();
        }
    }
}
