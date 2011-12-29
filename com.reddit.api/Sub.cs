﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace com.reddit.api
{
    /// <summary>
    /// Represents a subreddit
    /// </summary>
    public sealed class Sub
    {
        #region // Properties //

        public string DisplayName
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public DateTime Created
        {
            get;
            set;
        }

        public DateTime CreatedUtc
        {
            get;
            set;
        }

        public bool Over18
        {
            get;
            set;
        }

        public int Subscribers
        {
            get;
            set;
        }

        public string ID
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sub"></param>
        /// <param name="after"></param>
        /// <param name="before"></param>
        /// <returns></returns>
        public static List<Sub> List(Session session, string after = "", string before = "")
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the currently subscribed sub reddit's of the user
        /// </summary>
        /// <returns></returns>
        public static List<Sub> GetMine(Session session)
        {
            var list = new List<Sub>();
            var request = new Request
            {
                Url = "http://www.reddit.com/reddits/mine.json",
                Method = "GET",                
                Cookie = session.Cookie
            };

            var json = string.Empty;
            if (request.Execute(out json) != System.Net.HttpStatusCode.OK)
            {
                // oops.
                throw new Exception(json);
            }

            var o = JObject.Parse(json);

            foreach (var sub in o["data"]["children"].Children()
                                                     .Select(sub => sub["data"]))
            {
                list.Add(new Sub
                {
                    DisplayName = sub["display_name"].ToString(),
                    Name = sub["name"].ToString(),
                    Title = sub["title"].ToString(),
                    Url = sub["url"].ToString(),
                    Created = Convert.ToInt32(sub["created"].ToString()).ToDateTime(),
                    CreatedUtc = Convert.ToInt32(sub["created_utc"].ToString()).ToDateTime(),
                    Over18 = Convert.ToBoolean(sub["over18"].ToString()),
                    Subscribers = Convert.ToInt32(sub["subscribers"].ToString()),
                    ID = sub["id"].ToString(),
                    Description = sub["description"].ToString()
                });
            }

            return list;
        }

        private const int Limit = 100;

        public static PostListing GetListing(Session session, string sub)
        {
            return GetListing(session, sub, SubSortBy.Hot, string.Empty, string.Empty);
        }

        public static PostListing GetListing(Session session, string sub, SubSortBy sort)
        {
            return GetListing(session, sub, sort, string.Empty, string.Empty);
        }

        public static PostListing GetListing(Session session, string sub, string after)
        {
            return GetListing(session, sub, SubSortBy.Hot, after, string.Empty);
        }

        public static PostListing GetListing(Session session, string sub, string after, string before)
        {
            return GetListing(session, sub, SubSortBy.Hot, after, before);
        }

        public static PostListing GetListing(Session session, string sub, SubSortBy sort, string after)
        {
            return GetListing(session, sub, sort, after, string.Empty);
        }

        public static PostListing GetListing(Session session, string sub, SubSortBy sort, string after, string before)
        {
            var url = "http://www.reddit.com/r/" + sub + "/";
            switch (sort)
            {
                case SubSortBy.Hot:
                    url += ".json?limit=" + Limit;
                    break;

                case SubSortBy.New:
                    url += "new/.json?sort=new&limit=" + Limit;
                    break;

                case SubSortBy.Rising:
                    url += "new/.json?sort=rising&limit=" + Limit;
                    break;

                case SubSortBy.TopAllTime:
                    url += "top/.json?sort=top&t=all&limit=" + Limit;
                    break;

                case SubSortBy.TopYear:
                    url += "top/.json?sort=top&t=year&limit=" + Limit;
                    break;

                case SubSortBy.TopMonth:
                    url += "top/.json?sort=top&t=month&limit=" + Limit;
                    break;

                case SubSortBy.TopWeek:
                    url += "top/.json?sort=top&t=week&limit=" + Limit;
                    break;

                case SubSortBy.TopToday:
                    url += "top/.json?sort=top&t=day&limit=" + Limit;
                    break;

                case SubSortBy.TopHour:
                    url += "top/.json?sort=top&t=hour&limit=" + Limit;
                    break;

                case SubSortBy.ControversalAllTime:
                    url += "controversial/.json?sort=controversial&t=all&limit=" + Limit;
                    break;

                case SubSortBy.ControversalYear:
                    url += "controversial/.json?sort=controversial&t=year&limit=" + Limit;
                    break;

                case SubSortBy.ControversalMonth:
                    url += "controversial/.json?sort=controversial&t=month&limit=" + Limit;
                    break;

                case SubSortBy.ControversalWeek:
                    url += "controversial/.json?sort=controversial&t=week&limit=" + Limit;
                    break;

                case SubSortBy.ControversalToday:
                    url += "controversial/.json?sort=controversial&t=day&limit=" + Limit;
                    break;

                case SubSortBy.ControversalHour:
                    url += "controversial/.json?sort=controversial&t=hour&limit=" + Limit;
                    break;
            }

            if (!string.IsNullOrEmpty(after))
                url += "&after=" + after;

            if (!string.IsNullOrEmpty(before))
                url += "&before=" + before;

            var request = new Request
            {
                Method = "GET",
                Cookie = session.Cookie,
                Url = url                 
            };

            var json = string.Empty;
            if (request.Execute(out json) != System.Net.HttpStatusCode.OK)
                throw new Exception(json);

            var o = JObject.Parse(json);

            // convert to a post listing
            var list = Post.FromJsonList(o["data"]["children"]);
            list.ModHash = o["data"]["modhash"].ToString(); 
            list.Before = o["data"]["before"].ToString();
            list.After = o["data"]["after"].ToString();            
            return list;

        }

        public static UserListing GetModerators(Session session, string sub)
        {
            throw new NotImplementedException();
        }

        public static UserListing GetContributors(Session session, string sub)
        {
            throw new NotImplementedException();
        }

        public static PostListing GetReportedPosts(Session session, string sub)
        {
            // 

            var request = new Request
            {
                Url = "http://www.reddit.com/r/" + sub + "/about/reports/.json",
                Method = "GET",
                Cookie = session.Cookie
            };

            throw new NotImplementedException();
        }

        public static void GetTrafficStats(Session session, string sub)
        {

            var request = new Request
            {
                Url = "http://www.reddit.com/r/" + sub + "/about/traffic/.json",
                Method = "GET",
                Cookie = session.Cookie
            };


            // Permission error is not thrown, just a 404
            // {"error": 404}

            throw new NotImplementedException();
        }

        public static PostListing GetSpam(Session session, string sub)
        {
            var request = new Request
            {
                Url = "http://www.reddit.com/r/" + sub + "/about/spam/.json",
                Method = "GET",
                Cookie = session.Cookie
            };

            throw new NotImplementedException();
        }

        public static LogListing GetModerationLog(Session session, string sub)
        {

            var request = new Request
            {
                Url = "http://www.reddit.com/r/" + sub + "/about/log/.json",
                Method = "GET",
                Cookie = session.Cookie
            };

            throw new NotImplementedException();
        }

        public static UserListing GetBannedUsers(Session session, string sub)
        {
            var request = new Request
            {
                Url = "http://www.reddit.com/r/" + sub + "/about/banned/.json",
                Method = "GET",
                Cookie = session.Cookie
            };

            throw new NotImplementedException();
        }

        /// <summary>
        /// A search limited to a specific sub reddit
        /// </summary>
        /// <param name="session"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<Sub> Search(Session session, string subID, string query)
        {
            // http://www.reddit.com/reddits/search.json?q=cats
            throw new NotImplementedException();

        }

    }
}
