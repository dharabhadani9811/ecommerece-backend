using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DataAccessLayer.Models;
using DataAccessLayer;
using System.Web;
using System.IO;
using BusinessLayer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace ECommerceApi.Controllers
{
    public class AdminController : ApiController
    {
        //private ECommerceContext db = new ECommerceContext();
        private AdminBL db = new AdminBL();

        // GET api/Admin
        //public IQueryable<Product> GetProducts()
        //{
        //    return db.Products;
        //}

        // GET api/Admin/5
        //[ResponseType(typeof(Product))]
        //public IHttpActionResult GetProduct(int id)
        //{
        //    Product product = db.Products.Find(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(product);
        //}

        //// PUT api/Admin/5
        //public IHttpActionResult PutProduct(int id, Product product)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != product.ProductID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(product).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST api/Admin
        [Authorize]
        [HttpPost]
        [Route("api/admin/addProduct")]
        public IHttpActionResult PostProduct(Product product)
        {
            RespMessage response = new RespMessage();
            Random r = new Random();
            string file_name = r.Next(200, 10000).ToString();

            byte[] imagebytes = Convert.FromBase64String(product.ProductImage);
            string filePath = HttpContext.Current.Server.MapPath("~/Images/" + Path.GetFileName(file_name) + ".jpg");
            string path = "Images/" + Path.GetFileName(file_name) + ".jpg";
            File.WriteAllBytes(filePath, imagebytes);

            product.ProductImage = path;
            response.Message = db.AddProduct(product);
            return Ok(response);


            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //db.Products.Add(product);
            //db.SaveChanges();

            //return CreatedAtRoute("DefaultApi", new { id = product.ProductID }, product);
        }

        [HttpGet]
        [Route("api/admin/login/{username}/{password}")]
        public IHttpActionResult login(string username, string password)
        {
            RespLogin response = new RespLogin();

            Admin a = db.adminLogin(username, password);
            if (a != null)
            {
                response.UserID = a.AdminID.ToString();
                response.EmailID = a.Username;
                response.Message = "Login successful";
                response.Token = generateToken();
            }
            else
            {
                response.UserID = "0";
                response.EmailID = "";
                response.Message = "Invalid email id or password";
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [Route("api/admin/getCategory")]
        public List<Category> getCategory()
        {
            return db.getCategory();
        }

        [Authorize]
        [HttpGet]
        [Route("api/admin/getProduct")]
        public List<ProductDetails> getProduct()
        {
            return db.getProducts();
        }

        [Authorize]
        [HttpGet]
        [Route("api/admin/deleteProduct/{ProductID}")]
        public IHttpActionResult getProduct(string ProductID)
        {
            RespMessage response = new RespMessage();
            response.Message = db.deleteProduct(ProductID);
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [Route("api/admin/getOrders")]
        public List<OrderDetails> getOrders()
        {
            return db.getOrders();
        }


        // DELETE api/Admin/5
        //[ResponseType(typeof(Product))]
        //public IHttpActionResult DeleteProduct(int id)
        //{
        //    Product product = db.Products.Find(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Products.Remove(product);
        //    db.SaveChanges();

        //    return Ok(product);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool ProductExists(int id)
        //{
        //    return db.Products.Count(e => e.ProductID == id) > 0;
        //}

        public string generateToken()
        {
            string securityKey = "12345678abcdrefgytwd";

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signInCred = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                    issuer: "abcd@domain.in",
                    audience: "Users",
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: signInCred
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


}