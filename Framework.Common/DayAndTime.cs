using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common
{
    public class DayAndTimeModel
    {
        public int Value { get; set; }
        public String Display { get; set; }
    }
    public class DayAndTime
    {
        static DayAndTime()
        {
            List<DayAndTimeModel> dayOfWeeks = new List<DayAndTimeModel>();
            DayOfWeeks = dayOfWeeks;
            dayOfWeeks.Add(new DayAndTimeModel()
            {
                Value = 1,
                Display = "Sunday"
            });
            dayOfWeeks.Add(new DayAndTimeModel()
            {
                Value = 2,
                Display = "Monday"
            });
            dayOfWeeks.Add(new DayAndTimeModel()
            {
                Value = 3,
                Display = "Tuesday"
            });
            dayOfWeeks.Add(new DayAndTimeModel()
            {
                Value = 4,
                Display = "Wednesday"
            });
            dayOfWeeks.Add(new DayAndTimeModel()
            {
                Value = 5,
                Display = "Thursday"
            });
            dayOfWeeks.Add(new DayAndTimeModel()
            {
                Value = 6,
                Display = "Friday"
            });
            dayOfWeeks.Add(new DayAndTimeModel()
            {
                Value = 7,
                Display = "Saturday"
            });


            Hours = new List<DayAndTimeModel>();

            for (int i = 0; i < 24; i++)
            {
                Hours.Add(new DayAndTimeModel()
                {
                    Value = i,
                    Display = i + " h"
                });
            }
        }
        public static List<DayAndTimeModel> DayOfWeeks { get; set; }
        public static List<DayAndTimeModel> Hours { get; set; }

        public static String TimeDisplay(float time)
        {
            var hour = (int)time;
            var minute = (time - hour) * 60;

            if (hour > 12)
            {
                if (minute > 0)
                {
                    return hour % 12 + " : " + minute + " pm";
                }
                else
                {
                    return hour % 12 + " pm";
                }
            }
            else
            {
                if (minute > 0)
                {
                    return hour + " : " + minute + " am";
                }
                else
                {
                    return hour + " am";
                }
            }
        }

    }
}
