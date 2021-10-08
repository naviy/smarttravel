namespace Luxena
{
	partial class LoginForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.eUserName = new System.Windows.Forms.TextBox();
			this.ePassword = new System.Windows.Forms.TextBox();
			this.cmdLogin = new System.Windows.Forms.Button();
			this.cmdClose = new System.Windows.Forms.Button();
			this.lError = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(56, 52);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(59, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Логин";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(43, 94);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 20);
			this.label2.TabIndex = 1;
			this.label2.Text = "Пароль";
			// 
			// eUserName
			// 
			this.eUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.eUserName.Location = new System.Drawing.Point(121, 45);
			this.eUserName.Name = "eUserName";
			this.eUserName.Size = new System.Drawing.Size(291, 30);
			this.eUserName.TabIndex = 2;
			this.eUserName.TextChanged += new System.EventHandler(this.eUserName_TextChanged);
			// 
			// ePassword
			// 
			this.ePassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ePassword.Location = new System.Drawing.Point(121, 87);
			this.ePassword.Name = "ePassword";
			this.ePassword.PasswordChar = '*';
			this.ePassword.Size = new System.Drawing.Size(291, 30);
			this.ePassword.TabIndex = 3;
			this.ePassword.TextChanged += new System.EventHandler(this.ePassword_TextChanged);
			// 
			// cmdLogin
			// 
			this.cmdLogin.Location = new System.Drawing.Point(13, 162);
			this.cmdLogin.Name = "cmdLogin";
			this.cmdLogin.Size = new System.Drawing.Size(189, 43);
			this.cmdLogin.TabIndex = 4;
			this.cmdLogin.Text = "Войти";
			this.cmdLogin.UseVisualStyleBackColor = true;
			this.cmdLogin.Click += new System.EventHandler(this.cmdLogin_Click);
			// 
			// cmdClose
			// 
			this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdClose.Location = new System.Drawing.Point(223, 162);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(189, 43);
			this.cmdClose.TabIndex = 5;
			this.cmdClose.Text = "Закрыть";
			this.cmdClose.UseVisualStyleBackColor = true;
			this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
			// 
			// lError
			// 
			this.lError.AutoSize = true;
			this.lError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.lError.Location = new System.Drawing.Point(117, 120);
			this.lError.Name = "lError";
			this.lError.Size = new System.Drawing.Size(308, 20);
			this.lError.TabIndex = 6;
			this.lError.Text = "Указан неверный логин или пароль";
			this.lError.Visible = false;
			// 
			// LoginForm
			// 
			this.AcceptButton = this.cmdLogin;
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdClose;
			this.ClientSize = new System.Drawing.Size(424, 220);
			this.Controls.Add(this.lError);
			this.Controls.Add(this.cmdClose);
			this.Controls.Add(this.cmdLogin);
			this.Controls.Add(this.ePassword);
			this.Controls.Add(this.eUserName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "LoginForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Luxena.Travel. Печать чеков. Форма входа";
			this.Load += new System.EventHandler(this.LoginForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox eUserName;
		private System.Windows.Forms.TextBox ePassword;
		private System.Windows.Forms.Button cmdLogin;
		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.Label lError;
	}
}