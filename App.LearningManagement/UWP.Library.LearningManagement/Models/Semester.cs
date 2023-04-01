using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Models
{
    public enum SeasonEnum
    {
        Fall,
        Spring,
        Summer
    }

    public enum YearEnum
    {
        Year_2023 = 2023,
        Year_2024 = 2024,
        Year_2025 = 2025,
        Year_2026 = 2026
    }


    public class Semester
    {
        public SeasonEnum Season { get; set; }
        public YearEnum Year { get; set; }

        public override string ToString()
        {
            return $"{Season} - {Year}";
        }
    }
}
