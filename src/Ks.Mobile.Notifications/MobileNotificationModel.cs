using System;

namespace Ks.Mobile.Notifications
{
    /// <summary>モバイル用お知らせモデル</summary>
    public class MobileNotificationModel
    {
        /// <summary>ID</summary>
        public Guid Id { get; set; }

        /// <summary>日付</summary>
        public DateTime Date { get; set; }

        /// <summary>タイトル</summary>
        public string Title { get; set; }

        /// <summary>重要なお知らせ</summary>
        public bool Important { get; set; }

        /// <summary>外部リンク</summary>
        public string Link { get; set; }

        /// <summary>既読済み</summary>
        public bool Readed { get; set; }
    }
}
