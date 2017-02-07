using System;
using System.Linq;
using asp_mvc_2.Models.DB;
using asp_mvc_2.Models.ViewModel;
using System.Collections.Generic;

namespace asp_mvc_2.Models.EntityManager
{
    public class UserManager
    {
        public void AddUserAccount(UserSignUpView user)
        {

            using (DEMODB1Entities db = new DEMODB1Entities())
            {
                SYSUser
                    SU = new SYSUser();
                SU.LoginName = user.LoginName;
                SU.PasswordEncryptedText = user.Password;
                SU.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SU.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1; ;
                SU.RowCreatedDateTime = DateTime.Now;
                SU.RowMOdifiedDateTime = DateTime.Now;

                db.SYSUser.Add(SU);
                db.SaveChanges();

                SYSUserProfile SUP = new SYSUserProfile();
                SUP.SYSUserID = SU.SYSUserID;
                SUP.FirstName = user.FirstName;
                SUP.LastName = user.LastName;
                SUP.Gender = user.Gender;
                SUP.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowCreatedDateTime = DateTime.Now; SUP.RowModifiedDateTime = DateTime.Now;

                db.SYSUserProfile.Add(SUP);
                db.SaveChanges();

                if (user.LOOKUPRoleID > 0)
                {
                    SYSUserRole SUR = new SYSUserRole();
                    SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                    SUR.SYSUserID = user.SYSUserID;
                    SUR.IsActive = true;
                    SUR.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                    SUR.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                    SUR.RowCreatedDateTime = DateTime.Now;
                    SUR.RowModifiedDateTime = DateTime.Now;

                    db.SYSUserRole.Add(SUR);
                    db.SaveChanges();
                }

            }
        }
        public bool IsLoginNameExist(string loginName)
        {
            using (DEMODB1Entities db = new DEMODB1Entities())
            {
                return db.SYSUser.Where(o => o.LoginName.Equals(loginName)).Any();
            }
        }

        public string GetUserPassword(string loginName) { using (DEMODB1Entities db = new DEMODB1Entities()) { var user = db.SYSUser.Where(o => o.LoginName.ToLower().Equals(loginName)); if (user.Any()) return user.FirstOrDefault().PasswordEncryptedText; else return string.Empty; } }

