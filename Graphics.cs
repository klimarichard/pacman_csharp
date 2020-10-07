using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace pacman {
    /// <summary>
    /// The <c>Assets</c> class holds all image and font data for the game.
    /// Every image or font displayed on the screen at one point in time
    /// or another is called from this class.
    /// All its methods and fields are static.
    /// </summary>
    public class Assets {
        private const int SIZE = 40;
        private const int BUTTON_WIDTH = 300;
        private const int BUTTON_HEIGHT = 75;

        public static Texture2D title, about, rules_controls, game_over, win;
        public static Texture2D pacMan;
        public static Texture2D icon;
        public static Texture2D[] playButton, menuButton, aboutButton, rulesButton;
        public static Texture2D[] pacManRight, pacManDown, pacManLeft, pacManUp;
        public static Texture2D enemyBlue, enemyPink, enemyPurple, enemyRed, enemyYellow, enemyDead;
        public static Texture2D foodNormal, foodKiller, foodBanana, foodCherry, foodStrawberry;
        public static Texture2D empty, wall;

        public static SpriteFont centeredStringFont, normalFont;

        private static ContentManager content;

        public Assets(ContentManager manager) {
            content = manager;
        }

        /// <summary>
        /// Loads all images to be displayed in game from resource files.
        /// </summary>
        public void Initialize() {
            title = content.Load<Texture2D>("title");
            icon = content.Load<Texture2D>("pacman");
            about = content.Load<Texture2D>("about_game");
            rules_controls = content.Load<Texture2D>("rules_controls");
            game_over = content.Load<Texture2D>("game_over");
            win = content.Load<Texture2D>("win");

            // all buttons are in one resource file
            SpriteSheet sheet = new SpriteSheet(content.Load<Texture2D>("buttons"));

            aboutButton = new Texture2D[2];
            aboutButton[0] = sheet.crop(0, 0, BUTTON_WIDTH, BUTTON_HEIGHT);
            aboutButton[1] = sheet.crop(BUTTON_WIDTH, 0, BUTTON_WIDTH, BUTTON_HEIGHT);

            menuButton = new Texture2D[2];
            menuButton[0] = sheet.crop(0, BUTTON_HEIGHT, BUTTON_WIDTH, BUTTON_HEIGHT);
            menuButton[1] = sheet.crop(BUTTON_WIDTH, BUTTON_HEIGHT, BUTTON_WIDTH, BUTTON_HEIGHT);

            playButton = new Texture2D[2];
            playButton[0] = sheet.crop(0, 2 * BUTTON_HEIGHT, BUTTON_WIDTH, BUTTON_HEIGHT);
            playButton[1] = sheet.crop(BUTTON_WIDTH, 2 * BUTTON_HEIGHT, BUTTON_WIDTH, BUTTON_HEIGHT);

            rulesButton = new Texture2D[2];
            rulesButton[0] = sheet.crop(0, 3 * BUTTON_HEIGHT, BUTTON_WIDTH, BUTTON_HEIGHT);
            rulesButton[1] = sheet.crop(BUTTON_WIDTH, 3 * BUTTON_HEIGHT, BUTTON_WIDTH, BUTTON_HEIGHT);

            // all game textures are in one resource file
            sheet = new SpriteSheet(content.Load<Texture2D>("textures"));

            enemyBlue = sheet.crop(0, 0, SIZE, SIZE);
            enemyDead = sheet.crop(SIZE, 0, SIZE, SIZE);
            enemyPink = sheet.crop(2 * SIZE, 0, SIZE, SIZE);
            enemyPurple = sheet.crop(3 * SIZE, 0, SIZE, SIZE);
            enemyRed = sheet.crop(4 * SIZE, 0, SIZE, SIZE);
            enemyYellow = sheet.crop(0, SIZE, SIZE, SIZE);

            foodBanana = sheet.crop(SIZE, SIZE, SIZE, SIZE);
            foodCherry = sheet.crop(2 * SIZE, SIZE, SIZE, SIZE);
            foodKiller = sheet.crop(3 * SIZE, SIZE, SIZE, SIZE);
            foodNormal = sheet.crop(4 * SIZE, SIZE, SIZE, SIZE);
            foodStrawberry = sheet.crop(0, 2 * SIZE, SIZE, SIZE);

            pacMan = sheet.crop(SIZE, 2 * SIZE, SIZE, SIZE);

            pacManDown = new Texture2D[6];
            pacManDown[0] = pacMan;
            pacManDown[1] = sheet.crop(2 * SIZE, 2 * SIZE, SIZE, SIZE);
            pacManDown[2] = sheet.crop(3 * SIZE, 2 * SIZE, SIZE, SIZE);
            pacManDown[3] = sheet.crop(4 * SIZE, 2 * SIZE, SIZE, SIZE);
            pacManDown[4] = pacManDown[2];
            pacManDown[5] = pacManDown[1];

            pacManLeft = new Texture2D[6];
            pacManLeft[0] = pacMan;
            pacManLeft[1] = sheet.crop(0, 3 * SIZE, SIZE, SIZE);
            pacManLeft[2] = sheet.crop(SIZE, 3 * SIZE, SIZE, SIZE);
            pacManLeft[3] = sheet.crop(2 * SIZE, 3 * SIZE, SIZE, SIZE);
            pacManLeft[4] = pacManLeft[2];
            pacManLeft[5] = pacManLeft[1];

            pacManRight = new Texture2D[6];
            pacManRight[0] = pacMan;
            pacManRight[1] = sheet.crop(3 * SIZE, 3 * SIZE, SIZE, SIZE);
            pacManRight[2] = sheet.crop(4 * SIZE, 3 * SIZE, SIZE, SIZE);
            pacManRight[3] = sheet.crop(0, 4 * SIZE, SIZE, SIZE);
            pacManRight[4] = pacManRight[2];
            pacManRight[5] = pacManRight[1];

            pacManUp = new Texture2D[6];
            pacManUp[0] = pacMan;
            pacManUp[1] = sheet.crop(SIZE, 4 * SIZE, SIZE, SIZE);
            pacManUp[2] = sheet.crop(2 * SIZE, 4 * SIZE, SIZE, SIZE);
            pacManUp[3] = sheet.crop(3 * SIZE, 4 * SIZE, SIZE, SIZE);
            pacManUp[4] = pacManUp[2];
            pacManUp[5] = pacManUp[1];

            wall = sheet.crop(4 * SIZE, 4 * SIZE, SIZE, SIZE);
            empty = new Texture2D(wall.GraphicsDevice, 40, 40);

            // load all fonts used in texts displayed on screen
            centeredStringFont = content.Load<SpriteFont>("fonts/centeredString");
            normalFont = content.Load<SpriteFont>("fonts/normalFont");
        }
    }

    /// <summary>
    /// The <code>SpriteSheet</code> inner class provides methods for slicing
    /// an image.
    /// It is used to take parts from large sprite sheets and return
    /// those parts as individual images.
    /// </summary>
    internal class SpriteSheet {
        private Texture2D sheet;

        /// <summary>
        /// Constructor of the <c>SpriteSheet</c> inner class. It takes
        /// the sprite sheet to be sliced as parameter.
        /// </summary>
        /// <param name="sheet">an image containing the sprite sheet
        ///                     to be sliced</param>
        public SpriteSheet(Texture2D sheet) {
            this.sheet = sheet;
        }

        /// <summary>
        /// Returns an image sliced from this inner class'
        /// sheet image.
        /// </summary>
        /// <param name="x">x-coordinate of upper left corner of
        ///                 the sliced part</param>
        /// <param name="y">y-coordinate of upper left corner of
        ///                 the sliced part</param>
        /// <param name="width">width of the sliced part</param>
        /// <param name="height">height of the sliced part</param>
        /// <returns>an image sliced from this class' sheet image</returns>
        public Texture2D crop(int x, int y, int width, int height) {
            Texture2D cropped = new Texture2D(sheet.GraphicsDevice, width, height);
            Color[] imageData = new Color[sheet.Width * sheet.Height];
            Color[] croppedImageData = new Color[width * height];

            sheet.GetData<Color>(imageData);

            int index = 0;

            for (int j = y; j < y + height; j++) {
                for (int i = x; i < x + width; i++) {
                    croppedImageData[index] = imageData[j * sheet.Width + i];
                    index++;
                }
            }

            cropped.SetData<Color>(croppedImageData);
            return cropped;
        }
    }

    /// <summary>
    /// The <c>Animation</c> class holds all necessary data
    /// for images that should be rendered with some
    /// animations.
    /// </summary>
    public class Animation {
        private readonly int speed;
        private int index;
        private long lastTime, timer;
        private readonly Texture2D[] types;

        /// <summary>
        /// Constructor of the <code>Animation</code> class. It takes the speed
        /// of the animation and an array of images that contains
        /// the animation frame by frame.It also sets local
        /// variables, such as the indexing variable and timer
        /// variables.
        /// <p>
        /// The<code> speed</code> field of the animation is a millisecond value,
        /// which means, that for example for <code>speed = 50 </ code >, the current
        /// frame of the animation will change every 50 ms, resulting in
        /// 20 frames per second animation.
        /// </summary>
        /// <param name="speed">animation speed</param>
        /// <param name="types">an array of images with the animation
        ///                     frame by frame</param>
        public Animation(int speed, Texture2D[] types) {
            this.speed = speed;
            this.types = types;
            index = 0;
            timer = 0;

            lastTime = Timing.Now();
        }

        /// <summary>
        /// Decides, whether it is time to change the indexing
        /// variable of the animation array.
        /// That means, that if more time than desired for
        /// animation <c>speed</c> has passed, the next frame will be
        /// indexed in <c>getCurrentType</c> method.
        /// </summary>
        public void tick() {
            timer += Timing.Now() - lastTime;
            lastTime = Timing.Now();

            if (timer > speed) {
                index++;
                timer = 0;
                // after last frame, show the first
                if (index >= types.Length) {
                    index = 0;
                }
            }
        }

        /// <summary>
        /// Gets current animation frame from animation array.
        /// The indexing variable is set in the <c>tick</c>
        /// method.
        /// </summary>
        /// <returns>current animation frame</returns>
        public Texture2D getCurrentType() {
            return types[index];
        }
    }
}
