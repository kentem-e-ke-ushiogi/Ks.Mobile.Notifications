using Xunit;

namespace Ks.Mobile.Notifications.Test
{
    public class NotificationModelExtentionsTest
    {
        [Fact]
        public void ToMobileModelTest()
        {
            NoticeModel expected = new()
            {
                Id = Guid.NewGuid(),
                Title = "お知らせ",
                Link = "https://www.kentem.jp/support/20230711_01/",
                SeverityLevel = SeverityLevel.None,
                Date = new DateTime(2024, 6, 17),
                IsNotifyApp = true,
                CustomerId = null,
                Detail = "",
                IsPublished = true,
                PublishDate = DateTime.Now,
                RelatedApplications = AppFlags.SiteBox,
            };

            var actual = expected.ToMobileModel();
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Link, actual.Link);
            Assert.Equal(expected.Date, actual.Date);
            Assert.False(actual.Important);

            // 重要なお知らせ
            expected.SeverityLevel = SeverityLevel.Important;
            actual = expected.ToMobileModel();
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Link, actual.Link);
            Assert.Equal(expected.Date, actual.Date);
            Assert.True(actual.Important);

        }
    }
}
