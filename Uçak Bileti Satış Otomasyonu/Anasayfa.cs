using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uçak_Bileti_Satış_Otomasyonu
{
    public partial class Anasayfa : Form
    {
        public Anasayfa()
        {
            InitializeComponent();
        }


        #region Değişkenler/Fields (Alanlar)

        //Panel ile Form'u Sürüklemek için alttaki 2 field'ı tanımlamak zorundayız.
        private const int HT_CAPTION = 0x2;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        //---------------------------------------------------------------------------

        DateTime dtGidisTarihi,dtDonusTarihi;

        UcusListeleri ul;

        byte acilmaSuresi = 0;

        bool durum = true;

        static public bool aktarmasiz, ekonomi, business;

        public bool gidisDonus, tekYon, gidisDonus2, tekYon2;

        public bool d_ekonomi, d_business;

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


        private void Anasayfa_Load(object sender, EventArgs e)
        {
       
            #region Anasayfa Düzen İşlemleri

            this.BackColor = Color.FromArgb(35, 47, 63);

            txtGidisNoktasi.BackColor = Color.FromArgb(35, 47, 63);
            txtInisNoktasi.BackColor = Color.FromArgb(35, 47, 63);
            txtGidisTarihi.BackColor = Color.FromArgb(35, 47, 63);
            txtDonusTarihi.BackColor = Color.FromArgb(35, 47, 63);

            iTalk_ProgressIndicatorUcusListeleriBekleniyor.P_BaseColor = Color.FromArgb(231, 72, 86);
            iTalk_ProgressIndicatorUcusListeleriBekleniyor.P_AnimationColor = Color.FromArgb(35, 47, 63);


            iTalk_SeparatorKalkis.BringToFront();
            iTalk_Separatorİnis.BringToFront();
            iTalk_SeparatorGidisTarihi.BringToFront();
            iTalk_SeparatorDonusTarihi.BringToFront();
            iTalk_ProgressIndicatorUcusListeleriBekleniyor.BringToFront();

            #endregion
        
            if(iTalk_RadioButtonGidisDonus.Checked)
            {
                gidisDonus = true;
                gidisDonus2 = true;

                tekYon = false;
                tekYon2 = false;
            }
                
            txtGidisTarihi.Text = DateTime.Today.ToShortDateString();
            txtDonusTarihi.Text = DateTime.Today.AddDays(1).ToShortDateString();

        }

        private void iTalk_RadioButtonTekYon_Click(object sender, EventArgs e)
        {
            tekYon = true;
            tekYon2 = true;

            gidisDonus = false;
            gidisDonus2 = false;
        }

        private void iTalk_RadioButtonGidisDonus_Click(object sender, EventArgs e)
        {
            gidisDonus = true;
            gidisDonus2 = true;

            tekYon = false;
            tekYon2 = false;
        }


        #region Gidiş ve İniş Noktalarını Karşılıklı Değiştirme

        /* 'picDegistir' adındaki pictureBox'a tıklanınca aynı anda karşılıklı olarak textBox'larda
            gidiş ve iniş noktalarını değiştirmek için Thread yapılarını kullandım.
       */

        void txtGidisNoktasiKarsilikliDegistirme()
        {
            txtGidisNoktasi.Text = txtInisNoktasi.Text;
        }

        void txtInisNoktasiKarsilikliDegistirme()
        {
            txtInisNoktasi.Text = txtGidisNoktasi.Text;
        }

        private void picDegistir_Click(object sender, EventArgs e)
        {
            Thread thr1 = new Thread(new ThreadStart(txtGidisNoktasiKarsilikliDegistirme));
            Thread thr2 = new Thread(new ThreadStart(txtInisNoktasiKarsilikliDegistirme));

            CheckForIllegalCrossThreadCalls = false;

            thr1.Start();
            thr2.Start();
        }

        #endregion

        #region textBox'ların Klavye Tuş Bastırmama Durumlarını Kontrol Etme

        private void txtGidisNoktasi_KeyPress(object sender, KeyPressEventArgs e)
        {

            if ((e.KeyChar >= 33) && (e.KeyChar <= 64))
                e.Handled = true; //Klavyeden eğer sayı tuşlarına basılırsa bunları yazdırma.
            else if ((e.KeyChar >= 91) && (e.KeyChar <= 96))
                e.Handled = true;
            else if ((e.KeyChar >= 123) && (e.KeyChar <= 127))
                e.Handled = true;
            else
                e.Handled = false; //Yukarıdaki ASCII kodlarına karşılık gelen tuşların haricinde olan bütün tuşların basılmasını izin ver.

        }

        private void txtInisNoktasi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 33) && (e.KeyChar <= 64))
                e.Handled = true; //Klavyeden eğer sayı tuşlarına basılırsa bunları yazdırma.
            else if ((e.KeyChar >= 91) && (e.KeyChar <= 96))
                e.Handled = true;
            else if ((e.KeyChar >= 123) && (e.KeyChar <= 127))
                e.Handled = true;
            else
                e.Handled = false; //Yukarıdaki ASCII kodlarına karşılık gelen tuşların haricinde olan bütün tuşların basılmasını izin ver.
        }

        private void txtGidisTarihi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= 32) && (e.KeyChar <= 45)) || (e.KeyChar == 47))
                e.Handled = true; //Nokta hariç. O yüzdenn ASCII kodu 46 olan nokta (.) tuşunu yukarıda şarta alınmadı. Böylece klavyeden basılması sağlandı.
            else if ((e.KeyChar >= 58) && (e.KeyChar <= 127))
                e.Handled = true;
            else
                e.Handled = false; //Yukarıdaki ASCII 'lere karşılık gelen tuşların klavyeden basılmasını engelle, sadece sayıları ve nokta (.) tuşlarının basılmasını sağla.

        }

        private void txtDonusTarihi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= 32) && (e.KeyChar <= 45)) || (e.KeyChar == 47))
                e.Handled = true; //Nokta hariç. O yüzdenn ASCII kodu 46 olan nokta (.) tuşunu yukarıda şarta alınmadı. Böylece klavyeden basılması sağlandı.
            else if ((e.KeyChar >= 58) && (e.KeyChar <= 127))
                e.Handled = true;
            else if (iTalk_RadioButtonGidisDonus.Checked) //"Gidiş Dönüş" seçilirse Dönüş Tarihine klavyeden herhangi bir tuşa basılsın.
                e.Handled = false;
            else if (iTalk_RadioButtonTekYon.Checked) //"Tek Yön" seçilirse Dönüş Tarihine klavyeden herhangi bir tuşa basılmasın.
                e.Handled = true;
            else
                e.Handled = false; //Yukarıdaki ASCII 'lere karşılık gelen tuşların klavyeden basılmasını engelle, sadece sayıları ve nokta (.) tuşlarının basılmasını sağla.
        }

        #endregion

        #region checkBox'ları Kontrol Etme

        private void ambiance_CheckBoxEkonomi_Click(object sender, EventArgs e)
        {
            if (ambiance_CheckBoxBusiness.Checked == true)
            {
                ambiance_CheckBoxBusiness.Checked = false;
                ambiance_CheckBoxEkonomi.Checked = true;
            }
        }

        private void ambiance_CheckBoxBusiness_Click(object sender, EventArgs e)
        {
            if (ambiance_CheckBoxEkonomi.Checked == true)
            {
                ambiance_CheckBoxEkonomi.Checked = false;
                ambiance_CheckBoxBusiness.Checked = true;
            }
        }

        #endregion


        #region 'UçuşListeleri' Formunu Açmak

        void sartlarinSaglanmamasi()
        {

            #region txtGidis ve txtInis Kontrol
          
            if (txtGidisNoktasi.AutoCompleteCustomSource.Contains(txtGidisNoktasi.Text) == false)
            {
                durum = false;
                errorProvTxtGidisNoktasi.SetError(txtGidisNoktasi, "Gidiş konumu boş veya yanlış girilemez.");
            }

            if (txtInisNoktasi.AutoCompleteCustomSource.Contains(txtInisNoktasi.Text) == false)
            {
                durum = false;
                errorProvTxtInisNoktasi.SetError(txtInisNoktasi, "İniş konumu boş veya yanlış girilemez.");
            }

            if ((txtGidisNoktasi.AutoCompleteCustomSource.Contains(txtGidisNoktasi.Text) == true) && (txtInisNoktasi.AutoCompleteCustomSource.Contains(txtInisNoktasi.Text) == true))
            {
                if (txtGidisNoktasi.Text == txtInisNoktasi.Text)
                {
                    errorProvTxtGidisNoktasi.SetError(txtGidisNoktasi, "Gidiş-İniş konumu aynı olamaz.");
                    errorProvTxtInisNoktasi.SetError(txtInisNoktasi, "Gidiş-İniş konumu aynı olamaz.");

                    durum = false;
                }
            }

            #endregion


            #region txtGidisTarihi ve txtDonusTarihi Kontrol

            if (iTalk_RadioButtonGidisDonus.Checked)
            {
                if (DateTime.Compare(dtGidisTarihi, DateTime.Today) == -1)
                {
                    durum = false;
                    errorProvTxtGidisTarihi.SetError(txtGidisTarihi, "Gidiş tarihine geçmiş bir tarih girilemez.");
                }

                else if (DateTime.Compare(dtDonusTarihi, DateTime.Today) == -1)
                {
                    durum = false;
                    errorProvTxtDonusTarihi.SetError(txtDonusTarihi, "Dönüş tarihine geçmiş bir tarih girilemez.");
                }

                else if((DateTime.Compare(dtGidisTarihi, dtDonusTarihi) == 1) && (DateTime.Compare(dtDonusTarihi, DateTime.Today.AddDays(-1)) == 1))
                {
                    durum = false;
                    errorProvTxtGidisTarihi.SetError(txtGidisTarihi, "Gidiş tarihi Dönüş tarihinden büyük olamaz.");
                }
            }

            else if (iTalk_RadioButtonTekYon.Checked)
            {
                if (DateTime.Compare(dtGidisTarihi, DateTime.Today) == -1)
                {
                    durum = false;
                    errorProvTxtGidisTarihi.SetError(txtGidisTarihi, "Gidiş tarihine geçmiş bir tarih girilemez.");
                }
            }

            #endregion


            #region NumericUpDownYetiskin ve NumericUpDownCocuk Kontrol

            if (ambiance_NumericUpDownYetiskin.Value == 0)
            {
                durum = false;
                errorProvNumericUpDownYetiskin.SetError(ambiance_NumericUpDownYetiskin, "Yetişkin yolcu sayısını belirtmeniz gerek.");
            }

            if (ambiance_NumericUpDownCocuk.Value > 2)
            {
                durum = false;
                errorProvNumericUpDownCocuk.SetError(ambiance_NumericUpDownCocuk, "Çocuk yolcu sayısı 2'den fazla olamaz.");
            }

            #endregion


            #region ambianceCheckBoxEkonomi ve ambianceCheckBoxBusiness Kontrol

            if ((ambiance_CheckBoxEkonomi.Checked == false) && (ambiance_CheckBoxBusiness.Checked == false))
            {
                durum = false;

                errorProvAmbChckBxBusiness.SetError(ambiance_CheckBoxBusiness, "Bilet türünüzü seçmek zorundasınız.");
            }

            #endregion

        }

        void sartlarinSaglanmasi()
        {

            #region txtGidis ve txtInis'lerin errorProvider 'lerini Temizleme

            if ((txtGidisNoktasi.AutoCompleteCustomSource.Contains(txtGidisNoktasi.Text) == true) && (txtGidisNoktasi.Text != txtInisNoktasi.Text))
            {
                if (txtInisNoktasi.Text != "")
                {
                    errorProvTxtGidisNoktasi.Clear();

                    durum = true;
                }
            }

            if ((txtInisNoktasi.AutoCompleteCustomSource.Contains(txtInisNoktasi.Text) == true) && (txtGidisNoktasi.Text != txtInisNoktasi.Text))
            {
                if (txtGidisNoktasi.Text != "")
                {
                    durum = true;
                    errorProvTxtInisNoktasi.Clear();

                }
            }

            if ((txtGidisNoktasi.AutoCompleteCustomSource.Contains(txtGidisNoktasi.Text) == true) && (txtInisNoktasi.AutoCompleteCustomSource.Contains(txtInisNoktasi.Text) == true))
            {
                if (txtGidisNoktasi.Text != txtInisNoktasi.Text)
                {
                    errorProvTxtGidisNoktasi.Clear();
                    errorProvTxtInisNoktasi.Clear();

                    durum = true;
                }
            }

            #endregion


            #region txtGidisTarihi ve txtDonusTarihi 'lerin errorProvider'lerini Temizleme

            if ((DateTime.Compare(dtGidisTarihi, DateTime.Today) != -1) && (DateTime.Compare(dtDonusTarihi, DateTime.Today) != -1))
            {
                if (DateTime.Compare(dtGidisTarihi, dtDonusTarihi) != 1)
                {
                    errorProvTxtGidisTarihi.Clear();
                    errorProvTxtDonusTarihi.Clear();

                    durum = true;
                }
            }

            #endregion


            #region NumericUpDownYetiskin ve NumericUpDownCocuk' ların errorProvider'lerini Temizleme

            if (ambiance_NumericUpDownYetiskin.Value != 0)
            {
                durum = true;
                errorProvNumericUpDownYetiskin.Clear();
            }

            if (!(ambiance_NumericUpDownCocuk.Value > 2))
            {
                durum = true;
                errorProvNumericUpDownCocuk.Clear();
            }

            #endregion


            #region ambianceCheckBoxEkonomi ve ambianceCheckBoxBusiness' lerin errorProvider'lerini Temizleme

            if ((ambiance_CheckBoxEkonomi.Checked == true) || (ambiance_CheckBoxBusiness.Checked == true))
            {
                durum = true;

                errorProvAmbChckBxBusiness.Clear();
            }


            #endregion


            #region UcusListeleri adlı Form'un Açılışını Sağlama

            if (iTalk_RadioButtonGidisDonus.Checked)
            {
                if(acilmaSuresi == 6)
                {
                    if (((txtGidisNoktasi.AutoCompleteCustomSource.Contains(txtGidisNoktasi.Text) == true) && (txtInisNoktasi.AutoCompleteCustomSource.Contains(txtInisNoktasi.Text) == true))
                          && ((txtGidisNoktasi.Text != txtInisNoktasi.Text) && ((txtGidisNoktasi.Text != "") && (txtInisNoktasi.Text != "")))
                       )
                    {
                        if ((DateTime.Compare(dtGidisTarihi, DateTime.Today) != -1) && (DateTime.Compare(dtDonusTarihi, DateTime.Today) != -1) && (DateTime.Compare(dtGidisTarihi, dtDonusTarihi) != 1))
                        {
                            if ((ambiance_NumericUpDownYetiskin.Value != 0) && (!(ambiance_NumericUpDownCocuk.Value > 2)))
                            {
                                if ((ambiance_CheckBoxEkonomi.Checked == true) || (ambiance_CheckBoxBusiness.Checked == true))
                                {
                                    timerUcusListeleriBekleniyor.Stop();

                                    ul = new UcusListeleri();
                                    ul.Show();
                                }
                            }
                        }
                    }
                }
            }

            else if(iTalk_RadioButtonTekYon.Checked)
            {
                if (acilmaSuresi == 6)
                {
                    if (((txtGidisNoktasi.AutoCompleteCustomSource.Contains(txtGidisNoktasi.Text) == true) && (txtInisNoktasi.AutoCompleteCustomSource.Contains(txtInisNoktasi.Text) == true))
                          && ((txtGidisNoktasi.Text != txtInisNoktasi.Text) && ((txtGidisNoktasi.Text != "") && (txtInisNoktasi.Text != "")))
                       )
                    {
                        if ((DateTime.Compare(dtGidisTarihi, DateTime.Today) != -1))
                        {
                            if ((ambiance_NumericUpDownYetiskin.Value != 0) && (!(ambiance_NumericUpDownCocuk.Value > 2)))
                            {
                                if ((ambiance_CheckBoxEkonomi.Checked == true) || (ambiance_CheckBoxBusiness.Checked == true))
                                {
                                    timerUcusListeleriBekleniyor.Stop();

                                    ul = new UcusListeleri();
                                    ul.Show();
                                }
                            }
                        }
                    }
                }
            }

            #endregion

        }

        private void iTalk_RadioButtonGidisDonus_CheckedChanged(object sender)
        {
            txtDonusTarihi.ForeColor = Color.Gainsboro;
        }

        private void iTalk_RadioButtonTekYon_CheckedChanged(object sender)
        {
            txtDonusTarihi.ForeColor = Color.FromArgb(231, 72, 86);
        }

        private void ambiance_ButtonUcusAra_Click(object sender, EventArgs e)
        {
            try
            {
                dtGidisTarihi = Convert.ToDateTime(txtGidisTarihi.Text);
                dtDonusTarihi = Convert.ToDateTime(txtDonusTarihi.Text);

                if ((((txtGidisNoktasi.Text.Contains(txtGidisNoktasi.AutoCompleteCustomSource[19]) == false)) && ((txtGidisNoktasi.Text.Contains(txtGidisNoktasi.AutoCompleteCustomSource[20]) == false))) || (((txtInisNoktasi.Text.Contains(txtInisNoktasi.AutoCompleteCustomSource[19]) == false)) && ((txtInisNoktasi.Text.Contains(txtInisNoktasi.AutoCompleteCustomSource[20]) == false))))
                {
                    sartlarinSaglanmamasi();

                    if (durum == false)
                        durum = true;
                    else
                    {
                        timerUcusListeleriBekleniyor.Start();

                        iTalk_ProgressIndicatorUcusListeleriBekleniyor.Visible = true;

                        aktarmasiz = ambiance_CheckBoxAktarmasiz.Checked;
                        ekonomi = ambiance_CheckBoxEkonomi.Checked;
                        business = ambiance_CheckBoxBusiness.Checked;

                        SoundPlayer sP = new SoundPlayer();
                        sP.SoundLocation = Application.StartupPath + "\\Plane Flying - Sound Effect.wav";

                        sP.Play();


                    }
                }

                if (ambiance_CheckBoxEkonomi.Checked)
                    d_ekonomi = true;
                else if (ambiance_CheckBoxBusiness.Checked)
                    d_business = true;
            }

            catch (FormatException)
            {
                MessageBox.Show("Geçerli bir tarih formatı giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void timerUcusListeleriBekleniyor_Tick(object sender, EventArgs e)
        {

            ++acilmaSuresi;

            sartlarinSaglanmasi();

        }


        #endregion


        private void btnKapat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAsagiIndir_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

    }
}
