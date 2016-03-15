using GeoAPI.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using System.Text;

namespace Crate.Client
{
	public class SqlRequest
	{
		public string stmt { get; set; }
		public object[] args { get; set; }
        public object[][] bulk_args { get; set; }

        public SqlRequest () {}

		public SqlRequest(string statement, params object[] args) {
			this.stmt = statement;
			this.args = args;
		}

        public SqlRequest(string statement, params object[][] args)
        {
            this.stmt = statement;
            this.bulk_args = args;
        }

        internal string ToJSON()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"stmt\":");
            sb.Append(JsonConvert.SerializeObject(stmt));

            if (bulk_args != null)
            {
                sb.Append(",\"bulk_args\":[");
                bool firstParamAdded = false;

                foreach (var arg in bulk_args)
                {
                    if (firstParamAdded)
                    {
                        sb.Append(",");
                    }

                    sb.Append("[");
                    AppendArgs(arg, sb);
                    sb.Append("]");

                    firstParamAdded = true;
                }
                sb.Append("]");
            }
            else
            {
                sb.Append(",\"args\":[");
                AppendArgs(args, sb);
                sb.Append("]");
            }

            sb.Append("}");
            return sb.ToString();
        }

        private void AppendArgs(object[] arguments, StringBuilder sb)
        {
            bool firstParamAdded = false;

            foreach (var param in arguments)
            {
                if (firstParamAdded)
                {
                    sb.Append(",");
                }

                if (param is IGeometry)
                {
                    sb.Append(new GeoJsonWriter().Write(param));
                }
                else
                {
                    sb.Append(JsonConvert.SerializeObject(param));
                }

                firstParamAdded = true;
            }
        }
    }
}
