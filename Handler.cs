namespace pacman {
    /// <summary>
    /// The <c>Handler</c> class enables different
    /// game objects and entities to access each other
    /// when necessary.
    /// </summary>
    public class Handler {
        private MyGame game;
        private Map map;
        private Audio audio;

        /// <summary>
        /// Constructor of the <c>Handler</c> class.
        /// It takes the game itself as a parameter.
        /// </summary>
        /// <param name="game">the game</param>
        public Handler(MyGame game) {
            this.game = game;
            audio = new Audio();
        }

        /// <summary>
        /// Resets game to its initial state.
        /// </summary>
        public void resetGame() {
            game.resetGame();
        }


        #region Getters & Setters
        /// <summary>
        /// Gets the game pixel width
        /// </summary>
        /// <returns>the game width in pixels</returns>
        public int getWidth() {
            return game.getWidth();
        }

        /// <summary>
        /// Gets the game pixel height
        /// </summary>
        /// <returns>the game height in pixels</returns>
        public int getHeight() {
            return game.getHeight();
        }

        /// <summary>
        /// Gets the game's key manager.
        /// </summary>
        /// <returns>key manager of the game</returns>
        public KeyManager getKeyManager() {
            return game.getKeyManager();
        }

        /// <summary>
        /// Gets the game's mouse manager.
        /// </summary>
        /// <returns>mouse manager of the game</returns>
        public MouseManager getMouseManager() {
            return game.getMouseManager();
        }

        /// <summary>
        /// Gets the game.
        /// </summary>
        /// <returns>the game</returns>
        public MyGame getGame() {
            return game;
        }

        /// <summary>
        /// Gets the game map.
        /// </summary>
        /// <returns>the game map</returns>
        public Map getMap() {
            return map;
        }

        /// <summary>
        /// Sets the game map.
        /// </summary>
        /// <param name="map">new game map</param>
        public void setMap(Map map) {
            this.map = map;
        }

        /// <summary>
        /// Gets the game audio class.
        /// </summary>
        /// <returns>audio class of the game</returns>
        public Audio getAudio()
        {
            return audio;
        }

        #endregion
    }
}
