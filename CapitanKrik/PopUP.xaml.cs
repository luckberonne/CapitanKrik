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

namespace CapitanKrik
{
    /// <summary>
    /// Lógica de interacción para PopUP.xaml
    /// </summary>
    public partial class PopUP : Window
    {
        public PopUP(string h, string t)
        {
            InitializeComponent();
            Header.Text = h;
            TextInfo.Text = t;
        }


        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Nav_pnl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.DragMove();
        }

    }
}
