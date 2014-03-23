using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Model;
using Newtonsoft.Json.Linq;
using SQLHelper;

namespace DAL
{
    public  class Handler
    {
        /// <summary>
        /// 1会员登录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RetObj member_login(ReqModel data)
        {
            try
            {
                if (Common.Tool.filterSql(data.user) == 1)
                {
                    return new RetObj("会员卡有误！");
                }
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT id,[编号],[名称],[密码]
                FROM [dbo].[客户档案]
                with(nolock) where [编号]='" + data.user + "'");
                ReturnModel retmodel = SqlHelper.getDataTable(strSql.ToString(), "客户档案");
                if (retmodel.Succeed)
                {
                    DataTable dt = retmodel.ReturnObj as DataTable;
                    if (dt.Rows.Count > 0)
                    {
                        string pwd = dt.Rows[0]["密码"].ToString();
                        int id = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                        string userpwd = Common.Tool.GetMD5_32(id + data.pass).ToUpper();
                        RetObj ret = new RetObj(true, "登录成功！");
                        if (userpwd.Equals(pwd))
                        {
                            ret.RetDics["id"] = id;
                            ret.RetDics["m_name"] = dt.Rows[0]["名称"].ToString().Trim();
                            ret.RetDics["m_num"] = dt.Rows[0]["编号"].ToString().Trim();
                        }
                        else
                        {
                            ret.RetDics["state"] = false;
                            ret.RetDics["info"] = "密码有误！";
                        }
                        return ret;
                    }
                    return new RetObj( "会员不存在！");
                }
                else
                {
                    return new RetObj(retmodel.Message);
                }

            }
            catch (Exception ex)
            {
                return new RetObj(ex.Message);
            }
        }

        /// <summary>
        /// 2管理员登录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RetObj admin_login(ReqModel data)
        {
            try
            {
                if (Common.Tool.filterSql(data.user) == 1)
                {
                //    dynamic model2 = new JObject();
                //    model2.state = false;
                //    model2.info = "帐号有误！";
                //    return model2;
                    return new RetObj("帐号有误！");
                }
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@" select id,[姓名],[密码] from dbo.操作员档案 with(nolock)
                where [姓名]='" + data.user + "'");
                ReturnModel retmodel = SqlHelper.getDataTable(strSql.ToString(), "操作员档案");
                if (retmodel.Succeed)
                {
                    DataTable dt = retmodel.ReturnObj as DataTable;
                    if (dt.Rows.Count > 0)
                    {
                        string pwd = dt.Rows[0]["密码"].ToString();
                        int id = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                        string userpwd = Common.Tool.GetMD5_32(id + data.pass).ToUpper();
                        RetObj model = new RetObj(true,"登录成功！");
                        //dynamic model = new JObject();
                        if (userpwd.Equals(pwd))
                        {
                            model.RetDics.Add("a_id",dt.Rows[0]["id"].ToString().Trim());
                            model.RetDics.Add("a_name", dt.Rows[0]["姓名"].ToString().Trim());
                        }
                        else
                        {
                            model.RetDics["state"] = false;
                            model.RetDics["info"] = "密码有误！";
                        }
                        return model;
                    }
                 
                    return new RetObj("帐号不存在！");
                }
                else
                {
                    return new RetObj(retmodel.Message);
                }

            }
            catch (Exception ex)
            {
                return new RetObj(ex.Message);
            }
        }

        /// <summary>
        /// 3获取商品分类信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RetObj get_goods_category(ReqModel data)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@" SELECT [id] as c_id
                                  ,[名称] as c_title
                                  ,[级别] as c_level
                                  ,(select id from [商品类型] where 编号=a.[父编号]) as c_pid
                              FROM [cypf_web_test].[dbo].[商品类型] a with(nolock) ");

                //Dictionary<string,string> retdic = new   Dictionary<string,string>();
                //retdic.Add("c_id", "c_id");
                //retdic.Add("c_title", "c_title");
                //retdic.Add("c_pid", "c_pid");
                //retdic.Add("c_level", "c_level");
                //ReturnModel retmodel = SQLHelper.SqlHelper.GetDataListByString<Categorys>(strSql.ToString(), null, retdic);

