/*
 * Creado por SharpDevelop.
 * Usuario: tetra
 * Fecha: 25/05/2017
 * Hora: 22:45
 * Licencia GNU GPL V3
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Gabriel.Cat.S.Extension;
using Gabriel.Cat.S.Utilitats;
using Microsoft.Win32;
using PokemonGBAFramework.Core;

namespace PokemonErranteGBA
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static RomGba Rom { get; set; }
		public static PokemonErrante.Mapa Mapa { get; set; }
		public static string FileName { get; set; }
        public static IEnumerable RutasSalto { get;  set; }

        public MainWindow()
		{
			InitializeComponent();
		}

		void MiCargar_Click(object sender, RoutedEventArgs e)
		{
			Word aux0=new Word(0);
			OpenFileDialog opn=new OpenFileDialog();
			opn.Filter="Pokemon GBA|*.gba";
			if(opn.ShowDialog().GetValueOrDefault())
			{
				try{
					Rom=new RomGba(opn.FileName);
					FileName = opn.FileName;
					Mapa = PokemonErrante.Mapa.Get(Rom);
					RutasSalto =new object[] { "" }.AfegirValors( PokemonGBAFramework.Core.Mapa.Basic.Bank.Get(Rom, PokemonErrante.Mapa.GetBank(Rom)).Maps);
					visorRutas.Load(Mapa);
						miExportarScript.IsEnabled=true;
						cmbPokedex.IsEnabled=true;
						sePokemonActual.RomActual=Rom;
						cmbPokedex.ItemsSource=Pokemon.GetOrdenNacional(Rom).Filtra((p)=>p.OrdenGameFreak>0&&p.Descripcion.Altura!=default);
						
						cmbPokedex.SelectedIndex=0;
						switch (Rom.Edicion.Version) {
							case Edicion.Pokemon.Zafiro:
								Background= System.Windows.Media.Brushes.LightCoral;
								break;
							case Edicion.Pokemon.Rubi:
								Background= System.Windows.Media.Brushes.LightSkyBlue;
								break;
						case Edicion.Pokemon.RubiOZafiro:
							Background = System.Windows.Media.Brushes.LightSkyBlue;
							break;
						case Edicion.Pokemon.Esmeralda:
								Background= System.Windows.Media.Brushes.LightSeaGreen;
							//	this.Icon=Imagenes.EsmeraldaIco.ToImage().Source;
								break;
							case Edicion.Pokemon.RojoFuego:
								Background= System.Windows.Media.Brushes.LightSalmon;
								
								//this.Icon=Imagenes.RojoFuegoIco.ToImage().Source;
								break;
							case Edicion.Pokemon.VerdeHoja:
								Background= System.Windows.Media.Brushes.LightGreen;
								//this.Icon=Imagenes.VerdeHojaIco.ToImage().Source;
								break;
						case Edicion.Pokemon.RojoOVerde:
							Background = System.Windows.Media.Brushes.LightGreen;
							//this.Icon=Imagenes.VerdeHojaIco.ToImage().Source;
							break;

					}
					
				}catch(Exception m){
					//MessageBox.Show("Hay problemas para cargar la rom actual...\n"+m.Message,"Atención",MessageBoxButton.OK,MessageBoxImage.Error);
				}
				
			}else if(Rom!=null){
			//	MessageBox.Show("No se ha cambiado la rom");
			}else{
				MessageBox.Show("No se ha cargado nada");
			}
		}
		void MiSobre_Click(object sender, RoutedEventArgs e)
		{
			if(MessageBox.Show("Autor: Pikachu240\nLiencia:GNU GPL V3\nInvestigado por  Razhier de Wahack \n¿Quieres ver el código fuente?","Sobre la App",MessageBoxButton.YesNo,MessageBoxImage.Information,MessageBoxResult.Yes)==MessageBoxResult.Yes)
				new Uri("https://github.com/TetradogPokemonGBA/PokemonErranteGBA").Abrir();
		}
		void CmbPokedex_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Pokemon pokemonActual=cmbPokedex.SelectedItem as Pokemon;
			BitmapAnimated frames;
			if (pokemonActual!=null)
			{
				frames = pokemonActual.Sprites.Frontales.GetAnimacionImagenFrontal(pokemonActual.Sprites.PaletaNomal);
				//frames.RemoveFrame(0);
				frames.FrameChanged += (s, img) =>
				{
					Action act = () => imgPokemon.SetImage(img);
					Dispatcher.BeginInvoke(act);
				};
				frames.Start();
		
				sePokemonActual.PokemonActual=new PokemonErrante.Pokemon() { Errante = pokemonActual };
			}
		}
		void MiExportarScript_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog sfdScriptActual=new SaveFileDialog();
			if(sfdScriptActual.ShowDialog().GetValueOrDefault())
				System.IO.File.WriteAllText(sfdScriptActual.FileName+".rbc",sePokemonActual.GetScriptString());
		}
		public static void Save()
		{
			System.IO.File.WriteAllBytes(MainWindow.FileName, Rom.Data.Bytes);
		}
	}
}