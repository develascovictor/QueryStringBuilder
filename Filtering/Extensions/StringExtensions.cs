using System;
using System.Collections.Generic;
using System.Linq;

namespace Filtering.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Method to trim a string variable, also making sure if value is null, turn to string.Empty
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static string NullTrim(this string stringValue)
        {
            return (stringValue ?? string.Empty).Trim();
        }

        /// <summary>
        /// Method to concatenate an int array's elements into single string and separate them by commas ","
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToCommaFormat(this IEnumerable<int> array)
        {
            var output = (array ?? new List<int>()).Select(x => x.ToString()).ToCommaFormat();
            return output;
        }

        /// <summary>
        /// Method to concatenate an long array's elements into single string and separate them by commas ","
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToCommaFormat(this IEnumerable<long> array)
        {
            var output = (array ?? new List<long>()).Select(x => x.ToString()).ToCommaFormat();
            return output;
        }

        /// <summary>
        /// Method to concatenate a string array's elements into single string and separate them by commas ","
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToCommaFormat(this IEnumerable<string> array)
        {
            const string comma = ",";
            var updatedArray = (array ?? new List<string>()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var output = updatedArray.Any() ? string.Join($"{comma} ", updatedArray) : string.Empty;

            return output;
        }

        /// <summary>
        /// Method to concatenate a string array's elements into single string and separate them by pipes "|"
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToPipeFormat(this IEnumerable<string> array)
        {
            const string pipe = "|";
            var updatedArray = (array ?? new List<string>()).Where(x => !string.IsNullOrWhiteSpace(x) && !x.Contains(pipe)).ToList();
            var output = updatedArray.Any() ? string.Join(pipe, updatedArray) : null;

            return output;
        }

        /// <summary>
        /// Method to concatenate an int array's elements into single string and list them with bullets
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToBulletFormat(this IEnumerable<int> array)
        {
            var output = (array ?? new List<int>()).Select(x => x.ToString()).ToBulletFormat();
            return output;
        }

        /// <summary>
        /// Method to concatenate a long array's elements into single string and list them with bullets
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToBulletFormat(this IEnumerable<long> array)
        {
            var output = (array ?? new List<long>()).Select(x => x.ToString()).ToBulletFormat();
            return output;
        }

        /// <summary>
        /// Method to concatenate a string array's elements into single string and list them with bullets
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToBulletFormat(this IEnumerable<string> array)
        {
            const string bullet = "\n- ";
            var updatedArray = (array ?? new List<string>()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var output = updatedArray.Any() ? updatedArray.Aggregate(string.Empty, (current, item) => $"{current}{bullet}{item.Replace(bullet, "* ").Trim()}") : string.Empty;

            return output;
        }

        /// <summary>
        /// Generate a random string using a provided length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            if (length < 0)
            {
                length = 0;
            }

            var random = new Random();
            var output = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

            return output;
        }
    }
}