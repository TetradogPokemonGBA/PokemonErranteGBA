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
using Gabriel.Cat.S.Extension;
using Gabriel.Cat.S.Utilitats;
using PokemonErranteGBA.WPF;
using PokemonGBAFramework.Core;

namespace PokemonErranteGBA
{
	/// <summary>
	/// Interaction logic for ScriptEditor.xaml
	/// </summary>
	public partial class ScriptEditor : UserControl
	{
		const string ESTA = "Quitar", NOESTA = "Insertar";
		RomGba romActual;
		PokemonErrante.Pokemon pokemonActual;
		public ScriptEditor()
		{
			
			InitializeComponent();
			//pongo los stats y en tag pongo el stat que toque
			swDormido.ImgOn=Resource1.Dormido.ToImage().Source;
			swDormido.ImgOff= Resource1.Dormido_Off.ToImage().Source;
			
			ugEstados.Children.Add( new Gabriel.Cat.Wpf.SwitchImg(Resource1.Congelado, Resource1.Congelado_Off){Tag=PokemonErrante.Pokemon.Stat.Congelado});
			ugEstados.Children.Add( new Gabriel.Cat.Wpf.SwitchImg(Resource1.Paralizado, Resource1.Paralizado_Off){Tag=PokemonErrante.Pokemon.Stat.Paralizado});
			ugEstados.Children.Add( new Gabriel.Cat.Wpf.SwitchImg(Resource1.Quemado, Resource1.Quemado_Off){Tag=PokemonErrante.Pokemon.Stat.Quemado});
			ugEstados.Children.Add( new Gabriel.Cat.Wpf.SwitchImg(Resource1.Envenenado, Resource1.Envenenado_Off){Tag=PokemonErrante.Pokemon.Stat.Envenenado});
			ugEstados.Children.Add( new Gabriel.Cat.Wpf.SwitchImg(Resource1.Envenenamiento_grave, Resource1.Envenenamiento_grave_Off){Tag=PokemonErrante.Pokemon.Stat.EnvenenamientoGrave});
			for(int i=0;i<ugEstados.Children.Count;i++)
				((Gabriel.Cat.Wpf.SwitchImg)ugEstados.Children[i]).MouseLeftButtonUp+=PonEstado;
			RomActual=null;
			cmbTurnosDormido.ItemsSource=Enum.GetNames(typeof(PokemonErrante.Pokemon.Dormido));
			cmbTurnosDormido.SelectedIndex=0;
			cmbTurnosDormido.SelectionChanged+=PonTurnosDormido;
		}

		public RomGba RomActual {
			get {
				return romActual;
			}
			set {
				romActual = value;
				btnInsertarQuitarScriptBasico.IsEnabled = romActual != null;
				btnVerScript.IsEnabled = romActual != null;
				txtVida.IsEnabled=romActual!=null;
				cmbTurnosDormido.IsEnabled=romActual!=null;
				txtNivel.IsEnabled=romActual!=null;
				for(int i=0;i<ugEstados.Children.Count;i++)
					((Gabriel.Cat.Wpf.SwitchImg)ugEstados.Children[i]).IsEnabled=romActual!=null;
			}
		}
		public PokemonErrante.Pokemon PokemonActual {
			get{ return pokemonActual; }
			set {
				Word maxVida;
				pokemonActual = value;
				//actualizo los datos
				if (string.IsNullOrEmpty(txtNivel.Text)) {
					pokemonActual.Nivel =new Word(50);
					txtNivel.TextChanged -= TxtNivel_TextChanged;
					txtNivel.Text = pokemonActual.Nivel.ToString();
					txtNivel.TextChanged += TxtNivel_TextChanged;
				} else
					try {
					pokemonActual.Nivel =new Word(  ushort.Parse(txtNivel.Text));
				} catch {
				}
				
				pokemonActual.TurnosDormido=(PokemonErrante.Pokemon.Dormido)cmbTurnosDormido.SelectedIndex;
				maxVida = ((ushort)pokemonActual.Errante.Stats.CalculaHp(pokemonActual.Nivel));
				if (pokemonActual.Vida == default)
					pokemonActual.Vida = maxVida;
				txtVida.Text = maxVida.ToString();
				txtVidaTotal.Text = " /" + txtVida.Text;
				SetEstadoPokemon();
				BuscaScript();
			}
		}
		
