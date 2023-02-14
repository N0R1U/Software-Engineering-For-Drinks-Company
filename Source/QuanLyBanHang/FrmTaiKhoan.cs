using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang
{
    public partial class FrmTaiKhoan : Form
    {
        DBQuanLyBanHangDataContext db = new DBQuanLyBanHangDataContext();
        public FrmTaiKhoan()
        {
            InitializeComponent();
        }

        private void LoadDataTaiKhoan() {
            dataTaiKhoan.DataSource = from a in db.TaiKhoans select new {
                a.MaTK,
                a.TK,
                a.MK,
                a.MaNV,
                a.Quyen,
                a.TrangThai
            };
            cbQuyen.DropDownStyle = ComboBoxStyle.DropDownList;
            cbQuyen.SelectedItem = "User";
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = true;
            cbMaNV.Enabled = true;
            cbMaNV.SelectedIndex = -1;
        }


        private void LoadComboMaNV()
        {
            cbMaNV.DataSource = db.NhanViens.OrderBy(n => n.MaNV).Select(n => n.MaNV);
        }

        private void ClearTxT()
        {
            txtMaTK.Clear();
            txtTaiKhoan.Clear();
            txtMatKhau.Clear();
            txtTrangThai.Text = "Offline";
        }

        private void FrmTaiKhoan_Load(object sender, EventArgs e)
        {
            
            LoadDataTaiKhoan();
            LoadComboMaNV();
            cbMaNV.SelectedIndex = -1;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTaiKhoan.Text.Trim().Length.Equals(0) || txtMatKhau.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Please fullfill your information.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    TaiKhoan checkNV = db.TaiKhoans.SingleOrDefault(n => n.TK.Equals(txtTaiKhoan.Text.Trim()));
                    if (checkNV != null)
                    {
                        MessageBox.Show("This account is already exist.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTaiKhoan.Focus();
                    }
                    else
                    {
                        TaiKhoan checkMaNV = db.TaiKhoans.SingleOrDefault(n => n.MaNV.Equals(cbMaNV.Text.Trim()));
                        NhanVien checkTonTaiMa = db.NhanViens.SingleOrDefault(n => n.MaNV.Equals(cbMaNV.Text.Trim()));
                        if (checkMaNV != null)
                        {
                            MessageBox.Show(cbMaNV.Text.ToString() + "already have an account.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if (checkMaNV == null)
                        {
                            if (cbMaNV.Text.Equals("") && cbQuyen.Text.Equals("Admin"))
                            {
                                TaiKhoan tk = new TaiKhoan();
                                tk.TK = txtTaiKhoan.Text.Trim();
                                tk.MK = txtMatKhau.Text.Trim();
                                tk.MaNV = cbMaNV.Text.Trim().Equals("") ? null : cbMaNV.Text.Trim();
                                tk.Quyen = cbQuyen.Text.Trim();
                                tk.TrangThai = txtTrangThai.Text;
                                db.TaiKhoans.InsertOnSubmit(tk);
                                db.SubmitChanges();
                                MessageBox.Show("Added successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearTxT();
                                LoadDataTaiKhoan();
                            }
                            else
                            {
                                if (cbMaNV.Text.Trim().Equals("") && cbQuyen.Text.Trim().Equals("User"))
                                {
                                    MessageBox.Show("Please choose an user ID.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else if (checkTonTaiMa == null)
                                {
                                    MessageBox.Show(cbMaNV.Text.ToString() + "is not exist.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else if (cbMaNV.Text.Trim().Length.Equals(0) && cbQuyen.Text.Equals("User"))
                                {
                                    MessageBox.Show("Please choose an user ID.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else if (cbMaNV.Text.Trim().Length > 0 && cbQuyen.Text.Equals("Admin"))
                                {
                                    MessageBox.Show("Please choose correct privilege type.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    TaiKhoan tk = new TaiKhoan();
                                    tk.TK = txtTaiKhoan.Text.Trim();
                                    tk.MK = txtMatKhau.Text.Trim();
                                    tk.MaNV = cbMaNV.Text.Trim().Equals("") ? null : cbMaNV.Text.Trim();
                                    tk.Quyen = cbQuyen.Text.Trim();
                                    tk.TrangThai = txtTrangThai.Text;
                                    db.TaiKhoans.InsertOnSubmit(tk);
                                    db.SubmitChanges();
                                    MessageBox.Show("Added successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    ClearTxT();
                                    LoadDataTaiKhoan();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaTK.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Please choose an account to delete.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    TaiKhoan tk = db.TaiKhoans.SingleOrDefault(n => n.MaTK.Equals(txtMaTK.Text));
                    if (tk == null)
                    {
                        MessageBox.Show("Please choose an account to delete.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (tk.TrangThai == "Online")
                    {
                        MessageBox.Show("This account is currently online, cannot be deleted.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (MessageBox.Show("Are you sure? This action cannot be undone.", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            db.TaiKhoans.DeleteOnSubmit(tk);
                            db.SubmitChanges();
                            MessageBox.Show("Deleted successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataTaiKhoan();
                            ClearTxT();
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaTK.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Please choose an account for editing.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (txtTaiKhoan.Text.Trim().Length.Equals(0) || txtMatKhau.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Please fullfill your information.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    NhanVien checkTonTaiMa = db.NhanViens.SingleOrDefault(n => n.MaNV.Equals(cbMaNV.Text.Trim()));
                    if (checkTonTaiMa == null)
                    {
                        if (cbMaNV.Text.Equals("") && cbQuyen.Text.Equals("Admin"))
                        {
                            TaiKhoan tk = db.TaiKhoans.SingleOrDefault(n => n.MaTK.Equals(txtMaTK.Text.Trim()));
                            tk.TK = txtTaiKhoan.Text.Trim();
                            tk.MK = txtMatKhau.Text.Trim();
                            tk.MaNV = cbMaNV.Text.Trim().Equals("") ? null : cbMaNV.Text.Trim();
                            tk.Quyen = cbQuyen.Text.Trim();
                            db.SubmitChanges();
                            MessageBox.Show("Edited successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataTaiKhoan();
                            ClearTxT();
                        }
                        else
                        {
                            if (cbMaNV.Text.Trim().Equals("") && cbQuyen.Text.Trim().Equals("User"))
                            {
                                MessageBox.Show("Please choose an user ID for editing.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show(cbMaNV.Text.ToString() + "is not exist.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else if (cbMaNV.Text.Trim().Length.Equals(0) && cbQuyen.Text.Equals("User"))
                    {
                        MessageBox.Show("Please choose an user ID for editing.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (!btnThem.Enabled || cbMaNV.Enabled)
                    {
                        if (!cbMaNV.Enabled)
                        {
                            TaiKhoan tk = db.TaiKhoans.SingleOrDefault(n => n.MaTK.Equals(txtMaTK.Text.Trim()));
                            tk.TK = txtTaiKhoan.Text.Trim();
                            tk.MK = txtMatKhau.Text.Trim();
                            tk.MaNV = cbMaNV.Text.Trim();
                            tk.Quyen = cbQuyen.Text.Trim();
                            db.SubmitChanges();
                            MessageBox.Show("Edited successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataTaiKhoan();
                            ClearTxT();
                        }
                        else
                        {
                            TaiKhoan tk = db.TaiKhoans.SingleOrDefault(n => n.MaTK.Equals(txtMaTK.Text.Trim()));
                            tk.TK = txtTaiKhoan.Text.Trim();
                            tk.MK = txtMatKhau.Text.Trim();
                            tk.MaNV = cbMaNV.Text.Trim();
                            tk.Quyen = cbQuyen.Text.Trim();
                            db.SubmitChanges();
                            MessageBox.Show("Edited successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataTaiKhoan();
                            ClearTxT();
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void cbMaNV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            cbMaNV.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            txtTaiKhoan.Clear();
            txtMatKhau.Clear();
            txtMaTK.Clear();
            cbMaNV.SelectedIndex = -1;
            cbQuyen.SelectedIndex = 0;
            txtTrangThai.Text = "Offline";
        }

        private void cbQuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!txtMaTK.Text.Trim().Length.Equals(0))
                {
                    if (!btnThem.Enabled)
                    {
                        if (cbQuyen.Text.Equals("User"))
                        {
                            TaiKhoan tk = db.TaiKhoans.SingleOrDefault(n => n.MaTK == int.Parse(txtMaTK.Text.ToString()));
                            if (tk != null)
                            {
                                if (tk.MaNV == null)
                                {
                                    cbMaNV.Text = "";
                                    cbMaNV.Enabled = true;
                                }
                                else
                                {
                                    cbMaNV.Text = tk.MaNV.ToString();
                                    cbMaNV.Enabled = false;
                                }
                            }
                        }
                        else
                        {
                            cbMaNV.Enabled = false;
                            cbMaNV.Text = "";
                        }
                    }
                }
                else
                {
                    if (cbQuyen.Text.Equals("User"))
                    {
                        cbMaNV.Enabled = true;
                    }
                    else
                    {
                        cbMaNV.Enabled = false;
                        cbMaNV.Text = "";
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void iconPictureBox8_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void iconPictureBox7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void dataTaiKhoan_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataTaiKhoan.SelectedRows)
                {
                    txtMaTK.Text = dr.Cells[0].Value.ToString();
                    txtTaiKhoan.Text = dr.Cells[1].Value.ToString();
                    txtMatKhau.Text = dr.Cells[2].Value.ToString();
                    cbMaNV.Text = dr.Cells[3].Value == null ? "" : dr.Cells[3].Value.ToString();
                    cbQuyen.Text = dr.Cells[4].Value.ToString();
                    txtTrangThai.Text = dr.Cells[5].Value.ToString();
                }
                btnThem.Enabled = false;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                cbMaNV.Enabled = false;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void txtTrangThai_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
