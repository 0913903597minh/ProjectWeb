using ProjectWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Service
{
    public class UserService 
    {
        ProjectWebEntities db = new ProjectWebEntities();
        public ProjectWeb.User Login(string email, string password)
        {
            try
            {
                var userInfo = db.Users.Where(x =>(x.user_email.Equals(email) || x.user_name.Equals(email)) && ( x.is_delete == false || x.is_delete == null)).FirstOrDefault();
                if (userInfo != null)
                {
                    string stringPwd = userInfo.user_password.Trim();
                    string passwordHash = Utilities.SecurityHelper.sha256Hash(userInfo.user_password.Trim());
                    if (password == stringPwd)
                    {
                        return userInfo;
                    }
                    else if (passwordHash.Equals(password))
                    {
                        userInfo.user_password = password;
                        db.SaveChanges();
                        return userInfo;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool Insert(User model)
        {
            try
            {
                db.Users.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Update(User model)
        {
            try
            {
                User user = GetById(model.user_id);
                user.user_name = model.user_name;
                user.user_birthday = model.user_birthday;
                user.user_phone = model.user_phone;
                user.user_address = model.user_address;
                user.user_email = model.user_email;
                user.user_gender = model.user_gender;

                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Update()
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool UpdatePassword(User model)
        {
            try
            {
                User user = GetById(model.user_id);
                user.user_password = model.user_password;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                User user = GetById(id);
                user.is_delete = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public User GetById(int id)
        {
            try
            {
                return db.Users.Find(id);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public List<User> GetAllByPhone(string phone)
        {
            return db.Users.Where(x => x.user_phone.Contains(phone)).ToList();
        }
        public List<User> GetAll()
        {
            try
            {
                return db.Users.Where(x => x.is_delete == false || x.is_delete == null).OrderByDescending(x => x.user_id).ToList();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        //public List<User> GetAllUser(int skip, int size)
        //{
        //    try
        //    {
        //        return db.Users.Where(x => x.is_delete == false || x.is_delete == null).OrderByDescending(x => x.user_id).Skip(skip).Take(size).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }

        //}
        //public List<User> GetAllUserByRole(int role, int skip, int size)
        //{
        //    try
        //    {
        //        return db.Users.Where(x => (x.is_delete == false || x.is_delete == null) && x.user_role_id == role).OrderByDescending(x => x.user_id).Skip(skip).Take(size).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }

        //}
        //public List<User> GetAllUserByRoleAndKey(string key, int role, int skip, int size)
        //{
        //    try
        //    {
        //        return db.Users.Where(x => (x.IsDelete == 0 || x.IsDelete == null) && (x.Phone.Contains(key)) && x.Role == role).OrderByDescending(x => x.CreateDate).Skip(skip).Take(size).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }

        //}
        ////Count
        //public int CountAll()
        //{
        //    try
        //    {
        //        return db.Users.Count(x => x.IsDelete == 0 || x.IsDelete == null);
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }

        //}
        //public int CountAllUserByRole(int role)
        //{
        //    try
        //    {
        //        return ApplicationDbContext.Users.Count(x => (x.IsDelete == 0 || x.IsDelete == null) && x.Role == role);
        //    }
        //    catch (Exception e)
        //    {

        //        return 0;
        //    }

        //}
        //public int CountAllUserByRoleAndKey(string key, int role)
        //{
        //    try
        //    {
        //        return ApplicationDbContext.Users.Count(x => (x.IsDelete == 0 || x.IsDelete == null) && (x.Phone.Contains(key)) && x.Role == role);
        //    }
        //    catch (Exception e)
        //    {
        //        return 0;
        //    }

        //}

        //Check
        public bool CheckExistUserEmail(User model)
        {
            try
            {
                return db.Users.Count(x => x.user_email == model.user_email && (x.is_delete == false || x.is_delete == null) && x.user_id != model.user_id)>0;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public bool CheckExistPhone(User model)
        {
            try
            {
                var user = db.Users.First(x => x.user_phone == model.user_phone && (x.is_delete == false || x.is_delete == null) && x.user_id != model.user_id);
                if (user != null)
                    return true;
                else return false;
            }
            catch (Exception e)
            {

                return false;
            }

        }
    }
}