        public bool IsUserInRole(string loginName, string roleName)
        {
            using (DEMODB1Entities db = new DEMODB1Entities())
            {
                SYSUser SU = db.SYSUser.Where(o =>
                    o.LoginName.ToLower().Equals(loginName)).FirstOrDefault();
                if (SU != null)
                {
                    var roles = from q in db.SYSUserRole
                                join r in db.LOOKUPRole on q.LOOKUPRoleID equals r.LOOKUPRoleID
                                where r.RoleName.Equals(roleName) &&
                                q.SYSUserID.Equals(SU.SYSUserID)
                                select r.RoleName;

                    if (roles != null)
                    {
                        return roles.Any();
                    }
                } return false;
            }



        }
        public List<UserLoginView.LOOKUPAvailableRole> GetAllRoles()
        {
            using (DEMODB1Entities db = new DEMODB1Entities())
            {
                var roles = db.LOOKUPRole.Select(o => new UserLoginView.LOOKUPAvailableRole
                {
                    LOOKUPRoleID = o.LOOKUPRoleID,
                    RoleName = o.RoleName,
                    RoleDescription = o.RoleDescription
                }).ToList();
                return roles;
            }
        }
        public int GetUserID(string loginName)
        {
            using (DEMODB1Entities db = new DEMODB1Entities())
            {
                var user = db.SYSUser.Where(o => o.LoginName.Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().SYSUserID;
            }
            return 0;
        }
        public List<UserLoginView.UserProfileView> GetAllUserProfiles()
        {
            List<UserLoginView.UserProfileView> profiles = new List<UserLoginView.UserProfileView>();
            using (DEMODB1Entities db = new DEMODB1Entities())
            {
                UserLoginView.UserProfileView UPV;
                var users = db.SYSUser.ToList();
                foreach (SYSUser u in db.SYSUser)
                {
                    UPV = new UserLoginView.UserProfileView();
                    UPV.SYSUserID = u.SYSUserID;
                    UPV.LoginName = u.LoginName;
                    UPV.Password = u.PasswordEncryptedText;
                    var SUP = db.SYSUserProfile.Find(u.SYSUserID);
                    if (SUP != null)
                    {
                        UPV.FirstName = SUP.FirstName;
                        UPV.LastName = SUP.LastName;
                        UPV.Gender = SUP.Gender;
                    }
                    var SUR = db.SYSUserRole.Where(o => o.SYSUserID.Equals(u.SYSUserID));
                    if (SUR.Any())
                    {
                        var userRole = SUR.FirstOrDefault();
                        UPV.LOOKUPRoleID = userRole.LOOKUPRoleID;
                        UPV.RoleName = userRole.LOOKUPRole.RoleName;
                        UPV.IsRoleActive = userRole.IsActive;
                    }
                    profiles.Add(UPV);
                }
            }
            return profiles;
        }
        public UserLoginView.UserDataView GetUserDataView(string loginName)
        {
            UserLoginView.UserDataView UDV = new UserLoginView.UserDataView();
            List<UserLoginView.UserProfileView> profiles = GetAllUserProfiles();
            List<UserLoginView.LOOKUPAvailableRole> roles = GetAllRoles();
            int? userAssignedRoleID = 0, userID = 0;
            string userGender = string.Empty;
            userID = GetUserID(loginName);
            using (DEMODB1Entities db = new DEMODB1Entities())
            {
                userAssignedRoleID = db.SYSUserRole.Where(o => o.SYSUserID == userID).FirstOrDefault().LOOKUPRoleID;
                userGender = db.SYSUserProfile.Where(o => o.SYSUserID == userID).FirstOrDefault().Gender;
            }
            List<UserLoginView.Gender> genders = new List<UserLoginView.Gender>();
            genders.Add(new UserLoginView.Gender { Text = "Male", Value = "M" });
            genders.Add(new UserLoginView.Gender { Text = "Female", Value = "F" });
            UDV.UserProfile = profiles;
            UDV.UserRoles = new UserLoginView.UserRoles { SelectedRoleID = userAssignedRoleID, UserRoleList = roles };
            UDV.UserGender = new UserLoginView.UserGender { SelectedGender = userGender, Gender = genders };
            return UDV;
        }
        public void UpdateUserAccount(UserLoginView.UserProfileView user)
        {
            using (DEMODB1Entities db = new DEMODB1Entities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        SYSUser SU = db.SYSUser.Find(user.SYSUserID);
                        SU.LoginName = user.LoginName;
                        SU.PasswordEncryptedText = user.Password;
                        SU.RowCreatedSYSUserID = user.SYSUserID;
                        SU.RowModifiedSYSUserID = user.SYSUserID;
                        SU.RowCreatedDateTime = DateTime.Now;
                        SU.RowMOdifiedDateTime = DateTime.Now;
                        db.SaveChanges();
                        var userProfile = db.SYSUserProfile.Where(o => o.SYSUserID == user.SYSUserID);
                        if (userProfile.Any())
                        {
                            SYSUserProfile SUP = userProfile.FirstOrDefault();
                            SUP.SYSUserID = SU.SYSUserID;
                            SUP.FirstName = user.FirstName;
                            SUP.LastName = user.LastName;
                            SUP.Gender = user.Gender;
                            SUP.RowCreatedSYSUserID = user.SYSUserID;
                            SUP.RowModifiedSYSUserID = user.SYSUserID;
                            SUP.RowCreatedDateTime = DateTime.Now;
                            SUP.RowModifiedDateTime = DateTime.Now;
                            db.SaveChanges();
                        }
                        if (user.LOOKUPRoleID > 0)
                        {
                            var userRole = db.SYSUserRole.Where(o => o.SYSUserID == user.SYSUserID);
                            SYSUserRole SUR = null;
                            if (userRole.Any())
                            {
                                SUR = userRole.FirstOrDefault();
                                SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                                SUR.SYSUserID = user.SYSUserID;
                                SUR.IsActive = true;
                                SUR.RowCreatedSYSUserID = user.SYSUserID;
                                SUR.RowModifiedSYSUserID = user.SYSUserID;
                                SUR.RowCreatedDateTime = DateTime.Now;
                                SUR.RowModifiedDateTime = DateTime.Now;
                            }
                            else
                            {
                                SUR = new SYSUserRole();
                                SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                                SUR.SYSUserID = user.SYSUserID;
                                SUR.IsActive = true;
                                SUR.RowCreatedSYSUserID = user.SYSUserID;
                                SUR.RowModifiedSYSUserID = user.SYSUserID;
                                SUR.RowCreatedDateTime = DateTime.Now;
                                SUR.RowModifiedDateTime = DateTime.Now;
                                db.SYSUserRole.Add(SUR);
                            }
                            db.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }

            }

        }
        public void DeleteUser(int userID)
        {
            using (DEMODB1Entities db = new DEMODB1Entities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var SUR = db.SYSUserRole.Where(o => o.SYSUserID == userID);
                        if (SUR.Any())
                        {
                            db.SYSUserRole.Remove(SUR.FirstOrDefault());
                            db.SaveChanges();
                        }
                        var SUP = db.SYSUserProfile.Where(o => o.SYSUserID == userID);
                        if (SUP.Any())
                        {
                            db.SYSUserProfile.Remove(SUP.FirstOrDefault());
                            db.SaveChanges();
                        }
                        var SU = db.SYSUser.Where(o => o.SYSUserID == userID);
                        if (SU.Any())
                        {
                            db.SYSUser.Remove(SU.FirstOrDefault());
                            db.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }




    }


}




