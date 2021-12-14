using DataAccessLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class AdminBL
    {
        private ECommerceContext db = new ECommerceContext();

        public string AddProduct(Product prod)
        {
            try
            {
                int rowsAffected = 0;
                db.Products.Add(prod);
                rowsAffected = db.SaveChanges();

                if (rowsAffected == 0)
                    return "Something went wrong";
                else
                    return "Product added";
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }

        public Admin adminLogin(string username, string password)
        {
            Admin admin = db.Admins.SingleOrDefault(a => a.Username == username && a.Password == password);
            return admin;
        }

        public List<Category> getCategory()
        {
            return db.Categories.ToList();
        }

        public List<ProductDetails> getProducts()
        {
            return db.Database.SqlQuery<ProductDetails>("select p.ProductID,p.ProductName,p.ProductPrice,p.ProductDescription,p.ProductImage,c.CategoryName from Products p inner join Categories c on p.CategoryID = c.CategoryID").ToList();
            //return db.Products.ToList();
        }

        public string deleteProduct(string ProductID)
        {
            int rowsAffected = 0;
            Product product = db.Products.Find(Convert.ToInt32(ProductID));
            db.Products.Remove(product);
            rowsAffected = db.SaveChanges();

            if (rowsAffected == 0)
                return "Something went wrong";
            else
                return "Product deleted";
        }

        public List<OrderDetails> getOrders()
        {
            return db.Database.SqlQuery<OrderDetails>("select CONCAT(u.FirstName,' ',u.LastName) as [UserName],u.[EmailID],u.[Address],o.oID,p.ProductName,o.ProductQty,(p.ProductPrice * o.ProductQty) as [TotalPrice],o.OrderDate from Users u Inner Join Orders o on u.UserID = o.UserID Inner Join Products p on p.ProductID = o.ProductID order by o.OrderDate desc").ToList();
        }

    }
}
