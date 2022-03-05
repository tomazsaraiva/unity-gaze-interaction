#region Includes
using System.Text;

using UnityEngine;
#endregion

namespace WVF.Extensions
{
    public static class WWWExtensions
    {
        private const string KEY_VALUE_FORMAT = "\n{0} : {1}";

        public static string Print(this WWWForm form)
        {
            var builder = new StringBuilder();

            if (form.headers?.Count > 0)
            {
                builder.Append("Headers");
                foreach (var header in form.headers.Keys)
                {
                    builder.AppendFormat(KEY_VALUE_FORMAT, header, form.headers[header]);
                }
            }

            if (form.data?.Length > 0)
            {
                builder.AppendLine("Data");
                builder.Append(Encoding.Default.GetString(form.data));
            }

            return builder.ToString();
        }
    }
}