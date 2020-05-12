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
            for (int i = 0; i < SaltoErrante.Rutas.Length; i++)
            {
                cmbRutas = new ComboBox();
                cmbRutas.Width = 150;
                cmbRutas.ItemsSource = MainWindow.RutasSalto;
                cmbRutas.SelectedIndex = SaltoErrante.Rutas[i]==byte.MaxValue?0: SaltoErrante.Rutas[i]+1;
                stkRutas.Children.Add(cmbRutas);
            }
        }
        public void Save()
        {
            int aux;
            for (int i = 0; i < stkRutas.Children.Count; i++)
            {
               aux= (stkRutas.Children[i] as ComboBox).SelectedIndex;
                if (aux < 1)
                    aux = byte.MaxValue+1;
                SaltoErrante.Rutas[i] =(byte) (aux-1);
            }

        }

}
}
