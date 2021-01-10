using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pacman {
    /// <summary>
    /// <c>EntityManager</c> is a class that holds all entities present
    /// in current game. It contains methods for handling events
    /// happening to entities.
    /// </summary>
    public class EntityManager {
        private Handler handler;
        private PacMan pacMan;
        private Enemy pacManEater;
        private EnemyPink pinkie;
        private List<Entity> entities;

        /// <summary>
        /// Constructor of <c>EntityManager</c> class takes the game
        /// handler for having access to the game map and its
        /// elements and an instance of <c>PacMan</c>.
        /// It also initializes the list of entities.
        /// </summary>
        /// <param name="handler">game handler for accessing other
        ///                       elements of the game</param>
        /// <param name="pacMan">instance of PacMan character</param>
        public EntityManager(Handler handler, PacMan pacMan) {
            this.handler = handler;
            this.pacMan = pacMan;
            entities = new List<Entity>();
        }

        /// <summary>
        /// Adds food to the list of entities.
        /// Food is always added to the front of the list,
        /// in order to render it first.
        /// </summary>
        /// <param name="e">instance of Entity (this method is only
        ///                 called when the instance is of type
        ///                 Food)</param>
        public void addFood(Entity e) {
            entities.Insert(0, e);
        }

        /// <summary>
        /// Adds moving entity to the list of entities.
        /// Moving entities are always added to the rear of
        /// the list, in order to render them last.
        /// </summary>
        /// <param name="e">instance of Entity (this method is only
        ///                 called when the instance is of type
        ///                 Moving)</param>
        public void addMoving(Entity e) {
            entities.Add(e);
        }

        /// <summary>
        /// Ensures that all entities in the list of entities
        /// perform their tick method.
        /// </summary>
        public void tick() {
            for (int i = 0; i < entities.Count(); i++) {
                entities[i].tick();

                if (!entities[i].isActive()) {
                    entities.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Ensures that all entities in the list of entities
        /// perform their render method.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public void render(SpriteBatch sb) {
            foreach (Entity entity in entities) {
                entity.render(sb);
            }
        }

        #region Getters & Setters

        /// <summary>
        /// Gets game handler.
        /// </summary>
        /// <returns>game handler</returns>
        public Handler getHandler() {
            return handler;
        }

        /// <summary>
        /// Sets game handler.
        /// </summary>
        /// <param name="handler">new game handler</param>
        public void setHandler(Handler handler) {
            this.handler = handler;
        }

        /// <summary>
        /// Gets PacMan.
        /// </summary>
        /// <returns>PacMan</returns>
        public PacMan getPacMan() {
            return pacMan;
        }

        /// <summary>
        /// Gets the enemy, which is currently eating PacMan.
        /// </summary>
        /// <returns>enemy currently eatin PacMan</returns>
        public Enemy getPacManEater() {
            return pacManEater;
        }

        /// <summary>
        /// Sets the enemy, which is currently eating PacMan.
        /// </summary>
        /// <param name="pacManEater">new PacMan eater</param>
        public void setPacManEater(Enemy pacManEater) {
            this.pacManEater = pacManEater;
        }

        /// <summary>
        /// Gets the pink enemy.
        /// </summary>
        /// <returns>the pink enemy</returns>
        public EnemyPink getPinkie() {
            return pinkie;
        }

        /// <summary>
        /// Sets the pink enemy.
        /// </summary>
        /// <param name="pinkie">new pink enemy</param>
        public void setPinkie(EnemyPink pinkie) {
            this.pinkie = pinkie;
        }

        /// <summary>
        /// Gets the list of entities.
        /// </summary>
        /// <returns>the list of entities</returns>
        public List<Entity> getEntities() {
            return entities;
        }

        /// <summary>
        /// Sets the list of entities.
        /// </summary>
        /// <param name="entities">new list of entities</param>
        public void setEntities(List<Entity> entities) {
            this.entities = entities;
        }

        #endregion
    }

    /// <summary>
    /// <c>Entity</c> is an abstract class for all different types
    /// of entities in the game.
    /// </summary>
    public abstract class Entity {
        private const int DEFAULT_SIZE = 40;
        protected Handler handler;
        protected float x, y;
        protected int width, height;
        protected bool isCollidable;
        protected Rectangle bounds;
        private bool active = true;

        /// <summary>
        /// Constructor of abstract entity class always called
        /// by all entities present in game. It takes the game handler
        /// for having access to the game map and its elements,
        /// an x-coordinate and a y-coordinate, which are both
        /// absolute in pixels, rather than in map tiles.
        /// 
        /// It also sets local variables, x and y-coordinates in map
        /// tiles and the bounding rectangle.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="x">absolute x-coordinate in pixels</param>
        /// <param name="y">absolute y-coordinate in pixels</param>
        public Entity(Handler handler, float x, float y) {
            this.handler = handler;
            this.x = x;
            this.y = y;
            this.width = DEFAULT_SIZE;
            this.height = DEFAULT_SIZE;

            bounds = new Rectangle(0, 0, width, height);
        }

        /// <summary>
        /// Checks if this entity collides with any other entity.
        /// </summary>
        /// <param name="xOff">x-coordinate to be checked</param>
        /// <param name="yOff">y-coordinate to be checked</param>
        /// <returns>entity colliding with this entity, or <c>null</c>, if there aren't any collisions</returns>
        protected Entity checkCollisions(float xOff, float yOff) {
            foreach (Entity e in handler.getMap().getEntityManager().getEntities()) {
                if (e.Equals(this))
                    continue;
                if (e.getBounds(0f, 0f).Intersects(getBounds(xOff, yOff)))
                    return e;
            }

            return null;
        }

        /// <summary>
        /// Calculates bounds of this entity with offsets in
        /// both directions.
        /// </summary>
        /// <param name="xOff">x axis offset</param>
        /// <param name="yOff">y axis offset</param>
        /// <returns>bounding rectangle of this offset entity</returns>
        private Rectangle getBounds(float xOff, float yOff) {
            return new Rectangle((int) (x + bounds.X + xOff), (int) (y + bounds.Y + yOff), bounds.Width, bounds.Height);
        }

        public abstract void tick();

        public abstract void render(SpriteBatch sb);

        #region Getters & Setters

        /// <summary>
        /// Gets x-coordinate of this entity.
        /// </summary>
        /// <returns>x-coordinate of this entity</returns>
        public float getX() {
            return x;
        }

        /// <summary>
        /// Sets x-coordinate of this entity.
        /// </summary>
        /// <param name="x">new x-coordinate</param>
        public void setX(float x) {
            this.x = x;
        }

        /// <summary>
        /// Gets y-coordinate of this entity.
        /// </summary>
        /// <returns>y-coordinate of this entity</returns>
        public float getY() {
            return y;
        }

        /// <summary>
        /// Sets y-coordinate of this entity.
        /// </summary>
        /// <param name="y">new y-coordinate</param>
        public void setY(float y) {
            this.y = y;
        }

        /// <summary>
        /// Gets entity's active status.
        /// </summary>
        /// <returns><c>true</c>, if entity is active, <c>false</c> otherwise</returns>
        public bool isActive() {
            return active;
        }

        /// <summary>
        /// Sets entity active status.
        /// </summary>
        /// <param name="active">new active status</param>
        public virtual void setActive(bool active) {
            this.active = active;
        }

        /// <summary>
        /// Gets entity's collidable status.
        /// </summary>
        /// <returns><c>false</c>, if entity is collidable, <c>true</c> otherwise</returns>
        public bool notCollidable() {
            return !isCollidable;
        }

        #endregion
    }

    /// <summary>
    /// <c>Moving</c> is an abstract class for all different types
    /// of moving entities in the game.
    /// </summary>
    public abstract class Moving : Entity {
        private const float DEFAULT_SPEED = 4.0f;

        protected float xSpawn, ySpawn;
        protected bool alive;
        protected float speed;
        protected float xMove, yMove;
        protected Orientation orientation;

        /// <summary>
        /// Constructor of abstract moving class always called
        /// by all moving entities present in game.It takes the
        /// game handler for having access to the game map and
        /// its elements, an x-coordinate and a y-coordinate,
        /// which are both absolute in pixels, rather than in map tiles.
        /// 
        /// It also sets local variables, such as alive status, speed,
        /// x and y-coordinates in map tiles and the bounding rectangle.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="x">absolute x-coordinate in pixels</param>
        /// <param name="y">absolute y-coordinate in pixels</param>
        public Moving(Handler handler, float x, float y) : base(handler, x, y) {
            alive = true;
            speed = DEFAULT_SPEED;
            xMove = 0;
            yMove = 0;
            xSpawn = x;
            ySpawn = y;
            isCollidable = true;

            bounds.X = 1;
            bounds.Y = 1;
            bounds.Width = 38;
            bounds.Height = 38;
        }

        /// <summary>
        /// General moving method for all moving entities
        /// in the game.It has two different implementations,
        /// the PacMan's and the enemies'.
        /// </summary>
        public abstract void move();

        /// <summary>
        /// Sets the x-coordinate for moved entity.
        /// It makes no changes, when <c>xMove</c>
        /// is 0.
        /// </summary>
        public abstract void moveX();

        /// <summary>
        /// Sets the y-coordinate for moved entity.
        /// It makes no changes, when <c>yMove</c>
        /// is 0.
        /// </summary>
        public abstract void moveY();

        /// <summary>
        /// Moves the entity to the right if it is possible
        /// and sets orientation to <c>RIGHT</c>.
        /// When it is not possible to move right, it makes
        /// no changes.
        /// </summary>
        protected void moveRight() {
            int tx = (int) (x + xMove + bounds.X + bounds.Width) / Tile.SIZE;
            if (canGoRight(tx)) {
                x += xMove;
            } else {
                x = tx * Tile.SIZE - 2 * bounds.X - bounds.Width;
            }
            orientation = Orientation.RIGHT;
        }

        /// <summary>
        /// Moves the entity to the left if it is possible
        /// and sets orientation to <c>LEFT</c>.
        /// When it is not possible to move left, it makes
        /// no changes.
        /// </summary>
        protected void moveLeft() {
            int tx = (int) (x + xMove + bounds.X) / Tile.SIZE;
            if (canGoLeft(tx)) {
                x += xMove;
            } else {
                x = tx * Tile.SIZE + Tile.SIZE;
            }
            orientation = Orientation.LEFT;
        }

        /// <summary>
        /// Moves the entity up if it is possible
        /// and sets orientation to <c>UP</c>.
        /// When it is not possible to move up, it makes
        /// no changes.
        /// </summary>
        protected void moveUp() {
            int ty = (int) (y + yMove + bounds.Y) / Tile.SIZE;
            if (canGoUp(ty)) {
                y += yMove;
            } else {
                y = ty * Tile.SIZE + Tile.SIZE;
            }
            orientation = Orientation.UP;
        }

        /// <summary>
        /// Moves the entity down if it is possible
        /// and sets orientation to <c>DOWN</c>.
        /// When it is not possible to move down, it makes
        /// no changes.
        /// </summary>
        protected void moveDown() {
            int ty = (int) (y + yMove + bounds.Y + bounds.Height) / Tile.SIZE;
            if (canGoDown(ty)) {
                y += yMove;
            } else {
                y = ty * Tile.SIZE - 2 * bounds.Y - bounds.Height;
            }
            orientation = Orientation.DOWN;
        }

        /// <summary>
        /// Checks the upper and lower right corner of
        /// the entity for collisions with walls.
        /// </summary>
        /// <param name="tx">x-coordinate of target tile</param>
        /// <returns><c>true</c>, if there is no collision, <c>false</c> otherwise</returns>
        protected bool canGoRight(int tx) {
            return noCollisionWithTile(tx, (int) (y + bounds.Y) / Tile.SIZE) &&
                    noCollisionWithTile(tx, (int) (y + bounds.Y + bounds.Height) / Tile.SIZE);
        }

        /// <summary>
        /// Checks the upper and lower left corner of
        /// the entity for collisions with walls.
        /// </summary>
        /// <param name="tx">x-coordinate of target tile</param>
        /// <returns><c>true</c>, if there is no collision, <c>false</c> otherwise</returns>
        protected bool canGoLeft(int tx) {
            return noCollisionWithTile(tx, (int) (y + bounds.Y) / Tile.SIZE) &&
                    noCollisionWithTile(tx, (int) (y + bounds.Y + bounds.Height) / Tile.SIZE);
        }

        /// <summary>
        /// Checks the left and right upper corner of
        /// the entity for collisions with walls.
        /// </summary>
        /// <param name="ty">y-coordinate of target tile</param>
        /// <returns><c>true</c>, if there is no collision, <c>false</c> otherwise</returns>
        protected bool canGoUp(int ty) {
            return noCollisionWithTile((int) (x + bounds.X) / Tile.SIZE, ty) &&
                    noCollisionWithTile((int) (x + bounds.X + bounds.Width) / Tile.SIZE, ty);
        }

        /// <summary>
        /// Checks the left and right lower corner of
        /// the entity for collisions with walls.
        /// </summary>
        /// <param name="ty">y-coordinate of target tile</param>
        /// <returns><c>true</c>, if there is no collision, <c>false</c> otherwise</returns>
        protected bool canGoDown(int ty) {
            return noCollisionWithTile((int) (x + bounds.X) / Tile.SIZE, ty) &&
                    noCollisionWithTile((int) (x + bounds.X + bounds.Width) / Tile.SIZE, ty);
        }

        /// <summary>
        /// Checks if tile on position <c>(x, y)</c>
        /// is free.
        /// </summary>
        /// <param name="x">x-coordinate of examined tile</param>
        /// <param name="y">y-coordinate of examined tile</param>
        /// <returns><c>true</c>, if tile is not a wall, <c>false</c> otherwise</returns>
        private bool noCollisionWithTile(int x, int y) {
            return !handler.getMap().getTile(x, y).isObstacle();
        }

        #region Getters & Setters

        /// <summary>
        /// Gets alive status of entity.
        /// </summary>
        /// <returns><c>false</c>, if entity is alive, <c>true</c> otherwise</returns>
        public bool notAlive() {
            return !alive;
        }

        /// <summary>
        /// Sets alive status of entity.
        /// </summary>
        /// <param name="alive">new alive status of entity</param>
        public virtual void setAlive(bool alive) {
            this.alive = alive;
        }

        /// <summary>
        /// Gets x-coordinate of entity's spawn point.
        /// </summary>
        /// <returns>x-coordinate of entity's spawn point</returns>
        public float getxSpawn() {
            return xSpawn;
        }

        /// <summary>
        /// Sets x-coordinate of entity's spawn point.
        /// </summary>
        /// <param name="xSpawn">new x-coordinate of entity's spawn point</param>
        public void setxSpawn(float xSpawn) {
            this.xSpawn = xSpawn;
        }

        /// <summary>
        /// Gets y-coordinate of entity's spawn point.
        /// </summary>
        /// <returns>y-coordinate of entity's spawn point</returns>
        public float getySpawn() {
            return ySpawn;
        }

        /// <summary>
        /// Sets y-coordinate of entity's spawn point.
        /// </summary>
        /// <param name="ySpawn">new y-coordinate of entity's spawn point</param>
        public void setySpawn(float ySpawn) {
            this.ySpawn = ySpawn;
        }

        /// <summary>
        /// Gets entity's orientation.
        /// </summary>
        /// <returns>entity's orientation</returns>
        public Orientation getOrientation() {
            return orientation;
        }

        /// <summary>
        /// Sets entity orientation.
        /// </summary>
        /// <param name="orientation">new entity orientation</param>
        public void setOrientation(Orientation orientation) {
            this.orientation = orientation;
        }

        #endregion
    }

    /// <summary>
    /// PacMan class controls PacMan's movements and rendering.
    /// It has specific movement behaviour, because its
    /// dependent on user keyboard input.
    /// </summary>
    public class PacMan : Moving {
        private const int PACMAN_SIZE = 38;
        private const int ANIMATION_SPEED = 50;

        private Animation aDown, aUp, aLeft, aRight;

        /// <summary>
        /// Constructor of the <c>Animation</c> class. It takes the speed
        /// of the animation and an array of images that contains
        /// the animation frame by frame.It also sets local
        /// variables, such as the indexing variable and timer
        /// variables.
        /// 
        /// The <c>speed</c> field of the animation is a millisecond value,
        /// which means, that for example for <c>speed = 50</code>, the current
        /// frame of the animation will change every 50 ms, resulting in
        /// 20 frames per second animation.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="x">absolute x-coordinate in pixels</param>
        /// <param name="y">absolute y-coordinate in pixels</param>
        public PacMan(Handler handler, float x, float y) : base(handler, x, y) {
            width = PACMAN_SIZE;
            height = PACMAN_SIZE;
            orientation = Orientation.RIGHT;

            bounds.X = 1;
            bounds.Y = 1;
            bounds.Width = 38;
            bounds.Height = 38;

            aDown = new Animation(ANIMATION_SPEED, Assets.pacManDown);
            aUp = new Animation(ANIMATION_SPEED, Assets.pacManUp);
            aLeft = new Animation(ANIMATION_SPEED, Assets.pacManLeft);
            aRight = new Animation(ANIMATION_SPEED, Assets.pacManRight);
        }

        /// <summary>
        /// PacMan's implementation of general move method
        /// which is abstract in <c>Moving</c> class.
        /// Checks for collisions and handles their
        /// consequences, if there are any.
        /// </summary>
        public override void move() {
            if ((xMove > 0) || (xMove < 0)) {
                Entity e = checkCollisions(xMove, 0f);
                // not colliding with any entity
                if (e == null) {
                    moveX();
                    // colliding with food
                } else if (e.notCollidable()) {
                    e.setActive(false);
                    moveX();
                    // colliding with an enemy
                } else if (typeof(Enemy).IsAssignableFrom(e.GetType())) {
                    if (((Enemy) e).notAlive()) {
                        moveX();
                    } else {
                        handleCollision((Enemy) e);
                    }
                }
            } else if ((yMove > 0) || (yMove < 0)) {
                Entity e = checkCollisions(0f, yMove);
                // not colliding with any entity
                if (e == null)
                    moveY();
                // colliding with food
                else if (e.notCollidable()) {
                    e.setActive(false);
                    moveY();
                    // colliding with an enemy
                } else if (typeof(Enemy).IsAssignableFrom(e.GetType())) {
                    if (((Enemy) e).notAlive()) {
                        moveY();
                    } else {
                        handleCollision((Enemy) e);
                    }
                }
            }
        }

        /// <summary>
        /// Handles PacMan's movement on the x axis.
        /// It is dependent on PacMan's current orientation,
        /// current alignment with the tile grid and user input.
        /// It makes no changes, when <c>xMove</c> is 0.
        /// </summary>
        public override void moveX() {
            //moving right
            if (xMove > 0) {
                switch (orientation) {
                    // if already moving in x-direction, change move immediately
                    case Orientation.RIGHT:
                    case Orientation.LEFT:
                        moveRight();
                        break;
                    // if moving up, check if aligned with tile grid, then try to move right,
                    // if not aligned with tile grid, continue moving up
                    case Orientation.UP:
                        if ((int) (y) % 40 == 0) {
                            int tx = (int) (x + xMove + bounds.X + bounds.Width) / Tile.SIZE;
                            if (canGoRight(tx)) {
                                moveRight();
                            } else {
                                xMove = 0;
                                yMove -= speed;
                                moveUp();
                            }
                        } else {
                            xMove = 0;
                            yMove -= speed;
                            moveUp();
                        }
                        break;
                    // if moving down, check if aligned with tile grid, then try to move right,
                    // if not aligned with tile grid, continue moving down
                    case Orientation.DOWN:
                        if ((int) (y) % 40 == 0) {
                            int tx = (int) (x + xMove + bounds.X + bounds.Width) / Tile.SIZE;
                            if (canGoRight(tx)) {
                                moveRight();
                            } else {
                                xMove = 0;
                                yMove = speed;
                                moveDown();
                            }
                        } else {
                            xMove = 0;
                            yMove = speed;
                            moveDown();
                        }
                        break;
                    default:
                        break;
                }

                //moving left
            } else if (xMove < 0) {
                switch (orientation) {
                    // if already moving in x-direction, change move immediately
                    case Orientation.LEFT:
                    case Orientation.RIGHT:
                        moveLeft();
                        break;
                    // if moving up, check if aligned with tile grid, then try to move left,
                    // if not aligned with tile grid, continue moving up
                    case Orientation.UP:
                        if ((int) (y) % 40 == 0) {
                            int tx = (int) (x + xMove + bounds.X) / Tile.SIZE;
                            if (canGoLeft(tx)) {
                                moveLeft();
                            } else {
                                xMove = 0;
                                yMove -= speed;
                                moveUp();
                            }
                        } else {
                            xMove = 0;
                            yMove -= speed;
                            moveUp();
                        }
                        break;
                    // if moving down, check if aligned with tile grid, then try to move left,
                    // if not aligned with tile grid, continue moving down
                    case Orientation.DOWN:
                        if ((int) (y) % 40 == 0) {
                            int tx = (int) (x + xMove + bounds.X) / Tile.SIZE;
                            if (canGoLeft(tx)) {
                                moveLeft();
                            } else {
                                xMove = 0;
                                yMove = speed;
                                moveDown();
                            }
                        } else {
                            xMove = 0;
                            yMove = speed;
                            moveDown();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Handles PacMan's movement on the y axis.
        /// It is dependent on PacMan's current orientation,
        /// current alignment with the tile grid and user input.
        /// It makes no changes, when <c>yMove</c> is 0.
        /// </summary>
        public override void moveY() {
            //moving up
            if (yMove < 0) {
                switch (orientation) {
                    // if already moving in y-direction, change move immediately
                    case Orientation.UP:
                    case Orientation.DOWN:
                        moveUp();
                        break;
                    // if moving left, check if aligned with tile grid, then try to move up,
                    // if not aligned with tile grid, continue moving left
                    case Orientation.LEFT:
                        if ((int) (x) % 40 == 0) {
                            int ty = (int) (y + yMove + bounds.Y) / Tile.SIZE;
                            if (canGoUp(ty)) {
                                moveUp();
                            } else {
                                yMove = 0;
                                xMove -= speed;
                                moveLeft();
                            }
                        } else {
                            yMove = 0;
                            xMove -= speed;
                            moveLeft();
                        }
                        break;
                    // if moving right, check if aligned with tile grid, then try to move up,
                    // if not aligned with tile grid, continue moving right
                    case Orientation.RIGHT:
                        if ((int) (x) % 40 == 0) {
                            int ty = (int) (y + yMove + bounds.Y) / Tile.SIZE;
                            if (canGoUp(ty)) {
                                moveUp();
                            } else {
                                yMove = 0;
                                xMove = speed;
                                moveRight();
                            }
                        } else {
                            yMove = 0;
                            xMove = speed;
                            moveRight();
                        }
                        break;
                    default:
                        break;
                }
                //moving down
            } else if (yMove > 0) {
                switch (orientation) {
                    // if already moving in y-direction, change move immediately
                    case Orientation.DOWN:
                    case Orientation.UP:
                        moveDown();
                        break;
                    // if moving left, check if aligned with tile grid, then try to move down,
                    // if not aligned with tile grid, continue moving left
                    case Orientation.LEFT:
                        if ((int) (x) % 40 == 0) {
                            int ty = (int) (y + yMove + bounds.Y + bounds.Height) / Tile.SIZE;
                            if (canGoDown(ty)) {
                                moveDown();
                            } else {
                                yMove = 0;
                                xMove -= speed;
                                moveLeft();
                            }
                        } else {
                            yMove = 0;
                            xMove -= speed;
                            moveLeft();
                        }
                        break;
                    // if moving right, check if aligned with tile grid, then try to move down,
                    // if not aligned with tile grid, continue moving right
                    case Orientation.RIGHT:
                        if ((int) (x) % 40 == 0) {
                            int ty = (int) (y + yMove + bounds.Y + bounds.Height) / Tile.SIZE;
                            if (canGoDown(ty)) {
                                moveDown();
                            } else {
                                yMove = 0;
                                xMove = speed;
                                moveRight();
                            }
                        } else {
                            yMove = 0;
                            xMove = speed;
                            moveRight();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Handles PacMan's collision with a ghost.
        /// In accordance to current game situation, it sets
        /// corresponding action.
        /// </summary>
        /// <param name="e">enemy colliding with PacMan</param>
        private void handleCollision(Enemy e) {
            if (handler.getMap().isBlueGhosts()) {
                // PacMan can eat enemies and the colliding one is not immune to be eaten
                if (e.notImmuneToBlue()) {
                    e.setAlive(false);
                    moveX();
                    // PacMan loses a life
                } else {
                    handler.getGame().setLives(handler.getGame().getLives() - 1);
                    handler.getMap().setEatingPacMan(true, e);
                }
                // PacMan loses a life
            } else {
                handler.getGame().setLives(handler.getGame().getLives() - 1);
                handler.getMap().setEatingPacMan(true, e);
            }
        }

        /// <summary>
        /// Calculates next animation image and next move of PacMan.
        /// Next move is only calculated, if the game is running
        /// and PacMan is not in process of losing a life.
        /// </summary>
        public override void tick() {
            xMove = 0;
            yMove = 0;
            aDown.tick();
            aUp.tick();
            aLeft.tick();
            aRight.tick();

            if (handler.getMap().notLevelBegin() && !handler.getMap().isEatingPacMan()) {
                getInput();
                move();
            }
        }

        /// <summary>
        /// According to user input, it sets the
        /// <c>xMove</c> and <c>yMove</c>
        /// variables to desired values.
        /// </summary>
        private void getInput() {
            if (handler.getKeyManager().up) {
                yMove = -speed;
            }
            if (handler.getKeyManager().down) {
                yMove = speed;
            }
            if (handler.getKeyManager().left) {
                xMove = -speed;
            }
            if (handler.getKeyManager().right) {
                xMove = speed;
            }
        }

        /// <summary>
        /// Draws current animation of PacMan.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public override void render(SpriteBatch sb) {
            sb.Draw(getCurrentAnimationType(),
                new Rectangle((int) x + bounds.X, (int) y + bounds.Y, width, height),
                Color.White);
        }

        /// <summary>
        /// Gets current animation type for PacMan rendering.
        /// </summary>
        /// <returns>current animation to render</returns>
        private Texture2D getCurrentAnimationType() {
            if (orientation == Orientation.LEFT) {
                return aLeft.getCurrentType();
            } else if (orientation == Orientation.UP) {
                return aUp.getCurrentType();
            } else if (orientation == Orientation.DOWN) {
                return aDown.getCurrentType();
            } else {
                return aRight.getCurrentType();
            }
        }
    }

    /// <summary>
    /// <c>Enemy</c> is an abstract class for all different types
    /// of ghosts present in the game.
    /// <para>Each type of ghost has different behaviour regarding
    /// his movements in chase and scatter modes. All share the
    /// same behaviour, when in blue-ghosts mode or dead mode.</para>
    /// </summary>
    public abstract class Enemy : Moving {
        protected bool rendered;
        protected int xTile, yTile;
        protected int targetX, targetY;
        protected int[] dX = { 1, -1, 0, 0 };
        protected int[] dY = { 0, 0, -1, 1 };
        protected Random random;
        private int resurrectionTimer, eatingPacManConstant = 0;
        private bool immuneToBlue;
        private bool[] free;
        private double currentMinimal;

        /// <summary>
        /// Constructor of abstract enemy class always called
        /// by all ghosts present in game.It takes the game handler
        /// for having access to the game map and its elements,
        /// an x-coordinate and a y-coordinate, which are both
        /// absolute in pixels, rather than in map tiles.
        /// 
        /// It also sets local variables, such as random number
        /// generator, x and y-coordinates in map tiles and
        /// the bounding rectangle.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="x">absolute x-coordinate in pixels</param>
        /// <param name="y">absolute y-coordinate in pixels</param>
        public Enemy(Handler handler, float x, float y) : base(handler, x, y) {
            random = new Random();

            immuneToBlue = false;
            orientation = orientation.fromInt(random.Next(3));

            targetX = (int) xSpawn / Tile.SIZE;
            targetY = (int) ySpawn / Tile.SIZE;

            bounds.X = 4;
            bounds.Y = 2;
            bounds.Width = 32;
            bounds.Height = 36;
        }

        /// <summary>
        /// Enemies' implementation of general move method
        /// which is abstract in <c>Moving</c> class.
        /// Checks for collision with PacMan and handles its
        /// consequences, if there are any.
        /// </summary>
        public override void move() {
            if ((xMove > 0) || (xMove < 0)) {
                Entity e = checkCollisions(xMove, 0f);
                // not colliding with any entity, or colliding with food or other enemy
                if ((e == null) || (e.notCollidable()) || (typeof(Enemy).IsAssignableFrom(e.GetType()))) {
                    moveX();
                    // colliding with PacMan
                } else if (typeof(PacMan).IsAssignableFrom(e.GetType())) {
                    if (this.notAlive()) {
                        moveX();
                    } else {
                        handleCollision();
                    }
                }
            } else if ((yMove > 0) || (yMove < 0)) {
                Entity e = checkCollisions(0f, yMove);
                // not colliding with any entity, or colliding with food or other enemy
                if ((e == null) || (e.notCollidable()) || (typeof(Enemy).IsAssignableFrom(e.GetType()))) {
                    moveY();
                    // colliding with PacMan
                } else if (typeof(PacMan).IsAssignableFrom(e.GetType())) {
                    if (this.notAlive()) {
                        moveY();
                    } else {
                        handleCollision();
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override void moveX() {
            if (xMove > 0) {
                moveRight();
            } else if (xMove < 0) {
                moveLeft();
            }
        }

        /// <inheritdoc/>
        public override void moveY() {
            //moving up
            if (yMove < 0) {
                moveUp();
            } else if (yMove > 0) {
                moveDown();
            }
        }

        /// <summary>
        /// Handles enemies' collision with PacMan.
        /// In accordance to current game situation, it sets
        /// corresponding action.
        /// </summary>
        private void handleCollision() {
            if (handler.getMap().isBlueGhosts()) {
                // PacMan can eat enemies and the colliding one is not immune to be eaten
                if (this.notImmuneToBlue()) {
                    this.setAlive(false);
                    // PacMan loses a life
                } else {
                    handler.getGame().setLives(handler.getGame().getLives() - 1);
                    handler.getMap().setEatingPacMan(true, this);
                }
                // PacMan loses a life
            } else {
                handler.getGame().setLives(handler.getGame().getLives() - 1);
                handler.getMap().setEatingPacMan(true, this);
            }
        }

        /// <summary>
        /// Basic action when moving in chase mode.
        /// It sets the enemy's coordinates to new ones.
        /// </summary>
        protected virtual void moveChase() {
            setMovement(targetX, targetY);
        }

        /// <summary>
        /// Basic action when moving in scatter mode.
        /// It sets the enemy's coordinates to new ones.
        /// </summary>
        protected virtual void moveScatter() {
            setMovement(targetX, targetY);
        }

        /// <summary>
        /// Handles movement of enemies, when they can be eaten
        /// by PacMan.
        /// <para>When PacMan eats a killer food, the enemies turn blue
        /// and can be temporarily eaten by PacMan. In this state,
        /// they choose their destination tile randomly on every
        /// crossroads.</para>
        /// </summary>
        private void moveBlue() {
            targetX = random.Next(20);
            targetY = random.Next(20);

            setMovement(targetX, targetY);
        }

        /// <summary>
        /// Handles movement of enemies, when they are not alive.
        /// <para>When PacMan eats an enemy, the enemy has to return
        /// to its spawn coordinates before it is resurrected.
        /// If it comes to its spawn coordinates too quickly, it
        /// has to wait some time (the overall time of being dead
        /// cannot be less than 2 seconds).</para>
        /// <para>If enemy is already present at its spawn coordinates,
        /// it is resurrected, if the minimum dead time has
        /// passed.Otherwise, the target tile is the enemy's
        /// spawn tile.</para>
        /// </summary>
        private void moveDead() {
            if ((x == xSpawn) && (y == ySpawn)) {
                if (resurrectionTimer < 0) {
                    setAlive(true);
                }
                return;
            }

            targetX = (int) xSpawn / Tile.SIZE;
            targetY = (int) ySpawn / Tile.SIZE;

            setMovement(targetX, targetY);
        }

        /// <summary>
        /// Moves the enemy, which is eating PacMan.
        /// If this enemy is the one that caught PacMan and is eating him,
        /// it is moved to PacMan's coordinates in 5 steps, creating
        /// an animation of eating PacMan.
        /// </summary>
        private void moveEating() {
            float pacManX = handler.getMap().getEntityManager().getPacMan().getX();
            float pacManY = handler.getMap().getEntityManager().getPacMan().getY();

            xMove += (1f / (5 - eatingPacManConstant)) * (pacManX - x);
            yMove += (1f / (5 - eatingPacManConstant)) * (pacManY - y);

            eatingPacManConstant = (eatingPacManConstant + 1) % 5;
        }

        /// <summary>
        /// Finds free neighbour tiles to go to.
        /// This method is only called when enemy is aligned
        /// with the tile grid.It also ensures, that the enemy
        /// won't turn around unless it is necessary (it is the
        /// only possible move).
        /// </summary>
        private void findFree() {
            free = new bool[4];
            int freeCount = 0;

            free[orientation.toInt(Orientation.RIGHT)] = (handler.getMap().getTile(xTile + 1, yTile).getId() ==
                    Tile.empty.getId());
            free[orientation.toInt(Orientation.LEFT)] = (handler.getMap().getTile(xTile - 1, yTile).getId() ==
                    Tile.empty.getId());
            free[orientation.toInt(Orientation.UP)] = (handler.getMap().getTile(xTile, yTile - 1).getId() ==
                    Tile.empty.getId());
            free[orientation.toInt(Orientation.DOWN)] = (handler.getMap().getTile(xTile, yTile + 1).getId() ==
                    Tile.empty.getId());

            foreach (bool freeTile in free) {
                if (freeTile) {
                    freeCount++;
                }
            }

            //ensuring ghosts wouldn't get stuck in tunnels (no turning back other than single-choice corners)
            if (freeCount > 1) {
                switch (orientation) {
                    case Orientation.RIGHT:
                        free[orientation.toInt(Orientation.LEFT)] = false;
                        break;
                    case Orientation.LEFT:
                        free[orientation.toInt(Orientation.RIGHT)] = false;
                        break;
                    case Orientation.UP:
                        free[orientation.toInt(Orientation.DOWN)] = false;
                        break;
                    case Orientation.DOWN:
                        free[orientation.toInt(Orientation.UP)] = false;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Chooses a free neighbour tile which is the closest
        /// to the target tile and sets enemy's movement in that
        /// direction.
        /// </summary>
        /// <param name="targetX">x-coordinate of target tile</param>
        /// <param name="targetY">y-coordinate of target tile</param>
        private void setMovement(int targetX, int targetY) {
            for (int i = 0; i < free.Length; i++) {
                if (free[i]) {
                    double distance = Distance(xTile + dX[i], yTile + dY[i], targetX, targetY);
                    if (distance < currentMinimal) {
                        currentMinimal = distance;
                        xMove = dX[i] * speed;
                        yMove = dY[i] * speed;
                    }
                }
            }
        }

        /// <summary>
        /// Immediately flips the enemy's orientation.
        /// This method is called whenever there is a change
        /// in enemies' movement mode, for example from chase
        /// to scatter or from blue ghosts to chase.
        /// </summary>
        public void flipOrientation() {
            switch (orientation) {
                case Orientation.RIGHT:
                    orientation = Orientation.LEFT;
                    break;
                case Orientation.LEFT:
                    orientation = Orientation.RIGHT;
                    break;
                case Orientation.UP:
                    orientation = Orientation.DOWN;
                    break;
                case Orientation.DOWN:
                    orientation = Orientation.UP;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Gets euclidean distance between two tiles.
        /// </summary>
        /// <param name="x1">x-coordinate of first tile</param>
        /// <param name="y1">y-coordinate of first tile</param>
        /// <param name="x2">x-coordinate of second tile</param>
        /// <param name="y2">y-coordinate of second tile</param>
        /// <returns>euclidean distance between the two tiles</returns>
        protected double Distance(int x1, int y1, int x2, int y2) {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        /// <summary>
        /// Chooses which calculation of next enemy move should be done
        /// and calls it.
        /// </summary>
        public override void tick() {
            xMove = 0;
            yMove = 0;

            if (handler.getMap().notLevelBegin()) {
                // if this is the enemy eating PacMan, move this accordingly and return
                if (handler.getMap().isEatingPacMan()) {
                    if (handler.getMap().getEntityManager().getPacManEater() == this) {
                        moveEating();
                        x += xMove;
                        y += yMove;
                        return;
                    }
                }

                // if enemy is not aligned to the tile grid, continue movement in current direction
                if (x % 40 != 0) {
                    switch (orientation) {
                        case Orientation.RIGHT:
                            xMove += speed;
                            break;
                        case Orientation.LEFT:
                            xMove -= speed;
                            break;
                        default:
                            break;
                    }
                } else if (y % 40 != 0) {
                    switch (orientation) {
                        case Orientation.UP:
                            yMove -= speed;
                            break;
                        case Orientation.DOWN:
                            yMove += speed;
                            break;
                        default:
                            break;
                    }
                } else {
                    // getting ghosts position in tile-coordinates (this code is only reached, if it is aligned to the grid)
                    xTile = (int) x / Tile.SIZE;
                    yTile = (int) y / Tile.SIZE;

                    // find options for next move and reset currentMinimal
                    findFree();
                    currentMinimal = Double.PositiveInfinity;

                    // adjust speed and move regarding to current game situation
                    if (!alive) {
                        resurrectionTimer--;
                        speed = 8;
                        moveDead();
                    } else if (handler.getMap().isBlueGhosts()) {
                        speed = 1;
                        moveBlue();
                    } else if (handler.getMap().isChase()) {
                        speed = 2.5f;
                        moveChase();
                    } else if (handler.getMap().isScatter()) {
                        speed = 4;
                        moveScatter();
                    }
                }

                move();
            }
        }

        /// <summary>
        /// Draws enemy if it is dead or if the game is in blue-ghosts mode.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public override void render(SpriteBatch sb) {
            rendered = false;
            if (notAlive()) {
                sb.Draw(Assets.enemyDead,
                    new Rectangle((int) x, (int) y, width, height),
                    Color.White);
                rendered = true;
                return;
            }
            if (handler.getMap().isBlueGhosts()) {
                if (!immuneToBlue) {
                    sb.Draw(Assets.enemyBlue,
                    new Rectangle((int) x, (int) y, width, height),
                    Color.White);
                    rendered = true;
                }
            }
        }

        #region Getters & Setters

        /// <summary>
        /// Sets alive status of enemy and adjusts local variables
        /// according to new alive status.
        /// </summary>
        /// <param name="alive">new alive status of enemy</param>
        public override void setAlive(bool alive) {
            base.setAlive(alive);

            if (alive) {
                immuneToBlue = true;
            } else {
                Thread t = new Thread(() => handler.getAudio().playOneShot(Assets.ghostEaten));
                t.Start();
                handler.getGame().setScore(handler.getGame().getScore() + 200);
                resurrectionTimer = 100;
            }
        }

        /// <summary>
        /// Gets immunity status of enemy.
        /// </summary>
        /// <returns><c>false</c>, if enemy is immune, <c>true</c> otherwise</returns>
        public bool notImmuneToBlue() {
            return !immuneToBlue;
        }

        /// <summary>
        /// Gets immunity status of enemy.
        /// </summary>
        /// <param name="immuneToBlue"><c>false</c>, if enemy is immune, <c>true</c> otherwise</param>
        public void setImmuneToBlue(bool immuneToBlue) {
            this.immuneToBlue = immuneToBlue;
        }

        #endregion
    }

    /// <summary>
    /// <c>EnemyPink</c> class controls movements and rendering
    /// of the pink enemy.
    /// </summary>
    public class EnemyPink : Enemy {

        /// <summary>
        /// Constructor of <c>EnemyPink</c> class only calls constructor
        /// of the abstract <c>Enemy</c> class. It takes the game handler
        /// for having access to the game map and its elements,
        /// an x-coordinate and a y-coordinate, which are both
        /// absolute in pixels, rather than in map tiles.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="x">absolute x-coordinate in pixels</param>
        /// <param name="y">absolute y-coordinate in pixels</param>
        public EnemyPink(Handler handler, float x, float y) : base(handler, x, y) {

        }

        /// <summary>
        ///  Pink enemy's implementation of chase mode movement.
        ///
        /// <para>In the chase mode, the pink enemy's target tile is
        /// PacMan's current position.</para>
        /// </summary>
        protected override void moveChase() {
            targetX = (int) handler.getMap().getEntityManager().getPacMan().getX() / Tile.SIZE;
            targetY = (int) handler.getMap().getEntityManager().getPacMan().getY() / Tile.SIZE;

            base.moveChase();
        }

        /// <summary>
        /// Pink enemy's implementation of scatter mode movement.
        /// <para>In the scatter mode, the pink enemy's target tile is
        /// located outside the maze in the upper left corner,
        /// resulting in it circulating around obstacles as near
        /// that corner as possible.</para>
        /// </summary>
        protected override void moveScatter() {
            targetX = -1;
            targetY = -1;

            base.moveScatter();
        }

        /// <summary>
        /// Draws pink enemy's image if it wasn't rendered by
        /// super method.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public override void render(SpriteBatch sb) {
            base.render(sb);
            if (!rendered) {
                sb.Draw(Assets.enemyPink,
                    new Rectangle((int) x, (int) y, width, height),
                    Color.White);
            }
        }
    }

    /// <summary>
    /// <c>EnemyPurple</c> class controls movements and rendering
    /// of the purple enemy.
    /// </summary>
    public class EnemyPurple : Enemy {
        /// <summary>
        /// Constructor of <c>EnemyPurple</c> class only calls constructor
        /// of the abstract <c>Enemy</c> class. It takes the game handler
        /// for having access to the game map and its elements,
        /// an x-coordinate and a y-coordinate, which are both
        /// absolute in pixels, rather than in map tiles.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="x">absolute x-coordinate in pixels</param>
        /// <param name="y">absolute y-coordinate in pixels</param>
        public EnemyPurple(Handler handler, float x, float y) : base(handler, x, y) {

        }

        /// <summary>
        /// Purple enemy's implementation of chase mode movement.
        /// <para>In the chase mode, the purple enemy's target tile is
        /// 4 tiles in front of PacMan, but if the purple enemy's
        /// euclidean distance from PacMan is less than 6, its
        /// target tile is located outside the maze in the lower
        /// right corner, resulting in it circulating around
        /// obstacles as near that corner as possible.</para>
        /// </summary>
        protected override void moveChase() {
            int pacManX = (int) handler.getMap().getEntityManager().getPacMan().getX() / Tile.SIZE;
            int pacManY = (int) handler.getMap().getEntityManager().getPacMan().getY() / Tile.SIZE;

            if (Distance(xTile, yTile, pacManX, pacManY) > 6) {
                targetX = pacManX +
                        dX[orientation.toInt(handler.getMap().getEntityManager().getPacMan().getOrientation())] * 4;
                targetY = pacManY +
                        dY[orientation.toInt(handler.getMap().getEntityManager().getPacMan().getOrientation())] * 4;
            } else {
                targetX = handler.getMap().getWidth();
                targetY = handler.getMap().getHeight();
            }

            base.moveChase();
        }

        /// <summary>
        /// Purple enemy's implementation of scatter mode movement.
        /// <para>In the scatter mode, the purple enemy's target tile is
        /// located outside the maze in the lower right corner,
        /// resulting in it circulating around obstacles as near
        /// that corner as possible.</para>
        /// </summary>
        protected override void moveScatter() {
            targetX = 18;
            targetY = 20;

            base.moveScatter();
        }

        /// <summary>
        /// Draw purple enemy's image if it wasn't rendered by
        /// super method.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public override void render(SpriteBatch sb) {
            base.render(sb);

            if (!rendered) {
                sb.Draw(Assets.enemyPurple,
                    new Rectangle((int) x, (int) y, width, height),
                    Color.White);
            }
        }
    }

    /// <summary>
    /// <c>EnemyRed</c> class controls movements and rendering
    /// of the red enemy.
    /// </summary>
    public class EnemyRed : Enemy {
        /// <summary>
        /// Constructor of <c>EnemyRed</c> class only calls constructor
        /// of the abstract <c>Enemy</c> class. It takes the game handler
        /// for having access to the game map and its elements,
        /// an x-coordinate and a y-coordinate, which are both
        /// absolute in pixels, rather than in map tiles.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="x">absolute x-coordinate in pixels</param>
        /// <param name="y">absolute y-coordinate in pixels</param>
        public EnemyRed(Handler handler, float x, float y) : base(handler, x, y) {

        }

        /// <summary>
        /// Red enemy's implementation of chase mode movement.
        /// <para>In the chase mode, the red enemy's target tile is
        /// calculated from positions of PacMan and the pink enemy.
        /// It computes a vector from PacMan to the pink enemy,
        /// subtracts it from PacMan's position and that is its
        /// target tile.</para>
        /// <para><b>Example:</b>
        /// PacMan's position is (3, 3), the pink enemy's position
        /// is (5, 7). The vector from PacMan to the pink enemy is
        /// (2, 4), the target tile is (3, 3) - (2, 4) = (1, -1).</para>
        /// </summary>
        protected override void moveChase() {
            int pacManX = (int) handler.getMap().getEntityManager().getPacMan().getX() / Tile.SIZE;
            int pacManY = (int) handler.getMap().getEntityManager().getPacMan().getY() / Tile.SIZE;

            int pinkieX = (int) handler.getMap().getEntityManager().getPinkie().getX() / Tile.SIZE;
            int pinkieY = (int) handler.getMap().getEntityManager().getPinkie().getY() / Tile.SIZE;

            targetX = pacManX - (pinkieX - pacManX);
            targetY = pacManY - (pinkieY - pacManY);

            base.moveChase();
        }

        /// <summary>
        /// Red enemy's implementation of scatter mode movement.
        /// <para>In the scatter mode, the red enemy's target tile is
        /// located outside the maze in the upper right corner,
        /// resulting in it circulating around obstacles as near
        /// that corner as possible.</para>
        /// </summary>
        protected override void moveScatter() {
            targetX = handler.getMap().getWidth();
            targetY = -1;

            base.moveScatter();
        }

        /// <summary>
        /// Draw red enemy's image if it wasn't rendered by
        /// super method.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public override void render(SpriteBatch sb) {
            base.render(sb);

            if (!rendered) {
                sb.Draw(Assets.enemyRed,
                    new Rectangle((int) x, (int) y, width, height),
                    Color.White);
            }
        }
    }

    /// <summary>
    /// <c>EnemyYellow</c> class controls movements and rendering
    /// of the yellow enemy.
    /// </summary>
    public class EnemyYellow : Enemy {
        /// <summary>
        /// Constructor of <c>EnemyYellow</c> class only calls constructor
        /// of the abstract <c>Enemy</c> class. It takes the game handler
        /// for having access to the game map and its elements,
        /// an x-coordinate and a y-coordinate, which are both
        /// absolute in pixels, rather than in map tiles.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="x">absolute x-coordinate in pixels</param>
        /// <param name="y">absolute y-coordinate in pixels</param>
        public EnemyYellow(Handler handler, float x, float y) : base(handler, x, y) {

        }

        /// <summary>
        /// Yellow enemy's implementation of chase mode movement. 
        /// <para>In the chase mode, the yellow enemy's target tile is
        /// completely random on every crossroads, resulting in
        /// its chaotic movement in the maze.</para>
        /// </summary>
        protected override void moveChase() {
            targetX = random.Next(20);
            targetY = random.Next(20);

            base.moveChase();
        }

        /// <summary>
        /// Yellow enemy's implementation of scatter mode movement.
        /// <para>In the scatter mode, the yellow enemy's target tile is
        /// located outside the maze in the lower left corner,
        /// resulting in it circulating around obstacles as near
        /// that corner as possible.</para>
        /// </summary>
        protected override void moveScatter() {
            targetX = -1;
            targetY = handler.getMap().getHeight();

            base.moveScatter();
        }

        /// <summary>
        /// Draw yellow enemy's image if it wasn't rendered by
        /// super method.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public override void render(SpriteBatch sb) {
            base.render(sb);

            if (!rendered) {
                sb.Draw(Assets.enemyYellow,
                    new Rectangle((int) x, (int) y, width, height),
                    Color.White);
            }
        }
    }

    /// <summary>
    /// <c>Food</c> is an abstract class for all different types
    /// of food in the game.
    /// </summary>
    public abstract class Food : Entity {
        private readonly int SCORE;

        /// <summary>
        /// Constructor of abstract food class always called
        /// by all food present in game.It takes the game handler
        /// for having access to the game map and its elements,
        /// an x-coordinate, a y-coordinate, which are both
        /// absolute in pixels, rather than in map tiles, and
        /// a score value of the food.
        /// It also sets local variables, and the bounding
        /// rectangle.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="x">absolute x-coordinate in pixels</param>
        /// <param name="y">absolute y-coordinate in pixels</param>
        /// <param name="score">score value of the food</param>
        public Food(Handler handler, float x, float y, int score) : base(handler, x, y) {
            SCORE = score;
            isCollidable = false;
            bounds.X = 19;
            bounds.Y = 19;
            bounds.Width = 2;
            bounds.Height = 2;
        }

        public override void tick() {

        }

        /// <summary>
        /// Sets active status of the food.
        /// If the food becomes inactive (it is
        /// eaten by PacMan), its score is
        /// added to player's score.
        /// </summary>
        /// <param name="active">new active status</param>
        public override void setActive(bool active) {
            base.setActive(active);

            if (!active) {
                handler.getGame().setScore(handler.getGame().getScore() + SCORE);
            }
        }
    }

    /// <summary>
    /// <c>FoodBanana</c> class controls rendering of the
    /// banana food.
    /// </summary>
    public class FoodBanana : Food {
        /// <summary>
        /// Constructor of <c>FoodBanana</c> class only calls
        /// constructor of the abstract <c>Food</c> class with score
        /// set to 200. It takes the game handler for having
        /// access to the game map and its elements,
        /// an x-coordinate and a y-coordinate, which are both
        /// absolute in pixels, rather than in map tiles.
        /// It also sets local variables, and the bounding
        /// rectangle.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="x">absolute x-coordinate in pixels</param>
        /// <param name="y">absolute y-coordinate in pixels</param>
        public FoodBanana(Handler handler, float x, float y) : base(handler, x, y, 200) {

        }

        /// <summary>
        /// Draws banana food's image.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public override void render(SpriteBatch sb) {
            sb.Draw(Assets.foodBanana,
                new Rectangle((int) x, (int) y, width, height),
                Color.White);
        }
    }

    /// <summary>
    /// <c>FoodCherry</c> class controls rendering of the
    /// banana food.
    /// </summary>
    public class FoodCherry : Food {
        /// <summary>
        /// Constructor of <c>FoodCherry</c> class only calls
        /// constructor of the abstract <c>Food</c> class with score
        /// set to 100. It takes the game handler for having
        /// access to the game map and its elements,
        /// an x-coordinate and a y-coordinate, which are both
        /// absolute in pixels, rather than in map tiles.
        /// It also sets local variables, and the bounding
        /// rectangle.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="x">absolute x-coordinate in pixels</param>
        /// <param name="y">absolute y-coordinate in pixels</param>
        public FoodCherry(Handler handler, float x, float y) : base(handler, x, y, 100) {

        }

        /// <summary>
        /// Draws cherry food's image.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public override void render(SpriteBatch sb) {
            sb.Draw(Assets.foodCherry,
                new Rectangle((int) x, (int) y, width, height),
                Color.White);
        }
    }

    /// <summary>
    /// <c>FoodStrawberry</c> class controls rendering of the
    /// banana food.
    /// </summary>
    public class FoodStrawberry : Food {
        /// <summary>
        /// Constructor of <c>FoodStrawberry</c> class only calls
        /// constructor of the abstract <c>Food</c> class with score
        /// set to 150. It takes the game handler for having
        /// access to the game map and its elements,
        /// an x-coordinate and a y-coordinate, which are both
        /// absolute in pixels, rather than in map tiles.
        /// It also sets local variables, and the bounding
        /// rectangle.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="x">absolute x-coordinate in pixels</param>
        /// <param name="y">absolute y-coordinate in pixels</param>
        public FoodStrawberry(Handler handler, float x, float y) : base(handler, x, y, 150) {

        }

        /// <summary>
        /// Draws strawberry food's image.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public override void render(SpriteBatch sb) {
            sb.Draw(Assets.foodStrawberry,
                new Rectangle((int) x, (int) y, width, height),
                Color.White);
        }
    }

    /// <summary>
    /// <c>FoodKiller</c> class controls rendering of the
    /// killer food.
    /// </summary>
    public class FoodKiller : Food {
        /// <summary>
        /// Constructor of <c>FoodKiller</c> class only calls
        /// constructor of the abstract <c>Food</c> class with score
        /// set to 0. It takes the game handler for having
        /// access to the game map and its elements,
        /// an x-coordinate and a y-coordinate, which are both
        /// absolute in pixels, rather than in map tiles.
        /// It also sets local variables, and the bounding
        /// rectangle.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="x">absolute x-coordinate in pixels</param>
        /// <param name="y">absolute y-coordinate in pixels</param>
        public FoodKiller(Handler handler, float x, float y) : base(handler, x, y, 0) {

        }

        /// <summary>
        /// Sets active status of the food.
        /// If the killer food becomes inactive
        /// (it is eaten by PacMan), the enemies
        /// turn blue and can be temporarily eaten
        /// by PacMan.
        /// </summary>
        /// <param name="active">new active status</param>
        public override void setActive(bool active) {
            base.setActive(active);

            if (!active) {
                handler.getMap().setBlueGhosts(true);
            }
        }

        /// <summary>
        /// Draws killer food's image.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public override void render(SpriteBatch sb) {
            sb.Draw(Assets.foodKiller,
                new Rectangle((int) x, (int) y, width, height),
                Color.White);
        }
    }

    /// <summary>
    /// <c>FoodNormal</c> class controls rendering of the
    /// normal food.
    /// </summary>
    public class FoodNormal : Food {
        /// <summary>
        /// Constructor of <c>FoodNormal</c> class only calls
        /// constructor of the abstract <c>Food</c> class with score
        /// set to 1. It takes the game handler for having
        /// access to the game map and its elements,
        /// an x-coordinate and a y-coordinate, which are both
        /// absolute in pixels, rather than in map tiles.
        /// It also sets local variables, and the bounding
        /// rectangle.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        /// <param name="x">absolute x-coordinate in pixels</param>
        /// <param name="y">absolute y-coordinate in pixels</param>
        public FoodNormal(Handler handler, float x, float y) : base(handler, x, y, 1) {

        }

        /// <summary>
        /// Sets active status of the food.
        /// If the food becomes inactive (it is
        /// eaten by PacMan), its score is
        /// added to player's score. If this was
        /// the last remaining normal food in
        /// the maze, level up.
        /// </summary>
        /// <param name="active">new active status</param>
        public override void setActive(bool active) {
            base.setActive(active);

            handler.getMap().setFoodCount(handler.getMap().getFoodCount() - 1);

            if (handler.getMap().getFoodCount() == 0) {
                handler.getGame().getGameState().levelUp();
            }
        }

        /// <summary>
        /// Draws normal food's image.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public override void render(SpriteBatch sb) {
            sb.Draw(Assets.foodNormal,
                new Rectangle((int) x, (int) y, width, height),
                Color.White);
        }
    }
}
