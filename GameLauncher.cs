using System;

namespace pacman {
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class GameLauncher {
        private const int WIDTH = 1200;
        private const int HEIGHT = 800;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            using (var game = new MyGame("Hungry PacMan", WIDTH, HEIGHT))
                game.Run();
        }
    }
#endif
}
