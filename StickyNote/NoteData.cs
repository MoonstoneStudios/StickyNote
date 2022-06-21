using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace StickyNote
{
    /// <summary>
    /// The settings for the note.
    /// </summary>
    public class NoteData
    {
        /// <summary>
        /// The background color of the note.
        /// </summary>
        public SolidColorBrush BackgroudColor { get; set; } = new SolidColorBrush()
        {
            Color = (Color)ColorConverter.ConvertFromString("#FFFFE66E")
        };

        /// <summary>
        /// The font to use.
        /// </summary>
        public FontFamily FontFamily { get; set; } = new FontFamily("Segoe UI");

        /// <summary>
        /// The size of the font in the editor.
        /// </summary>
        public float FontSize { get; set; } = 14;

        /// <summary>
        /// The position of the note.
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// The title of the note.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// If the note is pinned.
        /// </summary>
        public bool Pinned { get; set; } = true;

        /// <summary>
        /// The size of the note.
        /// </summary>
        public Point Size { get; set; } = new Point(450, 450);

        /// <summary>
        /// The index of the note.
        /// </summary>
        public int Index { get; set; }

    }
}
