using System;

namespace pacman {
    /// <summary>
    /// Provides static methods for working with time in this program.
    /// </summary>
    public static class Timing {
        /// <summary>
        /// Returns this moment's millisecond count.
        /// </summary>
        /// <returns>this moment's millisecond count</returns>
        public static long Now() {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}
