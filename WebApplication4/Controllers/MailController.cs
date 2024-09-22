using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendEmail(EmailModel model, HttpPostedFileBase Attachment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var fromAddress = new MailAddress(model.From);
                    var toAddress = new MailAddress(model.To);
                    string fromPassword = "wjfz kosd tgpu spnx"; // Use App Password for Gmail

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com", // Use the correct SMTP host
                        Port = 587, // Use port 587 for Gmail
                        EnableSsl = true, // Enable SSL
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };

                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = model.Subject,
                        Body = model.Notes
                    })
                    {
                        if (Attachment != null && Attachment.ContentLength > 0)
                        {
                            var attachment = new Attachment(Attachment.InputStream, Attachment.FileName);
                            message.Attachments.Add(attachment);
                        }

                        smtp.Send(message);
                    }

                    ViewBag.Message = "Email sent successfully!";
                }
                catch (SmtpException smtpEx)
                {
                    ViewBag.Message = "SMTP Error: " + smtpEx.Message;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error: " + ex.Message;
                }
            }

            return View("Index");
        }

    }
}