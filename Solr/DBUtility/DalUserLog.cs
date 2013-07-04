using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solr.Log;
using Solr.MODEL;
using Solr.Config;

namespace Solr.DBUtility
{
    public class DalUserLog : BaseSQL
    {
        private DataTable GetData(int id)
        {
            try
            {
                string sql = string.Format("SELECT " + config.TopCount + " Id,[type],linkId,linkType,typeProId FROM UserLog WHERE Id>{0} ORDER BY Id ASC", id);
                return this.ExecuteDataTable(sql);
            }
            catch (Exception ex)
            {
                Log4Helper.AddError(ex);
                return null;
            }
        }
        public List<ModUserLog> GetList()
        {
            int id = Convert.ToInt32(ConfigSetting.GetValue("LastLogId"));
            DataTable dt = this.GetData(id);
            List<ModUserLog> list = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                list = new List<ModUserLog>();
                ModUserLog mod = null;
                foreach (DataRow dr in dt.Rows)
                {
                    mod = new ModUserLog();
                    mod.Id = Convert.ToInt32(dr["Id"]);
                    mod.LinkId = Convert.ToInt32(dr["LinkId"]);
                    mod.LinkType = Convert.ToByte(dr["LinkType"]);
                    mod.Type = Convert.ToByte(dr["Type"]);
                    mod.TypeProId = dr["TypeProId"].ToString();
                    list.Add(mod);
                }
            }
            return list;
        }
    }
}
