using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodDataService.Api;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class MandatoryUserFilterActionsTests
	{

		// dev = us03pustmnl2z7c9jm9vy1f9qiy2ve36
		// staging = ljez1ya6tbjq9guoyq3ykregqt140lss
		[Test]
		public void Edit()
		{
			var uri = @"/gdc/md/us03pustmnl2z7c9jm9vy1f9qiy2ve36/obj/146618";
			var oldExpression = @"[/gdc/md/us03pustmnl2z7c9jm9vy1f9qiy2ve36/obj/113446]=[/gdc/md/us03pustmnl2z7c9jm9vy1f9qiy2ve36/obj/113446/elements?id=27]";
			var oldTitle = @"qagraceechen+spa1@gmail.com - Sourcing Partner ID";

			var newExpression = @"[/gdc/md/us03pustmnl2z7c9jm9vy1f9qiy2ve36/obj/113446]=[/gdc/md/us03pustmnl2z7c9jm9vy1f9qiy2ve36/obj/113446/elements?id=86407]";
			var newTitle = @"test change title";

			var actions = new MandatoryUserFilterActions();
			actions.Update(uri, newTitle, newExpression);
		}
	}
}


//{
//   "userFilter" : {
//      "content" : {
//         "objects" : [
//            {
//               "link" : "/gdc/md/us03pustmnl2z7c9jm9vy1f9qiy2ve36/obj/113446",
//               "author" : "/gdc/account/profile/5e18e975328a472dd2fbf21c8fca069f",
//               "tags" : "",
//               "created" : "2012-08-16 18:46:18",
//               "deprecated" : "0",
//               "summary" : "",
//               "title" : "Sourcing Partner ID",
//               "category" : "attribute",
//               "updated" : "2012-08-16 18:46:19",
//               "contributor" : "/gdc/account/profile/5e18e975328a472dd2fbf21c8fca069f"
//            },
//            {
//               "category" : "attributeElement",
//               "title" : "25",
//               "attributeUri" : "/gdc/md/us03pustmnl2z7c9jm9vy1f9qiy2ve36/obj/113446",
//               "uri" : "/gdc/md/us03pustmnl2z7c9jm9vy1f9qiy2ve36/obj/113446/elements?id=27"
//            }
//         ],
//         "tree" : {
//            "content" : [
//               {
//                  "value" : "/gdc/md/us03pustmnl2z7c9jm9vy1f9qiy2ve36/obj/113446",
//                  "position" : {
//                     "column" : 1,
//                     "line" : 2
//                  },
//                  "type" : "attribute object"
//               },
//               {
//                  "value" : "/gdc/md/us03pustmnl2z7c9jm9vy1f9qiy2ve36/obj/113446/elements?id=27",
//                  "position" : {
//                     "column" : 55,
//                     "line" : 2
//                  },
//                  "type" : "attributeElement object"
//               }
//            ],
//            "position" : {
//               "column" : 54,
//               "line" : 2
//            },
//            "type" : "="
//         },
//         "expression" : "[/gdc/md/us03pustmnl2z7c9jm9vy1f9qiy2ve36/obj/113446]=[/gdc/md/us03pustmnl2z7c9jm9vy1f9qiy2ve36/obj/113446/elements?id=27]"
//      },
//      "meta" : {
//         "author" : "/gdc/account/profile/5e18e975328a472dd2fbf21c8fca069f",
//         "uri" : "/gdc/md/us03pustmnl2z7c9jm9vy1f9qiy2ve36/obj/146618",
//         "tags" : "",
//         "created" : "2012-09-19 04:00:22",
//         "identifier" : "ai6ArYAviqQZ",
//         "deprecated" : "0",
//         "summary" : "",
//         "title" : "qagraceechen+spa1@gmail.com - Sourcing Partner ID",
//         "category" : "userFilter",
//         "updated" : "2012-09-19 04:00:22",
//         "contributor" : "/gdc/account/profile/5e18e975328a472dd2fbf21c8fca069f"
//      }
//   }
//}