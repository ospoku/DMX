using Microsoft.AspNetCore.Identity;

namespace DMX.Models
{
    public class AppUser:IdentityUser
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }

        public string Fullname
        {
            get
            {
                return Firstname
                    + "  "
                    + Surname; } }


    
        public bool IsDeleted { get; set; }
   
       
    }

    
}
