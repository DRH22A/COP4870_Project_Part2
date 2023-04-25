using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Models
{
    public enum SeasonEnum
    {
        Spring,
        Fall,
        Summer
    }

    public enum YearEnum
    {
        Year_2018 = 2018,
        Year_2019 = 2019,
        Year_2020 = 2020,
        Year_2021 = 2021,
        Year_2022 = 2022,
        Year_2023 = 2023,
        Year_2024 = 2024,
        Year_2025 = 2025
    }

    public class Semester
    {
        public SeasonEnum Season { get; set; }
        public YearEnum Year { get; set; }
        public SeasonEnum[] Seasons => (SeasonEnum[])Enum.GetValues(typeof(SeasonEnum));
        public YearEnum[] Years => (YearEnum[])Enum.GetValues(typeof(YearEnum));

        public static Semester CurrentSemester { get; set; } = new Semester { Season = SeasonEnum.Fall, Year = YearEnum.Year_2023 };

        public override string ToString()
        {
            return $"{Season} - {Year}";
        }
    }
}
