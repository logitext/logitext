using System;
using System.Collections.Generic;
using System.Text;

namespace Scraper
{
    class IsbnConverter
	{
	    /// <summary>
	    /// Converts ISBN13 code to ISBN10 code
	    /// </summary>
	    /// <param name="isbn13">code to convert
	    /// <returns>empty if the parameter is invalid, otherwise the converted value</returns>
	    public static string ConvertTo10(string isbn13)
	    {
	        string isbn10 = string.Empty;
	        long temp;
	 
	        // *************************************************
	        // Validation of isbn13 code can be done by        *
	        // using this snippet found here:                  *
	        // http://www.dreamincode.net/code/snippet5385.htm *
	        // *************************************************
	 
	        if (!string.IsNullOrEmpty(isbn13) &&
	            isbn13.Length == 13 &&
	            Int64.TryParse(isbn13, out temp))
	        {
	            isbn10 = isbn13.Substring(3, 9);
	            int sum = 0;
	            for (int i = 0; i< 9; i++)
	                sum += Int32.Parse(isbn10[i].ToString()) * (i + 1);
	 
	            int result = sum % 11;
	            char checkDigit = (result > 9) ? 'X' : result.ToString()[0];
	            isbn10 += checkDigit;
	        }
	 
	        return isbn10;
	    }
	 
	    /// <summary>
	    /// Converts ISBN10 code to ISBN13 code
	    /// </summary>
	    /// <param name="isbn10">code to convert
	    /// <returns>empty if the parameter is invalid, otherwise the converted value</returns>
	    public static string ConvertTo13(string isbn10)
	    {
	        string isbn13 = string.Empty;
	        long temp;
	 
	        // *************************************************
	        // Validation of isbn10 code can be done by        *
	        // using this snippet found here:                  *
	        // http://www.dreamincode.net/code/snippet5385.htm *
	        // *************************************************
	 
	        if (!string.IsNullOrEmpty(isbn10) &&
	            isbn10.Length == 10 &&
	            Int64.TryParse(isbn10, out temp))
	        {
	            int result = 0;
	            isbn13 = "978" + isbn10.Substring(0, 9);
	            for (int i = 0; i<isbn13.Length; i++)
	                result += int.Parse(isbn13[i].ToString()) * ((i % 2 == 0) ? 1 : 3);
	             
	            int checkDigit = 10 - (result % 10);
	            isbn13 += checkDigit.ToString();
	        }
	 
	        return isbn13;
	    }
	}
}
