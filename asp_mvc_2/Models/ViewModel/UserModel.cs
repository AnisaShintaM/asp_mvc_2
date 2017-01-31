using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; 

namespace asp_mvc_2.Models.ViewModel
{
    public class UserModel
    {
    }

    public class UserLoginView { [Key] public int SYSUserID { get; set; } [Required(ErrorMessage = "*")] [Display(Name = "Login ID")] public string LoginName { get; set; } [Required(ErrorMessage = "*")] [DataType(DataType.Password)] [Display(Name = "Password")] public string Password { get; set; } }
}