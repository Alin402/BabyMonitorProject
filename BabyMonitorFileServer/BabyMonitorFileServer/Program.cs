using System.Text.RegularExpressions;
using System.Xml.Linq;
using System;
using BabyMonitorFileServer;

AppServer app = new AppServer("6060", "C:\\Users\\balan\\Desktop\\uploaded images");
await app.ListenAsync();