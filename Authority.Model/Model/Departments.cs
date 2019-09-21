using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Authority.Model.Model
{
   public class Departments
    {

        [Key]
        public int Id { get; set; }

        public string DepartmentName { get; set; }

        public int Count { get; set; }



    }
}
