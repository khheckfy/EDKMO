using System;
using System.Text;

namespace EDKMO.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static string CreateExceptionDump(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            Exception currEx = ex;
            int i = 0;
            while (currEx != null)
            {
                sb.AppendFormat("Deep: {0}. Type: {1}. Source: {2}\n", i++, currEx.GetType().ToString(), currEx.Source);
                sb.AppendFormat("Message: {0}\n", currEx.Message);
                sb.AppendFormat("StackTrace: \n{0}", currEx.StackTrace);
                sb.AppendLine();
                sb.AppendLine("**************************************************************");
                sb.AppendLine();
                sb.AppendLine();
                currEx = currEx.InnerException;
            }
            return sb.ToString();
        }
    }
}
