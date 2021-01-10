using Microsoft.Xna.Framework.Graphics;

using System.Threading;

namespace pacman {
    /// <summary>
    /// The <c>Map</c> class is the class responsible for
    /// controlling all the game characters by calling their
    /// <c>tick</c> and <c>render</c> methods.
    /// </summary>
    public class Map {
        private const int WALL = 0;
        private const int KILLER_FOOD = 1;
        private const int BANANA = 2;
        private const int CHERRY = 3;
        private const int STRAWBERRY = 4;
        private const int ENEMY_PINK = 5;
        private const int ENEMY_PURPLE = 6;
        private const int ENEMY_RED = 7;
        private const int ENEMY_YELLOW = 8;
        private const int EMPTY_WITHOUT_FOOD = 9;
        private const int EMPTY = 31;

        private Handler handler;
        private int width, height;
        private int[,] tiles;
        private int foodCount;
        private int level;
        private EntityManager entityManager;

        private bool blueGhosts, levelBegin, eatingPacMan;
        private bool chase;
        private bool scatter;
        private long blueGhostTimer, levelBeginTimer;
        private long chaseTimer, scatterTimer, eatingTimer, eatingInnerTimer;
        private long lastTime, chaseLastTime, scatterLastTime, eatingLastTime;

        /// <summary>
        /// Constructor of the <c>Map</c> class. It takes the game handler,
        /// for having access to all elements of the game, and level number as
        /// parameters.
        /// It calls the<c>loadMap</c> method to load the game map of
        /// the correct level.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="level">current game level (map to be loaded)</param>
        public Map(Handler handler, int level) {
            this.handler = handler;
            entityManager = new EntityManager(handler, new PacMan(handler, 0, 0));
            loadMap(level);
        }

        /// <summary>
        /// Loads the level map from resource map file and initializes
        /// all necessary local variables accordingly.
        /// </summary>
        /// <param name="level">current game level</param>
        private void loadMap(int level) {
            // all levels are of same width and height (20 x 20)
            width = 20;
            height = 20;

            this.level = level;

            // initializing default level state (level is beginning, then chase mode first, no blue ghosts)
            blueGhosts = false;
            levelBegin = true;
            eatingPacMan = false;
            setChase(true);

            levelBeginTimer = 3000;
            lastTime = Timing.Now();

            string[] file = LevelLoader.loadMapToString("Content/maps/" + level + ".lvl").Split(new char[] { '\n' });

            // first line of resource map file contains PacMan starting position
            string[] pacManCoordinates = file[0].Split(new char[] { ' ' });
            int pacManSpawnX = LevelLoader.parseInt(pacManCoordinates[0]);
            int pacManSpawnY = LevelLoader.parseInt(pacManCoordinates[1]);
            entityManager.getPacMan().setxSpawn(pacManSpawnX * Tile.SIZE);
            entityManager.getPacMan().setySpawn(pacManSpawnY * Tile.SIZE);
            entityManager.getPacMan().setX(entityManager.getPacMan().getxSpawn());
            entityManager.getPacMan().setY(entityManager.getPacMan().getySpawn());

            // initialize the tile array
            tiles = new int[width, height];

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    // there is no food underneath PacMan's starting position
                    if ((y == pacManSpawnY) && (x == pacManSpawnX)) {
                        tiles[x, y] = EMPTY;
                        continue;
                    }

                    // convert character from resource map file to integer
                    int c = LevelLoader.charToInt(file[1 + y][x]);

                    switch (c) {
                        case WALL:
                            tiles[x, y] = WALL;
                            continue;
                        case KILLER_FOOD:
                            entityManager.addFood(new FoodKiller(handler, x * Tile.SIZE, y * Tile.SIZE));
                            break;
                        case BANANA:
                            entityManager.addFood(new FoodBanana(handler, x * Tile.SIZE, y * Tile.SIZE));
                            break;
                        case CHERRY:
                            entityManager.addFood(new FoodCherry(handler, x * Tile.SIZE, y * Tile.SIZE));
                            break;
                        case STRAWBERRY:
                            entityManager.addFood(new FoodStrawberry(handler, x * Tile.SIZE, y * Tile.SIZE));
                            break;
                        case ENEMY_PINK:
                            // pink enemy needs to be accessed by red enemy to properly count red enemy's target tile,
                            // hence it is stored in the entity manager by itself
                            EnemyPink pinkie = new EnemyPink(handler, x * Tile.SIZE, y * Tile.SIZE);
                            entityManager.addMoving(pinkie);
                            entityManager.setPinkie(pinkie);
                            break;
                        case ENEMY_PURPLE:
                            entityManager.addMoving(new EnemyPurple(handler, x * Tile.SIZE, y * Tile.SIZE));
                            break;
                        case ENEMY_RED:
                            entityManager.addMoving(new EnemyRed(handler, x * Tile.SIZE, y * Tile.SIZE));
                            break;
                        case ENEMY_YELLOW:
                            entityManager.addMoving(new EnemyYellow(handler, x * Tile.SIZE, y * Tile.SIZE));
                            break;
                        case EMPTY:
                            entityManager.addFood(new FoodNormal(handler, x * Tile.SIZE, y * Tile.SIZE));
                            foodCount++;
                            break;
                        case EMPTY_WITHOUT_FOOD:
                        default:
                            break;
                    }

                    tiles[x, y] = EMPTY;
                }
            }

