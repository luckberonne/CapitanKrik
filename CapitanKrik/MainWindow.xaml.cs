using System;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CapitanKrik
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }


        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set tooltip visibility

            if (Tg_Btn.IsChecked == true)
            {
                tt_subir.Visibility = Visibility.Collapsed;
                tt_renombrar.Visibility = Visibility.Collapsed;
                tt_backup.Visibility = Visibility.Collapsed;
                tt_reiniciar.Visibility = Visibility.Collapsed;
                tt_configuracion.Visibility = Visibility.Collapsed;
                tt_log.Visibility = Visibility.Collapsed;
                tt_cerrar.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_subir.Visibility = Visibility.Visible;
                tt_renombrar.Visibility = Visibility.Visible;
                tt_backup.Visibility = Visibility.Visible;
                tt_reiniciar.Visibility = Visibility.Visible;
                tt_configuracion.Visibility = Visibility.Visible;
                tt_log.Visibility = Visibility.Visible;
                tt_cerrar.Visibility = Visibility.Visible;
            }
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 1;
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 0.3;
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public List<Archivos> ListArchivos { get; set; } = ItemsArchivos.GetTodoItems();

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            switch (TABS.SelectedIndex)
            {
                case 0:
                    TABS.SelectedIndex = 1;
                    break;
                case 2:
                    TABS.SelectedIndex = 1;
                    break;
                default:
                    TABS.SelectedIndex = 0;
                    break;
            }
        }

        private void ListViewItem_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            switch (TABS.SelectedIndex)
            {
                case 0:
                    TABS.SelectedIndex = 2;
                    break;
                case 1:
                    TABS.SelectedIndex = 2;
                    break;
                default:
                    TABS.SelectedIndex = 0;
                    break;
            }
        }

        private void nav_pnl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void nav_pnl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
                this.WindowState = WindowState.Minimized;
        }

        private void nav_pnl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
            }
        }

    }

    public class ItemsArchivos
    {
        public List<Archivos> MYLIST { get; set; } = GetTodoItems();
        public static List<Archivos> GetTodoItems()
        {
            var ListArchivos = new List<Archivos>();
            ListArchivos.Add(new Archivos() { nombreArchivo = "FACTA_QUILMES_LANONIMA_00060760.txt" });
            ListArchivos.Add(new Archivos() { nombreArchivo = "NCREA_QUILMES_LANONIMA_00007699.txt" });
            ListArchivos.Add(new Archivos() { nombreArchivo = "NDEBA_ANDINAARG_LANONIMA_00000213.txt" });
            ListArchivos.Add(new Archivos() { nombreArchivo = "NDEBA_ANDINAARG_LANONIMA_06789089.txt" });
            ListArchivos.Add(new Archivos() { nombreArchivo = "NCREA_PEPSICO_LANONIMA_00000358.txt" });
            ListArchivos.Add(new Archivos() { nombreArchivo = "FACTA_QUILMES_LANONIMA_00060760.txt" });
            ListArchivos.Add(new Archivos() { nombreArchivo = "NCREA_QUILMES_LANONIMA_00007699.txt" });
            ListArchivos.Add(new Archivos() { nombreArchivo = "NDEBA_ANDINAARG_LANONIMA_00000213.txt" });
            ListArchivos.Add(new Archivos() { nombreArchivo = "NDEBA_ANDINAARG_LANONIMA_06789089.txt" });
            ListArchivos.Add(new Archivos() { nombreArchivo = "NCREA_PEPSICO_LANONIMA_00000358.txt" });

            return ListArchivos;
        }
    }

    public class Archivos
    {
        public string nombreArchivo { get; set; }
        public string tipoDocumento { get; set; }
        public string emisor { get; set; }
        public string receptor { get; set; }
        public string numeroDocumento { get; set; }

        public override string ToString()
        {
            return this.nombreArchivo;
        }
    }
}
