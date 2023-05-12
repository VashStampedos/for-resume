using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebWithEntity.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Не указан Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Не указан Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
    }
}
