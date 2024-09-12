using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz
{
    static class TextMngr
    {
        private static Dictionary<string, TextObject> textObjects;

        static TextMngr()
        {
            textObjects = new Dictionary<string, TextObject>();
        }

        public static void AddText(string textId, string text, Vector2 pos, float charWidth = 0, float charHeight = 0, Font font = null)
        {
            if (text == null || textId == null)
            {
                return;
            }

            if (textId == "")
            {
                throw new FormatException("textId can't be empty");
            }

            textObjects.Add(textId, new TextObject(text, pos, charWidth, charHeight, font));
        }

        public static void EditText(string textId, string newText)
        {
            if (newText == null || textId == null)
            {
                return;
            }

            if (textId == "")
            {
                throw new FormatException("textId can't be empty");
            }

            Font font = textObjects[textId].Font;
            Vector2 pos = textObjects[textId].Chars[0].Position;
            float charWidth = textObjects[textId].PixelsCharWidth;
            float charHeight = textObjects[textId].PixelsCharHeight;

            RemoveText(textId);
            AddText(textId, newText, pos, charWidth, charHeight, font);
        }

        public static bool TextExists(string textId)
        {
            if (textObjects.ContainsKey(textId))
            {
                return true;
            }

            return false;
        }

        public static float GetTextLength(string textId)
        {
            int longestLineLength = GetTextLengthWithLongestLine(textObjects[textId].Text.Split('\n'));

            return textObjects[textId].Chars[0].Width * longestLineLength;
        }

        private static int GetTextLengthWithLongestLine(string[] textToHead)
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

        public static void SetTextActive(string textId, bool value)
        {
            textObjects[textId].IsActive = value;
        }

        public static void RemoveText(string textId)
        {
            if (!textObjects.ContainsKey(textId))
            {
                return;
            }

            textObjects[textId].RemoveText();
            textObjects.Remove(textId);
        }
    }
}
