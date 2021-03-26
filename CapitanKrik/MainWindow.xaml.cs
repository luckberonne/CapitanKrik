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
using System.Diagnostics;

namespace CapitanKrik
{

    public partial class MainWindow : Window
    {



        public MainWindow()
        {
            InitializeComponent();
            CargarConfig();
            CambiarEntorno();

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
                TextEntorno.Visibility = Visibility.Collapsed;
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
                TextEntorno.Visibility = Visibility.Visible;
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
                    Archivos.Archivo arch = ListArchivos.FirstOrDefault(s => s.NombreArchivo.Contains(item.ToString()));
                    arch.IsChecked = true;
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
                this.MaxHeight = SystemParameters.WorkArea.Height;

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
            if (TempConf.PopUps)
            {
                PopUP w = new PopUP("Proximamente", "Esta función aún no esta disponible") { Owner = this };
                w.ShowDialog();
            }
        }

        private async void Salida_Click(object sender, RoutedEventArgs e)
        {
            await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/ProcesoSalida", Salida.IsChecked);
            if (TempConf.PopUps)
            {
                PopUP w = new PopUP("Proximamente", "Esta función aún no esta disponible") { Owner = this };
                w.ShowDialog();
            }
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

            if (TempConf.PopUps)
            {
                PopUP w = new PopUP("Proximamente", "Esta función aún no esta disponible") { Owner = this };
                w.ShowDialog();
            }
        }

        private async void Value_LostFocus(object sender, RoutedEventArgs e)
        {
            SliderCant.Value = Int32.Parse(value.Text);
            await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/CantidadArchivos", value.Text);

            if (TempConf.PopUps)
            {
                PopUP w = new PopUP("Proximamente", "Esta función aún no esta disponible") { Owner = this };
                w.ShowDialog();
            }
        }

        bool x = true;
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FileSystemWatcher fsw = new FileSystemWatcher(TempConf.CarpetaSubida)
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



            if (TempConf.PrimeraVez)
            {
                PopUP w = new PopUP("Bienvenido", "CapitanKrik es un programa para facilitar la subida de documentos; \n \nCreada por Lucas Beronne");
                w.ShowDialog();
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/PrimeraVez", false);
            }
        }

        private void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            if (x)
            {
                ListArchivos = null;

                ListArchivos = Archivos.GetListArchivos();
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ListArch.ItemsSource = null;
                    ListArch.ItemsSource = ListArchivos;
                }));
            }

        }

        public void reiniciar()
        {
            ListArchivos = null;

            ListArchivos = Archivos.GetListArchivos();
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                ListArch.ItemsSource = null;
                ListArch.ItemsSource = ListArchivos;
            }));
        }

        private void BtnSubir_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TABS.SelectedIndex = 0;
            x = false;
            bool i = false;
            GetListSelected();
            Archivos.Elegir();
            
            if (TempConf.Renombrar)
            {
                Archivos.Renombrar();
                i = true;
            }


            if (TempConf.BackUPs)
            {
                Archivos.BackUp();
            }

            Archivos.Subir();

            if (TempConf.Eliminar)
            {
                Archivos.Delete();
                i = true;
            }

            if (i)
            {
                reiniciar();
            }
            LimpiarSelected();
            x = true;

            if (TempConf.PopUps)
            {
                PopUP w = new PopUP("Información", "Usted subio los archivos seleccionados a la carpeta destino") { Owner = this };
                w.ShowDialog();
            }

        }

        private void Borrar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TABS.SelectedIndex = 0;
            GetListSelected();
            Archivos.Delete();

            if (TempConf.PopUps)
            {
                PopUP w = new PopUP("Información", "Usted Borro los archivos seleccionados de la carpeta") { Owner = this };
                w.ShowDialog();
            }
        }

        private void Renombrar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TABS.SelectedIndex = 0;


            GetListSelected();
            Archivos.Elegir();
            Archivos.Renombrar();

            if (TempConf.PopUps)
            {
                PopUP w = new PopUP("Información", "Usted Renombro los archivos seleccionados de la carpeta") { Owner = this };
                w.ShowDialog();
            }

        }

        private void BackUp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TABS.SelectedIndex = 0;

            GetListSelected();
            Archivos.BackUp();

            LimpiarSelected();

            if (TempConf.PopUps)
            {
                PopUP w = new PopUP("Información", "Usted hizo BackUP de los archivos seleccionados a la carpeta indicada") { Owner = this };
                w.ShowDialog();
            }
        }

        private void ListArch_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var item in files)
                {
                    File.Copy(item, System.IO.Path.Combine(MainWindow.TempConf.CarpetaSubida, System.IO.Path.GetFileName(item)), true);
                }
            }
        }

        private async void PopUps_Click(object sender, RoutedEventArgs e)
        {
            await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/PopUps", PopUPS.IsChecked);
        }

        private async void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/Eliminar", Eliminar.IsChecked);
        }

        private async void Renombrar_Click(object sender, RoutedEventArgs e)
        {
            await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/Renombrar", Renomb.IsChecked);
        }

        private async void BackUPs_Click(object sender, RoutedEventArgs e)
        {
            await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/BackUPs", BackUPs.IsChecked);
        }

        private async void Entorno_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ("CN" == TempConf.Entorno)
            {
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/Entorno", "QA");
                TempConf.Entorno = "QA";
            }
            else
            {
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/Entorno", "CN");
                TempConf.Entorno = "CN";
            }
            CambiarEntorno();
        }

        public void CambiarEntorno()
        {
            if (TempConf.Entorno == "CN")
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri("\\Assets\\cn.png", UriKind.Relative));
                ImgEntorno.Source = img.Source;
                TextEntorno.Content = "Consultoria";
                TEntorno.Text = "Consultoria";
                TempConf.Entorno = "CN";
            }
            else if (TempConf.Entorno == "QA")
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri("\\Assets\\qa.png", UriKind.Relative));
                ImgEntorno.Source = img.Source;
                TextEntorno.Content = "QA";
                TEntorno.Text = "QA";
                TempConf.Entorno = "QA";
            }
        }

        private void TextBlock_LeftDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                Process.Start((System.IO.Path.Combine(TempConf.CarpetaSubida, ((System.Windows.Controls.TextBlock)e.Source).Text.ToString())));
            }
        }
    }




}
