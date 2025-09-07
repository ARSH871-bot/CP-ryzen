using System;
using System.Collections.Generic;
using System.Timers;

namespace ShippingManagementSystem
{
    /// <summary>
    /// Manages user sessions with timeout and security features
    /// </summary>
    public static class SessionManager
    {
        private static Dictionary<string, SessionInfo> activeSessions = new Dictionary<string, SessionInfo>();
        private static readonly int SessionTimeoutMinutes = 30;
        private static readonly int MaxConcurrentSessions = 3;
        private static Timer cleanupTimer;

        static SessionManager()
        {
            InitializeCleanupTimer();
        }

        /// <summary>
        /// Initialize timer for automatic session cleanup
        /// </summary>
        private static void InitializeCleanupTimer()
        {
            cleanupTimer = new Timer(300000); // 5 minutes
            cleanupTimer.Elapsed += CleanupExpiredSessions;
            cleanupTimer.AutoReset = true;
            cleanupTimer.Start();
        }

        /// <summary>
        /// Create a new user session
        /// </summary>
        public static string CreateSession(User user, string ipAddress = "")
        {
            try
            {
                // Check for existing sessions for this user
                CleanupUserSessions(user.Username);

                // Check concurrent session limit
                int userSessionCount = GetActiveSessionCount(user.Username);
                if (userSessionCount >= MaxConcurrentSessions)
                {
                    // Remove oldest session
                    RemoveOldestUserSession(user.Username);
                }

                string sessionId = Guid.NewGuid().ToString();
                activeSessions[sessionId] = new SessionInfo
                {
                    SessionId = sessionId,
                    User = user,
                    LoginTime = DateTime.Now,
                    LastActivity = DateTime.Now,
                    IpAddress = ipAddress,
                    IsActive = true,
                    FailedAttempts = 0
                };

                // Log session creation
                ErrorHandler.LogInfo($"Session created for user: {user.Username} from IP: {ipAddress}", "SessionManager");

                return sessionId;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Create Session", false);
                return null;
            }
        }

