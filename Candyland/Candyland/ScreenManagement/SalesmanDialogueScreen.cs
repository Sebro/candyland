﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Candyland
{
    class SalesmanDialogueScreen : DialogListeningScreen
    {
        Texture2D OwnTalkBubble;

        Rectangle ownDiagBox;

        private string option1, option2, option3, option4;

        private string Text = "";
        private string Picture = "testBonus";

        int activeIndex = 0;
        int numberOfOptions = 4;

        bool isTimeToAnswer = false;

        public SalesmanDialogueScreen(string text, string picture)
        {
            this.Text = text;
            this.Picture = picture;
        }

        public override void Open(Game game)
        {
            base.Open(game);

            OwnTalkBubble = ScreenManager.Content.Load<Texture2D>("ScreenTextures/talkBubbleOwn");

            ownDiagBox = new Rectangle(screenWidth / 3, 0, screenWidth * 2 / 3, screenHeight * 2 / 3);

            option1 = "Reden";
            option2 = "Einkaufen";
            option3 = "Reisen";
            option4 = "Auf Wiedersehen!";

            Text = wrapText(Text, font, TextBox);
            // Check if text needs scrolling
            lineCapacity = (TextBox.Height - offset) / font.LineSpacing;
        }

        public override void Update(GameTime gameTime)
        {
            bool enterPressed = false;

            // look at input and update button selection
            switch (ScreenManager.Input)
            {
                case InputState.Enter: enterPressed = true; break;
                case InputState.Up: activeIndex--; break;
                case InputState.Down: activeIndex++; break;
            }
            if (activeIndex >= numberOfOptions) activeIndex = 0;
            if (activeIndex < 0) activeIndex = numberOfOptions - 1;

            // Selected Button confirmed by pressing Enter
            if (enterPressed && isTimeToAnswer)
            {
                switch (activeIndex)
                {
                    case 0:  break;
                    case 1: this.ScreenState = ScreenState.Hidden;
                        ScreenManager.ActivateNewScreen(new ShopScreen()); break;
                    case 2: ScreenManager.ActivateNewScreen(new TravelScreen()); break;
                    case 3: ScreenManager.ResumeLast(this); break;
                }
                return;
            }

            // Check if text needs scrolling
            if (numberOfLines > lineCapacity)
            {
                canScroll = true;

                // If other person is still talking continue through text by pressing Enter
                if (enterPressed)
                {
                    Text = removeReadLines(Text, lineCapacity);
                }

                timePastSinceLastArrowBling += gameTime.ElapsedGameTime.Milliseconds;

                if (timePastSinceLastArrowBling > 400)
                {
                    if (timePastSinceLastArrowBling > 800)
                    {
                        arrowBlink = false;
                        timePastSinceLastArrowBling = 0;
                    }
                    else
                        arrowBlink = true;
                }
            }
            else
            {
                canScroll = false;
                isTimeToAnswer = true;
            }

        }

        public override void Draw(GameTime gameTime)
        {
            int leftAlign = ownDiagBox.X + ownDiagBox.Width / 5;
            int topAlign = ownDiagBox.Y + ownDiagBox.Height / 5; 

            SpriteBatch m_sprite = ScreenManager.SpriteBatch;

            m_sprite.Begin();

            m_sprite.Draw(TalkBubble, DiagBox, Color.White);
            m_sprite.Draw(talkingNPC, pictureNPC, Color.White);

            m_sprite.DrawString(font, Text, new Vector2(TextBox.X, TextBox.Y), Color.Black);

            if (isTimeToAnswer)
            {
                m_sprite.Draw(OwnTalkBubble, ownDiagBox, Color.White);

                m_sprite.DrawString(font, option1, new Vector2(leftAlign, topAlign), Color.Black);
                m_sprite.DrawString(font, option2, new Vector2(leftAlign, topAlign + lineDist ), Color.Black);
                m_sprite.DrawString(font, option3, new Vector2(leftAlign, topAlign + lineDist * 2), Color.Black);
                m_sprite.DrawString(font, option4, new Vector2(leftAlign, topAlign + lineDist * 3), Color.Black);

                // Draw active option in different color
                switch (activeIndex)
                {
                    case 0: m_sprite.DrawString(font, option1, new Vector2(leftAlign, topAlign), Color.Green); break;
                    case 1: m_sprite.DrawString(font, option2, new Vector2(leftAlign, topAlign + lineDist), Color.Green); break;
                    case 2: m_sprite.DrawString(font, option3, new Vector2(leftAlign, topAlign + lineDist * 2), Color.Green); break;
                    case 3: m_sprite.DrawString(font, option4, new Vector2(leftAlign, topAlign + lineDist * 3), Color.Green); break;
                }
            }
            //other person is still talking
            else
            {
                if (canScroll)
                {
                    if (arrowBlink)
                    {
                        m_sprite.Draw(arrowDown, new Rectangle(DiagBox.Right - 35, DiagBox.Bottom - 30, 30, 15), Color.White);
                    }
                    else m_sprite.Draw(arrowDown, new Rectangle(DiagBox.Right - 35, DiagBox.Bottom - 25, 30, 15), Color.White);
                }
            }
            m_sprite.End();

            //// we need the following as spriteBatch.Begin() sets them to None and AlphaBlend
            //// which breaks our model rendering
            GraphicsDevice m_graphics = ScreenManager.Game.GraphicsDevice;
            m_graphics.DepthStencilState = DepthStencilState.Default;
            m_graphics.BlendState = BlendState.Opaque;
        }
    }
}
