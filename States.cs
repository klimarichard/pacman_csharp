using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using System.Threading;

namespace pacman {
    /// <summary>
    /// <c>State</c> is an abstract class for all different types
    /// of game states the game can be in.
    /// <para>Each type of state has different appearance as well as different
    /// user interaction options. This class also contains the static
    /// methods <c>getState</c> and <c>setState</c> which are
    /// responsible for changing of the current state.</para>
    /// </summary>
    public abstract class State {
        private static State currentState = null;

        protected const int BUTTON_WIDTH = 300;
        protected const int BUTTON_HEIGHT = 75;
        protected const int BUTTON_DIFFERENCE = 110;
        protected const int RIGHT_COLUMN_SIZE = 400;
        protected const int RIGHT_COLUMN_INDENT = 50;

        protected Handler handler;

        public UserInterfaceManager manager;

        /// <summary>
        /// Constructor of the abstract state class. It takes
        /// the game handler, for having access to all elements
        /// of the game as a parameter.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        public State(Handler handler) {
            this.handler = handler;
        }

        /// <summary>
        /// Gets current state.
        /// </summary>
        /// <returns>current state</returns>
        public static State getState() {
            return currentState;
        }

        /// <summary>
        /// Sets current state.
        /// </summary>
        /// <param name="state">new current state</param>
        public static void setState(State state) {
            // game should start exactly where we left off
            if (typeof(GameState).IsAssignableFrom(state.GetType()))
                ((GameState)state).getMap().setLastTime(Timing.Now());
            currentState = state;
        }

        /// <summary>
        /// Updates all user interface elements.
        /// </summary>
        public virtual void tick() {
            manager.tick();
        }

