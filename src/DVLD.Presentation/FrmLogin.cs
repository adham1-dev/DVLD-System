using DVLD.Business.DTOs;
using DVLD.Business.Services;
using DVLD.Presentation.Generic;
using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DVLD.Presentation
{
    public partial class FrmLogin : Form
    {
        private readonly LoginValidatinService _service = new LoginValidatinService();
        private const string REGISTRY_PATH = @"Software\AdhamProjects\DVLD";

        private static readonly byte[] Key = Encoding.UTF8.GetBytes("DVLD_SecretKey12");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("DVLD_InitVector1");

        public FrmLogin()
        {
            InitializeComponent();
            GetRemmberMeInfo();
        }

        private void GetRemmberMeInfo()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH))
            {
                if (key != null)
                {
                    string username = key.GetValue("Username")?.ToString() ?? "";
                    string encryptedPass = key.GetValue("Password")?.ToString() ?? "";

                    if (!string.IsNullOrEmpty(username))
                    {
                        txtUsername.Text = username;
                        txtpass.Text = UnprotectString(encryptedPass);
                        cbRemmberMe.Checked = true;
                    }
                }
            }
        }

        private void SaveRememberMeInfo()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(REGISTRY_PATH))
            {
                key?.SetValue("Username", txtUsername.Text.Trim());
                key?.SetValue("Password", ProtectString(txtpass.Text));
            }
        }

        private void ClearRegistry()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
            {
                if (key != null)
                {
                    key.DeleteValue("Username", false);
                    key.DeleteValue("Password", false);
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            UIHelper.Execute(() =>
            {
                _service.ValidateLoginInfo(txtUsername.Text, txtpass.Text, out int UserID);

                var CurrentUser = new UserDTO
                {
                    ID = UserID,
                    UserName = txtUsername.Text,
                    Password = txtpass.Text,
                };

                if (cbRemmberMe.Checked)
                {
                    SaveRememberMeInfo();
                }
                else
                {
                    ClearRegistry();
                }

                var MainForm = new MainForm(CurrentUser);
                this.Hide();
                MainForm.ShowDialog();
                this.Close();
            });
        }

        #region AES Encryption Helpers
        private string ProtectString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return "";

            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        private string UnprotectString(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return "";

            try
            {
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream(buffer))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }
}