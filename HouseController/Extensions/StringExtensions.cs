using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseController.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Converts a string to bytes for send a string with the socket
		/// </summary>
		/// <param name="message">Message to convert</param>
		/// <returns>The Message in bytes</returns>
		public static byte[] EncodeMessage(this string message)
		{
			return Encoding.UTF8.GetBytes(message);
		}

		/// <summary>
		/// Converts bytes to a string to read a data received from the socket
		/// </summary>
		/// <param name="message">Data in bytes</param>
		/// <returns>The message in String</returns>
		public static string DecodeMessage(this byte[] message, int buffer)
		{
			//Get the string from the first element until the length of the data less 1 cause of the intial element 0
			//We delete possible whitespaces with Trim method
			return Encoding.UTF8.GetString(message, 0, buffer).Trim();
		}
	}
}
