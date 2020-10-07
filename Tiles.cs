using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pacman {
    /// <summary>
    /// The Tile class holds data for a single map tile.
    /// </summary>
    public class Tile {
        public const int SIZE = 40;

        public static Tile[] tiles = new Tile[32];
        public static Tile wall = new Wall(0);
        public static Tile empty = new Empty(31);

        private readonly int id;
        private Texture2D texture;

        /// <summary>
        /// Constructor of the Tile class. It takes the image
        /// that the tile is rendered with and a unique Tile type
        /// id.
        /// </summary>
        /// <param name="texture">an image of the tile</param>
        /// <param name="id">unique id depending on tile type</param>
        public Tile(Texture2D texture, int id) {
            this.texture = texture;
            this.id = id;

            tiles[id] = this;
        }

        /// <summary>
        /// Draws this tile's image on given coordinates.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        /// <param name="x">x-coordinate to draw the image</param>
        /// <param name="y">y-coordinate to draw the image</param>
        public void render(SpriteBatch sb, int x, int y) {
            sb.Draw(texture, new Rectangle(x, y, SIZE, SIZE), Color.White);
        }

        /// <summary>
        /// Gets obstacle status of this tile.
        /// </summary>
        /// <returns>obstacle status of this tile</returns>
        public virtual bool isObstacle() {
            return false;
        }

        /// <summary>
        /// Gets id of this tile.
        /// </summary>
        /// <returns>id of this tile</returns>
        public int getId() {
            return id;
        }
    }

    /// <summary>
    /// The Empty tile class holds data for empty tiles.
    /// </summary>
    public class Empty : Tile {
        /// <summary>
        /// Constructor of the Empty tile class. It takes
        /// the empty tile's unique id as parameter.
        /// </summary>
        /// <param name="id">empty tile id</param>
        public Empty(int id) : base(Assets.empty, id) {

        }
    }

    /// <summary>
    /// The Wall tile class holds data for wall tiles.
    /// </summary>
    public class Wall : Tile {
        /// <summary>
        /// Constructor of the Wall tile class. It takes
        /// the wall tile's unique id as parameter.
        /// </summary>
        /// <param name="id">wall tile id</param>
        public Wall(int id) : base(Assets.wall, id) {

        }

        /// <inheritdoc/>
        public override bool isObstacle() {
            return true;
        }
    }

}
