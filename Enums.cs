namespace pacman {
    public enum Orientation {
        RIGHT, LEFT, UP, DOWN
    }

    /// <summary>
    /// The <c>Orientation</c> class holds data for storing
    /// information about moving entities' orientation.
    /// It contains two static method to transfer the
    /// enum's values to and from integer value respectively.
    /// </summary>
    public static class OrientationExtensions {
        /// <summary>
        /// Gets <c>orientation</c> value from given integer value.
        /// </summary>
        /// <param name="n">integer value of the <c>orientation</c></param>
        /// <returns><c>orientation</c> value from given integer value</returns>
        public static Orientation fromInt(this Orientation o, int n) {
            switch (n) {
                case 0:
                    return Orientation.RIGHT;
                case 1:
                    return Orientation.LEFT;
                case 2:
                    return Orientation.UP;
                case 3:
                    return Orientation.DOWN;
                default:
                    return Orientation.RIGHT;
            }
        }

        /// <summary>
        /// Gets integer value of given <c>orientation</c> value.
        /// </summary>
        /// <param name="o"><c>orientation</c> value</param>
        /// <returns>integer value of given <c>orientation</c> value</returns>
        public static int toInt(this Orientation o1, Orientation o) {
            switch (o) {
                case Orientation.RIGHT:
                    return 0;
                case Orientation.LEFT:
                    return 1;
                case Orientation.UP:
                    return 2;
                case Orientation.DOWN:
                    return 3;
                default:
                    return 0;
            }
        }
    }
}