        /// <summary>
        /// Validate if session is still active and update last activity
        /// </summary>
        public static bool ValidateSession(string sessionId)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionId) || !activeSessions.ContainsKey(sessionId))
                    return false;

                var session = activeSessions[sessionId];

                // Check if session is expired
                if (DateTime.Now.Subtract(session.LastActivity).TotalMinutes > SessionTimeoutMinutes)
                {
                    EndSession(sessionId, "Session timeout");
                    return false;
                }

                // Check if session is still active
                if (!session.IsActive)
                    return false;

                // Update last activity
                session.LastActivity = DateTime.Now;
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Validate Session", false);
                return false;
            }
        }

        /// <summary>
        /// Get user information from session
        /// </summary>
        public static User GetSessionUser(string sessionId)
        {
            if (ValidateSession(sessionId))
            {
                return activeSessions[sessionId].User;
            }
            return null;
        }

        /// <summary>
        /// End a specific session
        /// </summary>
        public static void EndSession(string sessionId, string reason = "User logout")
        {
            try
            {
                if (activeSessions.ContainsKey(sessionId))
                {
                    var session = activeSessions[sessionId];
                    session.IsActive = false;
                    session.LogoutTime = DateTime.Now;

                    ErrorHandler.LogInfo($"Session ended for user: {session.User.Username}, Reason: {reason}", "SessionManager");

                    activeSessions.Remove(sessionId);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "End Session", false);
            }
        }

        /// <summary>
        /// End all sessions for a specific user
        /// </summary>
        public static void EndAllUserSessions(string username, string reason = "Admin action")
        {
            try
            {
                var sessionsToRemove = new List<string>();

                foreach (var kvp in activeSessions)
                {
                    if (kvp.Value.User.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                    {
                        sessionsToRemove.Add(kvp.Key);
                    }
                }

                foreach (string sessionId in sessionsToRemove)
                {
                    EndSession(sessionId, reason);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "End All User Sessions", false);
            }
        }

        /// <summary>
        /// Get count of active sessions for a user
        /// </summary>
        private static int GetActiveSessionCount(string username)
        {
            int count = 0;
            foreach (var session in activeSessions.Values)
            {
                if (session.User.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && session.IsActive)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Remove oldest session for a user
        /// </summary>
        private static void RemoveOldestUserSession(string username)
        {
            string oldestSessionId = null;
            DateTime oldestTime = DateTime.MaxValue;

            foreach (var kvp in activeSessions)
            {
                if (kvp.Value.User.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                    kvp.Value.LoginTime < oldestTime)
                {
                    oldestTime = kvp.Value.LoginTime;
                    oldestSessionId = kvp.Key;
                }
            }

            if (oldestSessionId != null)
            {
                EndSession(oldestSessionId, "Concurrent session limit exceeded");
            }
        }

        /// <summary>
        /// Clean up expired sessions for a specific user
        /// </summary>
        private static void CleanupUserSessions(string username)
        {
            var expiredSessions = new List<string>();

            foreach (var kvp in activeSessions)
            {
                if (kvp.Value.User.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    if (DateTime.Now.Subtract(kvp.Value.LastActivity).TotalMinutes > SessionTimeoutMinutes)
                    {
                        expiredSessions.Add(kvp.Key);
                    }
                }
            }

            foreach (string sessionId in expiredSessions)
            {
                EndSession(sessionId, "Session cleanup - expired");
            }
        }

        /// <summary>
        /// Automatic cleanup of expired sessions
        /// </summary>
        private static void CleanupExpiredSessions(object sender, ElapsedEventArgs e)
        {
            try
            {
                var expiredSessions = new List<string>();

                foreach (var kvp in activeSessions)
                {
                    if (DateTime.Now.Subtract(kvp.Value.LastActivity).TotalMinutes > SessionTimeoutMinutes)
                    {
                        expiredSessions.Add(kvp.Key);
                    }
                }

                foreach (string sessionId in expiredSessions)
                {
                    EndSession(sessionId, "Automatic cleanup - expired");
                }

                if (expiredSessions.Count > 0)
                {
                    ErrorHandler.LogInfo($"Cleaned up {expiredSessions.Count} expired sessions", "SessionManager");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Session Cleanup", false);
            }
        }

        /// <summary>
        /// Get session statistics
        /// </summary>
        public static SessionStatistics GetSessionStatistics()
        {
            try
            {
                return new SessionStatistics
                {
                    ActiveSessions = activeSessions.Count,
                    TotalSessionsCreatedToday = GetTodaySessionCount(),
                    AverageSessionDuration = GetAverageSessionDuration()
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get Session Statistics", false);
                return new SessionStatistics();
            }
        }

        private static int GetTodaySessionCount()
        {
            // This would typically come from database logs
            // For now, return current active count
            return activeSessions.Count;
        }

        private static TimeSpan GetAverageSessionDuration()
        {
            if (activeSessions.Count == 0)
                return TimeSpan.Zero;

            TimeSpan totalDuration = TimeSpan.Zero;
            foreach (var session in activeSessions.Values)
            {
                totalDuration = totalDuration.Add(DateTime.Now.Subtract(session.LoginTime));
            }

            return TimeSpan.FromTicks(totalDuration.Ticks / activeSessions.Count);
        }

        /// <summary>
        /// Record failed login attempt
        /// </summary>
        public static void RecordFailedAttempt(string username, string ipAddress)
        {
            try
            {
                ErrorHandler.LogInfo($"Failed login attempt for user: {username} from IP: {ipAddress}", "SessionManager");

                // Additional security logic can be added here:
                // - IP blocking after multiple failures
                // - Account lockout
                // - Suspicious activity detection
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Record Failed Attempt", false);
            }
        }

        /// <summary>
        /// Dispose resources when application closes
        /// </summary>
        public static void Dispose()
        {
            try
            {
                cleanupTimer?.Stop();
                cleanupTimer?.Dispose();

                // End all active sessions
                var allSessions = new List<string>(activeSessions.Keys);
                foreach (string sessionId in allSessions)
                {
                    EndSession(sessionId, "Application shutdown");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "SessionManager Dispose", false);
            }
        }
    }

    /// <summary>
    /// Session information container
    /// </summary>
    public class SessionInfo
    {
        public string SessionId { get; set; }
        public User User { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LastActivity { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string IpAddress { get; set; }
        public bool IsActive { get; set; }
        public int FailedAttempts { get; set; }
    }

    /// <summary>
    /// Session statistics for monitoring
    /// </summary>
    public class SessionStatistics
    {
        public int ActiveSessions { get; set; }
        public int TotalSessionsCreatedToday { get; set; }
        public TimeSpan AverageSessionDuration { get; set; }
    }
}