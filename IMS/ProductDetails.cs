using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace IMS
{
    public partial class ProductDetails : Form
    {
        IMSEntities db = CommonUtilities.getConnection();

        public ProductDetails()
        {
            InitializeComponent();
        }

        static String productName;
        static long categoryId;
        static long productId;

        private void ProductDetails_Load(object sender, EventArgs e)
        {
            textBox_productName.Text = productName;
        }

        public void setProductName(Hashtable productDetails)
        {
            productName = productDetails["ProductName"] as String;

            Object categoryId = productDetails["CategoryId"];
            long catId = long.Parse(categoryId.ToString());

            ProductDetails.categoryId = catId;

            Object productId = productDetails["ProductId"];
            long prodId = long.Parse(productId.ToString());

            ProductDetails.productId = prodId;
        }

        private void button_addProductDetails_Click(object sender, EventArgs e)
        {
            String errorMessage = "";
            if(textBox_quantity.Text.Trim().Length == 0)
            {
                errorMessage += "Please Enter Product Quantity \n";
                MessageBox.Show(errorMessage, "Error");
                return;
            }
            int result;
            
            if (! Int32.TryParse(textBox_quantity.Text.Trim(), out result))
            {
                errorMessage += "\n Please Enter Valid Product Quantity \n";
                MessageBox.Show(errorMessage, "Error");
                return;
            }


            Product productDetails = new Product();

            long lastProductId = 0;
            if (db.Products.Where(x => x.CategoryId == ProductDetails.categoryId).Count() > 0)
                lastProductId = db.Products.Where(x => x.CategoryId == ProductDetails.categoryId).Max(x => x.ProductId);

            productDetails.ProductId = lastProductId + 1;
            productDetails.ProductName = ProductDetails.productName; 
            productDetails.CategoryId = ProductDetails.categoryId;
            productDetails.Product_Details = richTextBox_productDescription.Text.Trim();
            productDetails.Quantity = Int32.Parse(textBox_quantity.Text.Trim());


            db.Products.Add(productDetails);
            db.SaveChanges();

            MessageBox.Show("Added " + ProductDetails.productName + " Details Successfully","Success");
            this.Hide();
        }
    }
}
