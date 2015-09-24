using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using unirest_net.http;

namespace TouchOfGoodNews
{
    public partial class frmMain : Form
    {

        List<string> titlesInDB = new List<string>();
        List<string> linksInDB = new List<string>();
        Timer timeDownload = new Timer();
        //string latFeed = @"http://pipes.yahoo.com/pipes/pipe.run?_id=5dc8a87340794a992d374f7389268bc3&_render=rss";
        //string seattleFeed = @"http://seattletimes.com/rss/seattletimes.xml";
        /*string[] latFeeds = new string[]{"http://www.latimes.com/nation/rss2.0.xml","http://www.latimes.com/world/rss2.0.xml",
            "http://www.latimes.com/business/rss2.0.xml","http://www.latimes.com/local/rss2.0.xml"};
        string[] seattleFeeds = new string[] { "http://seattletimes.com/rss/nationworld.xml", "http://seattletimes.com/rss/localnews.xml", "http://seattletimes.com/rss/businesstechnology.xml" };*/
        string[] gnnFeeds = new string[] { "http://www.goodnewsnetwork.org/category/news-business/feed/", "http://www.goodnewsnetwork.org/category/news-world/feed/", "http://www.goodnewsnetwork.org/category/news-earth/feed/" };
        public frmMain()
        {
            InitializeComponent();
            string server = "server";
            string database = "goodnews";
            string uid = "username";
            string password = "password";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            MySqlConnection myConnection = new MySqlConnection(connectionString);
            myConnection.Open();
            MySqlCommand comm = new MySqlCommand("SELECT Title FROM Articles", myConnection);
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                titlesInDB.Add(reader["Title"].ToString());
            }
            reader.Close();
            comm = new MySqlCommand("SELECT Link FROM Articles", myConnection);
            reader = comm.ExecuteReader();
            while (reader.Read())
            {
                linksInDB.Add(reader["Link"].ToString());
            }
            myConnection.Close();

            timeDownload.Tick += new EventHandler(timeDownload_Tick);
            timeDownload.Interval = 1200000;
        }


        private void downloadAllFeeds()
        {
            /*foreach (string s in latFeeds)
                downloadToDB("Los Angeles Times", s);
            foreach (string s in seattleFeeds)
                downloadToDB("Seattle Times", s);*/
            foreach (string s in gnnFeeds)
                downloadToDB("Good News Network", s);
            downloadToDB("Huffington Post", "http://www.huffingtonpost.com/feeds/verticals/good-news/index.xml");
            downloadToDB("Daily Good", "http://www.servicespace.org/rss/sol.php");
            lvTitles.Items.Clear();
            for (int i = titlesInDB.Count - 1; i >= 0; i--)
                lvTitles.Items.Add(titlesInDB[i]);
            this.Text = titlesInDB.Count.ToString();
        }


        private void cmdGo_Click(object sender, EventArgs e)
        {
            if (cmdGo.Text.Length <= 3)
            {
                downloadAllFeeds();
                timeDownload.Start();
                cmdGo.Text = "Stop";
            }
            else
            {
                timeDownload.Stop();
                cmdGo.Text = "Go";
            }
        }

        private void timeDownload_Tick(object sender, EventArgs e)
        {
            timeDownload.Stop();
            downloadAllFeeds();
            timeDownload.Start();
        }

