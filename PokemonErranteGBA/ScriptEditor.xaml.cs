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
		const string ESTA="Quitar",NOESTA="Insertar";
		RomData romActual;
		PokemonErrante.Pokemon pokemonActual;
		public ScriptEditor()
		{
			InitializeComponent();
		}

		public RomData RomActual {
			get {
				return romActual;
			}
			set{
				romActual=value;
				btnInsertarQuitarScriptBasico.IsEnabled=romActual!=null;
				btnVerScript.IsEnabled=romActual!=null;
				
			}
		}
		public PokemonErrante.Pokemon PokemonActual {
			get{return pokemonActual;}
			set{
				pokemonActual=value;
				//actualizo los datos
				BuscaScript();
			}
		}
	
		int BuscaScript()
		{
			int offset=RomActual.Rom.Data.SearchArray(PokemonErrante.Pokemon.BytesScript(romActual.Edicion,romActual.Compilacion,PokemonActual));
			if(offset>0){
				txtOffset.Text=(Gabriel.Cat.Hex)offset; 
				btnInsertarQuitarScriptBasico.Content=ESTA;}
			else{
				txtOffset.Text="";
				btnInsertarQuitarScriptBasico.Content=NOESTA;}
			return offset;
		}

		public string GetScript()
		{
			return PokemonErrante.Pokemon.Script(romActual.Edicion,romActual.Compilacion,PokemonActual);
		}
		void BtnVerScript_Click(object sender, RoutedEventArgs e)
		{
			new VisorScript(romActual,GetScript()).Show();
		}
		void BtnInsertarQuitarScriptBasico_Click(object sender, RoutedEventArgs e)
		{
			byte[] bytes=PokemonErrante.Pokemon.BytesScript(romActual.Edicion,romActual.Compilacion,PokemonActual);
			if(btnInsertarQuitarScriptBasico.Content.ToString()==ESTA)
			{
				RomActual.Rom.Data.Remove(BuscaScript(),bytes.Length);
				
				
			}else{
				RomActual.Rom.Data.SetArray(bytes);
			}
			try{
			RomActual.Rom.Save();
			}catch{
			if(	MessageBox.Show("No se ha podido guardar, cierra algun programa que lo bloquee estilo AdvanceMap y continua","Alguna app no deja guardar",MessageBoxButton.YesNo,MessageBoxImage.Error)==MessageBoxResult.Yes)
				try{romActual.Rom.Save();}catch{MessageBox.Show("no se ha podido prueba reiniciando el pc...");}
			}
			BuscaScript();
		}
	}
}