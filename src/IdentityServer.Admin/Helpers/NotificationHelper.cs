﻿namespace IdentityServer.Admin.Helpers
{
    public class NotificationHelper
    {
        public const string NotificationKey = "IdentityServerAdmin.Notification";

        public class Alert
        {
            public AlertType Type { get; set; }
            public string Message { get; set; }
            public string Title { get; set; }
        }

        public enum AlertType
        {
            Info,
            Success,
            Warning,
            Error
        }
    }
}
