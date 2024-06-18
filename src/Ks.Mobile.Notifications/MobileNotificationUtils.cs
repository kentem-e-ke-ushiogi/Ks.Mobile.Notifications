using Ks.Web.ApiService.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ks.Mobile.Notifications
{
    /// <summary>モバイル用お知らせ機能</summary>
    public class MobileNotificationUtils
    {
        private static KsCloudApiClient KsCloudApi;
        private static string JsonFolderPath;

        /// <summary>既読リスト保存先 </summary>
        /// <param name="jsonFolderPath"></param>
        public static void Initialize(string jsonFolderPath)
        {
            var api = KsCloudApiClient.FromWebRequest(null);
            Initialize(jsonFolderPath, api);
        }

        internal static void Initialize(string jsonFolderPath, KsCloudApiClient apiClient)
        {
            JsonFolderPath = jsonFolderPath;
            KsCloudApi = apiClient;
        }

        /// <summary> お知らせ一覧取得</summary>
        public static Task<MobileNotificationModel[]> GetMobileNotifications()
        {
            return GetMobileNoticeModels(false);
        }

        /// <summary> 重要なお知らせ一覧取得</summary>
        public static Task<MobileNotificationModel[]> GetImportantMobileNotifications()
        {
            return GetMobileNoticeModels(true);
        }

        private static async Task<MobileNotificationModel[]> GetMobileNoticeModels(bool important)
        {
            if (string.IsNullOrEmpty(JsonFolderPath) || KsCloudApi == null)
                throw new InvalidOperationException("Call Initialize Method");

            // KoDo:サーバーへの問い合わせ処理を実装
            var items = await GetDummyNoticeModelFromServer();
            if (items.Length == 0)
                return new MobileNotificationModel[0];
            
            List<MobileNotificationModel> list = new List<MobileNotificationModel>();
            // 公開日順で新しいものから並び替え
            foreach (var item in items.OrderByDescending(p => p.Date))
            {
                var model = item.ToMobileModel();
                list.Add(model);
            }
            if (important)
                return list.Where(p => p.Important).ToArray();
            return list.ToArray();
        }

        private static async Task<NoticeModel[]> GetDummyNoticeModelFromServer()
        {
            var task = await Task.Run(() =>
            {
                List<NoticeModel> list = new List<NoticeModel>();
                for (int i = 1; i <= 8; i++)
                {
                    var item = new NoticeModel()
                    {
                        Id = Guid.NewGuid(),
                        Title = "お知らせ" + i,
                        Link = "https://www.kentem.jp/support/20230711_01/",
                        SeverityLevel = SeverityLevel.None,
                        Date = DateTime.Now.AddDays(i),
                    };
                    list.Add(item);
                }
                var item2 = new NoticeModel()
                {
                    Id = Guid.NewGuid(),
                    Title = "【重要】お知らせ9",
                    Link = "https://www.kentem.jp/support/20230711_01/",
                    SeverityLevel = SeverityLevel.Important,
                    Date = DateTime.Now.AddDays(100),
                };
                list.Add(item2);
                var item3 = new NoticeModel()
                {
                    Id = Guid.NewGuid(),
                    Title = "【重要】お知らせ10",
                    Link = "https://www.kentem.jp/support/20230711_01/",
                    SeverityLevel = SeverityLevel.Important,
                    Date = DateTime.Now.AddDays(10),
                };
                list.Add(item3);
                return list.ToArray();
            });
            return task;
        }
    }
}
