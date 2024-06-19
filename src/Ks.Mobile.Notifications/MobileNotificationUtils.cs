using Ks.Web.ApiService.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ks.Mobile.Notifications
{
    /// <summary>モバイル用お知らせ機能</summary>
    public static class MobileNotificationUtils
    {
        private const string JsonFileName = "NotificationReadedList.json";
        private static KsCloudApiClient KsCloudApi;
        private static string JsonFolderPath;

        /// <summary>初回設定</summary>
        /// <param name="jsonFolderPath">既読リスト保存先</param>
        /// <param name="apiClient">通信API</param>
        public static void Initialize(string jsonFolderPath, KsCloudApiClient apiClient)
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

            var readedList = GetReadedList();

            // KsDo:サーバーへの問い合わせ処理を実装
            var items = await GetDummyNoticeModelFromServer();
            List<MobileNotificationModel> list = new List<MobileNotificationModel>();
            // 公開日順で新しいものから並び替え
            foreach (var item in items.OrderByDescending(p => p.Date))
            {
                var model = item.ToMobileModel();
                model.Readed = readedList.Contains(model.Id);
                list.Add(model);
            }
            if (important)
                return list.Where(p => p.Important).ToArray();
            return list.ToArray();
        }

        /// <summary> 未読のお知らせがある </summary>
        public static async Task<bool> HasUnreadNotification()
        {
            if (string.IsNullOrEmpty(JsonFolderPath) || KsCloudApi == null)
                throw new InvalidOperationException("Call Initialize Method");

            var res = await GetMobileNoticeModels(false);
            return res.Length > 0 && res.Any(p => !p.Readed);
        }

        /// <summary> 既読を付ける</summary>
        public static void SetReaded(Guid id)
        {
            if (string.IsNullOrEmpty(JsonFolderPath))
                throw new InvalidOperationException("Call Initialize Method");

            List<Guid> list = GetReadedList();
            if (list.Contains(id))
                return;
            list.Add(id);
            string filePath = Path.Combine(JsonFolderPath, JsonFileName);
            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText(filePath, json);
        }

        private static List<Guid> GetReadedList()
        {
            string filePath = Path.Combine(JsonFolderPath, JsonFileName);
            if (!File.Exists(filePath))
                return new List<Guid>();
            string json = File.ReadAllText(filePath);
            List<Guid> list = JsonConvert.DeserializeObject<List<Guid>>(json);
            return list ?? new List<Guid>();
        }

        // KsDo:確認用にダミーデータを作成。APIが組み込まれたらテストケースへ移動する

        private static async Task<NoticeModel[]> GetDummyNoticeModelFromServer()
        {
            var task = await Task.Run(() =>
            {
                List<NoticeModel> list = new List<NoticeModel>();
                for (int i = 1; i <= 8; i++)
                {
                    var item = new NoticeModel()
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-00000000000" + i),
                        Title = "お知らせ" + i,
                        Link = "https://www.kentem.jp/support/20230711_01/",
                        SeverityLevel = SeverityLevel.None,
                        Date = DateTime.Now.AddDays(i),
                    };
                    list.Add(item);
                }
                var item2 = new NoticeModel()
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000009"),
                    Title = "【重要】お知らせ9",
                    Link = "https://www.kentem.jp/support/20230711_01/",
                    SeverityLevel = SeverityLevel.Important,
                    Date = DateTime.Now.AddDays(100),
                };
                list.Add(item2);
                var item3 = new NoticeModel()
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000010"),
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
