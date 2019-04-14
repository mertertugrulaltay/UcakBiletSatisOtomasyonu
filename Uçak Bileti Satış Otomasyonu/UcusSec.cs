using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uçak_Bileti_Satış_Otomasyonu
{
    public partial class UcusSec : Form
    {
        public UcusSec()
        {
            InitializeComponent();
        }

        #region Değişkenler/Fields (Alanlar)

        //Panel ile Form'u Sürüklemek için alttaki 2 field'ı tanımlamak zorundayız.
        private const int HT_CAPTION = 0x2;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        //---------------------------------------------------------------------------

        Anasayfa anasayfa = (Anasayfa)Application.OpenForms["Anasayfa"];
        UcusListeleri uS = (UcusListeleri)Application.OpenForms["UcusListeleri"];

        OdemeBilgileri oB = new OdemeBilgileri();

        Random rnd = new Random();

        public bool b_EPosta, b_CepTelGeriyeKalan, b_Ad, b_Soyad, b_TCKimlikNo, b_DogumTarihi = true, b_radioButtonslar;

        public string tabloIsmi;

        public string yediYirmi, dokuzOtuzBes, onBirOtuz, onIkiOnBes, onUcSifirBes, onUcKirk, onDortYirmi, onDortElli, onBesOn;
        public string onBesKirkBes, onAltiKirk, onYediOnBes, onSekizOtuz, onDokuzSifirBes, yirmiOtuz, yirmiBirKirk, yirmiIkiElli, yirmiUcKirkBes;


        SQLiteConnection sqlConn = null;
        SQLiteCommand sqlComm = null;

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


        private void UcusSec_Load(object sender, EventArgs e)
        {

            #region Design

            this.BackColor = Color.FromArgb(35, 47, 63); //Siyah renk.


            picKirmiziCizgi1.BackColor = Color.FromArgb(231, 72, 86); //Kırmızı renk.

            picBoxKirmiziCizgi2.BackColor = Color.FromArgb(231, 72, 86);
            txtEPosta.BackColor = Color.FromArgb(35, 47, 63);
            txtCepTelArti90.BackColor = Color.FromArgb(35, 47, 63);
            txtCepTelGeriKalan.BackColor = Color.FromArgb(35, 47, 63);
       
            picBoxKirmiziCizgi3.BackColor = Color.FromArgb(231, 72, 86);
            txtAd.BackColor = Color.FromArgb(35, 47, 63);
            txtSoyad.BackColor = Color.FromArgb(35, 47, 63);
            txtTCKimlikNo.BackColor = Color.FromArgb(35, 47, 63);
            txtDogumTarihi.BackColor = Color.FromArgb(35, 47, 63);


            picBoxKirmiziCizgi4.BackColor = Color.FromArgb(231, 72, 86);
            picBoxSaat.BackColor = Color.FromArgb(35, 47, 63);
            lblUcusSuresiAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblDurumAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblGidisFirma.BackColor = Color.FromArgb(35, 47, 63);
            lblGidisSinifi.BackColor = Color.FromArgb(35, 47, 63);
            lblKalkisBaslik.BackColor = Color.FromArgb(35, 47, 63);
            lblKalkisKonum.BackColor = Color.FromArgb(35, 47, 63);
            lblVarisBaslik.BackColor = Color.FromArgb(35, 47, 63);
            lblVarisKonum.BackColor = Color.FromArgb(35, 47, 63);
            lblOnrezervasyonAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblBiletAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblBiletAciklama.Text = "Bu bilet özel fiyatlı bir promosyon bilettir. Bu nedenle\nbilette değişiklik yapılamaz, bilet iptal edilemez.";



            picBoxKirmiziCizgi5.BackColor = Color.FromArgb(231, 72, 86);
            picGidisGosterim.BackColor = Color.FromArgb(35, 47, 63);
            lblGidis.BackColor = Color.FromArgb(35, 47, 63);
            lblVaris.BackColor = Color.FromArgb(35, 47, 63);
            lblYonAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblYolcuAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblVergiYakitAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblHizmetAciklama.BackColor = Color.FromArgb(35, 47, 63);

            pnlToplamAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblToplamFiyatAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblOzetToplamFiyat.BackColor = Color.FromArgb(35, 47, 63);


            lblYolcuFiyat.BackColor = Color.FromArgb(35, 47, 63);
            lblVergiYakitFiyat.BackColor = Color.FromArgb(35, 47, 63);
            lblHizmetFiyat.BackColor = Color.FromArgb(35, 47, 63);

            #endregion

            #region Bilet Fiyatları Gösterim

            if (uS.yediYirmi2)
            {
                if(Anasayfa.ekonomi)
                {
                    if(uS.lblFiyat7nokta20.Text == "124,99 TL")
                    {
                        //104,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (104.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(124.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(124.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(124.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (104.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(124.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(124.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(124.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (104.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(124.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(124.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(124.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (104.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(124.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(124.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(124.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (104.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(124.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(124.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(124.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (104.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(124.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(124.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(124.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "07:20" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";

                    }
                    else if (uS.lblFiyat7nokta20.Text == "74,99 TL")
                    {
                        //54,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (54.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(74.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(74.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(74.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (54.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(74.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(74.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(74.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (54.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(74.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(74.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(74.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (54.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(74.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(74.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(74.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (54.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(74.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(74.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(74.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (54.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(74.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(74.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(74.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }


                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = anasayfa.txtInisNoktasi.Text;

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "07:20" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);
                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";

                    }

                    oB.tabloIsmi = "yediYirmi";

                }
                else if(Anasayfa.business)
                {
                    if (uS.lblFiyat7nokta20.Text == "127,99 TL")
                    {
                        //107,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (107.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(127.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(127.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(127.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (107.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(127.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(127.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(127.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (107.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(127.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(127.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(127.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (107.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(127.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(127.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(127.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (107.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(127.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(127.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(127.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (107.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(127.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(127.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(127.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        if (anasayfa.gidisDonus)
                        {
                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.gidisDonus = false;
                        }

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "07:20" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);
                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";

                    }
                    else if (uS.lblFiyat7nokta20.Text == "100,99 TL")
                    {
                        //80,99
                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (80.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(100.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(100.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(100.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (80.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(100.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(100.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(100.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (80.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(100.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(100.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(100.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (80.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(100.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(100.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(100.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (80.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(100.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(100.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(100.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (80.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(100.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(100.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(100.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }


                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";


                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "07:20" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);
                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";

                    }

                    oB.tabloIsmi = "yediYirmi";

                }

                uS.yediYirmi2 = false;
            }
            else if (uS.dokuzOtuzBes2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat9nokta35.Text == "139,99 TL")
                    {
                        //119,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (119.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(139.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(139.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(139.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (119.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(139.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(139.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(139.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (119.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(139.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(139.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(139.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (65.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (65.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (65.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";


                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        lblGidisFirma.Text = "Onur Air";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "09:35" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";

                    }
                    else if (uS.lblFiyat9nokta35.Text == "85,99 TL")
                    {
                        //65,99
                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (65.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(85.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(85.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(85.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (65.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(85.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(85.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(85.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (65.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(85.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(85.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(85.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (65.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (65.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (65.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(85.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";


                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "09:35" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "dokuzOtuzBes";
                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat9nokta35.Text == "143,99 TL")
                    {
                        //123,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (123.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(143.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(143.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(143.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (123.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(143.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(143.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(143.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (123.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(143.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(143.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(143.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (123.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(143.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(143.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(143.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (123.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(143.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(143.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(143.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (123.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(143.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(143.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(143.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";


                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        lblGidisFirma.Text = "Onur Air";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "09:35" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat9nokta35.Text == "115,99 TL")
                    {
                        //95,99
                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (95.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(115.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(115.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(115.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (95.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(115.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(115.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(115.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (95.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(115.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(115.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(115.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (95.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(115.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(115.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(115.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (95.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(115.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(115.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(115.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (95.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(115.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(115.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(115.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";


                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        lblGidisFirma.Text = "Onur Air";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "09:35" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "dokuzOtuzBes";

                }

                uS.dokuzOtuzBes2 = false;
            }
            else if (uS.onBirOtuz2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat11nokta30.Text == "150,99 TL")
                    {
                        //130,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (130.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(150.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(150.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(150.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (130.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(150.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(150.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(150.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (130.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(150.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(150.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(150.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (130.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(150.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(150.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(150.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (130.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(150.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(150.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(150.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (130.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(150.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(150.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(150.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu"; ; 

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "11:30" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat11nokta30.Text == "90,99 TL")
                    {
                        //70,99
                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (70.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(90.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(90.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(90.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (70.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(90.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(90.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(90.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (70.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(90.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(90.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(90.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (70.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(90.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(90.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(90.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (70.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(90.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(90.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(90.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (70.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(90.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(90.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(90.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu"; ;

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "11:30" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onBirOtuz";

                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat11nokta30.Text == "154,99 TL")
                    {
                        //134,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (134.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(154.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(154.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(154.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (134.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(154.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(154.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(154.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (134.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(154.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(154.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(154.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (134.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(154.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(154.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(154.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (134.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(154.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(154.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(154.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (134.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(154.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(154.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(154.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu"; ;

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "11:30" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat11nokta30.Text == "134,99 TL")
                    {
                        //114,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (114.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(134.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(134.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(134.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (114.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(134.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(134.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(134.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (114.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(134.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(134.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(134.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (114.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(134.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(134.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(134.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (114.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(134.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(134.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(134.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (114.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(134.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(134.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(134.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu"; ;


                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "11:30" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onBirOtuz";
                }

                uS.onBirOtuz2 = false;
            }
            else if (uS.onIkiOnBes2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat12nokta15.Text == "173,99 TL")
                    {
                        //153,99
                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (153.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (153.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (153.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (153.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (153.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (153.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }


                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";


                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "12:15" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat12nokta15.Text == "98,99 TL")
                    {
                        //78,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (78.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (78.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (78.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (78.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (78.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (78.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu"; ;

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "12:15" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onIkiOnBes";

                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat12nokta15.Text == "173,99 TL")
                    {
                        //153,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (153.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (153.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (153.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(173.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (153.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (153.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (153.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(173.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu"; ;


                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "12:15" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat12nokta15.Text == "98,99 TL")
                    {
                        //78,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (78.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (78.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (78.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(98.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (78.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (78.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (78.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(98.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu"; ;

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "12:15" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onIkiOnBes";
                }

                uS.onIkiOnBes2 = false;
            }
            else if (uS.onUcBes2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat13nokta05.Text == "185,99 TL")
                    {
                        //165,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (165.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(185.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(185.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(185.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (165.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(185.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(185.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(185.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (165.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(185.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(185.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(185.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (165.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(185.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(185.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(185.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (165.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(185.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(185.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(185.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (165.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(185.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(185.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(185.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu"; ; 

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        lblGidisFirma.Text = "Atlas Global";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "13:05" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat13nokta05.Text == "110,99 TL")
                    {
                        //90,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (90.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(110.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(110.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(110.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (90.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(110.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(110.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(110.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (90.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(110.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(110.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(110.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (90.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(110.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(110.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(110.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (90.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(110.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(110.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(110.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (90.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(110.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(110.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(110.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu"; ;

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        lblGidisFirma.Text = "Atlas Global";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "13:05" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onUcSifirBes";

                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat13nokta05.Text == "188,99 TL")
                    {
                        //168,99
                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (168.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(188.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(188.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(188.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (168.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(188.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(188.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(188.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (168.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(188.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(188.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(188.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (168.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(188.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(188.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(188.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (168.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(188.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(188.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(188.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (168.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(188.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(188.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(188.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu"; ;

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        lblGidisFirma.Text = "Atlas Global";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "13:05" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat13nokta05.Text == "160,99 TL")
                    {
                        //140,99
                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (140.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(160.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(160.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(160.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (140.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(160.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(160.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(160.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (140.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(160.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(160.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(160.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (140.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(160.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(160.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(160.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (140.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(160.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(160.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(160.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (140.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(160.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(160.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(160.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu"; ;

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        lblGidisFirma.Text = "Atlas Global";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "13:05" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onUcSifirBes";

                }

                uS.onUcBes2 = false;
            }
            else if (uS.onUcKirk2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat13nokta40.Text == "197,99 TL")
                    {
                        //177,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (177.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(197.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(197.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(197.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (177.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(197.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(197.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(197.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (177.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(197.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(197.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(197.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (177.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(197.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(197.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(197.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (177.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(197.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(197.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(197.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (177.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(197.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(197.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(197.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu"; ;

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        lblGidisFirma.Text = "Onur Air";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "13:40" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat13nokta40.Text == "131,99 TL")
                    {
                        //111,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (111.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(131.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(131.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(131.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (111.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(131.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(131.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(131.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (111.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(131.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(131.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(131.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (111.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(131.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(131.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(131.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (111.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(131.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(131.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(131.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (111.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(131.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(131.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(131.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        lblGidisFirma.Text = "Onur Air";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "13:40" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onUcKirk";

                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat13nokta40.Text == "199,99 TL")
                    {
                        //179,99
                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (177.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(199.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(199.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(199.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (177.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(199.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(199.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(199.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (177.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(199.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(199.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(199.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (177.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(199.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(199.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(199.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (177.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(199.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(199.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(199.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (177.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(199.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(199.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(199.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        lblGidisFirma.Text = "Onur Air";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "13:40" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat13nokta40.Text == "167,99 TL")
                    {
                        //147,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (147.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(167.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(167.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(167.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (147.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(167.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(167.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(167.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (147.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(167.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(167.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(167.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (147.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(167.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(167.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(167.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (147.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(167.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(167.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(167.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (147.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(167.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(167.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(167.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";


                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        lblGidisFirma.Text = "Onur Air";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "13:40" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onUcKirk";

                }

                uS.onUcKirk2 = false;
            }
            else if (uS.onDortYirmi2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat14nokta20.Text == "215,99 TL")
                    {
                        //195,99
                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (195.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(215.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(215.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(215.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (195.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(215.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(215.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(215.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (195.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(215.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(215.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(215.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (195.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(215.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(215.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(215.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (195.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(215.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(215.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(215.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (195.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(215.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(215.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(215.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        lblGidisFirma.Text = "AnadoluJet";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "14:20" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat14nokta20.Text == "149,99 TL")
                    {
                        //129,99
                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (129.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(149.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(149.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(149.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (129.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(149.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(149.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(149.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (129.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(149.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(149.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(149.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (129.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(149.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(149.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(149.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (129.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(149.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(149.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(149.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (129.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(149.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(149.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(149.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";


                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        lblGidisFirma.Text = "AnadoluJet";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "14:20" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onDortYirmi";

                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat14nokta20.Text == "218,99 TL")
                    {
                        //198,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (198.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (198.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (198.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (198.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (198.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (198.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        lblGidisFirma.Text = "AnadoluJet";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "14:20" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat14nokta20.Text == "175,99 TL")
                    {
                        //155,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (155.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(175.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(175.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(175.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (155.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(175.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(175.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(175.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (155.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(175.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(175.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(175.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (155.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(175.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(175.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(175.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (155.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(175.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(175.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(175.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (155.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(175.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(175.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(175.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        lblGidisFirma.Text = "AnadoluJet";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "14:20" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onDortYirmi";
                }

                uS.onDortYirmi2 = false;
            }
            else if (uS.onDortElli2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat14nokta50.Text == "243,99 TL")
                    {
                        //223,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (223.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(243.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(243.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(243.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (223.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(243.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(243.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(243.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (223.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(243.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(243.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(243.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (223.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(243.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(243.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(243.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (223.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(243.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(243.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(243.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (223.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(243.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(243.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(243.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "14:50" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat14nokta50.Text == "155,99 TL")
                    {
                        //135,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (135.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(155.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(155.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(155.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (135.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(155.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(155.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(155.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (135.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(155.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(155.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(155.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (135.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(155.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(155.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(155.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (135.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(155.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(155.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(155.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (135.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(155.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(155.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(155.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "14:50" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onDortElli";

                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat14nokta50.Text == "246,99 TL")
                    {
                        //226,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (226.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(246.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(246.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(246.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (226.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(246.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(246.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(246.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (226.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(246.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(246.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(246.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (226.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(246.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(246.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(246.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (226.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(246.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(246.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(246.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (226.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(246.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(246.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(246.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "14:50" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat14nokta50.Text == "187,99 TL")
                    {
                        //167,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (167.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(187.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(187.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(187.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (167.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(187.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(187.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(187.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (167.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(187.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(187.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(187.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (167.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(187.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(187.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(187.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (167.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(187.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(187.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(187.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (167.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(187.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(187.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(187.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "14:50" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onDortElli";

                }

                uS.onDortElli2 = false;
            }
            else if (uS.onBesOn2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat15nokta10.Text == "286,99 TL")
                    {
                        //266,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (266.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(286.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(286.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(286.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (266.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(286.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(286.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(286.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (266.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(286.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(286.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(286.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (266.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(286.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(286.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(286.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (266.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(286.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(286.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(286.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (266.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(286.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(286.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(286.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        lblGidisFirma.Text = "AnadoluJet";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "15:10" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat15nokta10.Text == "169,99 TL")
                    {
                        //149,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (149.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(169.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(169.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(169.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (149.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(169.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(169.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(169.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (149.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(169.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(169.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(169.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (149.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(169.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(169.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(169.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (149.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(169.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(169.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(169.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (149.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(169.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(169.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(169.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        lblGidisFirma.Text = "AnadoluJet";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "15:10" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onBesOn";

                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat15nokta10.Text == "289,99 TL")
                    {
                        //269,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (269.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(289.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(289.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(289.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (269.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(289.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(289.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(289.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (269.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(289.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(289.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(289.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (269.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(289.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(289.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(289.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (269.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(289.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(289.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(289.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (269.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(289.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(289.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(289.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        lblGidisFirma.Text = "AnadoluJet";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "15:10" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat15nokta10.Text == "196,99 TL")
                    {
                        //176,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (176.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (176.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (176.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (176.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (176.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (176.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        lblGidisFirma.Text = "AnadoluJet";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "15:10" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onBesOn";

                }

                uS.onBesOn2 = false;
            }
            else if (uS.onBesKirkBes2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat15nokta45.Text == "299,99 TL")
                    {
                        //279,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (279.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(299.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(299.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(299.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (279.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(299.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(299.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(299.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (279.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(299.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(299.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(299.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (279.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(299.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(299.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(299.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (279.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(299.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(299.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(299.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (279.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(299.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(299.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(299.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        lblGidisFirma.Text = "AtlasGlobal";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "15:45" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat15nokta45.Text == "178,99 TL")
                    {
                        //158,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (158.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(178.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(178.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(178.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (158.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(178.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(178.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(178.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (158.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(178.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(178.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(178.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (158.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(178.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(178.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(178.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (158.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(178.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(178.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(178.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (158.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(178.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(178.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(178.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        lblGidisFirma.Text = "AtlasGlobal";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "15:45" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onBesKirkBes";

                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat15nokta45.Text == "314,99 TL")
                    {
                        //294,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (294.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(314.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(314.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(314.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (294.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(314.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(314.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(314.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (294.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(314.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(314.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(314.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (294.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(314.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(314.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(314.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (294.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(314.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(314.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(314.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (294.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(314.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(314.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(314.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        lblGidisFirma.Text = "AtlasGlobal";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "15:45" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat15nokta45.Text == "209,99 TL")
                    {
                        //189,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (189.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(209.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(209.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(209.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (189.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(209.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(209.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(209.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (189.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(209.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(209.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(209.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (189.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(209.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(209.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(209.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (189.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(209.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(209.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(209.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (189.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(209.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(209.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(209.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        lblGidisFirma.Text = "AtlasGlobal";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "15:45" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onBesKirkBes";

                }
                uS.onBesKirkBes2 = false;
            }
            else if (uS.onAltiKirk2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat16nokta40.Text == "310,99 TL")
                    {
                        //290,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (290.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (290.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (290.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (290.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (290.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (290.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "16:40" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat16nokta40.Text == "184,99 TL")
                    {
                        //164,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (164.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(184.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(184.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(184.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (164.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(184.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(184.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(184.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (164.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(184.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(184.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(184.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (164.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(184.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(184.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(184.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (164.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(184.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(184.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(184.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (164.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(184.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(184.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(184.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "16:40" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onAltiKirk";

                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat16nokta40.Text == "327,99 TL")
                    {
                        //307,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (307.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(327.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(327.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(327.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (307.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(327.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(327.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(327.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (307.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(327.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(327.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(327.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (307.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(327.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(327.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(327.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (307.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(327.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(327.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(327.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (307.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(327.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(327.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(327.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "16:40" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat16nokta40.Text == "218,99 TL")
                    {
                        //198,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (198.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (198.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (198.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(218.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (198.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (198.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (198.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(218.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "16:40" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onAltiKirk";

                }

                uS.onAltiKirk2 = false;
            }
            else if (uS.onYediOnBes2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat17nokta15.Text == "317,99 TL")
                    {
                        //297,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (297.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (297.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (297.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (297.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (297.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (297.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "17:15" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat17nokta15.Text == "196,99 TL")
                    {
                        //176,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (176.66) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (176.66)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (176.66)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(196.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (176.66));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (176.66)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (176.66)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(196.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "17:15" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onYediOnBes";

                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat17nokta15.Text == "317,99 TL")
                    {
                        //297,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (297.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (297.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (297.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (297.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (297.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (297.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "17:15" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat17nokta15.Text == "221,99 TL")
                    {
                        //201,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (201.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(221.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(221.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(221.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (201.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(221.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(221.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(221.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (201.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(221.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(221.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(221.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (201.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(221.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(221.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(221.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (201.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(221.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(221.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(221.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (201.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(221.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(221.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(221.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "17:15" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onYediOnBes";
                }

                uS.onYediOnBes2 = false;
            }
            else if (uS.onSekizOtuz2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat18nokta30.Text == "336,99 TL")
                    {
                        //316,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (316.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(336.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(336.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(336.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (316.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(336.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(336.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(336.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (316.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(336.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(336.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(336.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (316.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(336.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(336.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(336.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (316.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(336.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(336.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(336.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (316.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(336.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(336.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(336.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        lblGidisFirma.Text = "Onur Air";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "18:30" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat18nokta30.Text == "201,99 TL")
                    {
                        //181,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (181.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(201.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(201.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(201.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (181.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(201.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(201.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(201.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (181.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(201.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(201.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(201.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (181.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(201.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(201.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(201.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (181.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(201.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(201.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(201.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (181.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(201.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(201.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(201.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        lblGidisFirma.Text = "Onur Air";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "18:30" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onSekizOtuz";
                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat18nokta30.Text == "348,99 TL")
                    {
                        //328,99
                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (328.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (328.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (328.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (328.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (328.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (328.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        lblGidisFirma.Text = "Onur Air";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "18:30" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat18nokta30.Text == "242,99 TL")
                    {
                        //222,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (222.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(242.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(242.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(242.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (222.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(242.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(242.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(242.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (222.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(242.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(242.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(242.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (222.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(242.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(242.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(242.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (222.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(242.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(242.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(242.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (222.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(242.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(242.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(242.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[2];
                        lblGidisFirma.Text = "Onur Air";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "18:30" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onSekizOtuz";

                }

                uS.onSekizOtuz2 = false;
            }
            else if (uS.onDokuzBes2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat19nokta05.Text == "348,99 TL")
                    {
                        //348,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (348.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (348.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (348.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(348.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (348.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (348.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (348.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(348.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";


                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        lblGidisFirma.Text = "Anadolu Jet";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "19:05" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat19nokta05.Text == "234,99 TL")
                    {
                        //214,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (214.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(234.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(234.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(234.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (214.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(234.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(234.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(234.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (214.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(234.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(234.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(234.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (214.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(234.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(234.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(234.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (214.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(234.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(234.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(234.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (214.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(234.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(234.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(234.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        lblGidisFirma.Text = "Anadolu Jet";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "19:05" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onDokuzSifirBes";

                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat19nokta05.Text == "352,99 TL")
                    {
                        //332,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (332.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(352.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(352.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(352.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (332.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(352.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(352.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(352.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (332.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(352.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(352.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(352.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (332.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(352.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(352.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(352.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (332.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(352.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(352.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(352.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (332.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(352.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(352.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(352.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        lblGidisFirma.Text = "Anadolu Jet";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "19:05" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat19nokta05.Text == "275,99 TL")
                    {
                        //255,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (255.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(275.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(275.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(275.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (255.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(275.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(275.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(275.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (255.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(275.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(275.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(275.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (255.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(275.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(275.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(275.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (255.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(275.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(275.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(275.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (255.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(275.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(275.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(275.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[0];
                        lblGidisFirma.Text = "Anadolu Jet";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "19:05" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "onDokuzSifirBes";
                }

                uS.onDokuzBes2 = false;
            }      
            else if (uS.yirmiOtuz2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat20nokta30.Text == "375,99 TL")
                    {
                        //355,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (355.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(375.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(375.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(375.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (355.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(375.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(375.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(375.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (355.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(375.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(375.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(375.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (355.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(375.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(375.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(375.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (355.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(375.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(375.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(375.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (355.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(375.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(375.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(375.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "20:30" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat20nokta30.Text == "258,99 TL")
                    {
                        //238,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (238.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(258.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(258.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(258.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (238.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(258.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(258.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(258.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (238.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(258.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(258.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(258.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (238.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(258.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(258.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(258.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (238.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(258.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(258.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(258.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (238.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(258.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(258.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(258.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "20:30" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "yirmiOtuz";
                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat20nokta30.Text == "373,99 TL")
                    {
                        //353,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (353.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(373.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(373.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(373.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (353.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(373.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(373.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(373.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (353.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(373.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(373.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(373.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (353.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(373.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(373.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(373.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (353.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(373.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(373.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(373.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (353.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(373.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(373.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(373.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "20:30" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat20nokta30.Text == "310,99 TL")
                    {
                        //290,99
                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (290.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (290.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (290.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(310.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (290.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (290.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (290.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(310.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[4];
                        lblGidisFirma.Text = "Turkish Airlines";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "20:30" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "20kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "yirmiOtuz";

                }

                uS.yirmiOtuz2 = false;
            } 
            else if (uS.yirmiBirKirk2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat21nokta40.Text == "384,99 TL")
                    {

                        //364.99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (364.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(384.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(384.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(384.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (364.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(384.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(384.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(384.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (364.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(384.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(384.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(384.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (364.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(384.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(384.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(384.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (364.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(384.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(384.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(384.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (364.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(384.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(384.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(384.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "21:40" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat21nokta40.Text == "287,99 TL")
                    {
                        //267,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (267.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(287.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(287.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(287.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (267.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(287.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(287.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(287.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (267.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(287.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(287.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(287.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (267.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(287.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(287.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(287.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (267.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(287.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(287.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(287.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (267.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(287.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(287.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(287.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "21:40" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "yirmiBirKirk";

                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat21nokta40.Text == "380,99 TL")
                    {
                        //360,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (360.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(380.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(380.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(380.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (360.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(380.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(380.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(380.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (360.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(380.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(380.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(380.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (360.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(380.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(380.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(380.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (360.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(380.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(380.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(380.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (360.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(380.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(380.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(380.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "21:40" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat21nokta40.Text == "354,99 TL")
                    {
                        //334,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (334.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(354.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(354.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(354.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (334.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(354.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(354.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(354.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (334.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(354.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(354.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(354.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (334.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(354.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(354.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(354.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (334.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(354.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(354.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(354.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (334.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(354.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(354.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(354.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "21:40" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "yirmiBirKirk";

                }

                uS.yirmiBirKirk2 = false;
            }
            else if (uS.yirmiIkiElli2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat22nokta50.Text == "410,99 TL")
                    {
                        //390,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (390.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(410.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(410.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(410.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (390.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(410.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(410.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(410.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (390.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(410.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(410.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(410.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (390.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(410.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(410.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(410.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (390.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(410.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(410.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(410.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (390.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(410.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(410.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(410.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "22:50" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat22nokta50.Text == "294,99 TL")
                    {
                        //274,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (274.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(294.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(294.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(294.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (274.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(294.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(294.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(294.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (274.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(294.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(294.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(294.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (274.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(294.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(294.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(294.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (274.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(294.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(294.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(294.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (274.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(294.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(294.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(294.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "22:50" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "yirmiIkiElli";

                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat22nokta50.Text == "423,99 TL")
                    {
                        //403,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (403.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(423.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(423.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(423.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (403.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(423.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(423.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(423.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (403.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(423.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(423.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(423.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (403.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(423.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(423.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(423.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (403.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(423.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(423.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(423.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (403.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(423.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(423.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(423.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "22:50" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat22nokta50.Text == "371,99 TL")
                    {
                        //351,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (351.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(371.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(371.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(371.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (351.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(371.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(371.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(371.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (351.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(371.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(371.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(371.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (351.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(371.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(371.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(371.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (351.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(371.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(371.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(371.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (351.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(371.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(371.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(371.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[3];
                        lblGidisFirma.Text = "Pegasus";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "22:50" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "yirmiIkiElli";

                }

                uS.yirmiIkiElli2 = false;

            }
            else if (uS.yirmiUcKirkBes2)
            {
                if (Anasayfa.ekonomi)
                {
                    if (uS.lblFiyat23nokta45.Text == "430,99 TL")
                    {
                        //410,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (410.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(430.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(430.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(430.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (410.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(430.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(430.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(430.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (410.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(430.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(430.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(430.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (410.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(430.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(430.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(430.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (410.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(430.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(430.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(430.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (410.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(430.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(430.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(430.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        lblGidisFirma.Text = "AtlasGlobal";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "23:45" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat23nokta45.Text == "317,99 TL")
                    {
                        //297,99
                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (297.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (297.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (297.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(317.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (297.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (297.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (297.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(317.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        lblGidisFirma.Text = "AtlasGlobal";
                        lblGidisSinifi.Text = "Sınıfı: Ekonomi";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "23:45" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "yirmiUcKirkBes";

                }
                else if (Anasayfa.business)
                {
                    if (uS.lblFiyat23nokta45.Text == "434,99 TL")
                    {
                        //414,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (414.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(434.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(434.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(434.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (414.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(434.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(434.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(434.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (414.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(434.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(434.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(434.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (414.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(434.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(434.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(434.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (414.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(434.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(434.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(434.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (414.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(434.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(434.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(434.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";


                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        lblGidisFirma.Text = "AtlasGlobal";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "23:45" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }
                    else if (uS.lblFiyat23nokta45.Text == "388,99 TL")
                    {
                        //368,99

                        if (anasayfa.gidisDonus)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (368.99) * 2);

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(388.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(388.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(388.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (368.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(388.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(388.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(388.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * 2 * (368.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(388.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(388.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(388.99) * 2 * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                            }

                            lblYonAciklama.Text = "Gidiş Dönüş";
                            anasayfa.gidisDonus = false;
                        }
                        else if (anasayfa.tekYon)
                        {
                            if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 0))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (368.99));

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = (Convert.ToDouble(388.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                                lblToplamUcretFiyati.Text = (Convert.ToDouble(388.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = (Convert.ToDouble(388.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)).ToString() + " TL";
                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 1))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (368.99)) + 50;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(388.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(388.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(388.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 50).ToString() + " TL";

                            }
                            else if ((anasayfa.ambiance_NumericUpDownCocuk.Value == 2))
                            {
                                double fiyat = ((anasayfa.ambiance_NumericUpDownYetiskin.Value) * (368.99)) + 100;

                                lblBiletFiyati.Text = fiyat.ToString() + " TL";
                                lblToplamFiyat.Text = ((Convert.ToDouble(388.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";
                                lblToplamUcretFiyati.Text = ((Convert.ToDouble(388.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                                lblYolcuFiyat.Text = fiyat.ToString() + " TL";
                                lblOzetToplamFiyat.Text = ((Convert.ToDouble(388.99) * (anasayfa.ambiance_NumericUpDownYetiskin.Value)) + 100).ToString() + " TL";

                            }

                            lblYonAciklama.Text = "Tek Yön";
                            anasayfa.tekYon = false;
                        }

                        int yolcuSayisi = (int)(anasayfa.ambiance_NumericUpDownYetiskin.Value + anasayfa.ambiance_NumericUpDownCocuk.Value);
                        lblYolcuSayisi.Text = yolcuSayisi.ToString() + " Yolcu";
                        lblYolcuAciklama.Text = yolcuSayisi.ToString() + " Yolcu";

                        picBagajGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        picGidisPhoto.BackgroundImage = ımageList_Havayollari.Images[1];
                        lblGidisFirma.Text = "AtlasGlobal";
                        lblGidisSinifi.Text = "Sınıfı: Business";

                        lblKalkisKonum.Text = Convert.ToDateTime(anasayfa.txtGidisTarihi.Text).ToLongDateString() + ", " + "23:45" + " -\n" + anasayfa.txtGidisNoktasi.Text;
                        lblVarisKonum.Text = Convert.ToDateTime(anasayfa.txtDonusTarihi.Text).ToLongDateString() + ", \n" + anasayfa.txtInisNoktasi.Text;

                        lblUcusSuresiAciklama.Text = "Uçuş Süresi: " + uS.lblUcusSuresi1.Text;

                        lblGidis.Text = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6);
                        lblVaris.Text = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblGidisBirlikte.Text = "Gidiş: " + anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 6) + " - " + anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 6);

                        lblBagajKutlesi.Text = "15kg/kişi promosyonlu";
                    }

                    oB.tabloIsmi = "yirmiUcKirkBes";
                }

                uS.yirmiUcKirkBes2 = false;
            }

            #endregion

        }


        #region textBox'ların Klavye Tuş Bastırmama Durumlarını Kontrol Etme

        private void txtEPosta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= 32) && (e.KeyChar <= 44)))
                e.Handled = true;
            else if ((e.KeyChar == 47))
                e.Handled = true;
            else if (((e.KeyChar >= 58 && (e.KeyChar <= 63))))
                e.Handled = true;
            else if (((e.KeyChar >= 91 && (e.KeyChar <= 94))))
                e.Handled = true;
            else if ((e.KeyChar == 96))
                e.Handled = true;
            else if (((e.KeyChar >= 123 && (e.KeyChar <= 127))))
                e.Handled = true;
            else
                e.Handled = false; //Geriye kalanları yazdır.
        }

        private void txtCepTelArti90_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtCepTelGeriKalan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= 48) && (e.KeyChar <= 57)))
                e.Handled = false; //Sadece rakamları yazdır.
            else if ((e.KeyChar == 8))
                e.Handled = false; //Silme (backspace <-- ) yazdır.
            else
                e.Handled = true; //Yazdırma
        }

        private void txtAd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( ((e.KeyChar >= 33) && (e.KeyChar <= 64)))
                e.Handled = true; //Yazdırma
            else if(((e.KeyChar >= 91) && (e.KeyChar <= 96)))
                e.Handled = true; 
            else if (((e.KeyChar >= 123) && (e.KeyChar <= 127)))
                e.Handled = true;
            else if ((e.KeyChar == 8))
                e.Handled = false; //Silme (backspace <-- ) yazdır.
            else
                e.Handled = false;
        }

        private void txtSoyad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= 33) && (e.KeyChar <= 64)))
                e.Handled = true; //Yazdırma
            else if (((e.KeyChar >= 91) && (e.KeyChar <= 96)))
                e.Handled = true;
            else if (((e.KeyChar >= 123) && (e.KeyChar <= 127)))
                e.Handled = true;
            else if ((e.KeyChar == 8))
                e.Handled = false; //Silme (backspace <-- ) yazdır.
            else
                e.Handled = false;

        }

        private void txtTCKimlikNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= 48) && (e.KeyChar <= 57)))
                e.Handled = false; //Sadece rakamları yazdır.
            else if ((e.KeyChar == 8))
                e.Handled = false; //Silme (backspace <-- ) yazdır.
            else
                e.Handled = true; //Yazdırma
        }

        private void txtDogumTarihi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= 32) && (e.KeyChar <= 45)) || (e.KeyChar == 47))
                e.Handled = true; //Nokta hariç. O yüzdenn ASCII kodu 46 olan nokta (.) tuşunu yukarıda şarta alınmadı. Böylece klavyeden basılması sağlandı.
            else if ((e.KeyChar >= 58) && (e.KeyChar <= 127))
                e.Handled = true;
            else
                e.Handled = false; //Yukarıdaki ASCII 'lere karşılık gelen tuşların klavyeden basılmasını engelle, sadece sayıları ve nokta (.) tuşlarının basılmasını sağla.
        }



        #endregion


        #region Ödemeye Git

        void sartlarinSaglanmaKontrolu()
        {

            try
            {
                DateTime dogumTarihi = Convert.ToDateTime(txtDogumTarihi.Text);

                #region txtEPosta

                if (txtEPosta.Text.Length == 0)
                {
                    b_EPosta = false;

                    errorProvTxtEPosta.SetError(txtEPosta, "E-Posta adresi boş geçilemez.");
                }
                else
                    b_EPosta = true;

                #endregion

                #region txtCepTelGeriKalan

                if (txtCepTelGeriKalan.Text.Length != 10)
                {
                    b_CepTelGeriyeKalan = false;

                    errorProvTxtCepTelGeriyeKalan.SetError(txtCepTelGeriKalan, "Cep telefon numarası 10 haneli olmalıdır.");
                }
                else
                    b_CepTelGeriyeKalan = true;

                #endregion

                #region txtAd

                if (txtAd.Text.Length == 0)
                {
                    b_Ad = false;

                    errorProvTxtAd.SetError(txtAd, "Ad alanı boş geçilemez.");
                }
                else
                    b_Ad = true;

                #endregion

                #region txtSoyad

                if (txtSoyad.Text.Length == 0)
                {
                    b_Soyad = false;

                    errorProvTxtSoyad.SetError(txtSoyad, "Soyad alanı boş geçilemez.");
                }
                else
                    b_Soyad = true;

                #endregion

                #region txtTCKimlikNo

                if (txtTCKimlikNo.Text.Length != 11)
                {
                    b_TCKimlikNo = false;

                    errorProvTxtTCKimlikNo.SetError(txtTCKimlikNo, "T.C. Kimlik Numarası 11 haneli olmalıdır.");
                }
                else
                    b_TCKimlikNo = true;

                #endregion

                #region txtDogumTarihi

                if (txtDogumTarihi.Text.Length != 10)
                    b_DogumTarihi = false;
                else
                    b_DogumTarihi = true;

                #endregion

                #region radioButtons'lar

                if (!(iTalk_RadioButton_Bay.Checked) && !(iTalk_RadioButton_Bayan.Checked))
                {
                    b_radioButtonslar = false;

                    errorProvRadioButtons.SetError(iTalk_RadioButton_Bayan, "Cinsiyet kısmı işaretlenmelidir.");
                }
                else
                    b_radioButtonslar = true;

                #endregion

            }
            catch (FormatException)
            {
                b_DogumTarihi = false;

                MessageBox.Show("Geçerli bir tarih formatı giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }     

        private void ambiance_Button_OdemeyeGit_Click(object sender, EventArgs e)

        {
            sartlarinSaglanmaKontrolu();

            if ((b_EPosta == true) && (b_CepTelGeriyeKalan == true) && (b_Ad == true) && (b_Soyad == true) && (b_TCKimlikNo == true)
               && (b_DogumTarihi == true) && (b_radioButtonslar == true))
            {
                oB.lblToplamTutarFiyat.Text = lblOzetToplamFiyat.Text;


                oB.tcKimlikNo = txtTCKimlikNo.Text;
                oB.ad = txtAd.Text;
                oB.soyad = txtSoyad.Text;

                #region Cinsiyet Alma

                if (iTalk_RadioButton_Bay.Checked)
                    oB.cinsiyet = "Erkek";
                else if (iTalk_RadioButton_Bayan.Checked)
                    oB.cinsiyet = "Kadın";

                #endregion

                #region Gidiş-Dönüş || Tek Yön Durumunu Alma

                if (anasayfa.tekYon2)
                {
                    oB.tekCiftYon = "Tek Yön";
                    oB.donusTarihi = "";
                }
                else if (anasayfa.gidisDonus2)
                {
                    oB.tekCiftYon = "Gidiş-Dönüş";
                    oB.donusTarihi = anasayfa.txtDonusTarihi.Text;
                }

                #endregion

                #region Bilet Sınıfı Durumu Alma

                if (anasayfa.d_ekonomi)
                    oB.biletSinifi = "Ekonomi";
                else if (anasayfa.d_business)
                    oB.biletSinifi = "Business";

                #endregion

                #region Bilet No Üretme

                int[] biletNoDizi = new int[9]; //9 karakterli bilet no.

                int sayac = 0;
                for (int i = 1; i <= 9; i++)
                {
                    biletNoDizi[sayac] = rnd.Next(0, 9);
                    ++sayac;
                }

                #endregion

                oB.dogumTarihi = txtDogumTarihi.Text;
                oB.cepTelefonu = txtCepTelArti90.Text + " " + txtCepTelGeriKalan.Text;
                oB.ePosta = txtEPosta.Text;
                oB.gidisNoktasi = anasayfa.txtGidisNoktasi.Text.Substring(anasayfa.txtGidisNoktasi.Text.Length - 5);
                oB.varisNoktasi = anasayfa.txtInisNoktasi.Text.Substring(anasayfa.txtInisNoktasi.Text.Length - 5);
                oB.gidisTarihi = anasayfa.txtGidisTarihi.Text;
                oB.donusTarihi = anasayfa.txtDonusTarihi.Text;
                oB.yetiskinSayisi = (int)anasayfa.ambiance_NumericUpDownYetiskin.Value;
                oB.cocukSayisi = (int)anasayfa.ambiance_NumericUpDownCocuk.Value;
                oB.yolcuSayisi = (int)((anasayfa.ambiance_NumericUpDownYetiskin.Value) + (anasayfa.ambiance_NumericUpDownCocuk.Value));
                oB.bagajKutlesi = lblBagajKutlesi.Text;
                oB.firma = lblGidisFirma.Text;
                oB.biletFiyati = lblOzetToplamFiyat.Text;
                oB.biletNo = (biletNoDizi[0].ToString() + biletNoDizi[1].ToString() + biletNoDizi[2].ToString() + biletNoDizi[3].ToString() + biletNoDizi[4].ToString() + biletNoDizi[5].ToString() + biletNoDizi[6].ToString() + biletNoDizi[7].ToString() + biletNoDizi[8].ToString());


                oB.Show();

                this.Visible = false;
            }
        }

        #endregion

        private void btnAsagiIndir_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
 