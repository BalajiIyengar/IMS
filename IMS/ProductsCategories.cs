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
    public partial class ProductsCategories : Form
    {
        public ProductsCategories()
        {
            InitializeComponent();
        }

        static IMSEntities db = null;

        private void button_AddCategory_Click(object sender, EventArgs e)
        {
            if (textBox_addNewCategory.Text.Trim().Length == 0)
            {
                CommonUtilities.showErrorPopUp("Please enter a Category Name");
                return;
            }
                if (categoryExists(textBox_addNewCategory.Text.Trim().ToLower()))
                {
                    CommonUtilities.showErrorPopUp(textBox_addNewCategory.Text.Trim().ToLower() + " Already Exists");
                    return;
                }
            

            long lastCategoryId = 0;

            if (db.Categories.Count() != 0)
            {
              
                lastCategoryId = db.Categories.Max(x => x.CategoryId);
            }
            long newCategoryId = lastCategoryId + 1;

            Category newCategory = new Category();
            newCategory.CategoryId = newCategoryId;
            newCategory.CategoryName = textBox_addNewCategory.Text.Trim().ToLower();

            db.Categories.Add(newCategory);
            db.SaveChanges();
            CommonUtilities.showSuccessPopup(textBox_addNewCategory.Text.Trim().ToLower() + "Category Added Successfully");
        }

        private void ProductsCategories_Load(object sender, EventArgs e)
        {
            db = CommonUtilities.getConnection();
            setDropDownValues(comboBox_existingCategories);
            setDropDownValues(comboBox_existingProductCategories);
            setDropDownValues(comboBox_modifyExistingCategories);
            setDropDownValues(comboBox_editCategories);


        }

        bool categoryExists(String categoryName)
        {
            bool exists = false;

            List<String> categoryNames = db.Categories.Select(x => x.CategoryName).ToList();
            if (categoryNames.Contains(categoryName))
                exists = true; 

            return exists;
        }

        private void button_refreshCategories_Click(object sender, EventArgs e)
        {
            setDropDownValues(comboBox_existingCategories);
        }


        void setDropDownValues(ComboBox destinationCmb)
        {
            destinationCmb.DataSource = db.Categories.ToList();
            destinationCmb.ValueMember = "CategoryId";
            destinationCmb.DisplayMember = "CategoryName";
        }

        private void button_refreshProducts_Click(object sender, EventArgs e)
        {
            setDropDownValues(comboBox_existingProductCategories);
        }

        private void button_addProduct_Click(object sender, EventArgs e)
        {
            if (textBox_newProduct.Text.Trim().Length == 0)
            {
                CommonUtilities.showErrorPopUp("Please Enter a Product Name");
                return;
            }

            Category selectedCategory = comboBox_existingProductCategories.SelectedItem as Category;
            if (db.Products.Where(x => x.CategoryId == selectedCategory.CategoryId && x.ProductName.ToLower().Equals(textBox_newProduct.Text.Trim().ToLower())).Count() > 0)
            {
                CommonUtilities.showErrorPopUp(textBox_newProduct.Text.Trim().ToLower() + " Already Available Under " + selectedCategory.CategoryName);
                return;
            }

            long lastProductId = 0;

            //if (db.Products.Count() != 0)
            //    lastProductId = db.Products.Max(x=>x.ProductId);


            if (db.Products.Where(x => x.CategoryId == selectedCategory.CategoryId).Count() > 0)
                lastProductId = db.Products.Where(x => x.CategoryId == selectedCategory.CategoryId).Max(x => x.ProductId);


            long newProductId = lastProductId + 1;

            Product newProduct = new Product();
            newProduct.ProductId = newProductId;
            newProduct.ProductName = textBox_newProduct.Text.Trim().ToLower();
            newProduct.CategoryId = selectedCategory.CategoryId;
            
            db.Products.Add(newProduct);
            db.SaveChanges();

            CommonUtilities.showSuccessPopup(textBox_newProduct.Text.Trim().ToLower() + " Added Successfully Under " + selectedCategory.CategoryName);

        }

        private void comboBox_modifyExistingCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            Category selected = comboBox_modifyExistingCategories.SelectedItem  as Category;
            label_renameCategory.Text = "Rename " + selected.CategoryName;
        }

        private void button_rename_Click(object sender, EventArgs e)
        {
            Category selected = comboBox_modifyExistingCategories.SelectedItem as Category;

            String categoryName = selected.CategoryName;

            if(categoryExists(textBox_modifyRenameAs.Text.Trim().ToLowerInvariant()))
            {
                CommonUtilities.showErrorPopUp(selected.CategoryName + " Already Exists");
                return;
            }

            Category newCategory = db.Categories.Where(x=>x.CategoryId == selected.CategoryId).Single();
            newCategory.CategoryName = textBox_modifyRenameAs.Text.Trim().ToLowerInvariant();

            db.SaveChanges();

            CommonUtilities.showSuccessPopup("Renamed " + categoryName + " As " + textBox_modifyRenameAs.Text.Trim().ToLowerInvariant());

        }

        private void button_refreshModifyCategory_Click(object sender, EventArgs e)
        {
            setDropDownValues(comboBox_modifyExistingCategories);
        }


        private void button_refreshEditProduct_Click(object sender, EventArgs e)
        {
            setDropDownValues(comboBox_editCategories);
        }

        private void comboBox_editCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            Category selectedCategory = comboBox_editCategories.SelectedItem as Category;

            comboBox_editCategoriesInProducts.DataSource = null;

            if (db.Products.Where(x => x.CategoryId == selectedCategory.CategoryId).Count() != 0)
            {
                List<Product> productsOfSelectedCategory = db.Products.Where(x => x.CategoryId == selectedCategory.CategoryId).ToList();

                comboBox_editCategoriesInProducts.DataSource = productsOfSelectedCategory;
                comboBox_editCategoriesInProducts.ValueMember = "ProductId";
                comboBox_editCategoriesInProducts.DisplayMember = "ProductName";
            }

            label6.Text = "Products in " + selectedCategory.CategoryName;

        }

        private void button_editProduct_Click(object sender, EventArgs e)
        {

            if(textBox_renameProduct.Text.Trim().Length == 0)
            {
                CommonUtilities.showErrorPopUp("Please enter a valid value");
            }

            Category selectedCategory = comboBox_editCategories.SelectedItem as Category;

            if (db.Products.Where(x => x.CategoryId == selectedCategory.CategoryId).Count() != 0)
            {
                List<String> productsOfSelectedCategory = db.Products.Where(x => x.CategoryId == selectedCategory.CategoryId).Select(x=>x.ProductName).ToList();

                if (productsOfSelectedCategory.Contains(textBox_renameProduct.Text.Trim().ToLower()))
                {
                    CommonUtilities.showErrorPopUp(textBox_renameProduct.Text.Trim().ToLower() + " Already Exists ");
                    return;
                }

                Product selectedProduct = comboBox_editCategoriesInProducts.SelectedItem as Product;
                String productName = selectedProduct.ProductName;

                Product editedProduct = db.Products.Where(x => x.ProductId == selectedProduct.ProductId && x.CategoryId == selectedCategory.CategoryId).Single();
                editedProduct.ProductName = textBox_renameProduct.Text.Trim().ToLower();

                db.SaveChanges();
                CommonUtilities.showSuccessPopup("Updated " + productName + " As " + textBox_renameProduct.Text.Trim().ToLower());
            }
        }

        private void comboBox_editCategoriesInProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            Product selectedProduct = comboBox_editCategoriesInProducts.SelectedItem as Product;
            if(selectedProduct != null)
            label_renameAs.Text = "Rename " + selectedProduct.ProductName + " As ";
        }

        private void comboBox_existingProductCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            Category selectedCategory = comboBox_existingProductCategories.SelectedItem as Category;

            comboBox_ProductsInCategory.DataSource = null;

            if (db.Products.Where(x => x.CategoryId == selectedCategory.CategoryId).Count() != 0)
            {
                List<Product> productsOfSelectedCategory = db.Products.Where(x => x.CategoryId == selectedCategory.CategoryId).ToList();

                comboBox_ProductsInCategory.DataSource = productsOfSelectedCategory;
                comboBox_ProductsInCategory.ValueMember = "ProductId";
                comboBox_ProductsInCategory.DisplayMember = "ProductName";
            }

            label5.Text = "Products in " + selectedCategory.CategoryName;
        }

        private void checkBox_addProductDetails_CheckedChanged(object sender, EventArgs e)
        {


            if (checkBox_addProductDetails.Checked)
            {
                if (textBox_newProduct.Text.Trim().Length == 0)
                {
                    CommonUtilities.showErrorPopUp("Please Enter a Product Name");
                    checkBox_addProductDetails.Checked = false;
                    return;
                }

                 Category selectedCategory = comboBox_existingProductCategories.SelectedItem as Category;
                 Product selectedProduct = comboBox_ProductsInCategory.SelectedItem as Product;
                
                List<String> productNames = db.Products.Where(x => x.CategoryId == selectedCategory.CategoryId).Select(x => x.ProductName).ToList();

                if (productNames.Contains(textBox_newProduct.Text.Trim().ToLower()))
                {
                    CommonUtilities.showErrorPopUp(textBox_newProduct.Text.Trim().ToLower() + " Already Exists");
                    return;
                }
               
                Hashtable productInfo = new Hashtable();
                productInfo["CategoryId"] = selectedCategory.CategoryId;
                productInfo["ProductId"] = selectedProduct.ProductId;
                productInfo["ProductName"] = textBox_newProduct.Text.Trim().ToLower();

                ProductDetails productDetails = new ProductDetails();
                productDetails.setProductName(productInfo);

                productDetails.Show();
            }
        }

        
       
    }
}
