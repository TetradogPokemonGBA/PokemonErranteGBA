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
using Gabriel.Cat.Extension;
using Gabriel.Cat.S.Utilitats;
using PokemonGBAFrameWork;

namespace PokemonErranteGBA
{
	/// <summary>
	/// Interaction logic for ScriptEditor.xaml
	/// </summary>
	public partial class ScriptEditor : UserControl
	{
		const string ESTA = "Quitar", NOESTA = "Insertar";
		RomData romActual;
		PokemonErrante.Pokemon pokemonActual;
		public ScriptEditor()
		{
			
			InitializeComponent();
			//pongo los stats y en tag pongo el stat que toque
			swDormido.ImgOn=Imagenes.Dormido.ToImage().Source;
			swDormido.ImgOff=Imagenes.Dormido_Off.ToImage().Source;
			
			ugEstados.Children.Add( new Gabriel.Cat.Wpf.SwitchImg(Imagenes.Congelado,Imagenes.Congelado_Off){Tag=PokemonErrante.Pokemon.Stat.Congelado});
			ugEstados.Children.Add( new Gabriel.Cat.Wpf.SwitchImg(Imagenes.Paralizado,Imagenes.Paralizado_Off){Tag=PokemonErrante.Pokemon.Stat.Paralizado});
			ugEstados.Children.Add( new Gabriel.Cat.Wpf.SwitchImg(Imagenes.Quemado,Imagenes.Quemado_Off){Tag=PokemonErrante.Pokemon.Stat.Quemado});
			ugEstados.Children.Add( new Gabriel.Cat.Wpf.SwitchImg(Imagenes.Envenenado,Imagenes.Envenenado_Off){Tag=PokemonErrante.Pokemon.Stat.Envenenado});
			ugEstados.Children.Add( new Gabriel.Cat.Wpf.SwitchImg(Imagenes.EnvenenamientoGrave,Imagenes.Envenenamiento_grave_Off){Tag=PokemonErrante.Pokemon.Stat.EnvenenamientoGrave});
			for(int i=0;i<ugEstados.Children.Count;i++)
				((Gabriel.Cat.Wpf.SwitchImg)ugEstados.Children[i]).MouseLeftButtonUp+=PonEstado;
			RomActual=null;
			cmbTurnosDormido.ItemsSource=Enum.GetNames(typeof(PokemonErrante.Pokemon.Dormido));
			cmbTurnosDormido.SelectedIndex=0;
			cmbTurnosDormido.SelectionChanged+=PonTurnosDormido;
		}

		public RomData RomActual {
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
				pokemonActual = value;
				//actualizo los datos
				if (string.IsNullOrEmpty(txtNivel.Text)) {
					pokemonActual.Nivel =new Word( (ushort)50);
					txtNivel.Text = ((ushort)pokemonActual.Nivel)+"";
				} else
					try {
					pokemonActual.Nivel =new Word(  ushort.Parse(txtNivel.Text));
				} catch {
				}
				
				pokemonActual.TurnosDormido=(PokemonErrante.Pokemon.Dormido)cmbTurnosDormido.SelectedIndex;
				
				txtVida.Text = ((ushort)pokemonActual.PokemonErrante.CalculaHp(pokemonActual.Nivel)).ToString();
				txtVidaTotal.Text = " /" + txtVida.Text;
				SetEstadoPokemon();
				BuscaScript();
			}
		}
		
		int BuscaScript()
		{
			int offset = RomActual.Rom.Data.SearchArray(PokemonErrante.Pokemon.GetScript(romActual.Edicion, romActual.Compilacion, PokemonActual).GetDeclaracion(RomActual.Rom));
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
			return GetScript().GetDeclaracionXSE(true,"ScriptPokemonErrante");
		}
		public Script GetScript()
		{
			return PokemonErrante.Pokemon.GetScript(romActual.Edicion, romActual.Compilacion, PokemonActual);
		}
		void BtnVerScript_Click(object sender, RoutedEventArgs e)
		{
			new VisorScript(romActual,PokemonActual.PokemonErrante.Nombre, GetScriptString()).Show();
		}

		void PonEstado(object sender, MouseButtonEventArgs e)
		{
			SetEstadoPokemon();
			BuscaScript();
		}

		void BtnInsertarQuitarScriptBasico_Click(object sender, RoutedEventArgs e)
		{
			byte[] bytes = GetScript().GetDeclaracion(RomActual.Rom);
			if (btnInsertarQuitarScriptBasico.Content.ToString() == ESTA) {
				RomActual.Rom.Data.Remove(BuscaScript(), bytes.Length);
				
				
			} else {
				RomActual.Rom.Data.SetArray(bytes);
			}
			try {
				RomActual.Rom.Save();
			} catch {
				if (MessageBox.Show("No se ha podido guardar, cierra algun programa que lo bloquee estilo AdvanceMap y continua", "Alguna app no deja guardar", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
					try {
					romActual.Rom.Save();
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
					PokemonActual.Nivel = ushort.Parse(txtNivel.Text);
					if (PokemonActual.Nivel > 100)
						PokemonActual.Nivel = (ushort)100;
					else if (PokemonActual.Nivel < 1)
						PokemonActual.Nivel = (ushort)1;
					
					txtVidaTotal.Text = " /" + (ushort)pokemonActual.PokemonErrante.CalculaHp(pokemonActual.Nivel);
					
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