using System;
using System.Windows;

namespace GothicModComposer.UI.Views
{
	public partial class InputDialog
	{
		public InputDialog(string title, string question, string defaultAnswer = "")
		{
			InitializeComponent();

			Title = title;
			lblQuestion.Content = question;
			txtAnswer.Text = defaultAnswer;
		}

		private void btnDialogOk_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void Window_ContentRendered(object sender, EventArgs e)
		{
			txtAnswer.SelectAll();
			txtAnswer.Focus();
		}

		public string Answer => txtAnswer.Text;
	}
}