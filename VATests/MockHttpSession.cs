using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VATests
{
    public class MockHttpSession : HttpSessionStateBase
    {
        Dictionary<string, object> m_SessionStorage = new Dictionary<string, object>();

        public override object this[string name]
        {
            get { return m_SessionStorage[name]; }
            set { m_SessionStorage[name] = value; }
        }
    }

    //In the MVCMockHelpers I modified the FakeHttpContext() method as shown below
   /* public static HttpContextBase FakeHttpContext()
    {
        var context = new Mock<HttpContextBase>();
        var request = new Mock<HttpRequestBase>();
        var response = new Mock<HttpResponseBase>();
        var session = new MockHttpSession();
        var server = new Mock<HttpServerUtilityBase>();

        context.Setup(ctx => ctx.Request).Returns(request.Object);
        context.Setup(ctx => ctx.Response).Returns(response.Object);
        context.Setup(ctx => ctx.Session).Returns(session);
        context.Setup(ctx => ctx.Server).Returns(server.Object);

        return context.Object;
    }*/
}