using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace StickyNote
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The save directory of the files.
        /// </summary>
        public static string SaveDir
        {
            get
            {
                return $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/MoonstoneStudios.StickyNote/";
            }
        }

        /// <summary>
        /// A list of all the sticky notes.
        /// </summary>
        public List<StickyNoteWindow> StickyNotes { get; set; } = new List<StickyNoteWindow>();

        /// <summary>
        /// If the app has started yet.
        /// </summary>
        private bool started = false;

        public App()
        {
            InitializeComponent();
            if (!started)
            {
                Load();
                started = true;
            }
        }

        /// <summary>
        /// Save all notes.
        /// </summary>
        public void SaveAll()
        {
            foreach (var note in StickyNotes)
            {
                note.Save();
            }
        }

        /// <summary>
        /// Load the sticky notes from a file.
        /// </summary>
        private void Load()
        {
            if (!Directory.Exists(SaveDir))
            {
                var window = new StickyNoteWindow();
                window.Show();
                StickyNotes.Add(window);
                return;
            }

            // get the stickynotes.
            string[] files = Directory.GetFiles(SaveDir, "*.stickynote");

            if (files.Length == 0)
            {
                var window = new StickyNoteWindow();
                window.Show();
                StickyNotes.Add(window);
                return;
            }

            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                string fileContent = File.ReadAllText(file);
                NoteData data = JsonConvert.DeserializeObject<NoteData>(fileContent);

                StickyNoteWindow window = new StickyNoteWindow()
                {
                    Data = data,
                };

                // https://stackoverflow.com/a/3244693
                window.WindowStartupLocation = WindowStartupLocation.Manual;

                window.Left = (double)data.Position.X;
                window.Top = (double)data.Position.Y;

                window.Width = data.Size.X;
                window.Height = data.Size.Y;

                // set to opposite because PinButtonClick flips it.
                window.Topmost = !data.Pinned;
                // change the icon
                window.PinButtonClick(window.pinButton, null);

                window.titleBox.Text = data.Title;

                window.bgBorder.Background = data.BackgroudColor;

                TextRange range;
                FileStream stream;
                range = new TextRange(window.noteEditor.Document.ContentStart, window.noteEditor.Document.ContentEnd);
                stream = new FileStream(file + "_content", FileMode.OpenOrCreate);
                range.Load(stream, DataFormats.XamlPackage);
                stream.Close();

                window.Show();
                StickyNotes.Add(window);

            }

        }

        

    }
}
