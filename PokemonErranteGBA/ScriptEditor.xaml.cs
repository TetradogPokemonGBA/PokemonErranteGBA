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
				txtTurnosDormido.IsEnabled=romActual!=null;
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
					pokemonActual.Nivel = 50;
					txtNivel.Text = pokemonActual.Nivel + "";
				} else
					try {
					pokemonActual.Nivel = int.Parse(txtNivel.Text);
				} catch {
				}
				try{
					pokemonActual.Dormido=int.Parse(txtTurnosDormido.Text);
				}catch{txtTurnosDormido.Text="0";}
				txtVida.Text = pokemonActual.PokemonErrante.CalculaHp(pokemonActual.Nivel) + "";
				txtVidaTotal.Text = " /" + txtVida.Text;
				SetEstadoPokemon();
				BuscaScript();
			}
		}
		
		int BuscaScript()
		{
			int offset = RomActual.Rom.Data.SearchArray(PokemonErrante.Pokemon.BytesScript(romActual.Edicion, romActual.Compilacion, PokemonActual));
			if (offset > 0) {
				txtOffset.Text = (Gabriel.Cat.Hex)offset;
				btnInsertarQuitarScriptBasico.Content = ESTA;
			} else {
				txtOffset.Text = "";
				btnInsertarQuitarScriptBasico.Content = NOESTA;
			}
			return offset;
		}

		public string GetScript()
		{
			return PokemonErrante.Pokemon.Script(romActual.Edicion, romActual.Compilacion, PokemonActual);
		}
		void BtnVerScript_Click(object sender, RoutedEventArgs e)
		{
			new VisorScript(romActual,PokemonActual.PokemonErrante.Nombre, GetScript()).Show();
		}

		void PonEstado(object sender, MouseButtonEventArgs e)
		{
				SetEstadoPokemon();
				BuscaScript();
		}

		void BtnInsertarQuitarScriptBasico_Click(object sender, RoutedEventArgs e)
		{
			byte[] bytes = PokemonErrante.Pokemon.BytesScript(romActual.Edicion, romActual.Compilacion, PokemonActual);
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
			
			try {
				PokemonActual.Nivel = int.Parse(txtNivel.Text);
				if (PokemonActual.Nivel > 100)
					PokemonActual.Nivel = 100;
				else if (PokemonActual.Nivel < 1)
					PokemonActual.Nivel = 1;
				
				txtVidaTotal.Text = " /" + pokemonActual.PokemonErrante.CalculaHp(pokemonActual.Nivel);
				
			} catch {
				pokemonActual.Nivel = 1;
			}
			
			txtNivel.Text = pokemonActual.Nivel + "";
			BuscaScript();
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
	
			try {
				PokemonActual.Vida = int.Parse(txtVida.Text);
				if (PokemonActual.Vida > short.MaxValue) {
					pokemonActual.Vida = short.MaxValue;
					
				} else if (pokemonActual.Vida < 0) {
					pokemonActual.Vida = 0;
				}
				
			} catch {
				pokemonActual.Vida = 1;
			}
			txtVida.Text = pokemonActual.Vida + "";
			BuscaScript();
		}
		void TxtTurnosDormido_TextChanged(object sender, TextChangedEventArgs e)
		{
			
			if(!string.IsNullOrEmpty(txtTurnosDormido.Text)){
				try {
					
					PokemonActual.Dormido = int.Parse(txtTurnosDormido.Text);
					
				} catch {
					pokemonActual.Dormido = 0;
				}
				txtTurnosDormido.TextChanged-=TxtTurnosDormido_TextChanged;
				txtTurnosDormido.Text = pokemonActual.Dormido + "";
				txtTurnosDormido.TextChanged+=TxtTurnosDormido_TextChanged;
				swDormido.EstadoOn=pokemonActual.Dormido>0;
				BuscaScript();}
			
		}
	}
}