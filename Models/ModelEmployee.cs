using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Design_and_Supervion_Issue_Tracking.Models
{
    public class ModelEmployee
    {
        [Display(Name = "Employee Id")]
        public int Id { get; set; }
        [Display(Name = "Unit Id")]
        public int UnitId { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Position Class Name is required.")]
        public string PositionClassName { get; set; }


    }
}