            // PacMan is added as last to the entity manager, in order to render him as last
            entityManager.addMoving(entityManager.getPacMan());
        }

        /// <summary>
        /// Restarts the moving entities positions after PacMan loses a life.
        /// It also clears the keyboard input data.
        /// </summary>
        private void restartAfterPacManDeath() {
            foreach (Entity e in entityManager.getEntities()) {
                if (typeof(Moving).IsAssignableFrom(e.GetType())) {
                    e.setX(((Moving) e).getxSpawn());
                    e.setY(((Moving) e).getySpawn());
                }
            }

            handler.getKeyManager().resetKeys();
            entityManager.getPacMan().setOrientation(Orientation.RIGHT);
            setLevelBegin(true);
        }

        /// <summary>
        /// Decides what actions to make based on current game situation.
        /// </summary>
        public void tick() {
            // when the level is in the levelBegin wait state, only PacMan is ticking (his animation doesn't stop)
            if (levelBegin) {
                long now = Timing.Now();
                levelBeginTimer -= (now - lastTime);
                lastTime = now;
                if (levelBeginTimer <= 0) {
                    setLevelBegin(false);
                }
                entityManager.getPacMan().tick();
                return;
            }

            // if the PacMan is being eaten, only the ghost eating PacMan is moving, and PacMan's animation doesn't stop
            if (eatingPacMan) {
                long now = Timing.Now();
                eatingTimer -= (now - eatingLastTime);
                eatingLastTime = now;
                if (eatingInnerTimer++ % 15 == 0) {
                    entityManager.getPacManEater().tick();
                    entityManager.getPacMan().tick();
                }
                if (eatingTimer <= 0) {
                    // PacMan lost all of his lives, game is over
                    if (handler.getGame().getLives() == 0) {
                        State.setState(new GameOverState(handler));
                        handler.getAudio().playSong(Assets.gameOver);
                        handler.getMouseManager().setManager(State.getState().getManager());
                    }
                    setEatingPacMan(false, null);
                    restartAfterPacManDeath();
                }
                return;
            }

            // every tick subtracts passed time from timer variable, so that the modes can properly interchange
            if (blueGhosts) {
                long now = Timing.Now();
                blueGhostTimer -= (now - lastTime);
                lastTime = now;
                if (blueGhostTimer <= 0) {
                    setBlueGhosts(false);
                }
            } else if (chase) {
                long now = Timing.Now();
                chaseTimer -= (now - chaseLastTime);
                chaseLastTime = now;
                if (chaseTimer <= 0) {
                    setChase(false);
                    setScatter(true);
                }
            } else if (scatter) {
                long now = Timing.Now();
                scatterTimer -= (now - scatterLastTime);
                scatterLastTime = now;
                if (scatterTimer <= 0) {
                    setScatter(false);
                    setChase(true);
                }
            }

            entityManager.tick();
        }

        /// <summary>
        /// Goes over the map tile by tile and renders them on the screen,
        /// the does the same with all entities.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public void render(SpriteBatch sb) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    getTile(x, y).render(sb, x * Tile.SIZE, y * Tile.SIZE);
                }
            }

            entityManager.render(sb);

            // if some ghost is eating PacMan, he is drawn once more, so he would appear as the topmost
            if (eatingPacMan) {
                entityManager.getPacManEater().render(sb);
            }
        }

        /// <summary>
        /// Gets tile on given coordinates.
        /// </summary>
        /// <param name="x">x-coordinate of the tile</param>
        /// <param name="y">y-coordinate of the tile</param>
        /// <returns>tile on given coordinates</returns>
        public Tile getTile(int x, int y) {
            // if the coordinates are out of bounds, return wall
            if ((x < 0) || (y < 0) || (x >= width) || (y >= height))
                return Tile.wall;

            Tile t = Tile.tiles[tiles[x, y]];

            // if the tile on given index in the tiles array is not initialized, return wall
            if (t == null) {
                return Tile.wall;
            }

            return t;
        }

        #region Getters & Setters

        /// <summary>
        /// Gets map width in tiles.
        /// </summary>
        /// <returns>map width in tiles</returns>
        public int getWidth() {
            return width;
        }

        /// <summary>
        /// Sets map width in tiles.
        /// </summary>
        /// <param name="width">new map width in tiles</param>
        public void setWidth(int width) {
            this.width = width;
        }

        /// <summary>
        /// Gets map height in tiles.
        /// </summary>
        /// <returns>map height in tiles</returns>
        public int getHeight() {
            return height;
        }

        /// <summary>
        /// Sets map height in tiles.
        /// </summary>
        /// <param name="height">new map height in tiles</param>
        public void setHeight(int height) {
            this.height = height;
        }

        /// <summary>
        /// Gets the map's entity manager.
        /// </summary>
        /// <returns>the map's entity manager</returns>
        public EntityManager getEntityManager() {
            return entityManager;
        }

        /// <summary>
        /// Gets blue ghosts status.
        /// </summary>
        /// <returns><c>true</c>, if blue ghosts mode is active, <c>false</c> otherwise</returns>
        public bool isBlueGhosts() {
            return blueGhosts;
        }

        /// <summary>
        /// Sets blue ghosts status.
        /// </summary>
        /// <param name="blueGhosts">new blue ghosts status</param>
        public void setBlueGhosts(bool blueGhosts) {
            this.blueGhosts = blueGhosts;
            if (blueGhosts) {
                blueGhostTimer = 5000;
                lastTime = Timing.Now();

                foreach (Entity e in entityManager.getEntities()) {
                    if (typeof(Enemy).IsAssignableFrom(e.GetType())) {
                        ((Enemy) e).setImmuneToBlue(false);
                        ((Enemy) e).flipOrientation();
                    }
                }
            } else {
                foreach (Entity e in entityManager.getEntities()) {
                    if (typeof(Enemy).IsAssignableFrom(e.GetType())) {
                        ((Enemy) e).flipOrientation();
                    }
                }
            }

            if (chase) {
                chaseLastTime = Timing.Now();
            }
            if (scatter) {
                scatterLastTime = Timing.Now();
            }
        }

        /// <summary>
        /// Gets chase status.
        /// </summary>
        /// <returns><c>true</c>, if chase mode is active, <c>false</c> otherwise</returns>
        public bool isChase() {
            return chase;
        }

        /// <summary>
        /// Sets chase status.
        /// </summary>
        /// <param name="chase">new chase status</param>
        private void setChase(bool chase) {
            this.chase = chase;
            if (chase) {
                chaseTimer = 10000;
                chaseLastTime = Timing.Now();
                foreach (Entity e in entityManager.getEntities()) {
                    if (typeof(Enemy).IsAssignableFrom(e.GetType())) {
                        ((Enemy) e).flipOrientation();
                    }
                }
            }
        }

        /// <summary>
        /// Gets scatter status.
        /// </summary>
        /// <returns><c>true</c>, if scatter mode is active, <c>false</c> otherwise</returns>
        public bool isScatter() {
            return scatter;
        }

        /// <summary>
        /// Sets scatter status.
        /// </summary>
        /// <param name="scatter">new scatter status</param>
        private void setScatter(bool scatter) {
            this.scatter = scatter;
            if (scatter) {
                scatterTimer = 10000 - (level - 1) * 1500;
                scatterLastTime = Timing.Now();
                foreach (Entity e in entityManager.getEntities()) {
                    if (typeof(Enemy).IsAssignableFrom(e.GetType())) {
                        ((Enemy) e).flipOrientation();
                    }
                }
            }
        }

        /// <summary>
        /// Gets eating PacMan status.
        /// </summary>
        /// <returns><c>true</c>, if eating PacMan mode is active, <c>false</c> otherwise</returns>
        public bool isEatingPacMan() {
            return eatingPacMan;
        }

        /// <summary>
        /// Sets eating PacMan status.
        /// </summary>
        /// <param name="eatingPacMan">new eating PacMan status</param>
        /// <param name="e">the enemy eating PacMan</param>
        public void setEatingPacMan(bool eatingPacMan, Enemy e) {
            this.eatingPacMan = eatingPacMan;
            entityManager.setPacManEater(e);
            if (eatingPacMan) {
                Thread t = new Thread(() => handler.getAudio().playOneShot(Assets.pacmanEaten));
                t.Start();
                eatingInnerTimer = 0;
                eatingTimer = 1500;
                eatingLastTime = Timing.Now();
            } else {
                if (chase) {
                    chaseLastTime = Timing.Now();
                }
                if (scatter) {
                    scatterLastTime = Timing.Now();
                }
            }
        }

        /// <summary>
        /// Gets level begin status.
        /// </summary>
        /// <returns><c>true</c>, if level begin mode is active, <c>false</c> otherwise</returns>
        public bool notLevelBegin() {
            return !levelBegin;
        }

        /// <summary>
        /// Sets level begin status.
        /// </summary>
        /// <param name="levelBegin">new level begin status</param>
        private void setLevelBegin(bool levelBegin) {
            this.levelBegin = levelBegin;
            if (levelBegin) {
                levelBeginTimer = 1000;
                lastTime = Timing.Now();
            } else {
                if (chase) {
                    chaseLastTime = Timing.Now();
                }
                if (scatter) {
                    scatterLastTime = Timing.Now();
                }
            }
        }

        /// <summary>
        /// Sets last time.
        /// </summary>
        /// <param name="lastTime">new value of last time</param>
        public void setLastTime(long lastTime) {
            this.lastTime = lastTime;
        }

        /// <summary>
        /// Gets current food count.
        /// </summary>
        /// <returns>current food count</returns>
        public int getFoodCount() {
            return foodCount;
        }

        /// <summary>
        /// Sets current food count.
        /// </summary>
        /// <param name="foodCount">new food count</param>
        public void setFoodCount(int foodCount) {
            this.foodCount = foodCount;
        }

        #endregion

    }
}
