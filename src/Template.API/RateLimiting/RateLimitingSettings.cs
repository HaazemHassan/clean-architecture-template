namespace Template.API.RateLimiting {
    public class RateLimitingSettings {
        public const string SectionName = "RateLimitingSettings";

        public DefaultLimiterSettings DefaultLimiter { get; set; } = new();
        public LoginLimiterSettings LoginLimiter { get; set; } = new();
        public int RetryAfterSeconds { get; set; }
    }

    public class DefaultLimiterSettings {
        public int WindowMinutes { get; set; }
        public int PermitLimit { get; set; }
        public int QueueLimit { get; set; }
        public int SegmentsPerWindow { get; set; }
    }

    public class LoginLimiterSettings {
        public int WindowMinutes { get; set; }
        public int PermitLimit { get; set; }
        public int QueueLimit { get; set; }
        public int SegmentsPerWindow { get; set; }
    }
}
