namespace GameOfLife
{
    using Microsoft.Xna.Framework.Input;
    
    public class InputState
    {
        public MouseState Mouse { get; private set; }
        public MouseState PreviusMouse { get; private set; }

        public KeyboardState Keyboard { get; private set; }
        public KeyboardState PreviusKeyboard { get; private set; }

        public bool ScrollWheelChanged
        {
            get { return this.Mouse.ScrollWheelValue != this.PreviusMouse.ScrollWheelValue; }
        }

        public int ScrollWheelDelta 
        {
            get { return this.Mouse.ScrollWheelValue - this.PreviusMouse.ScrollWheelValue; }
        }

        public void Update()
        {
            this.PreviusMouse = Mouse;
            this.PreviusKeyboard = Keyboard;
            this.Mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
            this.Keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        }

        public bool IsNewMouseLeftDown()
        {
            return (Mouse.LeftButton == ButtonState.Pressed) && (PreviusMouse.LeftButton != ButtonState.Pressed);
        }

        public bool IsKeyDown(Keys key)
        {
            return this.Keyboard.IsKeyDown(key);
        }

        public bool IsNewKeyDown(Keys key)
        {
            return this.Keyboard.IsKeyDown(key) && !this.PreviusKeyboard.IsKeyDown(key);
        }
    }
}
