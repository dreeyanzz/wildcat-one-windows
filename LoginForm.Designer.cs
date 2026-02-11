namespace wildcat_one_windows
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // === Form properties ===
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 680);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Wildcat One - Login";
            BackColor = Color.FromArgb(248, 245, 240); // #f8f5f0
            Font = new Font("Segoe UI", 9F);

            // === Shadow Panel (for depth effect behind card) ===
            shadowPanel = new Panel();
            shadowPanel.Size = new Size(404, 594);
            shadowPanel.Location = new Point(50, 45);
            shadowPanel.BackColor = Color.FromArgb(30, 122, 26, 61);

            // === Card Panel ===
            cardPanel = new Panel();
            cardPanel.Size = new Size(400, 590);
            cardPanel.Location = new Point(50, 42);
            cardPanel.BackColor = Color.White;
            cardPanel.Paint += CardPanel_Paint;

            // === Logo PictureBox ===
            logoPictureBox = new PictureBox();
            logoPictureBox.Size = new Size(320, 80);
            logoPictureBox.Location = new Point(40, 25);
            logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            logoPictureBox.BackColor = Color.Transparent;

            // === Branding Label ===
            brandingLabel = new Label();
            brandingLabel.Text = "Wildcat One";
            brandingLabel.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            brandingLabel.ForeColor = Color.FromArgb(122, 26, 61); // #7a1a3d
            brandingLabel.AutoSize = false;
            brandingLabel.Location = new Point(0, 115);
            brandingLabel.Size = new Size(400, 42);
            brandingLabel.TextAlign = ContentAlignment.MiddleCenter;

            // === Subtitle Label ===
            subtitleLabel = new Label();
            subtitleLabel.Text = "Student Portal";
            subtitleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            subtitleLabel.ForeColor = Color.FromArgb(52, 73, 94); // #34495e
            subtitleLabel.AutoSize = false;
            subtitleLabel.Location = new Point(0, 160);
            subtitleLabel.Size = new Size(400, 24);
            subtitleLabel.TextAlign = ContentAlignment.MiddleCenter;

            // === Description Label ===
            descriptionLabel = new Label();
            descriptionLabel.Text = "Virtus in Scientia et Tecnologia";
            descriptionLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Italic);
            descriptionLabel.ForeColor = Color.FromArgb(127, 140, 141); // #7f8c8d
            descriptionLabel.AutoSize = false;
            descriptionLabel.Location = new Point(0, 186);
            descriptionLabel.Size = new Size(400, 20);
            descriptionLabel.TextAlign = ContentAlignment.MiddleCenter;

            // === Error Panel (container with red left border) ===
            errorPanel = new Panel();
            errorPanel.Location = new Point(30, 218);
            errorPanel.Size = new Size(340, 0); // Hidden by default (height 0)
            errorPanel.BackColor = Color.FromArgb(255, 238, 238); // #fee
            errorPanel.Visible = false;
            errorPanel.Paint += ErrorPanel_Paint;

            // === Error Label ===
            errorLabel = new Label();
            errorLabel.Text = "";
            errorLabel.Font = new Font("Segoe UI", 9F);
            errorLabel.ForeColor = Color.FromArgb(231, 76, 60); // #e74c3c
            errorLabel.Location = new Point(14, 8);
            errorLabel.Size = new Size(316, 32);
            errorLabel.AutoSize = false;
            errorPanel.Controls.Add(errorLabel);

            // === Student ID Label ===
            studentIdLabel = new Label();
            studentIdLabel.Text = "Student ID";
            studentIdLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            studentIdLabel.ForeColor = Color.FromArgb(52, 73, 94); // #34495e
            studentIdLabel.Location = new Point(30, 228);
            studentIdLabel.AutoSize = true;

            // === Student ID TextBox ===
            studentIdTextBox = new TextBox();
            studentIdTextBox.Location = new Point(30, 252);
            studentIdTextBox.Size = new Size(340, 35);
            studentIdTextBox.Font = new Font("Segoe UI", 11F);
            studentIdTextBox.BackColor = Color.FromArgb(248, 249, 251); // #f8f9fb
            studentIdTextBox.BorderStyle = BorderStyle.FixedSingle;
            studentIdTextBox.PlaceholderText = "24-4339-705";
            studentIdTextBox.Text = "24-4339-705"; // DEV: pre-fill for convenience
            studentIdTextBox.TextChanged += Input_TextChanged;

            // === Password Label ===
            passwordLabel = new Label();
            passwordLabel.Text = "Password";
            passwordLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            passwordLabel.ForeColor = Color.FromArgb(52, 73, 94);
            passwordLabel.Location = new Point(30, 298);
            passwordLabel.AutoSize = true;

            // === Password TextBox ===
            passwordTextBox = new TextBox();
            passwordTextBox.Location = new Point(30, 322);
            passwordTextBox.Size = new Size(340, 35);
            passwordTextBox.Font = new Font("Segoe UI", 11F);
            passwordTextBox.BackColor = Color.FromArgb(248, 249, 251);
            passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
            passwordTextBox.UseSystemPasswordChar = true;
            passwordTextBox.PlaceholderText = "Enter your password";
            passwordTextBox.Text = "Atabotabo#9587"; // DEV: pre-fill for convenience
            passwordTextBox.TextChanged += Input_TextChanged;

            // === Show/Hide Password Button ===
            togglePasswordButton = new Button();
            togglePasswordButton.Text = "SHOW";
            togglePasswordButton.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            togglePasswordButton.ForeColor = Color.FromArgb(127, 140, 141);
            togglePasswordButton.FlatStyle = FlatStyle.Flat;
            togglePasswordButton.FlatAppearance.BorderSize = 0;
            togglePasswordButton.BackColor = Color.FromArgb(248, 249, 251);
            togglePasswordButton.Size = new Size(50, 28);
            togglePasswordButton.Location = new Point(316, 326);
            togglePasswordButton.Cursor = Cursors.Hand;
            togglePasswordButton.Click += TogglePasswordButton_Click;

            // === Birthdate Label (hidden by default) ===
            birthdateLabel = new Label();
            birthdateLabel.Text = "Birthdate";
            birthdateLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            birthdateLabel.ForeColor = Color.FromArgb(52, 73, 94);
            birthdateLabel.Location = new Point(30, 298);
            birthdateLabel.AutoSize = true;
            birthdateLabel.Visible = false;

            // === Birthdate DateTimePicker (hidden by default) ===
            birthdatePicker = new DateTimePicker();
            birthdatePicker.Location = new Point(30, 322);
            birthdatePicker.Size = new Size(340, 35);
            birthdatePicker.Font = new Font("Segoe UI", 11F);
            birthdatePicker.Format = DateTimePickerFormat.Long;
            birthdatePicker.MaxDate = DateTime.Today;
            birthdatePicker.Value = new DateTime(2000, 1, 1);
            birthdatePicker.Visible = false;
            birthdatePicker.ValueChanged += Input_TextChanged;

            // === Success Panel (green, hidden by default) ===
            successPanel = new Panel();
            successPanel.Location = new Point(30, 218);
            successPanel.Size = new Size(340, 0);
            successPanel.BackColor = Color.FromArgb(239, 255, 239); // #efffef
            successPanel.Visible = false;
            successPanel.Paint += SuccessPanel_Paint;

            // === Success Label ===
            successLabel = new Label();
            successLabel.Text = "";
            successLabel.Font = new Font("Segoe UI", 9F);
            successLabel.ForeColor = Color.FromArgb(39, 174, 96); // #27ae60
            successLabel.Location = new Point(14, 8);
            successLabel.Size = new Size(316, 32);
            successLabel.AutoSize = false;
            successPanel.Controls.Add(successLabel);

            // === Sign In Button ===
            signInButton = new Button();
            signInButton.Text = "Sign In";
            signInButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            signInButton.ForeColor = Color.White;
            signInButton.BackColor = Color.FromArgb(122, 26, 61); // #7a1a3d
            signInButton.FlatStyle = FlatStyle.Flat;
            signInButton.FlatAppearance.BorderSize = 0;
            signInButton.Size = new Size(340, 44);
            signInButton.Location = new Point(30, 380);
            signInButton.Cursor = Cursors.Hand;
            signInButton.Click += SignInButton_Click;

            // === Forgot Password LinkLabel ===
            forgotPasswordLink = new LinkLabel();
            forgotPasswordLink.Text = "Forgot Password?";
            forgotPasswordLink.Font = new Font("Segoe UI", 10F);
            forgotPasswordLink.LinkColor = Color.FromArgb(122, 26, 61);
            forgotPasswordLink.ActiveLinkColor = Color.FromArgb(93, 20, 48); // #5d1430
            forgotPasswordLink.Location = new Point(0, 436);
            forgotPasswordLink.Size = new Size(400, 22);
            forgotPasswordLink.AutoSize = false;
            forgotPasswordLink.TextAlign = ContentAlignment.MiddleCenter;
            forgotPasswordLink.LinkClicked += ForgotPasswordLink_LinkClicked;

            // Center logo
            logoPictureBox.Location = new Point((cardPanel.Width - logoPictureBox.Width) / 2, logoPictureBox.Location.Y);

            // Add controls to card
            cardPanel.Controls.Add(logoPictureBox);
            cardPanel.Controls.Add(brandingLabel);
            cardPanel.Controls.Add(subtitleLabel);
            cardPanel.Controls.Add(descriptionLabel);
            cardPanel.Controls.Add(errorPanel);
            cardPanel.Controls.Add(studentIdLabel);
            cardPanel.Controls.Add(studentIdTextBox);
            cardPanel.Controls.Add(passwordLabel);
            cardPanel.Controls.Add(passwordTextBox);
            cardPanel.Controls.Add(togglePasswordButton);
            cardPanel.Controls.Add(birthdateLabel);
            cardPanel.Controls.Add(birthdatePicker);
            cardPanel.Controls.Add(successPanel);
            cardPanel.Controls.Add(signInButton);
            cardPanel.Controls.Add(forgotPasswordLink);

            // Add panels to form
            Controls.Add(cardPanel);
            Controls.Add(shadowPanel);

            // Bring card in front of shadow
            cardPanel.BringToFront();

            // Accept button for Enter key
            AcceptButton = signInButton;
        }

        #endregion

        private Panel shadowPanel;
        private Panel cardPanel;
        private PictureBox logoPictureBox;
        private Label brandingLabel;
        private Label subtitleLabel;
        private Label descriptionLabel;
        private Panel errorPanel;
        private Label errorLabel;
        private Label studentIdLabel;
        private TextBox studentIdTextBox;
        private Label passwordLabel;
        private TextBox passwordTextBox;
        private Button togglePasswordButton;
        private Button signInButton;
        private Label birthdateLabel;
        private DateTimePicker birthdatePicker;
        private Panel successPanel;
        private Label successLabel;
        private LinkLabel forgotPasswordLink;
    }
}
