using Crystal.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Crystal.Tests
{
    public class CrystalMiddlewareTests
    {
        private CrystalMiddleware<long> _sut;

        [TestInitialize]
        public void SetUp()
        {
            var mockDelegate = new Mock<RequestDelegate>();
            _sut = new CrystalMiddleware<long>(mockDelegate.Object);
        }

        [TestMethod]
        public void Invoke_NoKeyProvided_InvalidRequestReturned()
        {
        }

        private static HttpContext MockContext => new Mock<HttpContext>().Object;
    }
}
