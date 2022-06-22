using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EImage = Emoji.Wpf.Image;

namespace StickyNote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StickyNoteWindow : Window
    {
        /// <summary>
        /// The settings for the note.
        /// </summary>
        public NoteData Data { get; set; } = new NoteData();

        public StickyNoteWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Pin the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PinButtonClick(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;

            Button b = (Button)sender;

            if (Topmost)
            {
                // change the image inside the pin.
                EImage.SetSource((DependencyObject)b.Content, "📍");
                b.Effect = null;
            }
            else
            {
                // change the image inside the pin.
                EImage.SetSource((DependencyObject)b.Content, "📌");
                // add effect
                b.Effect = new DropShadowEffect()
                {
                    Opacity = 0.6f,
                    BlurRadius = 3
                };
            }

            Data.Pinned = Topmost;

        }

        /// <summary>
        /// Close the note.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            // shutdown if shift held.
            if (Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                App app = (App)Application.Current;
                app.SaveAll();
                Application.Current.Shutdown();
            }
            // shutdown without saving if control and shift pressed
            else if (Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Application.Current.Shutdown();
            }
            else
            {
                Close();
                string file = App.SaveDir + $"{Data.Index}.stickynote";

                if (File.Exists(file))
                {
                    File.Delete(file);
                    File.Delete(file + "_content");
                }

            }
        }

        /// <summary>
        /// Make a new window close to the current.
        /// </summary>
        private void NewNoteButtonClick(object sender, RoutedEventArgs e)
        {
            // https://stackoverflow.com/a/2734831
            // https://stackoverflow.com/a/3244693
            StickyNoteWindow note = new StickyNoteWindow();
            note.Show();

            App app = (App)Application.Current;
            app.StickyNotes.Add(note);

            Point thisNote = PointToScreen(new Point(Left, Top));

            Point newNote = new Point(thisNote.X + 50, thisNote.Y + 50);

            newNote = PointFromScreen(newNote);

            note.Left = newNote.X;
            note.Top = newNote.Y;

            note.Data.Position = newNote;
            note.Data.Index = app.StickyNotes.Count;

        }

        /// <summary>
        /// Move the note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveWhenDragged(object sender, MouseButtonEventArgs e)
        {
            // only move with left mouse
            if (e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Released)
                DragMove();
        }

        /// <summary>
        /// Insert an image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
            if (d.ShowDialog() == true)
            {
                // https://stackoverflow.com/a/1963567
                Clipboard.SetImage(new BitmapImage(new Uri(d.FileName)));
                noteEditor.Paste();
            }
        }

        /// <summary>
        /// Print (might remove)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintNote(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                pd.PrintVisual(noteEditor, "Print Document");
            }
        }

        /// <summary>
        /// Save the note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Save(object sender, RoutedEventArgs e) => Save();

        /// <summary>
        /// Save the note
        /// </summary>
        public void Save()
        {
            Data.Position = new Point(Left, Top);
            Data.Pinned = Topmost;
            Data.BackgroudColor = (SolidColorBrush)Background;
            Data.FontFamily = noteEditor.FontFamily;
            Data.FontSize = (float)noteEditor.FontSize;
            Data.Size = new Point(Width, Height);
            Data.Title = titleBox.Text;

            string fileDir = App.SaveDir + $"{Data.Index}.stickynote";
            if (!Directory.Exists(App.SaveDir))
            {
                Directory.CreateDirectory(App.SaveDir);
            }

            string json = JsonConvert.SerializeObject(Data, Formatting.Indented);
            // save settings
            File.WriteAllText(fileDir, json);
            // save note.

            TextRange range;
            FileStream stream;
            range = new TextRange(noteEditor.Document.ContentStart, noteEditor.Document.ContentEnd);
            stream = new FileStream(fileDir + "_content", FileMode.Create);
            range.Save(stream, DataFormats.XamlPackage);
            stream.Close();

        }

        private void SaveAndCloseClick(object sender, RoutedEventArgs e)
        {
            Save();
            Close();
        }
    }


}
