﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.reddit.api.tests
{
    [TestClass]
    public class CommentTestClass
    {
        [TestMethod]
        public void GetCommentsForPost()
        {
            // login using regular creds
            var session = User.Login(Configuration.GetKey("username"), Configuration.GetKey("password"));

            // a story about a nice guy who donated his tickets to a child at christmas
            var postID = "nndrb";

            // get a post with comments
            var comments = Comment.GetCommentsForPost(session, postID);

            // list all them
            Assert.IsNotNull(comments);
            Assert.IsTrue(comments.Count > 0);
        }


        [TestMethod]
        public void SubmitComment()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void Vote_UpDownNull()
        {
            Assert.Fail();
        }
         

        [TestMethod]
        public void Hide_UnHide()
        {
            Assert.Fail();
        }
        
        [TestMethod]
        public void Save_UnSave()
        {
            Assert.Fail();
        }
         
    }
}
