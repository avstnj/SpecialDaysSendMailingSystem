using SpecialDaysSendMailingSystem.Model;
using System;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.ServiceModel.Activation;
using System.Data;
using System.Reflection;
using System.IO;
using System.Data.SqlClient;

namespace SpecialDaysSendMailingSystem
{
    public class General : IWindowsServiceContract
    {
        private Timer timer = null;
        public General()
        {
            this.timer = new Timer((double)Convert.ToInt32(ConfigurationManager.AppSettings["interval"]));
            this.timer.Enabled = true;
            this.timer.Elapsed += new ElapsedEventHandler(this.timer_Elapsed);
        }
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            using (var db = new GeneralContext())
            {
                List<tbl_users_detail> lstBirthDateUser = null;
                List<tbl_users_detail> lstStartWorkDateUser = null;
                List<tbl_users_detail> lstUserDetail = db.tbl_users_detail.Where(x => x.isActive == true).ToList();
                GetUserInfoBirthDateandStartWork(lstUserDetail, ref lstBirthDateUser, ref lstStartWorkDateUser);
                SendMailBirthDate(lstBirthDateUser);
                SendMailStartWork(lstStartWorkDateUser);
            }
            timer.Start();
        }

        private void GetUserInfoBirthDateandStartWork(List<tbl_users_detail> lstUserDetail, ref List<tbl_users_detail> lstBirthDateUser, ref List<tbl_users_detail> lstStartWorkDateUser)
        {
            if (lstUserDetail.Count > 0)
            {
                lstBirthDateUser = new List<tbl_users_detail>();
                lstStartWorkDateUser = new List<tbl_users_detail>();

                foreach (tbl_users_detail item in lstUserDetail)
                {
                    string dayMonthUser = item.dateofBirth != null ? item.dateofBirth.Value.ToString("dd/MM") : null;
                    string dayMonthToday = DateTime.Now.Date.ToString("dd/MM");
                    if (dayMonthUser == dayMonthToday)
                    {
                        lstBirthDateUser.Add(item);
                    }
                }
                foreach (tbl_users_detail itm in lstUserDetail)
                {
                    string dayMonthUser = itm.startDateofWork != null ? itm.startDateofWork.Value.ToString("dd/MM") : null;
                    string dayMonthToday = DateTime.Now.Date.ToString("dd/MM");
                    if (dayMonthUser == dayMonthToday)
                    {
                        lstStartWorkDateUser.Add(itm);
                    }
                }
            }
        }
        private void SendMailBirthDate(List<tbl_users_detail> lstBirthDateUser)
        {
            string[] mailaddresslist = null;
            if (lstBirthDateUser.Count > 0)
            {
                foreach (tbl_users_detail item in lstBirthDateUser)
                {
                    UserModel userModel = GetUserInfoByUserID(item.userID);
                    string mailTemplate = Template(userModel, Process.Birtdate);
                    //GetMailAddress(ref mailaddresslist);//Herkese gönderiyor...(Canlıya alınırken açılacak..)(1)
                    mailaddresslist = new string[1] { "tanju.avsar@asiselektronik.com.tr" };//Canlıya alınırken kapatılacak(2)
                    bool mailResult = MailHelper.SendEMail(mailaddresslist, "Doğum Günü Tebriki", mailTemplate);
                }
            }
        }
        private void SendMailStartWork(List<tbl_users_detail> lstStartWorkDateUser)
        {
            string[] mailaddresslist = null;
            if (lstStartWorkDateUser.Count > 0)
            {
                foreach (tbl_users_detail item in lstStartWorkDateUser)
                {
                    UserModel userModel = GetUserInfoByUserID(item.userID);
                    string mailTemplate = Template(userModel, Process.StartWork);
                    //GetMailAddress(ref mailaddresslist);//Herkese gönderiyor...(Canlıya alınırken açılacak..)(1)
                    mailaddresslist = new string[1] { "tanju.avsar@asiselektronik.com.tr" };//Canlıya alınırken kapatılacak(2)
                    bool mailResult = MailHelper.SendEMail(mailaddresslist, "İşe Başlangıç Tebriki", mailTemplate);
                }
            }
        }
        private UserModel GetUserInfoByUserID(int? userID)
        {
            UserModel userModel = null;
            using (var db = new GeneralContext())
            {
                tbl_users user = db.tbl_users.Where(x => x.user_UID == userID).FirstOrDefault();
                userModel = new UserModel()
                {
                    UserEmail = user.user_email,
                    UserName = user.user_FName + " " + user.user_LName,
                    UserPhone = user.user_mobile
                };
            }
            return userModel;
        }

        public void OnStart(string[] args)
        {
            this.timer.Start();
        }
        public void OnStop()
        {
            this.timer.Stop();
        }

        public string Template(UserModel model, Process process)
        {
            string messagecenter = null;
            StringBuilder sbb = new StringBuilder();
            try
            {
                switch (process)
                {
                    case Process.Birtdate:

                        sbb.Append("<html><body><strong>Doğum Günü Tebriki</strong><br><br>");

                        sbb.Append("<strong>Kişisel Bilgiler:</strong>  <br>");
                        sbb.Append("<em>İsim-Soyisim: </em><strong>" + model.UserName + "</strong><br>");
                        sbb.Append("<em>Cep Telefonu: </em><strong>" + model.UserPhone + "</strong><br>");
                        sbb.Append("<em>E-Posta Adresi: </em><strong>" + model.UserEmail + "</strong><br><br></body></html>");

                        messagecenter = System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailTemplate.html"));
                        messagecenter = messagecenter.Replace("#content#", sbb.ToString()).Replace("#name#", model.UserName);

                        break;
                    case Process.StartWork:
                        sbb.Append("<html><body><strong>işe Başlangıç Tebriki</strong><br><br>");

                        sbb.Append("<strong>Kişisel Bilgiler:</strong>  <br>");
                        sbb.Append("<em>İsim-Soyisim: </em><strong>" + model.UserName + "</strong><br>");
                        sbb.Append("<em>Cep Telefonu: </em><strong>" + model.UserPhone + "</strong><br>");
                        sbb.Append("<em>E-Posta Adresi: </em><strong>" + model.UserEmail + "</strong><br><br></body></html>");

                        messagecenter = System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailTemplate.html"));
                        messagecenter = messagecenter.Replace("#content#", sbb.ToString()).Replace("#name#", model.UserName);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                string ddd = ex.Message;
            }
            return messagecenter;
        }
        private void GetMailAddress(ref string[] mailaddresslist)
        {
            using (var db = new GeneralContext())
            {
                SqlParameter category = new SqlParameter("@CHANNEL_CD", "cPortal");
                List<UserEmailModel> userEmails = db.Database.SqlQuery<UserEmailModel>("usp_selectUsersInfo @CHANNEL_CD", category).ToList();
                if (userEmails.Count > 0)
                {
                    string[] mailaddresslst = new string[userEmails.Count];
                    for (int i = 0; i < userEmails.Count; i++)
                    {
                        mailaddresslst[i] = userEmails[i].user_email;
                    }
                    mailaddresslist = mailaddresslst;
                }
            }
        }
    }
    public class UserModel
    {
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
    }
    public class UserEmailModel
    {
        public string user_email { get; set; }
    }
    public enum Process
    {
        Birtdate = 1,
        StartWork = 2
    }
}
