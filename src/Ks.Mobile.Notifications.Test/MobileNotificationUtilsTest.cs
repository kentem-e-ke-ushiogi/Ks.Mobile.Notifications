using Ks.Web.ApiService.Client;
using Xunit;

namespace Ks.Mobile.Notifications.Test
{
    public class MobileNotificationUtilsTest
    {
        private readonly KsCloudApiClient _client;
        private readonly string _JsonFilePath;
        public MobileNotificationUtilsTest()
        {
            string path = Environment.CurrentDirectory;
            _JsonFilePath = Path.Combine(path, "NotificationReadedList.json");
            _client = KsCloudApiClient.FromInMemory();
            MobileNotificationUtils.Initialize(path, _client);
        }

        private void DeleteTemp()
        {
            if (File.Exists(_JsonFilePath))
                File.Delete(_JsonFilePath);
        }

        [Fact]
        public async Task InitializeTest()
        {
            MobileNotificationUtils.Initialize(null, null);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(MobileNotificationUtils.GetMobileNotifications);
            Assert.Equal("Call Initialize Method", exception.Message);
            exception = await Assert.ThrowsAsync<InvalidOperationException>(MobileNotificationUtils.GetImportantMobileNotifications);
            Assert.Equal("Call Initialize Method", exception.Message);
            exception = Assert.Throws<InvalidOperationException>(() => MobileNotificationUtils.SetReaded(Guid.NewGuid()));
            Assert.Equal("Call Initialize Method", exception.Message);

            MobileNotificationUtils.Initialize(Environment.CurrentDirectory, null);

            exception = await Assert.ThrowsAsync<InvalidOperationException>(MobileNotificationUtils.GetMobileNotifications);
            Assert.Equal("Call Initialize Method", exception.Message);
            exception = await Assert.ThrowsAsync<InvalidOperationException>(MobileNotificationUtils.GetImportantMobileNotifications);
            Assert.Equal("Call Initialize Method", exception.Message);
            exception = await Assert.ThrowsAsync<InvalidOperationException>(MobileNotificationUtils.HasUnreadNotification);
            Assert.Equal("Call Initialize Method", exception.Message);

            MobileNotificationUtils.Initialize(null, _client);

            exception = await Assert.ThrowsAsync<InvalidOperationException>(MobileNotificationUtils.GetMobileNotifications);
            Assert.Equal("Call Initialize Method", exception.Message);
            exception = await Assert.ThrowsAsync<InvalidOperationException>(MobileNotificationUtils.GetImportantMobileNotifications);
            Assert.Equal("Call Initialize Method", exception.Message);
            exception = await Assert.ThrowsAsync<InvalidOperationException>(MobileNotificationUtils.HasUnreadNotification);
            Assert.Equal("Call Initialize Method", exception.Message);
            exception = Assert.Throws<InvalidOperationException>(() => MobileNotificationUtils.SetReaded(Guid.NewGuid()));
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

            // 既読あり

            MobileNotificationUtils.SetReaded(res[0].Id);
            MobileNotificationUtils.SetReaded(res[5].Id);
            MobileNotificationUtils.SetReaded(res[9].Id);

            res = await MobileNotificationUtils.GetMobileNotifications();
            Assert.Equal(10, res.Length);
            Assert.True(res[0].Readed);
            Assert.False(res[1].Readed);
            Assert.False(res[2].Readed);
            Assert.False(res[3].Readed);
            Assert.False(res[4].Readed);
            Assert.True(res[5].Readed);
            Assert.False(res[6].Readed);
            Assert.False(res[7].Readed);
            Assert.False(res[8].Readed);
            Assert.True(res[9].Readed);

            DeleteTemp();
        }

        [Fact]
        public async Task GetImportantMobileNotificationsTest()
        {
            var res = await MobileNotificationUtils.GetImportantMobileNotifications();
            Assert.Equal(2, res.Length);
            Assert.Equal("【重要】お知らせ9", res[0].Title);
            Assert.Equal("【重要】お知らせ10", res[1].Title);

            // 既読あり
            MobileNotificationUtils.SetReaded(res[1].Id);
            res = await MobileNotificationUtils.GetImportantMobileNotifications();
            Assert.Equal(2, res.Length);
            Assert.False(res[0].Readed);
            Assert.True(res[1].Readed);

            DeleteTemp();
        }

        [Fact]
        public async Task HasUnreadNotificationTest()
        {
            // 全て未読
            var res = await MobileNotificationUtils.HasUnreadNotification();
            Assert.True(res);

            // 一部既読あり
            var items = await MobileNotificationUtils.GetMobileNotifications();
            MobileNotificationUtils.SetReaded(items[0].Id);
            res = await MobileNotificationUtils.HasUnreadNotification();
            Assert.True(res);

            // 全て既読
            Array.ForEach(items, (obj) => MobileNotificationUtils.SetReaded(obj.Id));
            res = await MobileNotificationUtils.HasUnreadNotification();
            Assert.False(res);

            DeleteTemp();
        }

        [Fact]
        public void SetReadedTest()
        {
            Guid id = Guid.NewGuid();
            MobileNotificationUtils.SetReaded(id);
            Assert.True(File.Exists(_JsonFilePath));

            DeleteTemp();
        }
    }
}
