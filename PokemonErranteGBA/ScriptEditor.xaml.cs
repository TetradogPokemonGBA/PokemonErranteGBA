/*
 * Creado por SharpDevelop.
 * Usuario: tetra
 * Fecha: 25/05/2017
 * Hora: 22:54
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
using PokemonGBAFrameWork;

namespace PokemonErranteGBA
{
	/// <summary>
	/// Interaction logic for ScriptEditor.xaml
	/// </summary>
	public partial class ScriptEditor : UserControl
	{
		PokemonErrante.Pokemon pokemonActual;
		public ScriptEditor()
		{
			InitializeComponent();
		}

		public PokemonErrante.Pokemon PokemonActual {
			get{return pokemonActual;}
			set{
				pokemonActual=value;
				//actualizo los datos
			}
		}
	
	}
}