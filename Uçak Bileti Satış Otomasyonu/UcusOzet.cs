using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uçak_Bileti_Satış_Otomasyonu
{
    public partial class UcusOzet : Form
    {
        public UcusOzet()
        {
            InitializeComponent();
        }

        #region Değişkenler/Fields (Alanlar)

        //Panel ile Form'u Sürüklemek için alttaki 2 field'ı tanımlamak zorundayız.
        private const int HT_CAPTION = 0x2;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        //---------------------------------------------------------------------------

        #endregion

        #region Panel ile Form'u Sürükleme

        private void pnlUst_MouseDown(object sender, MouseEventArgs e)
        {
            // Panel'e ya da Form'a gelen Mouse hareketlerini yakalama (Capture = false).
            (sender as Control).Capture = false;

            // Sanki başlık çubuğu (Caption Bar) üzerinde sol Mou butonu tıklaması
            // başlamış gibi yap: 1) önce sahte mesajı oluştur.

            Message msg = Message.Create(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);

            // 2) Sonra sahte mesajı uygulamanın WndProc() metoduna gönder.
            base.WndProc(ref msg);
        }


        #endregion

        private void UcusOzet_Load(object sender, EventArgs e)
        {

            #region Design

            this.BackColor = Color.FromArgb(35, 47, 63); //Siyah renk.

            lblBiletlendi.BackColor = Color.FromArgb(231, 72, 86); //Kırmızı renk.
            lblBagajHakkiAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblBagajHakki.BackColor = Color.FromArgb(35, 47, 63);
            lblOlusturmaTarihi.BackColor = Color.FromArgb(35, 47, 63);
            lblOlusturmaTarihiAciklama.BackColor = Color.FromArgb(35, 47, 63);

            lblGenelToplamAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblGenelToplam.BackColor = Color.FromArgb(35, 47, 63);

            lblYolcuSayisi.BackColor = Color.FromArgb(35, 47, 63);
            lblYolcuSayisiAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblAdiSoyadiAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblAdiSoyadi.BackColor = Color.FromArgb(35, 47, 63);
            lblDogumTarihiAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblDogumTarihi.BackColor = Color.FromArgb(35, 47, 63);
            lblBiletNoAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblBiletNo.BackColor = Color.FromArgb(35, 47, 63);

            lblBitisAciklama.BackColor = Color.FromArgb(35, 47, 63);


            #endregion

        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void btnAsagiIndir_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


    }
}
