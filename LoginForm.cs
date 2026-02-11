using wildcat_one_windows.Exceptions;
using wildcat_one_windows.Services;

namespace wildcat_one_windows
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            LoadLogo();
        }

        private void LoadLogo()
        {
            var logoPath = Path.Combine(AppContext.BaseDirectory, "Assets", "cit-logo.png");
            if (File.Exists(logoPath))
                logoPictureBox.Image = Image.FromFile(logoPath);
        }

        private async void SignInButton_Click(object? sender, EventArgs e)
        {
            HideError();
            SetLoading(true);

            try
            {
                var result = await AuthService.LoginAsync(
                    studentIdTextBox.Text.Trim(),
                    passwordTextBox.Text);

                // Success — open home form
                var home = new Form1();
                home.Show();
                home.FormClosed += (_, _) => Close();
                Hide();
            }
            catch (ValidationException ex)
            {
                ShowError(ex.Message);
            }
            catch (AuthenticationException ex)
            {
                ShowError(ex.Message);
            }
            catch (ApiException ex)
            {
                ShowError(ex.StatusCode == 0
                    ? "Network error. Please check your internet connection."
                    : ex.Message);
            }
            catch (Exception ex)
            {
                ShowError($"Login failed: {ex.Message}");
            }
            finally
            {
                SetLoading(false);
            }
        }

        private void ForgotPasswordLink_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(
                "Forgot Password functionality is coming soon.\nPlease contact your administrator for assistance.",
                "Coming Soon",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void TogglePasswordButton_Click(object? sender, EventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = !passwordTextBox.UseSystemPasswordChar;
            togglePasswordButton.Text = passwordTextBox.UseSystemPasswordChar ? "SHOW" : "HIDE";
        }

        private void Input_TextChanged(object? sender, EventArgs e)
        {
            if (errorPanel.Visible)
                HideError();
        }

        private void ShowError(string message)
        {
            errorLabel.Text = message;

            // Measure text height and resize panel
            using var g = errorLabel.CreateGraphics();
            var textSize = g.MeasureString(message, errorLabel.Font, errorLabel.Width);
            var panelHeight = Math.Max(40, (int)textSize.Height + 20);

            errorPanel.Height = panelHeight;
            errorLabel.Height = panelHeight - 16;
            errorPanel.Visible = true;

            // Shift form elements below error panel
            RepositionFormElements(true, panelHeight);
        }

        private void HideError()
        {
            errorPanel.Visible = false;
            errorPanel.Height = 0;
            errorLabel.Text = "";

            RepositionFormElements(false, 0);
        }

        private void RepositionFormElements(bool errorVisible, int errorHeight)
        {
            var baseY = errorVisible ? 218 + errorHeight + 10 : 228;

            studentIdLabel.Location = new Point(studentIdLabel.Location.X, baseY);
            studentIdTextBox.Location = new Point(studentIdTextBox.Location.X, baseY + 24);

            passwordLabel.Location = new Point(passwordLabel.Location.X, baseY + 70);
            passwordTextBox.Location = new Point(passwordTextBox.Location.X, baseY + 94);
            togglePasswordButton.Location = new Point(togglePasswordButton.Location.X, baseY + 98);

            signInButton.Location = new Point(signInButton.Location.X, baseY + 152);
            forgotPasswordLink.Location = new Point(forgotPasswordLink.Location.X, baseY + 208);
        }

        private void SetLoading(bool loading)
        {
            signInButton.Enabled = !loading;
            signInButton.Text = loading ? "Authenticating..." : "Sign In";
            studentIdTextBox.Enabled = !loading;
            passwordTextBox.Enabled = !loading;
            togglePasswordButton.Enabled = !loading;
            Cursor = loading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void CardPanel_Paint(object? sender, PaintEventArgs e)
        {
            // Draw 5px maroon top border
            using var brush = new SolidBrush(Color.FromArgb(122, 26, 61));
            e.Graphics.FillRectangle(brush, 0, 0, cardPanel.Width, 5);
        }

        private void ErrorPanel_Paint(object? sender, PaintEventArgs e)
        {
            // Draw 4px red left border
            using var brush = new SolidBrush(Color.FromArgb(231, 76, 60));
            e.Graphics.FillRectangle(brush, 0, 0, 4, errorPanel.Height);
        }
    }
}
