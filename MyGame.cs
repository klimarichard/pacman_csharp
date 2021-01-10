using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using System.Threading;

namespace pacman {
    /// <summary>
    /// The <c>PacMan</c> class is the main class of
    /// the HungryPacMan game responsible for running
    /// the game loop(<c>update</c> and <c>draw</c>).
    /// </summary>
    public class MyGame : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public State gameState;
        public State menuState;
        public State rulesState;
        public State aboutState;

        private KeyManager keyManager;
        private MouseManager mouseManager;

        private Handler handler;

        private string title;
        private int width;
        private int height;

        private int score;
        private int highScore;
        private int lives;

        /// <summary>
        /// Constructor of the main <c>PacMan</c> class.
        /// It takes the game's title, its width and height as
        /// parameters. It also initializes the local variables,
        /// such as <c>score</c>, <c>lives</c>, etc.
        /// </summary>
        /// <param name="title">the game title</param>
        /// <param name="width">the game width</param>
        /// <param name="height">the game height</param>
        public MyGame(string title, int width, int height) {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.title = title;
            this.height = height;
            this.width = width;

            this.Window.Title = title;
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            this.Window.AllowUserResizing = false;

            this.IsMouseVisible = true;

            score = 0;
            highScore = 0;
            lives = 3;

            keyManager = new KeyManager();
            mouseManager = new MouseManager();

            this.IsFixedTimeStep = true;
            this.graphics.SynchronizeWithVerticalRetrace = true;
            this.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 20);
        }

        /// <summary>
        /// Initializes the game at first start.
        /// </summary>
        protected override void Initialize() {
            handler = new Handler(this);

            Assets assets = new Assets(Content);
            assets.Initialize();

            gameState = new GameState(handler);
            rulesState = new RulesState(handler);
            aboutState = new AboutState(handler);
            menuState = new MenuState(handler);

            State.setState(menuState);
            handler.getMouseManager().setManager(State.getState().getManager());

            base.Initialize();
        }

        /// <summary>
        /// Loads all graphical and text content.
        /// </summary>
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void UnloadContent() {
            base.UnloadContent();
        }

        protected override void BeginRun()
        {
            handler.getAudio().playSong(Assets.menuLoop);
            base.BeginRun();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            keyManager.tick();
            mouseManager.tick();

            if (State.getState() != null) {
                State.getState().tick();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(new Color(0, 0, 139));

            spriteBatch.Begin();

            if (State.getState() != null) {
                State.getState().render(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Resets game data to initial ones.
        /// </summary>
        public void resetGame() {
            score = 0;
            lives = 3;
            gameState = new GameState(handler);
            keyManager.resetKeys();
        }

        #region Getters & Setters
        /// <summary>
        /// Gets the key manager.
        /// </summary>
        /// <returns>the key manager</returns>
        public KeyManager getKeyManager() {
            return keyManager;
        }

        /// <summary>
        /// Gets the mouse manager.
        /// </summary>
        /// <returns>the mouse manager</returns>
        public MouseManager getMouseManager() {
            return mouseManager;
        }

        /// <summary>
        /// Gets the game width.
        /// </summary>
        /// <returns>the game width</returns>
        public int getWidth() {
            return width;
        }

        /// <summary>
        /// Gets the game height.
        /// </summary>
        /// <returns>the game height</returns>
        public int getHeight() {
            return height;
        }

        /// <summary>
        /// Gets current score.
        /// </summary>
        /// <returns>current score</returns>
        public int getScore() {
            return score;
        }

        /// <summary>
        /// Sets the score.
        /// </summary>
        /// <param name="score">new score</param>
        public void setScore(int score) {
            this.score = score;
            if (score > highScore) {
                highScore = score;
            }
        }

        /// <summary>
        /// Gets the current high score.
        /// </summary>
        /// <returns>current high score</returns>
        public int getHighScore() {
            return highScore;
        }

        /// <summary>
        /// Gets current lives count.
        /// </summary>
        /// <returns>current lives count</returns>
        public int getLives() {
            return lives;
        }

        /// <summary>
        /// Sets current lives count.
        /// </summary>
        /// <param name="lives">new life count</param>
        public void setLives(int lives) {
            this.lives = lives;
        }

        /// <summary>
        /// Gets the <c>GameState</c>.
        /// </summary>
        /// <returns>the <c>GameState</c></returns>
        public GameState getGameState() {
            return (GameState) gameState;
        }

        #endregion
    }
}
