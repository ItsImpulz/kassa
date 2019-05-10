using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KassaApplicatieBTULIP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        dcBTulipDataContext db = new dcBTulipDataContext();

        public MainWindow()
        {
            InitializeComponent();
            SetData();
        }

        public void SetData()
        {
            dgKlanten.ItemsSource = db.customers.ToList();
            dgProducten.ItemsSource = db.products.ToList();
            dgOrder.ItemsSource = db.itemsinorders.ToList();

            cbKlant.ItemsSource = db.customers;
            cbKlant.DisplayMemberPath = "lastname";

            cbProduct.ItemsSource = db.products;
            cbProduct.DisplayMemberPath = "name";

            cbKlantFactuur.ItemsSource = db.customers;
            cbKlantFactuur.DisplayMemberPath = "lastname";

            cbProducttype.ItemsSource = db.producttypes;
            cbProducttype.DisplayMemberPath = "type";
        }           
                
        private void btnProducten_Click(object sender, RoutedEventArgs e)
        {
            dgKlanten.Visibility = Visibility.Collapsed;
            gKlanten.Visibility = Visibility.Collapsed;
            dgProducten.Visibility = Visibility.Visible;
            gProducten.Visibility = Visibility.Visible;
            gBestelling.Visibility = Visibility.Collapsed;
            tbWelkom.Visibility = Visibility.Collapsed;
            gFactuur.Visibility = Visibility.Collapsed;
            imgTulip.Visibility = Visibility.Collapsed;
            SetData();
        }

        private void btnKlanten_Click(object sender, RoutedEventArgs e)
        {
            dgKlanten.Visibility = Visibility.Visible;
            gKlanten.Visibility = Visibility.Visible;
            dgProducten.Visibility = Visibility.Collapsed;
            gProducten.Visibility = Visibility.Collapsed;
            gBestelling.Visibility = Visibility.Collapsed;
            tbWelkom.Visibility = Visibility.Collapsed;
            gFactuur.Visibility = Visibility.Collapsed;
            imgTulip.Visibility = Visibility.Collapsed;
            SetData();
        }

        private void btnBestelling_Click(object sender, RoutedEventArgs e)
        {
            dgKlanten.Visibility = Visibility.Collapsed;
            gKlanten.Visibility = Visibility.Collapsed;
            dgProducten.Visibility = Visibility.Collapsed;
            gProducten.Visibility = Visibility.Collapsed;
            gBestelling.Visibility = Visibility.Visible;
            tbWelkom.Visibility = Visibility.Collapsed;
            gFactuur.Visibility = Visibility.Collapsed;
            imgTulip.Visibility = Visibility.Collapsed;
            SetData();
        }

        private void btnFactuur_Click(object sender, RoutedEventArgs e)
        {
            dgKlanten.Visibility = Visibility.Collapsed;
            gKlanten.Visibility = Visibility.Collapsed;
            dgProducten.Visibility = Visibility.Collapsed;
            gProducten.Visibility = Visibility.Collapsed;
            gBestelling.Visibility = Visibility.Collapsed;
            tbWelkom.Visibility = Visibility.Collapsed;
            gFactuur.Visibility = Visibility.Visible;
            imgTulip.Visibility = Visibility.Collapsed;
            SetData();
        }
        
        private void btnKlant_Click(object sender, RoutedEventArgs e)
        {
            string sFirstname = txtVoornaam.Text;
            string sLastname = txtAchternaam.Text;
            string sStad = txtStad.Text;
            string sTel = txtTel.Text;
            string sMail = txtMail.Text;

            customer myCustomer = new customer();
            myCustomer.firstname = sFirstname;
            myCustomer.lastname = sLastname;
            myCustomer.city = sStad;
            myCustomer.phonenumber = sTel;
            myCustomer.email = sMail;

            db.customers.InsertOnSubmit(myCustomer);

            db.SubmitChanges();

            SetData();

            txtVoornaam.Text = string.Empty;
            txtAchternaam.Text = string.Empty;
            txtStad.Text = string.Empty;
            txtTel.Text = string.Empty;
            txtMail.Text = string.Empty;

            MessageBox.Show("Nieuwe klant " + myCustomer.firstname + " " + myCustomer.lastname + " is succesvol opgeslagen.");
        }

        private void btnProduct_Click(object sender, RoutedEventArgs e)
        {
            string sProductnaam = txtPrNaam.Text;
                     
            product myProduct = new product();
            myProduct.name = sProductnaam;
            myProduct.producttype = (producttype)cbProducttype.SelectedItem;

            db.products.InsertOnSubmit(myProduct);
            db.SubmitChanges();

            SetData();

            txtPrNaam.Text = string.Empty;
            MessageBox.Show("Nieuw product " + myProduct.name + " is succesvol opgeslagen.");
        }

        private void btnToevoegen_Click(object sender, RoutedEventArgs e)
        {
            order newO = new order();
            itemsinorder newIIO = new itemsinorder();

            newIIO.product = (product)cbProduct.SelectedItem;
            newIIO.amount = Convert.ToInt32(txtAantal.Text);
            newIIO.order = newO;

            newO.customer = (customer)cbKlant.SelectedItem;
            newO.date = DateTime.Now.Date;

            db.itemsinorders.InsertOnSubmit(newIIO);
            db.orders.InsertOnSubmit(newO);
            db.SubmitChanges();

            MessageBox.Show("De bestelling van " + cbKlant.Text + " is toegevoegd aan de database.");

            txtAantal.Text = " ";

            SetData();
            
        }
        
        public decimal getPrice(itemsinorder item, DateTime date)
        {
            foreach (pricehistory history in item.product.pricehistories){
                if (date >= history.startdate && (date <= history.enddate || history.enddate == null))
                {
                    return history.price;
                }
            }
            return 0;
        }

        private void btnBestellingen_Click(object sender, RoutedEventArgs e)
        {
            if (cbKlantFactuur.SelectedItem != null)
            {
                tbFactuur.Text = "";
                customer myCustomer = (customer)cbKlantFactuur.SelectedItem;
                tbFactuur.Text = "Voornaam " + myCustomer.firstname + "\n";
                tbFactuur.Text += "Achternaam: " + myCustomer.lastname + "\n";
                tbFactuur.Text += "Woonplaats: " + myCustomer.city + "\n";
                tbFactuur.Text += "Telefoonnummer: " + myCustomer.phonenumber + "\n";
                tbFactuur.Text += "E-mailadres: " + myCustomer.email + "\n";
            }

            if (cbKlantFactuur.SelectedItem != null)
            {
                tbFactuur1.Text = "";
                decimal totaalPrijs = 0;
                foreach(order ord in ((customer)cbKlantFactuur.SelectedItem).orders)
                {
                    tbFactuur1.Text += " ";
                    foreach(itemsinorder item in ord.itemsinorders)
                    {
                        decimal prijs = getPrice(item, ord.date);
                        decimal prijsProduct = prijs * item.amount;
                        tbFactuur1.Text += "x" + item.amount + " " + item.product.name + " €" + prijsProduct.ToString("0.00") + " " + ord.date + "\n";
                        totaalPrijs += prijs * item.amount;
                    }
                }

                tbFactuur1.Text += "============================\n";
                tbFactuur1.Text += "Totaal: €" + totaalPrijs.ToString("0.00") + "\n";
                tbFactuur1.Text += "============================";
            }

            else
            {
                MessageBox.Show("Eerst een klant selecteren!");
            }
        }
    }
}
