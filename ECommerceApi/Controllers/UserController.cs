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
using BusinessLayer;
using System.Net.Mail;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace ECommerceApi.Controllers
{
    public class UserController : ApiController
    {
        //private ECommerceContext db = new ECommerceContext();
        private UserBL db = new UserBL();

        // GET api/Default1
        //public IQueryable<User> GetUsers()
        //{
        //    return db.Users;
        //}

        //// GET api/Default1/5
        //[ResponseType(typeof(User))]
        //public IHttpActionResult GetUser(int id)
        //{
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(user);
        //}

        // PUT api/Default1/5
        //public IHttpActionResult PutUser(int id, User user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != user.UserID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
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

        // POST api/Default1
        [ResponseType(typeof(User))]
        [Route("api/user/register")]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                RespMessage response = new RespMessage();
                response.Message = db.AddUser(user);
                return Ok(response);
            }    
        }

        [HttpGet]
        [Route("api/user/login/{email}/{password}")]
        public IHttpActionResult login(string email, string password)
        {
            RespLogin response = new RespLogin();

            User u = db.login(email, password);
            if (u != null)
            {
                response.UserID = u.UserID.ToString();
                response.EmailID = u.EmailID;
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
        [Route("api/user/getProductByID/{ProductID}")]

        public List<ProductDetails> getProductByID(string ProductID)
        {
            return db.getProductByID(ProductID);
        }

        [Authorize]
        [HttpPost]
        [Route("api/user/addToCart")]
        public IHttpActionResult addToCart(Cart c)
        {
            RespMessage response = new RespMessage();
            response.Message = db.addToCart(c);
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [Route("api/user/getCartProducts/{UserID}")]

        public List<ProductDetails> getCartProducts(string UserID)
        {
            return db.getCartProducts(UserID);
        }

        [Authorize]
        [HttpGet]
        [Route("api/user/deleteCart/{UserID}/{ProductID}")]
        public IHttpActionResult deleteCart(string UserID, string ProductID)
        {
            RespMessage response = new RespMessage();
            response.Message = db.deleteCartProduct(UserID,ProductID);
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [Route("api/user/placeOrder")]
        public IHttpActionResult placeOrder(List<Order> orders)
        {
            RespMessage response = new RespMessage();
            response.Message = db.placeOrder(orders);
            return Ok(response);
        }



        //[HttpGet]
        //[Route("api/user/forgotPassword/{email}/")]
        //public IHttpActionResult forgotPassword(string email)
        //{
        //    RespMessage response = new RespMessage();
        //    User user = db.forgotPassword(email);
        //    if (user != null)
        //    {
        //        if (sendEmail(email, user.Password))
        //            response.Message = "Please check your email";
        //        else
        //            response.Message = "Error while sending email";
        //    }
        //    else
        //        response.Message = "Email id not found";
        //    return Ok(response);
        //}

        //private bool sendEmail(string email, string password)
        //{
        //    try
        //    {
        //        MailMessage mail = new MailMessage();
        //        mail.From = new MailAddress("drstrange1729001@gmail.com");  // dummy gmail account email
        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Port = 587;
        //        smtp.EnableSsl = true;
        //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new NetworkCredential("drstrange1729001@gmail.com", "Strange1729"); // dummy acc email and pass
        //        smtp.Host = "smtp.gmail.com";

        //        //recipient address
        //        mail.To.Add(new MailAddress(email));    // receiver's email address
        //        mail.Subject = "E-Commerce System";
        //        mail.Body = "Your password is :" + password;   // body of email
        //        smtp.Send(mail);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}


        // DELETE api/Default1/5
        //[ResponseType(typeof(User))]
        //public IHttpActionResult DeleteUser(int id)
        //{
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Users.Remove(user);
        //    db.SaveChanges();

        //    return Ok(user);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool UserExists(int id)
        //{
        //    return db.Users.Count(e => e.UserID == id) > 0;
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