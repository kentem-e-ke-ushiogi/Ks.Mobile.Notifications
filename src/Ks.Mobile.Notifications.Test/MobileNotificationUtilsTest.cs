using Ks.Web.ApiService.Client;
using Xunit;

namespace Ks.Mobile.Notifications.Test
{
    public class MobileNotificationUtilsTest
    {
        private readonly KsCloudApiClient _client;
        public MobileNotificationUtilsTest()
        {
            string path = Environment.CurrentDirectory;
            _client = KsCloudApiClient.FromInMemory();
            MobileNotificationUtils.Initialize(path, _client);
        }

        [Fact]
        public async Task InitializeTest()
        {
            MobileNotificationUtils.Initialize(null, null);
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(MobileNotificationUtils.GetMobileNotifications);
            Assert.Equal("Call Initialize Method", exception.Message);

            exception = await Assert.ThrowsAsync<InvalidOperationException>(MobileNotificationUtils.GetImportantMobileNotifications);
            Assert.Equal("Call Initialize Method", exception.Message);

            MobileNotificationUtils.Initialize(Environment.CurrentDirectory, null);
            exception = await Assert.ThrowsAsync<InvalidOperationException>(MobileNotificationUtils.GetMobileNotifications);
            Assert.Equal("Call Initialize Method", exception.Message);

            exception = await Assert.ThrowsAsync<InvalidOperationException>(MobileNotificationUtils.GetImportantMobileNotifications);
            Assert.Equal("Call Initialize Method", exception.Message);

            MobileNotificationUtils.Initialize(null, _client);
            exception = await Assert.ThrowsAsync<InvalidOperationException>(MobileNotificationUtils.GetMobileNotifications);
            Assert.Equal("Call Initialize Method", exception.Message);

            exception = await Assert.ThrowsAsync<InvalidOperationException>(MobileNotificationUtils.GetImportantMobileNotifications);
            Assert.Equal("Call Initialize Method", exception.Message);
        }

        [Fact]
        public async Task GetAllNotificationsTest()
        {
            var res = await MobileNotificationUtils.GetMobileNotifications();
            Assert.Equal(10, res.Length);
            Assert.Equal("【重要】お知らせ9", res[0].Title);
            Assert.Equal("【重要】お知らせ10", res[1].Title);
            Assert.Equal("お知らせ8", res[2].Title);
            Assert.Equal("お知らせ7", res[3].Title);
            Assert.Equal("お知らせ6", res[4].Title);
            Assert.Equal("お知らせ5", res[5].Title);
            Assert.Equal("お知らせ4", res[6].Title);
            Assert.Equal("お知らせ3", res[7].Title);
            Assert.Equal("お知らせ2", res[8].Title);
            Assert.Equal("お知らせ1", res[9].Title);
        }

        [Fact]
        public async Task GetImportantMobileNotificationsTest()
        {
            var res = await MobileNotificationUtils.GetImportantMobileNotifications();
            Assert.Equal(2, res.Length);
            Assert.Equal("【重要】お知らせ9", res[0].Title);
            Assert.Equal("【重要】お知らせ10", res[1].Title);
        }

    }
}
