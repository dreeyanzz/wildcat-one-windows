using wildcat_one_windows.Exceptions;
using wildcat_one_windows.Services;

namespace wildcat_one_windows
{
    public partial class LoginForm : Form
    {
        private bool _isForgotPasswordMode;

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

        // === Sign In ===

        private async void SignInButton_Click(object? sender, EventArgs e)
        {
            if (_isForgotPasswordMode)
            {
                await HandleForgotPassword();
                return;
            }

            await HandleLogin();
        }

        private async Task HandleLogin()
        {
            HideMessages();
            SetLoading(true);

            try
            {
                var result = await AuthService.LoginAsync(
                    studentIdTextBox.Text.Trim(),
                    passwordTextBox.Text
                );

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
                ShowError(
                    ex.StatusCode == 0
                        ? "Network error. Please check your internet connection."
                        : ex.Message
                );
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

        // === Forgot Password ===

        private async Task HandleForgotPassword()
        {
            HideMessages();
            SetLoading(true);

            try
            {
                var result = await AuthService.ForgotPasswordAsync(
                    studentIdTextBox.Text.Trim(),
                    birthdatePicker.Value
                );

                ShowSuccess(result.Message);
                studentIdTextBox.Text = "";
                birthdatePicker.Value = new DateTime(2000, 1, 1);
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
                ShowError(
                    ex.StatusCode == 0
                        ? "Network error. Please check your internet connection."
                        : ex.Message
                );
            }
            catch (Exception ex)
            {
                ShowError($"Password reset failed: {ex.Message}");
            }
            finally
            {
                SetLoading(false);
            }
        }

        // === Mode Switching ===

        private void ForgotPasswordLink_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            _isForgotPasswordMode = !_isForgotPasswordMode;
            HideMessages();
            ApplyMode();
        }

        private void ApplyMode()
        {
            if (_isForgotPasswordMode)
            {
                subtitleLabel.Text = "Reset Password";
                descriptionLabel.Text =
                    "Enter your Student ID and birthdate to reset your password";

                passwordLabel.Visible = false;
                passwordTextBox.Visible = false;
                togglePasswordButton.Visible = false;

                birthdateLabel.Visible = true;
                birthdatePicker.Visible = true;

                signInButton.Text = "Reset Password";
                forgotPasswordLink.Text = "Back to Login";
            }
            else
            {
                subtitleLabel.Text = "Student Portal";
                descriptionLabel.Text = "Virtus in Scientia et Tecnologia";

                passwordLabel.Visible = true;
                passwordTextBox.Visible = true;
                togglePasswordButton.Visible = true;

                birthdateLabel.Visible = false;
                birthdatePicker.Visible = false;

                signInButton.Text = "Sign In";
                forgotPasswordLink.Text = "Forgot Password?";
            }

            RepositionFormElements(false, 0);
        }

        // === Password Toggle ===

        private void TogglePasswordButton_Click(object? sender, EventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = !passwordTextBox.UseSystemPasswordChar;
            togglePasswordButton.Text = passwordTextBox.UseSystemPasswordChar ? "SHOW" : "HIDE";
        }

        // === Input Changed ===

        private void Input_TextChanged(object? sender, EventArgs e)
        {
            if (errorPanel.Visible || successPanel.Visible)
                HideMessages();
        }

        // === Error / Success Display ===

        private void ShowError(string message)
        {
            HideSuccess();
            errorLabel.Text = message;

            using var g = errorLabel.CreateGraphics();
            var textSize = g.MeasureString(message, errorLabel.Font, errorLabel.Width);
            var panelHeight = Math.Max(40, (int)textSize.Height + 20);

            errorPanel.Height = panelHeight;
            errorLabel.Height = panelHeight - 16;
            errorPanel.Visible = true;

            RepositionFormElements(true, panelHeight);
        }

        private void ShowSuccess(string message)
        {
            HideError();
            successLabel.Text = message;

            using var g = successLabel.CreateGraphics();
            var textSize = g.MeasureString(message, successLabel.Font, successLabel.Width);
            var panelHeight = Math.Max(40, (int)textSize.Height + 20);

            successPanel.Height = panelHeight;
            successLabel.Height = panelHeight - 16;
            successPanel.Visible = true;

            RepositionFormElements(true, panelHeight);
        }

        private void HideError()
        {
            errorPanel.Visible = false;
            errorPanel.Height = 0;
            errorLabel.Text = "";
        }

        private void HideSuccess()
        {
            successPanel.Visible = false;
            successPanel.Height = 0;
            successLabel.Text = "";
        }

        private void HideMessages()
        {
            HideError();
            HideSuccess();
            RepositionFormElements(false, 0);
        }

        // === Layout ===

        private void RepositionFormElements(bool messageVisible, int messageHeight)
        {
            var baseY = messageVisible ? 218 + messageHeight + 10 : 228;

            studentIdLabel.Location = new Point(30, baseY);
            studentIdTextBox.Location = new Point(30, baseY + 24);

            var secondFieldY = baseY + 70;
            passwordLabel.Location = new Point(30, secondFieldY);
            passwordTextBox.Location = new Point(30, secondFieldY + 24);
            togglePasswordButton.Location = new Point(316, secondFieldY + 28);
            birthdateLabel.Location = new Point(30, secondFieldY);
            birthdatePicker.Location = new Point(30, secondFieldY + 24);

            signInButton.Location = new Point(30, secondFieldY + 82);
            forgotPasswordLink.Location = new Point(0, secondFieldY + 138);
        }

        // === Loading State ===

        private void SetLoading(bool loading)
        {
            signInButton.Enabled = !loading;
            studentIdTextBox.Enabled = !loading;
            forgotPasswordLink.Enabled = !loading;
            Cursor = loading ? Cursors.WaitCursor : Cursors.Default;

            if (_isForgotPasswordMode)
            {
                signInButton.Text = loading ? "Sending..." : "Reset Password";
                birthdatePicker.Enabled = !loading;
            }
            else
            {
                signInButton.Text = loading ? "Authenticating..." : "Sign In";
                passwordTextBox.Enabled = !loading;
                togglePasswordButton.Enabled = !loading;
            }
        }

        // === Paint Events ===

        private void CardPanel_Paint(object? sender, PaintEventArgs e)
        {
            using var brush = new SolidBrush(Color.FromArgb(122, 26, 61));
            e.Graphics.FillRectangle(brush, 0, 0, cardPanel.Width, 5);
        }

        private void ErrorPanel_Paint(object? sender, PaintEventArgs e)
        {
            using var brush = new SolidBrush(Color.FromArgb(231, 76, 60));
            e.Graphics.FillRectangle(brush, 0, 0, 4, errorPanel.Height);
        }

        private void SuccessPanel_Paint(object? sender, PaintEventArgs e)
        {
            using var brush = new SolidBrush(Color.FromArgb(39, 174, 96));
            e.Graphics.FillRectangle(brush, 0, 0, 4, successPanel.Height);
        }
    }
}
