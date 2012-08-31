﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Hosting;
using System.Web.Optimization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsQuery.Mvc.Tests.Controllers;
using CsQuery.Mvc;
using CsQuery.ExtensionMethods.Internal;

namespace CsQuery.Mvc.Tests
{
    [TestClass]
    public class ScriptHelper
    {

        private string MapPath(string path)
        {
            return path;
        }

        [TestMethod]
        public void MoveScriptLocation()
        {
            CQ doc = CQ.Create(@"
<html>
    <head>
        <script src='~/script' class='inhead'></script>
    </head>
    <body>
        <div></div>
        <script src='~/script' class='inbody' data-location='head'></script>
    </body>
");
            var mgr = new ScriptManager(MapPath);
            mgr.Options = CsQueryViewEngineOptions.ProcessAllScripts | CsQueryViewEngineOptions.IgnoreMissingScripts;

            Assert.AreEqual(1,doc["head"].Children().Length);
            
            mgr.ResolveScriptDependencies(doc);


            Assert.AreEqual(3, doc["head"].Children().Length);
            Assert.AreEqual(doc["[data-location='head']"][0].ParentNode,doc["head"][0]);

            var libScript =doc["head > .csquery-generated"];
            Assert.AreEqual(1, libScript.Length, "The library include was added.");
            Assert.AreEqual(libScript[0].ParentNode.ChildElements.First(), libScript[0],"The library include was first element.");

        }

         
    }
}