        private void downloadToDB(string source, string feed)
        {
            WebClient w = new WebClient();
            string page = "";
            try
            {
                page = w.DownloadString(feed);
            }
            catch (Exception e)
            {
                this.Text = "Failed to Connect: " + e.Message;
                return;
            }
            //XDocument doc = XDocument.Parse(page);



            StringReader _r = new StringReader(page);
            XmlReader r = XmlReader.Create(_r);
            /*using (SqlConnection myConnection = new SqlConnection("user id=root;" +
                                   "password=root;server=.\\SQLEXPRESS;" +
                                   "Trusted_Connection=yes;" +
                                   "database=GoodNews; " +
                                  "connection timeout=10"))*/

            string server = "server";
            string database = "goodnews";
            string uid = "username";
            string password = "password";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();

                while (r.ReadToFollowing("item"))
                {
                    string category = "";
                    r.ReadToFollowing("title");
                    r.Read();
                    string title = r.Value;
                    r.ReadToFollowing("link");
                    r.Read();
                    string link = redirect(r.Value);
                    if (!link.Contains("~~~"))
                    {
                        if (link.IndexOf(".com") != link.LastIndexOf(".com"))
                            link = link.Substring(link.IndexOf(".com") + 4);
                        else if (link.IndexOf(".org") != link.LastIndexOf(".org"))
                            link = link.Substring(link.IndexOf(".org") + 4);
                    }
                    //process category out of link
                    foreach (string s in new string[] { "local", "nation", "world", "business", "health", "earth" })//{ "local", "nation", "world", "business", "sports", "entertainment", "health", "style", "travel", "opinion" }) //envelope?
                    {
                        if (link.ToLower().Contains(s)||feed.ToLower().Contains(s))
                            category = s;
                    }
                    r.ReadToFollowing("pubDate");
                    r.Read();
                    string date = r.Value;
                    date = date.Substring(0, date.IndexOf(':') + 6);

                    title = title.Replace("'", "''");
                    category = category.Replace("'", "''");
                    link = link.Replace("'", "''");

                    if (link.Contains("~~~"))
                        category = "";

                    if (link.ToLower().Contains("dailygood")||link.ToLower().Contains("huffington"))
                        category = " ";

                    if (!titlesInDB.Contains(title) && !linksInDB.Contains(link) && category.Length > 0)
                    {
                        string positive;
                        try
                        {//https://www.tweetsentimentapi.com/ maybe textalytics, or just take feeds only from the good news websites
                            HttpResponse<Dictionary<string, string>> responseTitle = Unirest.get("https://loudelement-free-natural-language-processing-service.p.mashape.com/nlp-text/?text=" + title) 
                                .header("X-Mashape-Authorization", "uu3wcz1s2hAmEpXwzkqQmlVabC4j81tT")
                                .asJson<Dictionary<string, string>>();
                            HttpResponse<Dictionary<string, string>> responseBody = Unirest.get("https://loudelement-free-natural-language-processing-service.p.mashape.com/nlp-url/?url=" + link)//Probably doesn't work
                                .header("X-Mashape-Authorization", "uu3wcz1s2hAmEpXwzkqQmlVabC4j81tT")
                                .asJson<Dictionary<string, string>>();
                            positive = (Convert.ToDouble(responseTitle.Body["sentiment-score"])+Convert.ToDouble(responseBody.Body["sentiment-score"]))+"";
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                            positive = "-1";
                        }

                        date = Convert.ToDateTime(date).ToString("yyyy-MM-dd hh:mm:ss");

                        if (source.ToLower().Contains("huffington"))
                            positive = (Convert.ToDouble(positive) - 0.6).ToString();//make HuffPo worse
                        //Instantiate a SQL Command with an INSERT query                        
                        MySqlCommand comm = new MySqlCommand("INSERT INTO articles(Title, Category, Link, Date, Source, Positive) VALUES('" + title + "','" + category + "','" + link + "','" + date + "','" + source + "','" + positive + "')", myConnection);
                        //Execute the query
                        comm.ExecuteNonQuery();

                        titlesInDB.Add(title);
                        linksInDB.Add(link);
                    }
                }
                /*lvTitles.Items.Clear();
                foreach (string s in titlesInDB)
                    lvTitles.Items.Add(s);*/
                myConnection.Close();
            }
        }

        private string redirect(string url, string prefix = "")
        {
            string newPrefix = "";
            if (url[0] != '/')
            {
                int count = 0;
                for (int i = 0; i < url.Length; i++)
                {
                    if (url[i] == '/')
                    {
                        if (count < 2)
                            count++;
                        else
                            break;
                    }

                    newPrefix += url[i];
                }
            }
            else
                return prefix + url;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.AllowAutoRedirect = false;
            webRequest.Timeout = 10000;

            string uriString = url;
            // Get the response ...
            try
            {
                using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    // Now look to see if it's a redirect
                    if ((int)webResponse.StatusCode >= 300 && (int)webResponse.StatusCode <= 399)
                        uriString = webResponse.Headers["Location"];
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return "~~~";
            }
            if (uriString != url)
            {
                return redirect(uriString, newPrefix);
            }
            else
            {
                return newPrefix + uriString;
            }
        }

        private void cmdDB_Click(object sender, EventArgs e)
        {
            HttpResponse<Dictionary<string, string>> response = Unirest.get(@"https://loudelement-free-natural-language-processing-service.p.mashape.com/nlp-url/?url=http%3A%2F%2Fwww.cnn.com%2F2013%2F05%2F09%2Fus%2Fohio-missing-women-found%2F")
                .header("X-Mashape-Authorization", "uu3wcz1s2hAmEpXwzkqQmlVabC4j81tT")
                .asJson<Dictionary<string, string>>();
            cmdDB.Text = response.Body["sentiment-score"];
            /*using (SqlConnection myConnection = new SqlConnection("user id=root;" +
                                   "password=root;server=.\\SQLEXPRESS;" +
                                   "Trusted_Connection=yes;" +
                                   "database=GoodNews; " +
                                   "connection timeout=10"))
            {
                //Open the Connection object
                myConnection.Open();
                //Instantiate a SQL Command with an INSERT query
                SqlCommand comm= new SqlCommand("DELETE FROM Articles WHERE Category='none'",myConnection);
                //Execute the query
                comm.ExecuteNonQuery();                
            }*/
        }
    }
}
