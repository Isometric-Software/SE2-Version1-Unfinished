using Microsoft.Xna.Framework.Input;
using SE2.Utilities;

namespace SE2.Data
{
    public class GameConfig
    {
        public DisplayConfig DisplayConfig { get; set; }
        public KeysConfig KeysConfig { get; set; }
        public MouseConfig MouseConfig { get; set; }

        internal static GameConfig DefaultConfig =>
            new GameConfig
            {
                DisplayConfig = new DisplayConfig
                {
                    ScreenResolution = new Size(1280, 720),
                    Fullscreen = false
                },
                KeysConfig = new KeysConfig
                {
                    Forward = Keys.W,
                    Backward = Keys.S,
                    Left = Keys.A,
                    Right = Keys.D,
                    Jump = Keys.Space,
                    Crouch = Keys.C
                },
                MouseConfig = new MouseConfig
                {
                    MouseSensitivity = 5,
                    PlaceButton = "LeftButton",
                    RemoveButton = "RightButton"
                }
            };
        
        internal static GameConfig LoadedConfig { get; set; }
        internal static DisplayConfig LoadedDisplayConfig => LoadedConfig.DisplayConfig;
        internal static KeysConfig LoadedKeysConfig => LoadedConfig.KeysConfig;
        internal static MouseConfig LoadedMouseConfig => LoadedConfig.MouseConfig;
    }

    public class KeysConfig
    {
        public Keys Forward { get; set; }
        public Keys Backward { get; set; }
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Jump { get; set; }
        public Keys Crouch { get; set; }
    }

    public class MouseConfig
    {
        public int MouseSensitivity { get; set; }
        public string PlaceButton { get; set; }
        public string RemoveButton { get; set; }
    }

    public class DisplayConfig
    {
        public Size ScreenResolution { get; set; }
        public bool Fullscreen { get; set; }
    }
}