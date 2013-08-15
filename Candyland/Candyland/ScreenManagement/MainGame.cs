﻿using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Candyland
{
    class MainGame : GameScreen
    {
        // the scene manager, most stuff happens in there
        SceneManager m_sceneManager;

        public override void Open(Game game)
        {
            this.isFullscreen = true;

            m_sceneManager = new SceneManager(ScreenManager);

            Song song = ScreenManager.Content.Load<Song>("Music/bgmusic");  // background music from http://longzijun.wordpress.com/2012/12/26/upbeat-background-music-free-instrumentals/
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;

            // Load all content required by the scene
            m_sceneManager.Load(ScreenManager.Content);
        }

        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.Input.Equals(InputState.Enter))
            {
                ScreenManager.ActivateNewScreen(new MainMenu());
            }
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                ScreenManager.ActivateNewScreen(new SalesmanDialogueScreen(GameConstants.tradesmanGreeting, "testBonus"));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                ScreenManager.ActivateNewScreen(new DialogListeningScreen("Hallo hallo", "testBonus"));
            }


            if (ScreenManager.Game.IsActive && (gameTime.TotalGameTime.Milliseconds % GameConstants.framerate == 0)
                && ((ScreenManager.Input.Equals(InputState.Enter)) || GameConstants.singlestepperOFF))
            {
                m_sceneManager.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.Orange);

            m_sceneManager.Draw(gameTime);
            m_sceneManager.Draw2D();
        }

        public override void Close()
        {
            //TODO Unload content;
        }
    }
}
