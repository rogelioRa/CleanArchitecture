using Xunit;

namespace SocialMedia.UnitTest.Services
{

    public class UserServiceTest
    {
        [Fact]
        public void ShouldBeNull()
        {
            string entrada = string.Empty;
            var result = string.Empty;
            Assert.Equal(string.Empty, result);
        }
    }
}