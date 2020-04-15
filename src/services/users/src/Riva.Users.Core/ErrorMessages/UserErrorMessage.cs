namespace Riva.Users.Core.ErrorMessages
{
    public class UserErrorMessage
    {
        public const string NotFound = "User is not found.";
        public const string AlreadyExist = "User already exist.";
        public const string InsufficientPrivilegesToEditAnnouncementPreferenceLimit = "Insufficient privileges to edit AnnouncementPreferenceLimit.";
        public const string InsufficientPrivilegesToEditAnnouncementSendingFrequency = "Insufficient privileges to edit AnnouncementSendingFrequency.";
        public const string AnnouncementPreferenceLimitExceeded = "AnnouncementPreferenceLimit has been exceeded.";
    }
}