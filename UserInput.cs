using Microsoft.Xna.Framework.Input;

namespace pacman {
    /// <summary>
    /// The <c>KeyManager</c> class is responsible
    /// for getting user keyboard input.
    /// </summary>
    public class KeyManager {
        public bool up, down, left, right;
        private bool[] keys;

        /// <summary>
        /// Constructor of the <c>KeyManager</c> class.
        /// </summary>
        public KeyManager() {
            keys = new bool[4];
        }

        /// <summary>
        /// Updates pressed arrow keys.
        /// </summary>
        public void tick() {
            KeyboardState state = Keyboard.GetState();

            keyPressed(state);

            up = keys[0];
            down = keys[1];
            left = keys[2];
            right = keys[3];
        }

        /// <summary>
        /// Updates pressed keys array.
        /// </summary>
        /// <param name="state">a KeyboardState representing the current
        ///                     pressed keys</param>
        private void keyPressed(KeyboardState state) {
            if (state.IsKeyDown(Keys.Up) & !up) {
                keys[0] = true;
                keys[1] = false;
                keys[2] = false;
                keys[3] = false;
            } else if (state.IsKeyDown(Keys.Down) & !down) {
                keys[0] = false;
                keys[1] = true;
                keys[2] = false;
                keys[3] = false;
            } else if (state.IsKeyDown(Keys.Left) & !left) {
                keys[0] = false;
                keys[1] = false;
                keys[2] = true;
                keys[3] = false;
            } else if (state.IsKeyDown(Keys.Right) & !right) {
                keys[0] = false;
                keys[1] = false;
                keys[2] = false;
                keys[3] = true;
            }
        }

        /// <summary>
        /// Resets all pressed keys, so that none is seen as pressed.
        /// </summary>
        public void resetKeys() {
            keys[0] = false;
            keys[1] = false;
            keys[2] = false;
            keys[3] = false;
        }
    }

    public class MouseManager {
        private bool leftPressed, rightPressed;
        private int mouseX, mouseY;
        private UserInterfaceManager manager;

        /// <summary>
        /// Updates mouse variables.
        /// </summary>
        public void tick() {
            MouseState state = Mouse.GetState();

            mousePressed(state);
            mouseReleased(state);
            mouseMoved(state);
        }

        /// <summary>
        /// Stores the pressed button to local variable.
        /// </summary>
        /// <param name="state">current mouse state</param>
        public void mousePressed(MouseState state) {
            if (state.LeftButton == ButtonState.Pressed) {
                leftPressed = true;
            } else if (state.RightButton == ButtonState.Pressed) {
                rightPressed = true;
            }
        }

        /// <summary>
        /// Stores the unpressed button to local variable.
        /// </summary>
        /// <param name="state">current mouse state</param>
        public void mouseReleased(MouseState state) {
            if ((state.LeftButton == ButtonState.Released) &&
                (leftPressed == true)) {
                leftPressed = false;

                if (manager != null) {
                    manager.onMouseRelease(state);
                }
            } else if ((state.RightButton == ButtonState.Released) &&
                (rightPressed = true)) {
                rightPressed = false;
            }
        }

        /// <summary>
        /// Stores mouse current coordinates to local variables.
        /// </summary>
        /// <param name="state">current mouse state</param>
        public void mouseMoved(MouseState state) {
            mouseX = state.X;
            mouseY = state.Y;

            if (manager != null) {
                manager.onMouseMove(state);
            }
        }

        /// <summary>
        /// Sets the user interface manager.
        /// </summary>
        /// <param name="manager">new user interface manager</param>
        public void setManager(UserInterfaceManager manager) {
            leftPressed = false;
            rightPressed = false;
            this.manager = manager;
        }
    }
}
