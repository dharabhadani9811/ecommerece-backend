using DataAccessLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserBL
    {
        private ECommerceContext db = new ECommerceContext();

        public string AddUser(User user)
        {
            try
            {
                int rowsAffected = 0;

                if (db.Users.Any(u => u.EmailID == user.EmailID))
                {
                    return "Email ID already exists";
                }
                else
                {
                    db.Users.Add(user);
                    rowsAffected = db.SaveChanges();

                    if (rowsAffected == 0)
                        return "Something went wrong";
                    else
                        return "Registration successful";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public User login(string email, string password)
        {
            User user = db.Users.SingleOrDefault(u => u.EmailID == email && u.Password == password);
            return user;
        }

        public List<ProductDetails> getProductByID(string ProductID)
        {
            return db.Database.SqlQuery<ProductDetails>("select p.ProductID,p.ProductName,p.ProductPrice,p.ProductDescription,p.ProductImage,c.CategoryName from Products p inner join Categories c on p.CategoryID = c.CategoryID where p.ProductID = '"+ProductID+"'").ToList();
            //return db.Products.ToList();
        }

        public string addToCart(Cart cart)
        {
             try
            {
                int rowsAffected = 0;

                if (db.Carts.Any(c => c.UserID == cart.UserID && c.ProductID == cart.ProductID))
                {
                    return "Product already added to cart";
                }
                else
                {
                    db.Carts.Add(cart);
                    rowsAffected = db.SaveChanges();

                    if (rowsAffected == 0)
                        return "Something went wrong";
                    else
                        return "Added to cart";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public List<ProductDetails> getCartProducts(string UserID)
        {
            return db.Database.SqlQuery<ProductDetails>("select p.ProductID,p.ProductName,p.ProductPrice,p.ProductDescription,p.ProductImage,c.CategoryName from Products p inner join Categories c on p.CategoryID = c.CategoryID inner join Carts ct on p.ProductID = ct.ProductID where ct.UserID = '" + UserID + "'").ToList();
        }

        public string deleteCartProduct(string UserID, string ProductID)
        {
            int rowsAffected = 0;

            int prod_id = Convert.ToInt32(ProductID);
            int user_id = Convert.ToInt32(UserID);


            Cart cart = db.Carts.Where(c => c.UserID == user_id && c.ProductID == prod_id).First();
            db.Carts.Remove(cart);
            rowsAffected = db.SaveChanges();

            if (rowsAffected == 0)
                return "Something went wrong";
            else
                return "Product deleted";
        }

        public string placeOrder(List<Order> orders)
        {
            try
            {
                int rowsAffected = 0;
                int cartRowsAffected = 0;

                Random r = new Random();
                int order_id = r.Next(9999, 99999);


                foreach (Order order in orders)
                {
                    order.oID = order_id;
                    order.OrderDate = DateTime.Now;
                    db.Orders.Add(order);
                    rowsAffected += db.SaveChanges();

                    Cart cart = db.Carts.Where(c => c.UserID == order.UserID && c.ProductID == order.ProductID).First();
                    db.Carts.Remove(cart);
                    cartRowsAffected += db.SaveChanges();
                }

                if (rowsAffected == 0)
                    return "Something went wrong";
                else
                    return "Order placed successfully";
           
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }

        public User forgotPassword(string email)
        {
            User user = db.Users.SingleOrDefault(u => u.EmailID == email);
            return user;
        }

    }

}
