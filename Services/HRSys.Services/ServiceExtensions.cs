using HRSys.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRSys.Services
{
    public static class ServiceExtensions
    {

        /// <summary>
        /// sets IsUpdateOperation
        /// </summary>
        /// <typeparam name="T"> T must implement IUpdatableDto interface</typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static T ToUpdatable<T>(this T dto) where T : IUpdatableDto
        {
            dto.IsUpdateOperation = true;
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"> T must implement IUpdatableDto interface </typeparam>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToUpdatable<T>(this IEnumerable<T> dtos, bool excludeNew = false) where T : IUpdatableDto
        {
            foreach (var i in dtos.Where(n => !excludeNew || n.Id != 0))
                i.IsUpdateOperation = true;
            return dtos;
        }
        /// <summary>
        /// sets IsUpdateOperation
        /// </summary>
        /// <typeparam name="T"> T must implement IUpdatableDto interface</typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static T ToUpdatableGuid<T>(this T dto) where T : IGuidUpdatableDto
        {
            dto.IsUpdateOperation = true;
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"> T must implement IUpdatableDto interface </typeparam>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToUpdatableGuid<T>(this IEnumerable<T> dtos, bool excludeNew = false) where T : IGuidUpdatableDto
        {
            foreach (var i in dtos.Where(n => !excludeNew || n.Id != Guid.Empty))
                i.IsUpdateOperation = true;
            return dtos;
        }

        //public static string To12Hours(this TimeSpan timeSpan )
        //{
            
        //    var hours = timeSpan.Hours;
        //    var minutes = timeSpan.Minutes;
        //    var amPmDesignator = "AM";
        //    if (hours == 0)
        //        hours = 12;
        //    else if (hours == 12)
        //        amPmDesignator = "PM";
        //    else if (hours > 12)
        //    {
        //        hours -= 12;
        //        amPmDesignator = "PM";
        //    }
        //    var formattedTime =string.Format("{0}:{1:00} {2}", hours, minutes, amPmDesignator);

        //    return formattedTime;
        //}

    }
}
