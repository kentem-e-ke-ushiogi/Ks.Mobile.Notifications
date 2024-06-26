﻿using System;

namespace Ks.Mobile.Notifications
{
    internal static class NotificationModelExtentions
    {
        internal static MobileNotificationModel ToMobileModel(this NoticeModel model)
        {
            return new MobileNotificationModel()
            {
                Id = model.Id,
                Date = model.Date,
                Title = model.Title,
                Important = model.SeverityLevel == SeverityLevel.Important,
                Link = model.Link,
            };
        }
    }

    // KsDo:サーバーから来る仮のデータを定義。APIができたら下記全てを削除します。

    internal class NoticeModel
    {
        /// <summary>ID</summary>
        public Guid Id { get; set; }

        /// <summary>日付</summary>
        public DateTime Date { get; set; }

        /// <summary>タイトル</summary>
        public string Title { get; set; }

        /// <summary>公開中</summary>
        public bool IsPublished { get; set; }

        /// <summary>アプリ通知するか</summary>
        public bool IsNotifyApp { get; set; }

        /// <summary>重要度レベル</summary>
        public SeverityLevel SeverityLevel { get; set; }

        /// <summary>
        /// <para>外部リンク</para>
        /// <para>外部リンクが null の場合は Detail の値をユーザーに表示します。</para></summary>
        public string Link { get; set; }

        /// <summary>詳細</summary>
        public string Detail { get; set; }

        /// <summary>予約公開日</summary>
        public DateTime? PublishDate { get; set; }

        /// <summary>公開先の会社ID</summary>
        public int? CustomerId { get; set; }

        /// <summary>対象アプリケーション</summary>
        public AppFlags RelatedApplications { get; set; }
    }

    /// <summary>アプリ毎のお知らせ識別フラグ</summary>
    [Flags]
    public enum AppFlags
    {
        /// <summary>SiteBox</summary>
        SiteBox = 4,
        /// <summary>全て</summary>
        All = ~0,
    }

    internal enum SeverityLevel
    {
        None = 0,
        Important = 1,
    }
}
