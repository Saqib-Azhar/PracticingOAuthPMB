using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using Practicing_OAuth;
using System.Web.Configuration;
using Practicing_OAuth.Models;

namespace Practicing_OAuth.Controllers
{
    public class HomeController : Controller
    {
        private static string SenderEmailId = WebConfigurationManager.AppSettings["DefaultEmailId"];
        private static string SenderEmailPassword = WebConfigurationManager.AppSettings["DefaultEmailPassword"];
        private static int SenderEmailPort = Convert.ToInt32(WebConfigurationManager.AppSettings["DefaultEmailPort"]);
        private static string SenderEmailHost = WebConfigurationManager.AppSettings["DefaultEmailHost"];
        private Practicing_OAuthEntities db = new Practicing_OAuthEntities();
        public ActionResult Index()
        {
            var ProductsList = db.Products.ToList();
            ViewBag.ProductsList = ProductsList;
            ViewBag.CategoriesList = db.Categories.ToList();
            ViewBag.CategoryTypesList = db.CategoryTypes.ToList();
            return View();
        }

        
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Blog()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult PriceQuote()
        {
            return View();
        }
        public ActionResult ContactDetails(FormCollection contactDetails)
        {
            var Name = contactDetails["Name"];
            var Email = contactDetails["Email"];
            var Phone = contactDetails["Phone"];
            var Message = contactDetails["Message"];

            try
            {
                ContactUsData ContactUsObject = new ContactUsData();

                ContactUsObject.UserName = Name;
                ContactUsObject.Email = Email;
                ContactUsObject.PhoneNumber = Phone;
                ContactUsObject.Message = Message;
                ContactUsObject.IsDeleted = false;
                ContactUsObject.Seen = false;
                ContactUsObject.SeenTime = null;
                ContactUsObject.SubmittedTime = DateTime.Now;


                db.ContactUsDatas.Add(ContactUsObject);
                db.SaveChanges();
            }
            catch(Exception ex)
            { }
          


            var fromAddress = new MailAddress(SenderEmailId, "Contact Message By: " + Name);
            var toAddress = new MailAddress(SenderEmailId, "Print My Box");
            string fromPassword = SenderEmailPassword;
            string subject = "PrintMyBox Contact Us form Submission by: " + Name;
            string body = "Name: " + Name + "<br>Phone: " + Phone + "<br>" + "Email: " + Email + "<br>" + "Message: " + Message + "<br>Time: " + DateTime.Now;

            var smtp = new SmtpClient
            {
                Host = SenderEmailHost,
                Port = SenderEmailPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body,

            })
            {
                //message.Bcc.Add("support@printmybox.com");
                smtp.Send(message);
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            var fromSiteAddress = new MailAddress(SenderEmailId, "Print My Box");
            var toCustomerAddress = new MailAddress(Email, Name);
            string fromSitePassword = SenderEmailPassword;
            subject = "PrintMyBox: Your Contact Form has submitted, Thankyou for your precious time ";
            body = " <b>Form</b><br>Name: " + Name + "<br>Phone: " + Phone + "<br>" + "Email: " + Email + "<br>" + "Message: " + Message + "<br>Time: " + DateTime.Now;

            var smtp1 = new SmtpClient
            {
                Host = SenderEmailHost,
                Port = SenderEmailPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromSiteAddress.Address, fromSitePassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromSiteAddress, toCustomerAddress)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            })
            {
                smtp1.Send(message);
            }


