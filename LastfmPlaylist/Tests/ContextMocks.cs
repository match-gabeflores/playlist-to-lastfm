using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace Tests
{
    public class ContextMocks
    {
        public Mock<HttpContextBase> HttpContext { get; private set; }
        public Mock<HttpRequestBase> Request { get; private set; }
        public Mock<HttpResponseBase> Response { get; private set; }
        public Mock<ControllerContext> ControllerContext { get; private set; }
        public RouteData RouteData { get; private set; }
        public Mock<HttpPostedFileBase> HttpPostedFile { get; private set; }
        public ContextMocks(Controller onController)
        {
            // Define all the common context objects, plus relationships between them
            HttpContext = new Mock<HttpContextBase>();
            Request = new Mock<HttpRequestBase>();
            Response = new Mock<HttpResponseBase>();
            HttpContext.Setup(x => x.Request).Returns(Request.Object);
            HttpContext.Setup(x => x.Response).Returns(Response.Object);
            
            HttpContext.Setup(x => x.Session).Returns(new FakeSessionState());
            Request.Setup(x => x.Cookies).Returns(new HttpCookieCollection());
            Response.Setup(x => x.Cookies).Returns(new HttpCookieCollection());
            Request.Setup(x => x.QueryString).Returns(new NameValueCollection());
            Request.Setup(x => x.Form).Returns(new NameValueCollection());
            ControllerContext.Setup(x => x.HttpContext.Request.Files[0]).Returns(HttpPostedFile.Object);
            
            
            // todo - in future, separate into new method with parameters? to allow testing fileupload
            ControllerContext = new Mock<ControllerContext>();
            HttpPostedFile = new Mock<HttpPostedFileBase>();


            var context = new Mock<ControllerContext>();
            context.Setup(i => i.HttpContext.Request.Files.Count).Returns(1);
            context.Setup(i => i.HttpContext.Request.Files[0]).Returns(HttpPostedFile.Object);


            // Apply the mock context to the supplied controller instance
            RequestContext rc = new RequestContext(HttpContext.Object, new RouteData());
            onController.ControllerContext = new ControllerContext(rc, onController);
        }
        // Use a fake HttpSessionStateBase, because it's hard to mock it with Moq
        private class FakeSessionState : HttpSessionStateBase
        {
            Dictionary<string, object> items = new Dictionary<string, object>();
            public override object this[string name]
            {
                get { return items.ContainsKey(name) ? items[name] : null; }
                set { items[name] = value; }
            }
        }
    }
}
