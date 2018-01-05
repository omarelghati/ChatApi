using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Models
{
    public class User
    {
        public User()
        {
            PossibleFriends = new List<Friendship>();
            RequestSent = new List<Friendship>();
            RequestReceived = new List<Friendship>();

        }

        public long Id { get; set; }
        [Required]
        [StringLength(30)]
        public string FName { get; set; }
        [Required]
        [StringLength(30)]
        public string LName { get; set; }
        [Required]
        [StringLength(30)]
        public string Username { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Phone { get; set; }

        public DateTime DateOfJoin { get; set; }

        //[Required(ErrorMessage = "Please Upload a Valid Image File. Only jpg format allowed")]
        //[DataType(DataType.Upload)]
        //[Display(Name = "Upload Product Image")]
        //[FileExtensions(Extensions = "jpg")]
        //public IFormFile Image { get; set; }

        //public string ImageName { get; set; }
        public virtual ICollection<Friendship> PossibleFriends { get; set; }
        [NotMapped]
        public virtual ICollection<Friendship> RequestSent{ get; set; }
        [NotMapped]
        public virtual ICollection<Friendship> RequestReceived{ get; set; }

        [NotMapped]
        public string MemberSince { get; set; }
        
    }
    public static class Timer
    {
        public static string calculate(DateTime dt)
        {
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(dt);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("about {0} minutes ago", timeSpan.Minutes) :
                    "about a minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("about {0} hours ago", timeSpan.Hours) :
                    "about an hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("about {0} days ago", timeSpan.Days) :
                    "yesterday";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("about {0} months ago", timeSpan.Days / 30) :
                    "about a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("about {0} years ago", timeSpan.Days / 365) :
                    "about a year ago";
            }

            return result;
        }
    }
}