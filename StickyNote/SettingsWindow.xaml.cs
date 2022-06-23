using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StickyNote
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        /// <summary>
        /// The data.
        /// </summary>
        public NoteData Data { get; private set; }

        public SettingsWindow()
        {
            InitializeComponent();
            Data = new NoteData();
        }

        public SettingsWindow(NoteData noteData)
        {
            InitializeComponent();
            Data = noteData;
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DoneClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            

        }
    }
}
