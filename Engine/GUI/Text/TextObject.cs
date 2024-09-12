using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    class TextObject : IUpdatable
    {
        public string Text { get; private set; }

        public List<TextChar> Chars { get; private set; }
        public float PixelsCharWidth { get; private set; }
        public float PixelsCharHeight { get; private set; }
        public float CharWidth { get; private set; }
        public float CharHeight { get; private set; }

        public Vector2 Position { get { return Chars.Count > 0 ? Chars[0].Position : Vector2.Zero; } }
        public Font Font { get; private set; }
        public bool IsActive { get { return Chars.Count > 0 ? Chars[0].IsActive : false; } set { SetAllCharsActive(value); } }

        private bool isClickable;
        public static TextObject TextClicked;

        public TextObject(string text, Vector2 pos, float charWidth = 0, float charHeight = 0, Font font = null, bool isClickable = false)
        {
            if (font == null)
            {
                font = FontMngr.GetFont("comics");
            }

            AddText(text, pos, font, charWidth, charHeight);

            this.isClickable = isClickable;
            UpdateMngr.Add(this);
        }

        private void AddText(string text, Vector2 pos, Font font, float charWidth, float charHeight)
        {
            Text = text;
            Vector2 startPos = pos;
            Font = font;

            Chars = new List<TextChar>();
            charWidth = charWidth <= 0 ? Font.CharWidth : charWidth;
            charHeight = charHeight <= 0 ? Font.CharHeight : charHeight;
            PixelsCharWidth = charWidth;
            PixelsCharHeight = charHeight;
            CharWidth = Game.PixelsToUnits(charWidth);
            CharHeight = Game.PixelsToUnits(charHeight);

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    pos.X = startPos.X - CharWidth;
                    pos.Y += CharHeight;

                    continue;
                }

                Chars.Add(new TextChar(font, text[i], pos, PixelsCharWidth, PixelsCharHeight));
                pos.X += CharWidth;
            }

            IsActive = true;
        }

        public void EditPosition(Vector2 newPos)
        {
            Vector2 startNewPos = newPos;

            for (int i = 0, j = 0; i < Text.Length; i++)
            {
                if (Text[i] == '\n')
                {
                    newPos.X = startNewPos.X - CharWidth;
                    newPos.Y += CharHeight;

                    continue;
                }

                Chars[j].Position = newPos;
                newPos.X += CharWidth;

                j++;
            }
        }

        public void EditText(string newText)
        {
            Vector2 pos = Position;

            RemoveText();
            AddText(newText, pos, Font, PixelsCharWidth, PixelsCharHeight);
        }

        public float GetTextWidth()
        {
            if (Chars.Count == 0)
            {
                return 0;
            }

            int longestLineLength = GetTextLengthWithLongestLine(Text.Split('\n'));

            return CharWidth * longestLineLength;
        }

        public float GetTextHeight()
        {
            if (Chars.Count == 0)
            {
                return 0;
            }

            int textVerticalLength = Text.Split('\n').Length;

            return CharHeight * textVerticalLength;
        }

        private int GetTextLengthWithLongestLine(string[] textToHead)
        {
            int longestLineLength = textToHead[0].Length;

            for (int i = 1; i < textToHead.Length; i++)
            {
                if (textToHead[i].Length > longestLineLength)
                {
                    longestLineLength = textToHead[i].Length;
                }
            }

            return longestLineLength;
        }

        private void SetAllCharsActive(bool value)
        {
            for (int i = 0; i < Chars.Count; i++)
            {
                Chars[i].IsActive = value;
            }
        }

        public void RemoveText()
        {
            for (int i = 0; i < Chars.Count; i++)
            {
                DrawMngr.Remove(Chars[i]);
            }

            Chars.Clear();
            UpdateMngr.Remove(this);
        }

        private bool HasMouseClickedText()
        {
            return Game.MousePosition.X >= Position.X && Game.MousePosition.X <= Position.X + GetTextWidth() &&
                   Game.MousePosition.Y >= Position.Y && Game.MousePosition.Y <= Position.Y + GetTextHeight();
        }

        public void Update()
        {
            if (isClickable && IsActive && Game.IsMouseLeftPressed && Game.MousePosition == Game.LastMousePositionClicked && HasMouseClickedText())
            {
                TextClicked = this;
            }
        }
    }
}
