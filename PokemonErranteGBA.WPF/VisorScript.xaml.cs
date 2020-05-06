/*
 * Creado por SharpDevelop.
 * Usuario: tetra
 * Fecha: 05/26/2017
 * Hora: 01:07
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
using PokemonGBAFramework.Core;

namespace PokemonErranteGBA
{
	/// <summary>
	/// Interaction logic for VisorScript.xaml
	/// </summary>
	public partial class VisorScript : Window
	{
		public VisorScript(RomGba rom,string nombrePokemon,string script)
		{
			InitializeComponent();
			Title="Script-"+rom.Edicion.Version.ToString()+"-"+nombrePokemon;
			txtScritp.Text=script;
			switch (rom.Edicion.Version) {
				case Edicion.Pokemon.Zafiro:
					Background=Brushes.LightCoral;
					break;
				case Edicion.Pokemon.Rubi:
					Background=Brushes.LightSkyBlue;
					break;
				case Edicion.Pokemon.RubiOZafiro:
					Background = Brushes.LightSkyBlue;
					break;
				case Edicion.Pokemon.Esmeralda:
					Background=Brushes.LightSeaGreen;
					break;
				case Edicion.Pokemon.RojoFuego:
					
					Background=Brushes.LightSalmon;
					break;
				case Edicion.Pokemon.VerdeHoja:
					Background=Brushes.LightGreen;
					break;
				case Edicion.Pokemon.RojoOVerde:
					Background = Brushes.LightGreen;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}