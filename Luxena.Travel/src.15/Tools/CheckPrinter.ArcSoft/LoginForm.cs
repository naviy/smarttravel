using System;
using System.Windows.Forms;

using client.NET;


namespace Luxena
{

	public partial class LoginForm : Form
	{

		private readonly PrinterForm _printerForm;

		public LoginForm(PrinterForm printerForm)
		{
			InitializeComponent();
			_printerForm = printerForm;
		}

		private void cmdClose_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void cmdLogin_Click(object sender, EventArgs e)
		{
			var userName = eUserName.Text?.Trim();
			if (userName.No()) return;

			Cursor = Cursors.WaitCursor;

			try
			{
				cmdLogin.Enabled = false;
				if (_printerForm.LoadAuthorization(userName, ePassword.Text))
				{
					var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Luxena\Luxena.Travel.CheckPrinter");
					if (key != null)
					{
						key.SetValue("UserName", userName);
						key.Close();
					}

					DialogResult = DialogResult.OK;
					Close();
				}
				else
				{
					lError.Visible = true;
				}
			}
			finally
			{
				cmdLogin.Enabled = true;
				Cursor = Cursors.Default;
			}
		}


		private void eUserName_TextChanged(object sender, EventArgs e)
		{
			lError.Visible = false;
		}

		private void ePassword_TextChanged(object sender, EventArgs e)
		{
			lError.Visible = false;
		}


		private void LoginForm_Load(object sender, EventArgs e)
		{
			var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Luxena\Luxena.Travel.CheckPrinter");
			if (key != null)
			{
				eUserName.Text = (string)key.GetValue("UserName");
				key.Close();
			}
		}
	}

}