                //if (retmodel.Succeed)
                //{
                //    RetObj model = new RetObj(true, "获取成功！");
                //    model.RetDics.Add("categorys", retmodel.ReturnObj);
                //    return model;
                //}
                //else
                //{
                //    return new RetObj(retmodel.Message);
                //}
                ReturnModel retmodel = SqlHelper.getDataTable(strSql.ToString(), "商品类型");
                if (retmodel.Succeed)
                {
                    DataTable dt = retmodel.ReturnObj as DataTable;
                    if (dt.Rows.Count > 0)
                    {
                        RetObj model = new RetObj(true, "获取成功！");

                        StringBuilder sb = new StringBuilder();
                        sb.Append("[");
                        foreach (DataRow row in dt.Rows)
                        {
                            sb.Append("{\"c_id\":" + row["c_id"] + ",");
                            sb.Append("\"c_title\":\"" + row["c_title"] + "\",");
                            sb.Append("\"c_pid\":" + row["c_pid"] + ",");
                            sb.Append("\"c_level\":\"" + row["c_level"] + "\"},");
                         
                        }
                    
                        string tmp = sb.ToString().Substring(0, sb.ToString().Length - 1);
                        model.RetDics["categorys"] = tmp + "]";
                          
                        return model;
                    }

                    return new RetObj("商品分类无数据！");
                }
                else
                {
                    return new RetObj(retmodel.Message);
                }

            }
            catch (Exception ex)
            {
                return new RetObj(ex.Message);
            }
        }

        /// <summary>
        /// 4获取商品列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RetObj get_goods_list(ReqModel data)
        {
            try
            {
                if (Common.Tool.filterSql(data.user) == 1)
                {
                    return new RetObj("帐号有误！");
                }
                StringBuilder strSql = new StringBuilder();
                StringBuilder strCount = new StringBuilder();
                strSql.Append(@" select * from ( select 商品档案.id as g_id,cast(updateno as bigint) as  g_img_ts,
                                (case when 库存表.数量>0 then '有货' else '缺货' end) as g_status,
                                条码 as g_num,商品档案.名称 as g_name,包装规格 g_spec,
                                件售价 g_price,散售价 g_bulk_price from  dbo.商品档案 with(nolock)
                                join 库存表 on 商品档案.id =库存表.spid
                                where 商品档案.id in (select top ");
                strSql.Append( data.page_size +  @" a.id from
                                (select top " + data.page_size * (data.page_no - 1) + @" 商品档案.id from dbo.商品档案 with(nolock)
                                join 库存表 on 商品档案.id =库存表.spid
                                left join dbo.Mall_畅销关联表 on dbo.Mall_畅销关联表.spid = 商品档案.id
                                left join dbo.Mall_畅销类型 on Mall_畅销类型.id = dbo.Mall_畅销关联表.类型ID 
                                left join dbo.Mall_销量排行 on dbo.Mall_销量排行.spid = 商品档案.id 
                                join dbo.商品类型  on 商品类型.编号  = 商品档案.类型编号
                                where 1=1 
                              ");

                strCount.Append( @" select count(1) from dbo.商品档案 with(nolock)
                                join 库存表 on 商品档案.id =库存表.spid
                                left join dbo.Mall_畅销关联表 on dbo.Mall_畅销关联表.spid = 商品档案.id
                                left join dbo.Mall_畅销类型 on Mall_畅销类型.id = dbo.Mall_畅销关联表.类型ID 
                                left join dbo.Mall_销量排行 on dbo.Mall_销量排行.spid = 商品档案.id 
                                where 1=1 ");
                //畅销类型
                if(data.filter_type!=0)
                {
                    strSql.Append(@" and  dbo.Mall_畅销关联表.类型ID= "+data.filter_type);
                    strCount.Append(@" and  dbo.Mall_畅销关联表.类型ID= " + data.filter_type);
                }

                //keyword 
                if (!string.IsNullOrEmpty(data.keyword))
                {
                    strSql.Append(@" and  商品档案.名称 like '%" + data.keyword + "%' ");
                    strCount.Append(@" and  商品档案.名称 like '%" + data.keyword + "%' ");
                }
                strSql.Append(@" and  dbo.商品类型.id =" + data.type_id);
                strCount.Append(@" and  dbo.商品类型.id =" + data.type_id);

                //仅显示有库存数量
                if (data.in_stock == true)
                {
                    strSql.Append(@" and  库存表.数量 > 0 ");
                    strCount.Append(@" and  库存表.数量 > 0 ");
                }
              
                strSql.Append(@" order by 商品档案.id desc) as a order by 商品档案.id
                                )) as a ");
                //0-销量是倒序 
                //1 价格是升序
                //2 销售是升序
                //3是价格倒序

                //按销量降序
                if (data.sort_type == 0)
                {
                    strSql.Append(@" order by  dbo.Mall_销量排行.件数 desc ");
                }
                //按价格升序
                if (data.sort_type == 1)
                {
                    strSql.Append(@" order by  商品档案.零售价 asc ");
                }
                //按销量升序
                if (data.sort_type == 2)
                {
                    strSql.Append(@" order by  dbo.Mall_销量排行.件数 asc ");
                }
                //按价格降序
                if (data.sort_type == 3)
                {
                    strSql.Append(@" order by  商品档案.零售价 desc ");
                }
           
                ReturnModel retmodel = SqlHelper.getDataTable(strSql.ToString(), "a");
                if (retmodel.Succeed)
                {
                    DataTable dt = retmodel.ReturnObj as DataTable;
                    if (dt.Rows.Count > 0)
                    {
                        ReturnModel rm = SqlHelper.ExecuteScalarByString(strCount.ToString(),null);
                        if (rm.Succeed == false) { return new RetObj(rm.Message); }

                        RetObj model = new RetObj(true, "获取成功！");
                        //符合条件的总数
                        model.RetDics.Add("r_count",Convert.ToInt32(rm.ReturnObj));

                        StringBuilder sb = new StringBuilder();
                        sb.Append("[");
                        foreach (DataRow row in dt.Rows)
                        {
                            sb.Append("{\"g_id\":" + row["g_id"] + ",");
                            sb.Append("\"g_img_ts\":" + row["g_img_ts"] + ",");
                            sb.Append("\"g_status\":\"" + row["g_status"] + "\",");
                            sb.Append("\"g_num\":\"" + row["g_num"] + "\",");
                            sb.Append("\"g_name\":\"" + row["g_name"] + "\",");
                            sb.Append("\"g_spec\":" + row["g_spec"] + ",");
                            sb.Append("\"g_price\":" + row["g_price"] + ",");
                            sb.Append("\"g_bulk_price\":" + row["g_bulk_price"]+"},");
                         
                        }
                    
                        string tmp = sb.ToString().Substring(0, sb.ToString().Length - 1);
                        model.RetDics["goods"] = tmp + "]";
                          
                        return model;
                    }
                    RetObj rmt = new RetObj("结果无数据！");
                    rmt.RetDics.Add("r_count",0);
                    return rmt;
                }
                else
                {
                    return new RetObj(retmodel.Message);
                }

            }
            catch (Exception ex)
            {
                return new RetObj(ex.Message);
            }
        }

        /// <summary>
        /// 5 商品详细信息（get_goods_info）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RetObj get_goods_info(ReqModel data)
        {
            try
            {
                if (Common.Tool.filterSql(data.user) == 1)
                {
                    return new RetObj("帐号有误！");
                }
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@" select * from ( select top 1 商品档案.id as g_id,cast(updateno as bigint) as  g_img_ts,
                                (case when 库存表.数量>0 then '有货' else '缺货' end) as g_status,
                                条码 as g_num,商品档案.名称 as g_name,包装规格 g_spec,
                                件售价 g_price,散售价 g_bulk_price 
                                from  dbo.商品档案 with(nolock)
                                join 库存表 on 商品档案.id =库存表.spid where 商品档案.id ="+data.goods_id +") as a  ");
               
                ReturnModel retmodel = SqlHelper.getDataTable(strSql.ToString(), "a");
                if (retmodel.Succeed)
                {
                    DataTable dt = retmodel.ReturnObj as DataTable;
                    if (dt.Rows.Count > 0)
                    {
                        RetObj model = new RetObj(true, "获取成功！");
                      
                        StringBuilder sb = new StringBuilder();
                       
                        foreach (DataRow row in dt.Rows)
                        {
                            model.RetDics.Add("g_id", row["g_id"]);
                            model.RetDics.Add("g_img_ts", row["g_img_ts"]);
                            model.RetDics.Add("g_status", row["g_status"].ToString().Trim());
                            model.RetDics.Add("g_num", row["g_num"].ToString().Trim());
                            model.RetDics.Add("g_name", row["g_name"].ToString().Trim());
                            model.RetDics.Add("g_spec", row["g_spec"]);
                            model.RetDics.Add("g_price", row["g_price"]);
                            model.RetDics.Add("g_bulk_price", row["g_bulk_price:"]);
                           // model.RetDics.Add("g_info:", row["g_info"]);

                        }

                        return model;
                    }
                    RetObj rmt = new RetObj("结果无数据！");
                    rmt.RetDics.Add("r_count", 0);
                    return rmt;
                }
                else
                {
                    return new RetObj(retmodel.Message);
                }

            }
            catch (Exception ex)
            {
                return new RetObj(ex.Message);
            }
        }


        /// <summary>
        /// 6 商品图片更新接口
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RetObj get_goods_image(ReqModel data)
        {
            try
            {
                if (Common.Tool.filterSql(data.user) == 1)
                {
                    return new RetObj("帐号有误！");
                }

                string list = "( ";
                foreach (var item in data.goods_ids)
                {
                    list += item+",";
                }
                list = list.Substring(0,list.Length -1);
                list += ")";

                StringBuilder strSql = new StringBuilder();
                strSql.Append(@" select   商品档案.id as g_id,cast(updateno as bigint) as  g_img_ts,
                                 图片 as g_data from 商品档案 where 商品档案.id  in " + list +" order by id asc");
               
                ReturnModel retmodel = SqlHelper.getDataTable(strSql.ToString(), "a");
                if (retmodel.Succeed)
                {
                    DataTable dt = retmodel.ReturnObj as DataTable;
                  
                    if (dt.Rows.Count > 0)
                    {
                        RetObj model = new RetObj(true, "获取成功！");
                        List<Dictionary<string, object>> images = new List<Dictionary<string, object>>();
                        StringBuilder sb = new StringBuilder();
                        foreach (DataRow row in dt.Rows)
                        {
                            byte[] imgdata = (byte[])row["g_data"];
                       
                            Dictionary<string, object> dics = new Dictionary<string, object>();
                            //获取缩略图
                            if (data.img_type == 0)
                            {
                                dics.Add("g_data", Common.Tool.GetMicroImage(imgdata, 64, 64));
                            }
                            else
                            {
                                dics.Add("g_data", imgdata);
                            }
                            
                            dics.Add("g_img_ts", row["g_img_ts"]);
                            dics.Add("g_id", row["g_id"].ToString().Trim());
                            images.Add(dics);
                        }
                        model.RetDics.Add("images", images);
                        return model;
                    }
                    RetObj rmt = new RetObj("结果无数据！");
             
                    return rmt;
                }
                else
                {
                    return new RetObj(retmodel.Message);
                }

            }
            catch (Exception ex)
            {
                return new RetObj(ex.Message);
            }
        }

        /// <summary>
        /// 7 会员卡余额查询
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RetObj get_card_info(ReqModel data)
        {
            try
            {
                if (Common.Tool.filterSql(data.user) == 1)
                {
                    return new RetObj("帐号有误！");
                }
//                StringBuilder strSql = new StringBuilder();
//                strSql.Append(@" SELECT id,[编号],[名称],[密码]
//                FROM [dbo].[客户档案]
//                with(nolock) where id= " + data.member_id);

           
               // ReturnModel retmodel = SqlHelper.getDataTable(strSql.ToString(), "商品类型");
                //m_balance
                ReturnModel retmodel = new ReturnModel(){Succeed=true};
                if (retmodel.Succeed)
                {
                    RetObj ret = new RetObj(true,"获取成功！");
                    ret.RetDics.Add("m_balance",5000);
                    //DataTable dt = retmodel.ReturnObj as DataTable;
                    //if (dt.Rows.Count > 0)
                    //{
                    //    RetObj model = new RetObj(true, "登录成功！");

                    //    return model;
                    //}
                    return ret;
                    //return new RetObj("帐号不存在！");
                }
                else
                {
                    return new RetObj(retmodel.Message);
                }

            }
            catch (Exception ex)
            {
                return new RetObj(ex.Message);
            }
        }
    }
}