        /// <summary>
        /// Draws all user interface elements.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public virtual void render(SpriteBatch sb) {
            manager.render(sb);
        }

        /// <summary>
        /// Gets user interface manager.
        /// </summary>
        /// <returns>user interface manager</returns>
        public UserInterfaceManager getManager() {
            return manager;
        }

        /// <summary>
        /// Gets current level of the game.
        /// </summary>
        /// <returns>current level</returns>
        internal int getLevel()
        {
            return handler.getGame().getGameState().getLevel();
        }
    }

    /// <summary>
    /// The <c>GameState</c> class is responsible
    /// for creating and rendering the Games state, as well
    /// as for some parts of the game itself.
    /// It holds the current level of the game and the game map.
    /// </summary>
    public class GameState : State {
        private Map map;
        private int level;
        private UserInterfaceLabel labelScore, labelHighScore, labelLevel, labelLives;
        private Animation animation;

        /// <summary>
        /// Constructor of the <c>GameState</c> class.
        /// It calls the base constructor and then calls the
        /// <c>createGameState</c> method to initialize
        /// the appearance of the state.It also initializes
        /// local variables, such as level, animation and map.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        public GameState(Handler handler) : base(handler) {
            manager = new UserInterfaceManager(handler);
            level = 1;
            createGameState();
            animation = new Animation(80, Assets.pacManRight);
            map = new Map(handler, level);
            handler.setMap(map);
        }

        /// <summary>
        /// Creates all user interface elements of the state.
        /// </summary>
        private void createGameState() {
            // menu button
            manager.addObject(new UserInterfaceButton(
                handler.getGame().getWidth() - RIGHT_COLUMN_SIZE + ((RIGHT_COLUMN_SIZE - BUTTON_WIDTH) / 2f),
                (handler.getGame().getHeight() * 5f) / 6,
                BUTTON_WIDTH, BUTTON_HEIGHT, Assets.menuButton,
                () =>
                {
                    State.setState(handler.getGame().menuState);
                    if (!handler.getAudio().isPlayingMenu())
                    {
                        Thread t = new Thread(() => handler.getAudio().playSong(Assets.menuLoop, true));
                        t.Start();
                    }
                    handler.getMouseManager().setManager(State.getState().getManager());
                }));

            // level label
            labelLevel = new UserInterfaceLabel(
                    handler.getGame().getWidth() - RIGHT_COLUMN_SIZE + RIGHT_COLUMN_INDENT,
                    handler.getGame().getHeight() / 10f,
                    "Level", level.ToString());
            manager.addObject(labelLevel);

            // score label
            labelScore = new UserInterfaceLabel(
                    handler.getGame().getWidth() - RIGHT_COLUMN_SIZE + RIGHT_COLUMN_INDENT,
                    (handler.getGame().getHeight() * 3f) / 10,
                    "Score:", handler.getGame().getScore().ToString());
            manager.addObject(labelScore);

            // high-score label
            labelHighScore = new UserInterfaceLabel(
                    handler.getGame().getWidth() - RIGHT_COLUMN_SIZE + RIGHT_COLUMN_INDENT,
                    (handler.getGame().getHeight() * 9f) / 20,
                    "High score:", handler.getGame().getHighScore().ToString());
            manager.addObject(labelHighScore);

            // lives label
            labelLives = new UserInterfaceLabel(
                    handler.getGame().getWidth() - RIGHT_COLUMN_SIZE + RIGHT_COLUMN_INDENT,
                    (handler.getGame().getHeight() * 7f) / 10,
                    "Lives:", "");
            manager.addObject(labelLives);
        }

        /// <summary>
        /// Performs level up actions.
        /// Depending on level number, it loads the new
        /// level's map, or switches the game to Win state.
        /// </summary>
        public void levelUp() {
            level++;
            int timer = 1500;
            long now = Timing.Now();
            long lastTime = Timing.Now();

            while ((lastTime - now) < timer) {
                lastTime = Timing.Now();
            }

            // there are only seven levels, if the player wins the seventh, he wins the game
            if (level > 7) {
                State.setState(new WinState(handler));
                handler.getAudio().playSong(Assets.youWon);
                handler.getMouseManager().setManager(State.getState().getManager());
            } else {
                map = new Map(handler, level);
                handler.setMap(map);
                Thread t = new Thread(() => handler.getAudio().playSong(Assets.levelMusic[level - 1], true));
                t.Start();
            }
        }

        /// <summary>
        /// Updates level, score and high score count,
        /// all other user interface elements and the map.
        /// </summary>
        public override void tick() {
            labelLevel.setChangeable(level.ToString());
            labelScore.setChangeable(handler.getGame().getScore().ToString());
            labelHighScore.setChangeable(handler.getGame().getHighScore().ToString());
            map.tick();
            animation.tick();
            base.tick();
        }

        /// <summary>
        /// Draws all user interface elements, the map and lives count.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public override void render(SpriteBatch sb) {
            base.render(sb);
            map.render(sb);

            // draw as many lives, as are remaining
            switch (handler.getGame().getLives()) {
                case 3:
                    sb.Draw(animation.getCurrentType(),
                        new Rectangle(
                            (int) labelLives.getX() + 125 + 2 * animation.getCurrentType().Width + 3 * 20,
                            (int) labelLives.getY(),
                            animation.getCurrentType().Width,
                            animation.getCurrentType().Height),
                            Color.White);
                    goto case 2;
                case 2:
                    sb.Draw(animation.getCurrentType(),
                        new Rectangle(
                            (int) labelLives.getX() + 125 + animation.getCurrentType().Width + 2 * 20,
                            (int) labelLives.getY(),
                            animation.getCurrentType().Width,
                            animation.getCurrentType().Height),
                            Color.White);
                    goto case 1;
                case 1:
                    sb.Draw(animation.getCurrentType(),
                        new Rectangle(
                            (int) labelLives.getX() + 125 + 20,
                            (int) labelLives.getY(),
                            animation.getCurrentType().Width,
                            animation.getCurrentType().Height),
                            Color.White);
                    goto default;
                default:
                    break;
            }
        }

        /// <summary>
        /// Gets the map.
        /// </summary>
        /// <returns>the map</returns>
        public Map getMap() {
            return map;
        }

        /// <summary>
        /// Gets current level of the game.
        /// </summary>
        /// <returns>current level</returns>
        internal int getLevel()
        {
            return level;
        }
    }

    /// <summary>
    /// The <c>AboutState</c> class is responsible
    /// for creating and rendering the About state.
    /// </summary>
    public class AboutState : State {
        /// <summary>
        /// Constructor of the <c>AboutState</c> class.
        /// It calls the base constructor and then calls the
        /// <c>createAboutState</c> method to initialize
        /// the appearance of the state.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        public AboutState(Handler handler) : base(handler) {
            manager = new UserInterfaceManager(handler);
            createAboutState();
        }

        /// <summary>
        /// Creates all user interface elements of the state.
        /// </summary>
        private void createAboutState() {
            // main game image on top of the window
            manager.addObject(new UserInterfaceImage((handler.getGame().getWidth() - Assets.title.Width) / 2f,
                    50, Assets.title.Width, Assets.title.Height, Assets.title));

            // about game text
            manager.addObject(new UserInterfaceImage((handler.getGame().getWidth() - Assets.about.Width) / 2f,
                    265, Assets.about.Width, Assets.about.Height, Assets.about));

            // main menu button
            manager.addObject(new UserInterfaceButton((handler.getGame().getWidth() - BUTTON_WIDTH) / 2f,
                    (handler.getGame().getHeight() * 5f) / 6, BUTTON_WIDTH, BUTTON_HEIGHT,
                    Assets.menuButton,
                    () =>
                    {
                        State.setState(handler.getGame().menuState);
                        if (!handler.getAudio().isPlayingMenu())
                        {
                            Thread t = new Thread(() => handler.getAudio().playSong(Assets.menuLoop, true));
                            t.Start();
                        }
                        handler.getMouseManager().setManager(State.getState().getManager());
                    }
                    ));
        }
    }

    /// <summary>
    /// The <c>MenuState</c> class is responsible
    /// for creating and rendering the Menu state.
    /// </summary>
    public class MenuState : State {
        /// <summary>
        /// Constructor of the <c>MenuState</c> class.
        /// It calls the base constructor and then calls the
        /// <c>createMenuState</c> method to initialize
        /// the appearance of the state.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        public MenuState(Handler handler) : base(handler) {
            manager = new UserInterfaceManager(handler);
            createMenuState();
        }

        /// <summary>
        /// Creates all user interface elements of the state.
        /// </summary>
        private void createMenuState() {
            // main game image on top of the window
            manager.addObject(new UserInterfaceImage((handler.getGame().getWidth() - Assets.title.Width) / 2f,
                    50, Assets.title.Width, Assets.title.Height, Assets.title));

            // play button
            manager.addObject(new UserInterfaceButton((handler.getGame().getWidth() - BUTTON_WIDTH) / 2f,
                    (handler.getGame().getHeight() - BUTTON_HEIGHT) / 2f,
                    BUTTON_WIDTH, BUTTON_HEIGHT, Assets.playButton,
                    () =>
                    {
                        State.setState(handler.getGame().gameState);
                        handler.getMouseManager().setManager(State.getState().getManager());
                        Thread t = new Thread(() => handler.getAudio().playSong(Assets.levelMusic[handler.getGame().getGameState().getLevel() - 1], true));
                        t.Start();
                    }));

            // rules & controls button
            manager.addObject(new UserInterfaceButton((handler.getGame().getWidth() - BUTTON_WIDTH) / 2f,
                    (handler.getGame().getHeight() - BUTTON_HEIGHT) / 2f + BUTTON_DIFFERENCE,
                    BUTTON_WIDTH, BUTTON_HEIGHT, Assets.rulesButton,
                    () =>
                    {
                        State.setState(handler.getGame().rulesState);
                        handler.getMouseManager().setManager(State.getState().getManager());
                    }));

            // about button
            manager.addObject(new UserInterfaceButton((handler.getGame().getWidth() - BUTTON_WIDTH) / 2f,
                    (handler.getGame().getHeight() - BUTTON_HEIGHT) / 2f + 2 * BUTTON_DIFFERENCE,
                    BUTTON_WIDTH, BUTTON_HEIGHT, Assets.aboutButton,
                    () =>
                    {
                        State.setState(handler.getGame().aboutState);
                        handler.getMouseManager().setManager(State.getState().getManager());
                    }));
        }
    }

    /// <summary>
    /// The <code>RulesState</code> class is responsible
    /// for creating and rendering the Rules state.
    /// </summary>
    public class RulesState : State {
        /// <summary>
        /// Constructor of the <c>RulesState</c> class.
        /// It calls the base constructor and then calls the
        /// <c>createRulesState</c> method to initialize
        /// the appearance of the state.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        public RulesState(Handler handler) : base(handler) {
            manager = new UserInterfaceManager(handler);
            createRulesState();
        }

        /// <summary>
        /// Creates all user interface elements of the state.
        /// </summary>
        private void createRulesState() {
            // main game image on top of the window
            manager.addObject(new UserInterfaceImage((handler.getGame().getWidth() - Assets.title.Width) / 2f,
                    50, Assets.title.Width, Assets.title.Height, Assets.title));

            // rules & controls text
            manager.addObject(new UserInterfaceImage(
                    (handler.getGame().getWidth() - Assets.rules_controls.Width) / 2f,
                    265, Assets.rules_controls.Width,
                    Assets.rules_controls.Height, Assets.rules_controls));

            // play button
            manager.addObject(new UserInterfaceButton(
                    handler.getGame().getWidth() - RIGHT_COLUMN_SIZE + ((RIGHT_COLUMN_SIZE - BUTTON_WIDTH) / 2f),
                    (handler.getGame().getHeight() * 5f) / 6,
                    BUTTON_WIDTH, BUTTON_HEIGHT, Assets.playButton, () =>
                    {
                        State.setState(handler.getGame().gameState);
                        Thread t = new Thread(() => handler.getAudio().playSong(Assets.levelMusic[handler.getGame().getGameState().getLevel() - 1], true));
                        t.Start();
                        handler.getMouseManager().setManager(State.getState().getManager());
                    }));

            // menu button
            manager.addObject(new UserInterfaceButton((RIGHT_COLUMN_SIZE - BUTTON_WIDTH) / 2f,
                    (handler.getGame().getHeight() * 5f) / 6,
                    BUTTON_WIDTH, BUTTON_HEIGHT, Assets.menuButton, () =>
                    {
                        State.setState(handler.getGame().menuState);
                        if (!handler.getAudio().isPlayingMenu())
                        {
                            Thread t = new Thread(() => handler.getAudio().playSong(Assets.menuLoop, true));
                            t.Start();
                        }
                        handler.getMouseManager().setManager(State.getState().getManager());
                    }));
        }
    }

    /// <summary>
    /// <c>EndState</c> is an abstract class responsible
    /// for creating and rendering the end states.
    /// </summary>
    public abstract class EndState : State {
        private UserInterfaceCenteredString labelScore;
        private Texture2D image;

        /// <summary>
        /// Constructor of the <c>EndState</c> abstract class.
        /// It calls the base constructor and then calls the
        /// <c>createEndState</c> method to initialize
        /// the appearance of the state.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="image">an image displayed on top of the window</param>
        public EndState(Handler handler, Texture2D image) : base(handler) {
            manager = new UserInterfaceManager(handler);
            this.image = image;
            createEndState();
        }

        /// <summary>
        /// Creates all user interface elements of the state.
        /// </summary>
        private void createEndState() {
            manager.addObject(new UserInterfaceImage((handler.getGame().getWidth() - image.Width) / 2f,
                50, image.Width, image.Height, image));

            labelScore = new UserInterfaceCenteredString(450, handler.getWidth(),
                    handler.getGame().getScore().ToString());
            manager.addObject(labelScore);

            manager.addObject(new UserInterfaceButton((handler.getGame().getWidth() - BUTTON_WIDTH) / 2f,
                    (handler.getGame().getHeight() * 5f) / 6, BUTTON_WIDTH, BUTTON_HEIGHT,
                    Assets.menuButton, () =>
                    {
                        State.setState(handler.getGame().menuState);
                        if (!handler.getAudio().isPlayingMenu())
                        {
                            Thread t = new Thread(() => handler.getAudio().playSong(Assets.menuLoop, true));
                            t.Start();
                        }
                        handler.getMouseManager().setManager(State.getState().getManager());
                        handler.resetGame();
                    }));
        }

        /// <summary>
        /// Updates score count and all other user interface elements.
        /// </summary>
        public override void tick() {
            labelScore.setText(handler.getGame().getScore().ToString());
            base.tick();
        }
    }

    /// <summary>
    /// The <c>GameOverState</c> class is responsible
    /// for loading <c>game_over</c> image into
    /// <c>EndState</c> abstract class.
    /// </summary>
    public class GameOverState : EndState {
        public GameOverState(Handler handler) : base(handler, Assets.game_over) {

        }
    }

    /// <summary>
    /// The <c>WinState</c> class is responsible
    /// for loading <c>win</c> image into
    /// <c>EndState</c> abstract class.
    /// </summary>
    public class WinState : EndState {
        public WinState(Handler handler) : base(handler, Assets.win) {

        }
    }
}
