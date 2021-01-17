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
using FireSharp.Config;
using FireSharp.Response;
using FireSharp;
using Microsoft.Win32;
using System.ComponentModel;

namespace CapitanKrik
{

    public partial class MainWindow : Window
    {



        public MainWindow()
        {
            InitializeComponent();
            CargarConfig();

            Archivos.Elegir();
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set tooltip visibility

            if (Tg_Btn.IsChecked == true)
            {
                tt_subir.Visibility = Visibility.Collapsed;
                tt_renombrar.Visibility = Visibility.Collapsed;
                tt_backup.Visibility = Visibility.Collapsed;
                tt_borrar.Visibility = Visibility.Collapsed;
                tt_configuracion.Visibility = Visibility.Collapsed;
                tt_log.Visibility = Visibility.Collapsed;
                tt_cerrar.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_subir.Visibility = Visibility.Visible;
                tt_renombrar.Visibility = Visibility.Visible;
                tt_backup.Visibility = Visibility.Visible;
                tt_borrar.Visibility = Visibility.Visible;
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


        public static Configuracion.Config TempConf { get; set; } = Task.Run(() => Configuracion.GetConfig()).Result;

        public static BindingList<Archivos.Archivo> ListArchivos { get; set; } = Archivos.GetListArchivos();

        public static List<Logs.Log> ListLogs { get; set; } = Task.Run(() => Logs.GetLogItems()).Result;


        public void GetListSelected()
        {
            if (ListArch.SelectedItems.Count > 0)
            {
                foreach (var item in ListArch.SelectedItems)
                {
                    foreach (var arch in ListArchivos)
                    {
                        if (arch.NombreArchivo == item.ToString())
                        {
                            arch.IsChecked = true;

                        }
                    }
                }
            }
            else
            {
                foreach (var arch in ListArchivos)
                {
                    arch.IsChecked = true;
                }
            }

        }

        public void LimpiarSelected()
        {
            ListArch.SelectedIndex = -1;
            foreach (var arch in ListArchivos)
            {
                arch.IsChecked = false;
            }
        } 


        public void CargarConfig()
        {
            SliderCant.Minimum = 1;
        }

        public static void AddLogList(Logs.Log eslog)
        {
            ListLogs.Add(new Logs.Log() { Mensaje = eslog.ToString() });
            Logs.SetLog(eslog);
        }


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

        private void Nav_pnl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void Nav_pnl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Nav_pnl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
            }
        }


        private async void CSubida_GotFocus(object sender, RoutedEventArgs e)
        {
            var ofd = new System.Windows.Forms.FolderBrowserDialog();
            ofd.ShowDialog();
            if (ofd.SelectedPath.Length > 0)
            {
                CSubida.Text = ofd.SelectedPath;
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/CarpetaSubida", CSubida.Text);
                ListArchivos = Task.Run(() => Archivos.GetListArchivos()).Result;

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {

                    ListArch.ItemsSource = null;
                    ListArch.ItemsSource = ListArchivos;
                }));
                MainWindow_Loaded(sender, e);
            }
            TABS.Focus();
        }

        private async void CBackup_GotFocus(object sender, RoutedEventArgs e)
        {
            var ofd = new System.Windows.Forms.FolderBrowserDialog();
            ofd.ShowDialog();
            if(ofd.SelectedPath.Length > 0)
            {
                CBackup.Text = ofd.SelectedPath;
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/CarpetaBackUP", CBackup.Text);
            }
            TABS.Focus();
        }

        private async void Entrada_Click(object sender, RoutedEventArgs e)
        {
            await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/ProcesoEntrada", Entrada.IsChecked);
        }

        private async void Salida_Click(object sender, RoutedEventArgs e)
        {
            await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/ProcesoSalida", Salida.IsChecked);
        }

        private async void CBackup_LostFocus(object sender, RoutedEventArgs e)
        {
            await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/CarpetaBackUP", CBackup.Text);
        }

        private async void CSubida_LostFocus(object sender, RoutedEventArgs e)
        {
            await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/CarpetaSubida", CSubida.Text);
            ListArchivos = Task.Run(() => Archivos.GetListArchivos()).Result;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {

                ListArch.ItemsSource = null;
                ListArch.ItemsSource = ListArchivos;
            }));
            MainWindow_Loaded(sender, e);
        }

        private void CSubida_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                TABS.Focus();
                e.Handled = true;
            }
        }

        private async void SliderCant_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int someInt = (int)e.NewValue;
            string msg = String.Format("{0}", someInt);
            SliderCant.Value = someInt;
            this.value.Text = msg;
            await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/CantidadArchivos", value.Text);
        }

        private void Value_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                TABS.Focus();
                e.Handled = true;
            }
        }

        private async void Value_LostFocus(object sender, RoutedEventArgs e)
        {
            SliderCant.Value = Int32.Parse(value.Text);
            await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/CantidadArchivos", value.Text);
        }

        
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FirebaseResponse response = await Conexion.Cont().GetAsync(Environment.UserName + "/Configuracion/CarpetaSubida");
            string con = response.ResultAs<string>();
            FileSystemWatcher fsw = new FileSystemWatcher(con)
            {
                NotifyFilter = NotifyFilters.Attributes |
                                NotifyFilters.CreationTime |
                                NotifyFilters.FileName |
                                NotifyFilters.LastAccess |
                                NotifyFilters.LastWrite |
                                NotifyFilters.Size |
                                NotifyFilters.Security,

                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };

            fsw.Changed += new FileSystemEventHandler(Fsw_Changed);
            fsw.Created += new FileSystemEventHandler(Fsw_Changed);
            fsw.Deleted += new FileSystemEventHandler(Fsw_Changed);
            fsw.Renamed += new RenamedEventHandler(Fsw_Changed);

        }

        private void Fsw_Changed(object sender, FileSystemEventArgs e)
        {

            ListArchivos = Task.Run(() => Archivos.GetListArchivos()).Result;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {

                ListArch.ItemsSource = null;
                ListArch.ItemsSource = ListArchivos;

                Archivos.Elegir();
            }));
        }

        private void BtnSubir_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GetListSelected();
            Archivos.Subir();
            LimpiarSelected();
        }

        private void Borrar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GetListSelected();
            Archivos.Delete();
        }

        private void Renombrar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GetListSelected();
            Archivos.Renombrar();

        }

        private void BackUp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GetListSelected();
            Archivos.BackUp();

            LimpiarSelected();
        }
    }




}
