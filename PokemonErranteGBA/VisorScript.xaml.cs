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
using PokemonGBAFrameWork;

namespace PokemonErranteGBA
{
	/// <summary>
	/// Interaction logic for VisorScript.xaml
	/// </summary>
	public partial class VisorScript : Window
	{
		public VisorScript(RomData rom,string script)
		{
			InitializeComponent();
			Title="Script-"+rom.Rom.Nombre;
			txtScritp.Text=script;
			switch (rom.Edicion.AbreviacionRom) {
				case AbreviacionCanon.AXV:
					
					break;
				case AbreviacionCanon.AXP:
					break;
				case AbreviacionCanon.BPE:
					Background=Brushes.LightSeaGreen;
					break;
				case AbreviacionCanon.BPR:
					Background=Brushes.LightCoral;
					break;
				case AbreviacionCanon.BPG:
					Background=Brushes.LightGreen;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}