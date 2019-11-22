using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RootinTootinShootin
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class RootinTootinShootin : GameEnvironment
    {
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screen = new Point(1280, 720);
            ApplyResolutionSettings();

            // TODO: use this.Content to load your game content here
            GameStateManager.AddGameState("StartingState", new StartingState());
            GameStateManager.AddGameState("PlayingState", new PlayingState());
            GameStateManager.AddGameState("PausedState", new PausedState());
            GameStateManager.AddGameState("GameOverState", new GameOverState());
            GameStateManager.AddGameState("VictoryState", new VictoryState());

            GameStateManager.SwitchTo("StartingState"); //SWITCH TO STARTINGSTATE ONCE GAME WORKS
        }
    }
}