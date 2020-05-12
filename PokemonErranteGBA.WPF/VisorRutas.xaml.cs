using PokemonGBAFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Lógica de interacción para VisorRutas.xaml
    /// </summary>
    public partial class VisorRutas : UserControl
    {
        public VisorRutas()
        {
            InitializeComponent();
            Saltos = new List<PokemonErrante.Mapa.Salto>();
        }
        public List<PokemonErrante.Mapa.Salto> Saltos { get; set; }

        public void Load(PokemonErrante.Mapa mapa)
        {
            Salto salto;
            Saltos.Clear();
            Saltos.AddRange(mapa.Saltos);
            stkSaltos.Children.Clear();
            for (int i = 0; i < mapa.Saltos.Count; i++)
            {

                    salto = new Salto() { SaltoErrante = mapa.Saltos[i] };
                    salto.Refresh();
                stkSaltos.Children.Add(salto);

            }
            btnAñadir.IsEnabled = true;
            btnCheck.IsEnabled = true;
            btnSave.IsEnabled = true;
        }
        public PokemonErrante.Mapa Save()
        {
            PokemonErrante.Mapa mapa = new PokemonErrante.Mapa();
            for (int i = 0; i < stkSaltos.Children.Count; i++)
                (stkSaltos.Children[i] as Salto).Save();

            mapa.Saltos.AddRange(Saltos);
            return mapa;
        }

        private void btnAñadir_Click(object sender, RoutedEventArgs e)
        {
            if (stkSaltos.Children.Count < PokemonErrante.Mapa.MAXSALTOS)
            {
                stkSaltos.Children.Add(new Salto() { SaltoErrante=new PokemonErrante.Mapa.Salto() { Rutas = new byte[Saltos.First().Rutas.Length] } });
            }
        }

        private void btnCheck_Click(object sender=null, RoutedEventArgs e=null)
        {
            string mensaje = CheckMessage();
            MessageBox.Show(mensaje);

        }

        private string CheckMessage()
        {
            string mensaje;
            int codeCheck = Save().Check;
            switch (codeCheck)
            {
                case PokemonErrante.Mapa.ErrorSaltoRepetido: mensaje = "Salto repetido!"; break;
                case PokemonErrante.Mapa.ErrorNumeroDeSaltosInferior: mensaje = "Hay un salto que le faltan rutas!"; break;
                case PokemonErrante.Mapa.ErrorRutaIgualAlSaltoDeLaLinea: mensaje = "Ruta repetida a la línea!"; break;
                case PokemonErrante.Mapa.ErrorRutaSaltoRepetida: mensaje = "Salto repetido!"; break;
                case PokemonErrante.Mapa.ErrorRutaRepetidaEnLinea: mensaje = "Salto repetido en la misma línea!"; break;
                case PokemonErrante.Mapa.ErrorSaltoLineaNoEncontrado: mensaje = "Ruta no encontrada como Salto!!"; break;
                //case PokemonErrante.Mapa.ErrorNoHayNingunaRutaQueApunteAUnSalto:mensaje = "No hay ruta que apunte a un salto!";break;
                default: mensaje = "Todo correcto!"; break;
            }

            return mensaje;
        }

        private void lstSaltos_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if(MessageBox.Show("Estas seguro de querer eliminar este salto?","Atención",MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.No)==MessageBoxResult.Yes)
            //{
            //    stkSaltos.Children.Remove(e);
            //}
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Save().Check != PokemonErrante.Mapa.TodoCorrecto)
            {
                MessageBox.Show(CheckMessage());
            }
            else
            {
                PokemonErrante.Mapa.Set(MainWindow.Rom, Save());
                MainWindow.Save();
            }
        }
    }
}
