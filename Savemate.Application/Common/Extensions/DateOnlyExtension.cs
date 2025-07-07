using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Application.Common.Extensions
{
    public static class DateOnlyExtension
    {
        public static int CalculateAge(this DateOnly dob)
        {
            var today = DateTime.Today;
            int age = today.Year - dob.Year;

            if (dob.ToDateTime(TimeOnly.MinValue) > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }


    }

}

