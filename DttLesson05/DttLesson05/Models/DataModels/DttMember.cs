using Microsoft.AspNetCore.Mvc;

namespace DttLesson05.Models.DataModels
{
    public class DttMember 
    {
        public string DttMemberId { get; set; }
        public string DttUsername { get; set; }
        public string DttFullName { get; set; }

        public string DttPassword { get; set; }
        public string DttEmail { get; set; }
    }
}
