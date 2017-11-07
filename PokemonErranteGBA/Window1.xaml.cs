/*
 * Creado por SharpDevelop.
 * Usuario: tetra
 * Fecha: 25/05/2017
 * Hora: 22:45
 * Licencia GNU GPL V3
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Gabriel.Cat.Extension;
using Microsoft.Win32;
using PokemonGBAFrameWork;
namespace PokemonErranteGBA
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		RomData rom;
		public Window1()
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
					rom=new RomData(opn.FileName);
					
					if(rom.Rutas!=null)
					{
						miExportarScript.IsEnabled=true;
						cmbPokedex.IsEnabled=true;
						sePokemonActual.RomActual=rom;
						rom.Pokedex.Sort();
						cmbPokedex.ItemsSource=rom.Pokedex.Filtra((p)=>p.OrdenNacional>aux0);
						
						cmbPokedex.SelectedIndex=0;
						switch (rom.Edicion.AbreviacionRom) {
							case AbreviacionCanon.AXV:
								Background=Brushes.LightCoral;
								break;
							case AbreviacionCanon.AXP:
								Background=Brushes.LightSkyBlue;
								break;
							case AbreviacionCanon.BPE:
								Background=Brushes.LightSeaGreen;
							//	this.Icon=Imagenes.EsmeraldaIco.ToImage().Source;
								break;
							case AbreviacionCanon.BPR:
								Background=Brushes.LightSalmon;
								
								//this.Icon=Imagenes.RojoFuegoIco.ToImage().Source;
								break;
							case AbreviacionCanon.BPG:
								Background=Brushes.LightGreen;
								//this.Icon=Imagenes.VerdeHojaIco.ToImage().Source;
								break;
				
						}
					}
					else{
						rom=null;
						MessageBox.Show("La rom no es compatible por falta investigación...prueba en futuras versiones...");
					}
				}catch(Exception m){
					MessageBox.Show("Hay problemas para cargar la rom actual...\n"+m.Message,"Atención",MessageBoxButton.OK,MessageBoxImage.Error);
				}
				
			}else if(rom!=null){
				MessageBox.Show("No se ha cambiado la rom");
			}else{
				MessageBox.Show("No se ha cargado nada");
			}
		}
		void MiSobre_Click(object sender, RoutedEventArgs e)
		{
			if(MessageBox.Show("Autor: Pikachu240\nLiencia:GNU GPL V3\nInvestigado por  Razhier de Wahack \n¿Quieres ver el código fuente?","Sobre la App",MessageBoxButton.YesNo,MessageBoxImage.Information)==MessageBoxResult.Yes)
				System.Diagnostics.Process.Start("https://github.com/TetradogPokemonGBA/PokemonErranteGBA");
		}
		void CmbPokedex_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Pokemon pokemonActual=cmbPokedex.SelectedItem as Pokemon;
			if(pokemonActual!=null)
			{
				imgPokemon.SetImage(pokemonActual.Sprites.SpritesFrontales[0]);
				sePokemonActual.PokemonActual=new PokemonErrante.Pokemon(pokemonActual);
			}
		}
		void MiExportarScript_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog sfdScriptActual=new SaveFileDialog();
			if(sfdScriptActual.ShowDialog().GetValueOrDefault())
				System.IO.File.WriteAllText(sfdScriptActual.FileName+".rbc",sePokemonActual.GetScriptString());
		}
	}
}