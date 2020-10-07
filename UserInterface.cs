using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pacman {
    public delegate void ClickListener();

    /// <summary>
    /// The <c>UserInterfaceManager</c> class is responsible
    /// for updating and rendering all UI objects present in the game.
    /// </summary>
    public class UserInterfaceManager {
        private Handler handler;
        private List<UserInterfaceObject> objects;

        /// <summary>
        /// Constructor of the <c>UserInterfaceManager</c> class.
        /// It takes the game handler, for having access to all elements
        /// of the game, as a parameter. It also initializes the
        /// <c>objects</c> array.
        /// </summary>
        /// <param name="handler">game handler for accessing other elements
        ///                       of the game</param>
        public UserInterfaceManager(Handler handler) {
            this.handler = handler;
            objects = new List<UserInterfaceObject>();
        }

        /// <summary>
        /// Updates all objects in the <c>objects</c> array.
        /// </summary>
        public void tick() {
            foreach (UserInterfaceObject o in objects) {
                o.tick();
            }
        }

        /// <summary>
        /// Draws all objects from the <c>objects</c> array.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public void render(SpriteBatch sb) {
            foreach (UserInterfaceObject o in objects) {
                o.render(sb);
            }
        }

        /// <summary>
        /// Performs actions on mouse move on all objects
        /// from the <c>objects</c> array.
        /// </summary>
        /// <param name="state">current mouse state</param>
        public void onMouseMove(MouseState state) {
            foreach (UserInterfaceObject o in objects) {
                o.onMouseMove(state);
            }
        }

        /// <summary>
        /// Performs actions on mouse release on all objects
        /// from the <c>objects</c> array.
        /// </summary>
        /// <param name="state">current mouse state</param>
        public void onMouseRelease(MouseState state) {
            foreach (UserInterfaceObject o in objects) {
                o.onMouseRelease(state);
            }
        }

        /// <summary>
        /// Adds UI object to the <c>objects</c> array.
        /// </summary>
        /// <param name="o">the object to be added</param>
        public void addObject(UserInterfaceObject o) {
            objects.Add(o);
        }

        /// <summary>
        /// Gets the game handler.
        /// </summary>
        /// <returns>the game handler</returns>
        public Handler getHandler() {
            return handler;
        }

        /// <summary>
        /// Sets the game handler.
        /// </summary>
        /// <param name="handler">new game handler</param>
        public void setHandler(Handler handler) {
            this.handler = handler;
        }
    }

    /// <summary>
    /// <c>UserInterfaceObject</c> is an abstract class for
    /// all different types of user interface elements in the game.
    /// </summary>
    public abstract class UserInterfaceObject {
        protected float x, y;
        protected int width, height;
        protected bool hover = false;
        private Rectangle bounds;

        /// <summary>
        /// Constructor of the abstract UI object class always
        /// called by all UI objects present in the game.
        /// It takes x and y-coordinates of upper left corner
        /// of the object and its width and height as parameters.
        /// It also initializes the bounding rectangle.
        /// </summary>
        /// <param name="x">x-coordinate of the upper left corner</param>
        /// <param name="y">y-coordinate of the upper left corner</param>
        /// <param name="width">object width</param>
        /// <param name="height">object height</param>
        public UserInterfaceObject(float x, float y, int width, int height) {
            this.x = x;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            bounds = new Rectangle((int) x, (int) y, width, height);
        }

        public abstract void tick();

        public abstract void render(SpriteBatch sb);

        public abstract void onClick();

        /// <summary>
        /// Sets hover variable to <c>true</c> if mouse
        /// position is within the bounding rectangle.
        /// </summary>
        /// <param name="state">current mouse state</param>
        internal void onMouseMove(MouseState state) {
            hover = bounds.Contains(state.X, state.Y);
        }

        /// <summary>
        /// Calls the <c>onClick</c> method is mouse is released
        /// when within the bounding rectangle.
        /// </summary>
        /// <param name="state">current mouse state</param>
        internal void onMouseRelease(MouseState state) {
            if (hover) {
                onClick();
            }
        }

        #region Getters & Setters

        /// <summary>
        /// Gets x-coordinate.
        /// </summary>
        /// <returns>x-coordinate</returns>
        public float getX() {
            return x;
        }

        /// <summary>
        /// Sets x-coordinate.
        /// </summary>
        /// <param name="x">new x-coordinate</param>
        public void setX(float x) {
            this.x = x;
        }

        /// <summary>
        /// Gets y-coordinate.
        /// </summary>
        /// <returns>y-coordinate</returns>
        public float getY() {
            return y;
        }

        /// <summary>
        /// Sets y-coordinate.
        /// </summary>
        /// <param name="y">new y-coordinate</param>
        public void setY(float y) {
            this.y = y;
        }

        /// <summary>
        /// Gets object width.
        /// </summary>
        /// <returns>object width</returns>
        public int getWidth() {
            return width;
        }

        /// <summary>
        /// Sets object width.
        /// </summary>
        /// <param name="width">new object width</param>
        public void setWidth(int width) {
            this.width = width;
        }

        /// <summary>
        /// Gets object height.
        /// </summary>
        /// <returns>object height</returns>
        public int getHeight() {
            return height;
        }

        /// <summary>
        /// Sets object height.
        /// </summary>
        /// <param name="height">new object height</param>
        public void setHeight(int height) {
            this.height = height;
        }

        #endregion
    }

    /// <summary>
    /// The <c>UserInterfaceButton</c> class is responsible
    /// for actions and rendering of all buttons in the game.
    /// </summary>
    public class UserInterfaceButton : UserInterfaceObject {
        private Texture2D[] images;
        private ClickListener clicker;

        /// <summary>
        /// Constructor of the <c>UserInterfaceButton</c> class.
        /// It takes two more parameters than the base constructor -
        /// an array of images containing images for non-hover and hover
        /// appearance of the button, and an implementation of
        /// <c>ClickListener</c> delegate.
        /// </summary>
        /// <param name="x">x-coordinate of the upper left corner</param>
        /// <param name="y">y-coordinate of the upper left corner</param>
        /// <param name="width">button width</param>
        /// <param name="height">button height</param>
        /// <param name="images">an array of images containing non-hover and hover
        ///                      appearance of the button</param>
        /// <param name="clicker">an implementation of the <c>ClickListener</c>
        ///                       delegate</param>
        public UserInterfaceButton(float x, float y, int width, int height, Texture2D[] images, ClickListener clicker) :
            base(x, y, width, height) {
            this.images = images;
            this.clicker = clicker;
        }

        public override void tick() {

        }

        /// <summary>
        /// Draws button image depending on the hover status.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public override void render(SpriteBatch sb) {
            if (hover) {
                sb.Draw(images[1], new Vector2((int) x, (int) y), Color.White);
            } else {
                sb.Draw(images[0], new Vector2((int) x, (int) y), Color.White);
            }
        }

        /// <summary>
        /// Performs an action when the button is clicked.
        /// </summary>
        public override void onClick() {
            clicker();
            hover = false;
        }
    }

    /// <summary>
    /// The <c>UserInterfaceCenteredString</c> class is responsible
    /// for actions and rendering of all labels in the game, whose texts are
    /// aligned to center.
    /// </summary>
    public class UserInterfaceCenteredString : UserInterfaceObject {
        private readonly SpriteFont FONT = Assets.centeredStringFont;

        private new int width;

        private string text;

        /// <summary>
        /// Constructor of the <c>UserInterfaceCenteredString</c> class.
        /// It takes the y-coordinate of the upper left corner, the width and
        /// the label text as parameters.
        /// </summary>
        /// <param name="y">y-coordinate of the upper left corner</param>
        /// <param name="width">label width</param>
        /// <param name="text">a string containing the text of the label</param>
        public UserInterfaceCenteredString(float y, int width, string text) : base(0, y, 0, 0) {
            this.width = width;
            this.text = text;
        }

        public override void tick() {

        }

        /// <summary>
        /// Calculates pixel size of the string and draws in on the screen.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the text</param>
        public override void render(SpriteBatch sb) {
            Vector2 fontMetrics = FONT.MeasureString(text);

            float actualX = x + (width - fontMetrics.Length()) / 2f;
            sb.DrawString(FONT, text, new Vector2((int) actualX, (int) y), Color.White);
        }

        public override void onClick() {

        }

        /// <summary>
        /// Sets the label text.
        /// </summary>
        /// <param name="text">new label text</param>
        public void setText(string text) {
            this.text = text;
        }
    }

    /// <summary>
    /// The <c>UserInterfaceImage</c> class is responsible
    /// for actions and rendering of all non-play images in the game.
    /// </summary>
    public class UserInterfaceImage : UserInterfaceObject {
        private Texture2D image;

        /// <summary>
        /// Constructor of the <c>UserInterfaceImage</c> class.
        /// It takes one more parameter than the super constructor -
        /// an image to be drawn on the screen.
        /// </summary>
        /// <param name="x">x-coordinate of the upper left corner</param>
        /// <param name="y">y-coordinate of the upper left corner</param>
        /// <param name="width">button width</param>
        /// <param name="height">button height</param>
        /// <param name="image">an image</param>
        public UserInterfaceImage(float x, float y, int width, int height, Texture2D image) :
            base(x, y, width, height) {
            this.image = image;
        }

        public override void tick() {

        }

        /// <summary>
        /// Draws the image.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the images</param>
        public override void render(SpriteBatch sb) {
            sb.Draw(image,
                new Rectangle((int) x, (int) y, width, height),
                Color.White);
        }

        public override void onClick() {

        }
    }

    /// <summary>
    /// The <c>UserInterfaceLabel</c> class is responsible
    /// for actions and rendering of all labels in the game.
    /// </summary>
    public class UserInterfaceLabel : UserInterfaceObject {
        private readonly SpriteFont FONT = Assets.normalFont;

        private string fixedText, changeable;

        /// <summary>
        /// Constructor of the <c>UserInterfaceLabel</c> class.
        /// It takes x and y-coordiantes of the upper left corner,
        /// a fixed text and a changeable text as parameters.
        /// </summary>
        /// <param name="x">x-coordinate of the upper left corner</param>
        /// <param name="y">y-coordinate of the upper left corner</param>
        /// <param name="fixedText">a string with the fixed text</param>
        /// <param name="changeable">a string with the changeable text</param>
        public UserInterfaceLabel(float x, float y, string fixedText, string changeable) :
            base(x, y, 0, 0) {
            this.fixedText = fixedText;
            this.changeable = changeable;
        }

        public override void onClick() {

        }

        /// <summary>
        /// Draws the label on the screen.
        /// </summary>
        /// <param name="sb">SpriteBatch instance to draw the text</param>
        public override void render(SpriteBatch sb) {
            sb.DrawString(FONT,
                fixedText + " " + changeable,
                new Vector2((int) x, (int) y),
                Color.White);
        }

        public override void tick() {

        }

        /// <summary>
        /// Sets the changeable text.
        /// </summary>
        /// <param name="changeable">new changeable text</param>
        public void setChangeable(string changeable) {
            this.changeable = changeable;
        }
    }
}
