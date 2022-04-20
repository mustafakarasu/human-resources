using HumanResources.Models.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace HumanResources.Helpers
{
    public static class MailHelper
    {
        public static string CreateMailAddress(string firstName, string lastName, string parentMailAddress)
        {
            if (firstName.Contains(" "))
            {
                firstName = firstName.Replace(" ", "");
            }
            string[] parentMails = parentMailAddress.Split("@");
            string extensionMail = parentMails[1];
            string mailAddress = String.Concat(firstName, ".", lastName, "@", extensionMail).ToLower();

            return mailAddress;
        }

        public static bool SendMail(User user, Company company)
        {
            string subject = "Hesap Oluşturuldu";
            string role = "";
            string userCompany = "";
            string hireDate = "";

            if (user.HireDate.Date > DateTime.Now.Date)
                hireDate = user.HireDate.Date.ToLongDateString();
            else
                hireDate = DateTime.Now.Date.ToLongDateString();

            switch (user.RoleId)
            {
                case 1:
                    role = "site yöneticisi";
                    break;
                case 2:
                    role = "şirket yöneticisi";
                    userCompany = company.Name;
                    if (company.Title != null) userCompany += " " + company.Title;
                    userCompany += " şirketinde ";
                    break;
                case 3:
                    role = "personel";
                    userCompany = company.Name;
                    if (company.Title != null) userCompany += " " + company.Title;
                    userCompany += " şirketinde ";
                    break;
            }

            string userMessage = user.FirstName + " " + user.LastName + " </span></span></strong></span></p>              </div>              </div>              </td>              </tr>              </table>              <table border='0' cellpadding='10' cellspacing='0' class='text_block' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt;  word-break: break-word; ' width='100%'><tr><td><div style = 'font-family: sans-serif'><div style='font-size: 12px;  mso-line-height-alt: 14.399999999999999px;  color: #555555;  line-height: 1.2;  font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; '><p style = 'margin: 0;  font-size: 14px;  text-align: center; '><span style='font-size:18px; color:#000000; '>Sayın " + user.FirstName + " " + user.LastName + ", " + userCompany + role + " olarak insan kaynakları sistemimize eklenmiş bulunmaktasınız. " + hireDate + " tarihinden itibaren giriş yaparken kullanacağınız mail adresi ve geçici parolanız aşağıda belirtilmiştir. Lütfen geçici parolanızı değiştirmeyi unutmayınız!" +
                "<tr><td><div style = 'font-family: sans-serif'><div style='font-size: 12px;  mso-line-height-alt: 14.399999999999999px;  color: #555555;  line-height: 1.2;  font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; '><p style = 'margin: 0;  font-size: 14px;  text-align: start; '><span style='font-size:18px; color:#000000; '>Mail Adresi: " + user.Email + "</span></p></div></div></td></tr>" +
                "<tr><td><div style = 'font-family: sans-serif'><div style='font-size: 12px;  mso-line-height-alt: 14.399999999999999px;  color: #555555;  line-height: 1.2;  font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; '><p style = 'margin: 0;  font-size: 14px;  text-align: start; '><span style='font-size:18px; color:#000000; '>Geçici Parola: " + user.Password + "</ span ></ p ></ div ></ div ></ td ></ tr ></table>" + "<table border = '0' cellpadding= '0' cellspacing= '0' class='button_block' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt; ' width='100%'><tr><td style = 'padding-bottom:10px; padding-left:10px; padding-right:10px; padding-top:20px; text-align:center; '><div align='center'><a href = 'https://murettebatinsankaynaklari.azurewebsites.net' style='text-decoration:none; display:inline-block; color:#ffffff; background-color:#fc7318; border-radius:15px; width:auto; border-top:1px solid #fc7318; border-right:1px solid #fc7318; border-bottom:1px solid #fc7318; border-left:1px solid #fc7318; padding-top:10px; padding-bottom:10px; font-family:Lato, Tahoma, Verdana, Segoe, sans-serif; text-align:center; mso-border-alt:none; word-break:keep-all; ' target='_blank'><span style = 'padding-left:40px; padding-right:40px; font-size:16px; display:inline-block; letter-spacing:normal; '><span style='font-size: 16px;  line-height: 2;  word-break: break-word;  mso-line-height-alt: 32px; '><strong>GİRİŞ YAP</strong></span></span></a></div></td></tr></table>";

            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            MailMessage userMsj = new MailMessage();

            sc.Credentials = new NetworkCredential("ortaklargrup45@gmail.com", "45salihli45");

            userMsj.To.Add(user.Email);
            userMsj.From = new MailAddress("ortaklargrup45@gmail.com", "Murettebat İnsan Kaynakları", Encoding.UTF8);
            userMsj.Subject = subject;
            userMsj.SubjectEncoding = Encoding.UTF8;

            userMsj.AlternateViews.Add(GetHtml(Environment.CurrentDirectory + "\\wwwroot\\Images\\MailImage\\HumanResources.png", userMessage, "HOŞGELDİNİZ"));

            userMsj.BodyEncoding = Encoding.UTF8;
            userMsj.IsBodyHtml = true;

            sc.EnableSsl = true;

            try
            {
                sc.Send(userMsj);
                userMsj.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CompanyMail(Company company, User user)
        {
            string subject = "Hesap Oluşturuldu";
            string title = "";
            if (company.Title != null)
            {
                title = " " + company.Title;
            }

            string companyMessage = company.Name + title + "</span></span></strong></span></p>              </div>              </div>              </td>              </tr>              </table>              <table border='0' cellpadding='10' cellspacing='0' class='text_block' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt;  word-break: break-word; ' width='100%'><tr><td><div style = 'font-family: sans-serif'><div style='font-size: 12px;  mso-line-height-alt: 14.399999999999999px;  color: #555555;  line-height: 1.2;  font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; '><p style = 'margin: 0;  font-size: 14px;  text-align: center; '><span style='font-size:18px; color:#000000; '>" + "Aramıza hoşgeldiniz! İnsan kaynakları süreçleriniz <strong>" + user.FirstName + " " + user.LastName + "</strong> tarafından " + company.PackageStartingDate.ToLongDateString() + " ile " + company.PackageEndDate?.ToLongDateString() + " tarihleri arasında sistemimiz üzerinden yönetilecektir.";
            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            MailMessage companyMsj = new MailMessage();

            sc.Credentials = new NetworkCredential("ortaklargrup45@gmail.com", "45salihli45");

            companyMsj.To.Add(company.Email);
            companyMsj.From = new MailAddress("ortaklargrup45@gmail.com", "Murettebat İnsan Kaynakları", Encoding.UTF8);
            companyMsj.Subject = subject;
            companyMsj.SubjectEncoding = Encoding.UTF8;

            companyMsj.AlternateViews.Add(GetHtml(Environment.CurrentDirectory + "\\wwwroot\\Images\\MailImage\\HumanResources.png", companyMessage, "HOŞGELDİNİZ"));

            companyMsj.BodyEncoding = Encoding.UTF8;
            companyMsj.IsBodyHtml = true;

            sc.EnableSsl = true;

            try
            {
                sc.Send(companyMsj);
                companyMsj.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool UserLimitNoticeMail(User user, Company company)
        {
            string subject = "Kullanıcı Limit Bildirimi";
            string title = "";
            if (company.Title != null)
            {
                title = " " + company.Title;
            }

            string companyMessage = company.Name + title + "</span></span></strong></span></p>              </div>              </div>              </td>              </tr>              </table>              <table border='0' cellpadding='10' cellspacing='0' class='text_block' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt;  word-break: break-word; ' width='100%'><tr><td><div style = 'font-family: sans-serif'><div style='font-size: 12px;  mso-line-height-alt: 14.399999999999999px;  color: #555555;  line-height: 1.2;  font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; '><p style = 'margin: 0;  font-size: 14px;  text-align: center; '><span style='font-size:18px; color:#000000; '>Sayın <strong>" + user.FirstName + " " + user.LastName + "</strong>, " + company.Name + " " + title + " tarafından satın alınan " + company.PackageNumberOfUsers + " kullanıcı limitli " + company.PackageName + " isimli paketinizin %80'ini kullandığınızı bildiririz. Kalan kullanımlarınızı sistem üzerinden görüntüleyebilirsiniz.";

            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            MailMessage companyMsj = new MailMessage();

            sc.Credentials = new NetworkCredential("ortaklargrup45@gmail.com", "45salihli45");

            companyMsj.To.Add(user.Email);
            companyMsj.From = new MailAddress("ortaklargrup45@gmail.com", "Murettebat İnsan Kaynakları", Encoding.UTF8);
            companyMsj.Subject = subject;
            companyMsj.SubjectEncoding = Encoding.UTF8;

            companyMsj.AlternateViews.Add(GetHtml(Environment.CurrentDirectory + "\\wwwroot\\Images\\MailImage\\HumanResources.png", companyMessage, "Limit Bildirimi"));

            companyMsj.BodyEncoding = Encoding.UTF8;
            companyMsj.IsBodyHtml = true;

            sc.EnableSsl = true;

            try
            {
                sc.Send(companyMsj);
                companyMsj.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool EndDateNoticeMail(User user, Company company)
        {
            string subject = "Paket Bitiş Tarihi Bildirimi";
            string title = "";
            if (company.Title != null)
            {
                title = " " + company.Title;
            }

            string companyMessage = company.Name + title + "</span></span></strong></span></p>              </div>              </div>              </td>              </tr>              </table>              <table border='0' cellpadding='10' cellspacing='0' class='text_block' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt;  word-break: break-word; ' width='100%'><tr><td><div style = 'font-family: sans-serif'><div style='font-size: 12px;  mso-line-height-alt: 14.399999999999999px;  color: #555555;  line-height: 1.2;  font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; '><p style = 'margin: 0;  font-size: 14px;  text-align: center; '><span style='font-size:18px; color:#000000; '>Sayın <strong>" + user.FirstName + " " + user.LastName + "</strong>, " + company.Name + " " + title + " tarafından satın alınan " + company.PackageStartingDate.ToLongDateString() + " başlangıç tarihli " + company.PackageName + " isimli paketinizin " + company.PackageEndDate?.ToLongDateString() + " olan paket bitiş tarihinin yaklaştığını bildiririz. Kalan kullanımlarınızı sistem üzerinden görüntüleyebilirsiniz.";

            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            MailMessage companyMsj = new MailMessage();

            sc.Credentials = new NetworkCredential("ortaklargrup45@gmail.com", "45salihli45");

            companyMsj.To.Add(user.Email);
            companyMsj.From = new MailAddress("ortaklargrup45@gmail.com", "Murettebat İnsan Kaynakları", Encoding.UTF8);
            companyMsj.Subject = subject;
            companyMsj.SubjectEncoding = Encoding.UTF8;
            
            companyMsj.AlternateViews.Add(GetHtml(Environment.CurrentDirectory + "\\wwwroot\\Images\\MailImage\\HumanResources.png", companyMessage, "Paket Bildirimi"));

            companyMsj.BodyEncoding = Encoding.UTF8;
            companyMsj.IsBodyHtml = true;

            sc.EnableSsl = true;

            try
            {
                sc.Send(companyMsj);
                companyMsj.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static AlternateView GetHtml(string filePath, string message, string head)
        {
            LinkedResource linkedResource = new LinkedResource(filePath);
            linkedResource.ContentId = Guid.NewGuid().ToString();
            string htmlBody = "<!DOCTYPE html> <html lang = 'en' xmlns: o = 'urn:schemas-microsoft-com:office:office' xmlns: v = 'urn:schemas-microsoft-com:vml'>       <head>       <title></title>       <meta content = 'text/html;  charset=utf-8' http-equiv = 'Content-Type'/>          <meta content = 'width=device-width, initial-scale=1.0' name = 'viewport'/> <style>        * {                box-sizing: border-box;             }            body {            margin: 0;             padding: 0;             }            a[x-apple-data-detectors] {            color: inherit!important;                 text-decoration: inherit!important;             } # MessageViewBody a {        color: inherit;             text-decoration: none;         }        p {			line-height: inherit    }    @media(max-width:670px)    {			.icons-inner {            text-align: center;         }			.icons-inner td {        margin: 0 auto;         }			.row-content {        width: 100 % !important;         }			.column.border {        display: none;         }			.stack.column {        width: 100 %;         display: block;         }    }	</style>         </head><body style = 'background-color: #F5F5F5;  margin: 0;  padding: 0;  -webkit-text-size-adjust: none;  text-size-adjust: none; '><table border='0' cellpadding='0' cellspacing='0' class='nl-container' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt;  background-color: #F5F5F5; ' width='100%'><tbody><tr><td><table align = 'center' border='0' cellpadding='0' cellspacing='0' class='row row-1' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt; ' width='100%'><tbody><tr><td><table align = 'center' border='0' cellpadding='0' cellspacing='0' class='row-content stack' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt;  color: #000000;  width: 650px; ' width='650'><tbody><tr><td class='column column-1' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt;  font-weight: 400;  text-align: left;  vertical-align: top;  padding-top: 5px;  padding-bottom: 5px;  border-top: 0px;  border-right: 0px;  border-bottom: 0px;  border-left: 0px; ' width='100%'><div class='spacer_block' style='height:30px; line-height:30px; font-size:1px; '> </div></td></tr></tbody></table></td></tr></tbody></table><table align = 'center' border= '0' cellpadding= '0' cellspacing= '0' class='row row-2' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt; ' width='100%'><tbody><tr><td><table align = 'center' border='0' cellpadding='0' cellspacing='0' class='row-content stack' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt;  background-color: #D6E7F0;  color: #000000;  width: 650px; ' width='650'><tbody><tr><td class='column column-1' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt;  font-weight: 400;  text-align: left;  vertical-align: top;  padding-left: 25px;  padding-right: 25px;  padding-top: 5px;  padding-bottom: 60px;  border-top: 0px;  border-right: 0px;  border-bottom: 0px;  border-left: 0px; ' width='100%'><table border = '0' cellpadding='0' cellspacing='0' class='image_block' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt; ' width='100%'><tr><td style = 'padding-top:45px; width:100%; padding-right:0px; padding-left:0px; '><div align='center' style='line-height:10px'><img alt = 'Image' src='cid:" + linkedResource.ContentId + "' style='display: block;  height: auto;  border: 0;  width: 540px;  max-width: 100%; ' title='Image' width='540'/></div></td></tr></table><table border = '0' cellpadding='0' cellspacing='0' class='text_block' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt;  word-break: break-word; ' width='100%'><tr><td style = 'padding-left:15px; padding-right:10px; padding-top:20px; '><div style='font-family: sans-serif'><div style = 'font-size: 12px;  font-family: Lato, Tahoma, Verdana, Segoe, sans-serif;  mso-line-height-alt: 18px;  color: #052d3d;  line-height: 1.5; '><p style='margin: 0;  font-size: 14px;  text-align: center;  mso-line-height-alt: 75px; '><span style = 'font-size:50px; '><strong><span style='font-size:50px; '><span style = 'font-size:38px; '> " + head + " </span></span></strong></span></p>       <p style='margin: 0;  font-size: 14px;  text-align: center;  mso-line-height-alt: 51px; '><span style = 'font-size:34px; '><strong><span style='font-size:34px; '><span style = 'color:#2190e3; font-size:34px; '> " + message + "</td></tr></tbody></table></td></tr></tbody></table><table align = 'center' border= '0' cellpadding= '0' cellspacing= '0' class='row row-3' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt; ' width='100%'><tbody><tr><td><table align = 'center' border='0' cellpadding='0' cellspacing='0' class='row-content stack' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt;  color: #000000;  width: 650px; ' width='650'><tbody><tr><td class='column column-1' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt;  font-weight: 400;  text-align: left;  vertical-align: top;  padding-top: 20px;  padding-bottom: 60px;  border-top: 0px;  border-right: 0px;  border-bottom: 0px;  border-left: 0px; ' width='100%'><table border = '0' cellpadding='10' cellspacing='0' class='text_block' role='presentation' style='mso-table-lspace: 0pt;  mso-table-rspace: 0pt;  word-break: break-word; ' width='100%'><tr><td><div style = 'font-family: sans-serif'><div style='font-size: 12px;  mso-line-height-alt: 18px;  color: #555555;  line-height: 1.5;  font-family: Lato, Tahoma, Verdana, Segoe, sans-serif; '><p style = 'margin: 0;  font-size: 14px;  text-align: center; '> Murettebat İnsan Kaynakları </p></div></div></td></tr></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></table></td></tr></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></body></html>";

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(linkedResource);
            return alternateView;
        }
    }
}
