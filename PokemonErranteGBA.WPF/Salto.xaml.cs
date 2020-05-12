using PokemonGBAFramework.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PokemonErranteGBA.WPF
{
    /// <summary>
    /// Lógica de interacción para Salto.xaml
    /// </summary>
    public partial class Salto : UserControl
    {

        public Salto()
        {
            InitializeComponent();
        }
        public PokemonGBAFramework.Core.PokemonErrante.Mapa.Salto SaltoErrante { get; set; }
        public void Refresh()
        {
            ComboBox cmbRutas;
            stkRutas.Children.Clear();
            for (int i = 0; i < SaltoErrante.Rutas.Length && SaltoErrante.Rutas[i] != PokemonErrante.Mapa.MARCAFIN; i++)
            {
                cmbRutas = new ComboBox();
                cmbRutas.ItemsSource = MainWindow.RutasSalto;
                cmbRutas.SelectedIndex = SaltoErrante.Rutas[i];
                cmbRutas.MouseDoubleClick += Eliminar;
                stkRutas.Children.Add(cmbRutas);
            }
            btnAñadir.IsEnabled = stkRutas.Children.Count < SaltoErrante.Rutas.Length;
        }
        public void Save()
        {
            for (int i = 0; i < stkRutas.Children.Count; i++)
            {
                SaltoErrante.Rutas[i] = (byte)(stkRutas.Children[i] as ComboBox).SelectedIndex;
            }
            for (int i = stkRutas.Children.Count; i < SaltoErrante.Rutas.Length; i++)
                SaltoErrante.Rutas[i] = byte.MaxValue;
        }

        private void btnAñadir_Click(object sender, RoutedEventArgs e)
        {
            ComboBox cmbRutas = new ComboBox();
            cmbRutas.ItemsSource = MainWindow.RutasSalto;
            cmbRutas.SelectedIndex = 0;
            cmbRutas.MouseDoubleClick += Eliminar;
        stkRutas.Children.Add(cmbRutas);
            btnAñadir.IsEnabled = stkRutas.Children.Count<SaltoErrante.Rutas.Length;
        }
    void Eliminar(object s, EventArgs e)
    {
        Action act;

        if (stkRutas.Children.Count > PokemonErrante.Mapa.MINSALTOS)
            act = () => stkRutas.Children.Remove(s as UIElement);
        else
            act = () => MessageBox.Show("Como minimo tienen que haber  tres rutas!");

        Dispatcher.BeginInvoke(act);

    }
}
}
