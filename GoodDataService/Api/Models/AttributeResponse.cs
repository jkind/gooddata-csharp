using System.Collections.Generic;

namespace GoodDataService.Api.Models
{
	//{
	//"attribute" : {
	//   "content" : {
	//      "pk" : [
	//         {
	//            "data" : "/gdc/md/x1wu0915c0snr9yta1xpizj53oc5tkjn/obj/4369",
	//            "type" : "col"
	//         }
	//      ],
	//      "dimension" : "/gdc/md/x1wu0915c0snr9yta1xpizj53oc5tkjn/obj/4386",
	//      "displayForms" : [
	//         {
	//            "content" : {
	//               "formOf" : "/gdc/md/x1wu0915c0snr9yta1xpizj53oc5tkjn/obj/4394",
	//               "expression" : "[/gdc/md/x1wu0915c0snr9yta1xpizj53oc5tkjn/obj/4371]",
	//               "ldmexpression" : ""
	//            },
	//            "links" : {
	//               "elements" : "/gdc/md/x1wu0915c0snr9yta1xpizj53oc5tkjn/obj/4395/elements"
	//            },
	//            "meta" : {
	//               "author" : "/gdc/account/profile/2d24cf3a578510ceac9b5c93383bff75",
	//               "uri" : "/gdc/md/x1wu0915c0snr9yta1xpizj53oc5tkjn/obj/4395",
	//               "tags" : "",
	//               "created" : "2012-05-12 00:45:10",
	//               "identifier" : "label.segment.segment",
	//               "deprecated" : "0",
	//               "summary" : "",
	//               "title" : "Segment",
	//               "category" : "attributeDisplayForm",
	//               "updated" : "2012-05-12 00:45:10",
	//               "contributor" : "/gdc/account/profile/2d24cf3a578510ceac9b5c93383bff75"
	//            }
	//         }
	//      ],
	//      "fk" : [
	//         {
	//            "data" : "/gdc/md/x1wu0915c0snr9yta1xpizj53oc5tkjn/obj/4359",
	//            "type" : "col"
	//         }
	//      ]
	//   },
	//   "meta" : {
	//      "author" : "/gdc/account/profile/2d24cf3a578510ceac9b5c93383bff75",
	//      "uri" : "/gdc/md/x1wu0915c0snr9yta1xpizj53oc5tkjn/obj/4394",
	//      "tags" : "",
	//      "created" : "2012-05-12 00:45:09",
	//      "identifier" : "attr.segment.segment",
	//      "deprecated" : "0",
	//      "summary" : "",
	//      "title" : "Segment",
	//      "category" : "attribute",
	//      "updated" : "2012-05-12 00:45:10",
	//      "contributor" : "/gdc/account/profile/2d24cf3a578510ceac9b5c93383bff75"
	//   }
	//}
	public class AttributeResponse
	{
		public Attribute Attribute { get; set; }
	}

	public class Attribute
	{
		public ObjectMeta Meta { get; set; }
		public AttributeContent Content { get; set; }
	}

	public class AttributeContent
	{
		public List<PK> Pk { get; set; }
		public string Dimension { get; set; }
		public List<DisplayForms> DisplayForms { get; set; }
	}

	public class PK
	{
		public string Data { get; set; }
		public string Type { get; set; }
	}

	public class DisplayForms
	{
		public ObjectMeta Meta { get; set; }
		public DisplayFormContent Content { get; set; }
		public AttributeLinks Links { get; set; }
	}

	public class DisplayFormContent
	{
		public string FormOf { get; set; }
		public string Expression { get; set; }
		public string LdmExpression { get; set; }
	}

	public class AttributeLinks
	{
		public string Elements { get; set; }
	}

}