            return RedirectToAction("Contact");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult QuoteRequest(FormCollection fc, HttpPostedFileBase File_input)/*string ProductName, string Name, string Email, string Phone, string Stock, string Color, string Quantity, string Height, string Width, string Depth, string File, string Comments*/
        {
            try
            {
                string ProductName = fc["ProductName-input"];
                string Name = fc["Name-input"];
                string Email = fc["Email-input"];
                string Phone = fc["Phone-input"];
                string Stock = fc["Stock-input"];
                string Color = fc["Color-input"];
                string Quantity = fc["Quantity-input"];
                string Height = fc["Height-input"];
                string Width = fc["Width-input"];
                string Depth = fc["Depth-input"];
                string Comments = fc["Comments-input"];
                string ViewPath = fc["ViewPath"];
                HttpPostedFileBase AttachmentUserCopy = File_input;

                try
                {
                    PriceQuote QuoteObject = new PriceQuote();
                    if (AttachmentUserCopy != null)
                    {
                        string pic = System.IO.Path.GetFileName(AttachmentUserCopy.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/UploadedProductImages"), pic);
                        // file is uploaded
                        AttachmentUserCopy.SaveAs(path);

                        // save the image path path to the database or you can send image 
                        // directly to database
                        // in-case if you want to store byte[] ie. for DB
                        using (MemoryStream ms = new MemoryStream())
                        {
                            AttachmentUserCopy.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        QuoteObject.File = AttachmentUserCopy.FileName;

                    }
                    QuoteObject.UserName = Name;
                    QuoteObject.Email = Email;
                    QuoteObject.PhoneNumber = Phone;
                    QuoteObject.Quantity = Convert.ToInt32(Quantity);
                    QuoteObject.Stock = Stock;
                    QuoteObject.Color = Color;
                    QuoteObject.Height = Convert.ToInt32(Height);
                    QuoteObject.Width = Convert.ToInt32(Width);
                    QuoteObject.Depth = Convert.ToInt32(Depth);
                    QuoteObject.Comments = Comments;
                    QuoteObject.SubmittedTime = DateTime.Now;
                    QuoteObject.IsDeleted = false;
                    QuoteObject.SeenTime = null;
                    QuoteObject.Seen = false;

                    db.PriceQuotes.Add(QuoteObject);
                    db.SaveChanges();
                }
                catch (Exception ex)
                { }
                


                var fromAddress = new MailAddress(SenderEmailId, "Quote Request: " + Name);
                var toAddress = new MailAddress(Email, "Quote Request" + Name);
                string fromPassword = SenderEmailPassword;
                string subject = "Quote Request for Product " + ProductName;
                string body = "Name: " + Name + "<br>Phone: " + Phone + "<br>" + "Email: " + Email + "<br>" + "Product Details: " + ProductName + "<br>Stock: " + Stock + "<br>Color: " + Color + "<br>Quantity: " + Quantity + "<br>Dimensions: " + Height + "x" + Width + "x" + Depth + "<br>Comments: " + Comments + "<br>Time: " + DateTime.Now;

                var smtp = new SmtpClient
                {
                    Host = SenderEmailHost,
                    Port = SenderEmailPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body
                })
                {

                    if (File_input != null)

                    {

                        string fileName = Path.GetFileName(File_input.FileName);
                        File_input.InputStream.Seek(0, SeekOrigin.Begin);
                        message.Attachments.Add(new Attachment(File_input.InputStream, fileName, MediaTypeNames.Application.Octet));
                        message.Bcc.Add(SenderEmailId);
                    }

                    smtp.Send(message);
                }


                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                // var fromSiteAddress = new MailAddress(SenderEmailId, "Print My Box");
                //var toCustomerAddress = new MailAddress(Email, Name);
                //string fromSitePassword = SenderEmailPassword;
                //subject = "Quote Request Confirmation";
                //body = "Your Following Quote Request Submitted, Thankyou for your precious time <br><br><b>Quote Request</b><br>Name: " + Name + "<br>Phone: " + Phone + " <br> " + "Email: " + Email + " <br> " + "Product Details: " + ProductName + " <br> Stock: " + Stock + " <br> Color: " + Color + " <br> Quantity: " + Quantity + " <br> Dimensions: " + Height +"x"+ Width +"x" + Depth + " <br> Time: " + DateTime.Now;
                //HttpPostedFileBase File = null;
                //if (Request.Files.Count > 0)
                //{
                //    File = Request.Files[0];
                //}

                //var smtp1 = new SmtpClient
                //{
                //    Host = SenderEmailHost,
                //    Port = SenderEmailPort,
                //    EnableSsl = true,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    Credentials = new NetworkCredential(fromSiteAddress.Address, fromSitePassword),
                //    Timeout = 20000
                //};
                //using (var message1 = new MailMessage(fromSiteAddress, toCustomerAddress)
                //{
                //    IsBodyHtml = true,
                //    Subject = subject,
                //    Body = body
                //})
                //{

                //    if (AttachmentUserCopy != null)

                //    {

                //        string fileName = Path.GetFileName(AttachmentUserCopy.FileName);
                //        AttachmentUserCopy.InputStream.Seek(0, SeekOrigin.Begin);
                //        message1.Attachments.Add(new Attachment(AttachmentUserCopy.InputStream, fileName, MediaTypeNames.Application.Octet));

                //    }
                //    smtp1.Send(message1);
                //}


                //return new FilePathResult(ViewPath, "text/html");
                var smtp1 = new SmtpClient
                {
                    Host = SenderEmailHost,
                    Port = SenderEmailPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };

                using (var message1 = new MailMessage(SenderEmailId, SenderEmailId)
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body
                })
                {

                    if (File_input != null)

                    {

                        string fileName = Path.GetFileName(File_input.FileName);
                        File_input.InputStream.Seek(0, SeekOrigin.Begin);
                        message1.Attachments.Add(new Attachment(File_input.InputStream, fileName, MediaTypeNames.Application.Octet));

                    }

                    smtp1.Send(message1);
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
                //return Json("Something Went Wrong!", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}