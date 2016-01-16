using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace MVCControlsExtensions.UnitTests.Extensions
{
    public static class HttpHelper
    {
        public static void SetCurrent()
        {
            var memStream = new MemoryStream();
            var txtWriter = new StreamWriter(memStream);
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost", "abc=123"),
                new HttpResponse(txtWriter));
        }

        public static HtmlHelper<T> CreateHtmlHelper<T>(ViewDataDictionary vd)
            where T : new()
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Test");
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>(
                new ControllerContext(
                    new Mock<HttpContextBase>().Object,
                    routeData,
                    new Mock<ControllerBase>().Object
                    ),
                new Mock<IView>().Object,
                vd,
                new TempDataDictionary(),
                new StreamWriter(new MemoryStream())
                );

            ViewContext vc = new ViewContext();

            Mock<IViewDataContainer> mockDataContainer = new Mock<IViewDataContainer>();
            mockDataContainer.Setup(c => c.ViewData).Returns(vd);
            mockViewContext.Setup(x => x.ViewData).Returns(vd);
            IDictionary items = new Hashtable(); ;
            mockViewContext.Setup(x => x.HttpContext).Returns(GetMoqHttpContextBase(items: items));
            mockViewContext.Setup(x => x.RouteData).Returns(routeData);

            return new HtmlHelper<T>(mockViewContext.Object, mockDataContainer.Object);
        }

        private const string DesktopUserAgent = "Mozilla/4.0 (compatible; MSIE 6.1; Windows XP)";

        public static HttpContextBase GetMoqHttpContextBase(
            NameValueCollection queryString = null,
            HttpCookieCollection cookies = null,
            IPrincipal principal = null,
            NameValueCollection headers = null,
            IDictionary items = null)
        {

            var moqHttpContext = new Mock<HttpContextBase>();
            var moqRequest = new Mock<HttpRequestBase>();
            var moqResponse = new Mock<HttpResponseBase>();
            var moqServer = new Mock<HttpServerUtilityBase>();
            queryString = queryString ?? new NameValueCollection();
            cookies = cookies ?? new HttpCookieCollection();
            items = items ?? new Hashtable();


            moqRequest.SetupGet(x => x.QueryString).Returns(queryString);
            moqRequest.SetupGet(x => x.Cookies).Returns(cookies);
            moqRequest.SetupGet(x => x.Headers).Returns(headers);
            moqServer.Setup(x => x.MapPath(It.IsAny<string>())).Returns("http://tempuri.com");
            moqResponse.SetupGet(x => x.Cookies).Returns(cookies);

            var httpBrowserCapabilitiesBase = new Mock<HttpBrowserCapabilitiesBase>();
            moqRequest.Setup(x => x.Browser).Returns(httpBrowserCapabilitiesBase.Object);

            moqHttpContext.Setup(x => x.Request).Returns(moqRequest.Object);
            moqHttpContext.Setup(x => x.Response).Returns(moqResponse.Object);
            moqHttpContext.Setup(x => x.Server).Returns(moqServer.Object);
            moqHttpContext.SetupGet(x => x.Cache).Returns(HttpRuntime.Cache);

            moqRequest.Setup(x => x.UserAgent).Returns(DesktopUserAgent);

            //items[_browserOverrideKey] = "Test";
            moqHttpContext.Setup(x => x.Items).Returns(items);

            //moqHttpContext.Setup(x => x.User).Returns(() => new TestPrincipal());
            principal = principal ?? new ControllerExtensions.TestPrincipal();
            moqHttpContext.Setup(x => x.User).Returns(() => principal);
            return moqHttpContext.Object;
        }
    }

    internal sealed class MetadataHelper
    {
        public Mock<ModelMetadata> Metadata { get; set; }
        public Mock<ModelMetadataProvider> MetadataProvider { get; set; }

        public MetadataHelper()
        {
            MetadataProvider = new Mock<ModelMetadataProvider>();
            Metadata = new Mock<ModelMetadata>(MetadataProvider.Object, null, null, typeof(object), null);

            MetadataProvider.Setup(p => p.GetMetadataForProperties(It.IsAny<object>(), It.IsAny<Type>()))
                .Returns(new ModelMetadata[0]);
            MetadataProvider.Setup(p => p.GetMetadataForProperty(It.IsAny<Func<object>>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(Metadata.Object);
            MetadataProvider.Setup(p => p.GetMetadataForType(It.IsAny<Func<object>>(), It.IsAny<Type>()))
                .Returns(Metadata.Object);
        }
    }
}