		int BuscaScript()
		{
			int offset = RomActual.Data.SearchArray(PokemonErrante.GetScript(romActual.Edicion, PokemonActual).GetDeclaracion(RomActual));
			if (offset > 0) {
				txtOffset.Text = (Hex)offset;
				btnInsertarQuitarScriptBasico.Content = ESTA;
			} else {
				txtOffset.Text = "";
				btnInsertarQuitarScriptBasico.Content = NOESTA;
			}
			return offset;
		}

		public string GetScriptString()
		{
			return GetScript().GetDeclaracionXSE($"Script{GetName()}Errante",true);
		}

		private object GetName()
		{
			string name = pokemonActual.Errante.Nombre.ToString().ToLower();
			name = char.ToUpper(name[0]) + name.Substring(1);
			return name;
		}

		public Script GetScript()
		{
			SetEstadoPokemon();
			return PokemonErrante.GetScript(romActual.Edicion,PokemonActual);
		}
		void BtnVerScript_Click(object sender, RoutedEventArgs e)
		{
			new VisorScript(romActual,PokemonActual.Errante.Nombre.ToString(), GetScriptString()).Show();
		}

		void PonEstado(object sender, MouseButtonEventArgs e)
		{
			SetEstadoPokemon();
			BuscaScript();
		}

		void BtnInsertarQuitarScriptBasico_Click(object sender, RoutedEventArgs e)
		{
			byte[] bytes = GetScript().GetDeclaracion(RomActual);
			if (btnInsertarQuitarScriptBasico.Content.ToString() == ESTA) {
				RomActual.Data.Remove(BuscaScript(), bytes.Length);
				
				
			} else {
				RomActual.Data.SetArrayIfNotExist(bytes);
			}
			try {
				MainWindow.Save();
			} catch {
				if (MessageBox.Show("No se ha podido guardar, cierra algun programa que lo bloquee estilo AdvanceMap y continua", "Alguna app no deja guardar", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
					try {
						MainWindow.Save();
					} catch {
					MessageBox.Show("no se ha podido prueba reiniciando el pc...");
				}
			}
			BuscaScript();
		}
		void TxtNivel_TextChanged(object sender, TextChangedEventArgs e)
		{
			if(!string.IsNullOrEmpty(txtNivel.Text)){
				try {
					PokemonActual.Nivel =new Word(ushort.Parse(txtNivel.Text));
					if (PokemonActual.Nivel > 100)
						PokemonActual.Nivel = (ushort)100;
					else if (PokemonActual.Nivel < 1)
						PokemonActual.Nivel = (ushort)1;
					
					txtVidaTotal.Text = " /" + (ushort)pokemonActual.Errante.Stats.CalculaHp(pokemonActual.Nivel);
					
				} catch {
					pokemonActual.Nivel = (ushort)1;
				}
				txtNivel.TextChanged-=TxtNivel_TextChanged;
				txtNivel.Text = ((ushort)pokemonActual.Nivel) + "";
				txtNivel.TextChanged+=TxtNivel_TextChanged;
				BuscaScript();}
		}

		void SetEstadoPokemon()
		{
			Gabriel.Cat.Wpf.SwitchImg swEstado;
			//pongo al pokemon el estado
			for (int i = 0; i<ugEstados.Children.Count; i++) {
				swEstado = ugEstados.Children[i] as Gabriel.Cat.Wpf.SwitchImg;
				pokemonActual.SetStatNoDormido((PokemonErrante.Pokemon.Stat)swEstado.Tag, swEstado.EstadoOn);
			}
			
		}

		void TxtVida_TextChanged(object sender, TextChangedEventArgs e)
		{
			
			if(!string.IsNullOrEmpty(txtVida.Text)){
				try {
					PokemonActual.Vida = ushort.Parse(txtVida.Text);
					if (PokemonActual.Vida > ushort.MaxValue) {
						pokemonActual.Vida = ushort.MaxValue;
						
					} else if (pokemonActual.Vida < 0) {
						pokemonActual.Vida =(ushort) 0;
					}
					
				} catch {
					pokemonActual.Vida = ushort.MaxValue;
				}
				txtVida.Text = (ushort)pokemonActual.Vida + "";
				BuscaScript();}
		}

		void PonTurnosDormido(object sender, SelectionChangedEventArgs e)
		{


			pokemonActual.TurnosDormido=(PokemonErrante.Pokemon.Dormido)cmbTurnosDormido.SelectedIndex;

			swDormido.EstadoOn=pokemonActual.TurnosDormido!=PokemonErrante.Pokemon.Dormido.NoDormido;
			BuscaScript();
		}
	}

}