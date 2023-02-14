using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang
{
    public partial class UCLogin : UserControl
    {

        DBQuanLyBanHangDataContext db = new DBQuanLyBanHangDataContext();

        public UCLogin()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTaiKhoan.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Please enter username.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTaiKhoan.Focus();
                }
                else if (txtMatKhau.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Please enter password.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                }
                else
                {
                    TaiKhoan check = db.TaiKhoans.SingleOrDefault(n => n.TK.Equals(txtTaiKhoan.Text.Trim()) && n.MK.Equals(txtMatKhau.Text.Trim()) && n.Quyen.Equals(cbType.Text));
                    if (check == null)
                    {
                        MessageBox.Show("Username or password is invalid, please try again.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        if (cbType.Text.Equals("User"))
                        {
                            Model.maNV = check.MaNV;
                            check.TrangThai = "Online";
                            db.SubmitChanges();
                            MessageBox.Show("Login success.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ((Form)this.TopLevelControl).Hide();
                            FrmUser frmuser = new FrmUser();
                            frmuser.ShowDialog();
                        }
                        if (cbType.Text.Equals("Admin"))
                        {
                            check.TrangThai = "Online";
                            db.SubmitChanges();
                            Random _r = new Random();
                            var temp = _r.Next(0, 99999);
                            Model.maAM = txtTaiKhoan.Text + temp;
                            MessageBox.Show("Login success.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ((Form)this.TopLevelControl).Hide();
                            FrmUser frmuser = new FrmUser();
                            frmuser.ShowDialog();
                        }

                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void UCLogin_Load(object sender, EventArgs e)
        {
            if (checkboxShowPassword.Checked)
            {
                txtMatKhau.UseSystemPasswordChar = false;
            }
            else
            {
                txtMatKhau.UseSystemPasswordChar = true;
            }
            cbType.SelectedItem = "Admin";
        }

        private void checkboxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxShowPassword.Checked)
            {
                txtMatKhau.UseSystemPasswordChar = false;
            }
            else
            {
                txtMatKhau.UseSystemPasswordChar = true;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTaiKhoan_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
