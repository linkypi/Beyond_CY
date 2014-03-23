using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using DAL;
using JsonFx.Json;
using Model;
using Newtonsoft.Json.Linq;
using OMS.Tool;

namespace OMS
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
   [AspNetCompatibilityRequirements(RequirementsMode=AspNetCompatibilityRequirementsMode.Allowed)]
   [ServiceKnownType(typeof(Person))]
   [ServiceKnownType(typeof(object))]
    [ServiceContract]
    public class OMSService 
    {

       [OperationContract]
       [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
       public Person GetData()
       {
           return new Person() {Name="Jack" ,Age=24};
       }

       [OperationContract]
       [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
           UriTemplate = "/jmp",
           BodyStyle = WebMessageBodyStyle.WrappedRequest)]
       public string jmp(ReqModel data)
       {
           ReturnModel model = Validate(data);
           if (model.Succeed == false)
           {
               return  "";
           }
            string asd= Handler.admin_login(data).ToString().Replace("\r\n","");
            return asd;
       }

       [OperationContract]
       [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
           UriTemplate = "/exec",
           BodyStyle = WebMessageBodyStyle.WrappedRequest)]
       public Stream exec(ReqModel data)
       {
           //ReturnModel model = Validate(data);
           //if (model.Succeed == false)
           //{
           //    return (Stream)model.ReturnObj;
           //}

           switch (data.service)
           {
               case "member_login":
                   return Common.ReturnValue(Handler.member_login(data));

               case "admin_login":
                   return Common.ReturnValue(Handler.admin_login(data));
                 
               case "get_goods_category":
                   return Common.ReturnValue(Handler.get_goods_category(data));
                   
               case "get_goods_list":
                   return Common.ReturnValue(Handler.get_goods_list(data));

               case "get_goods_info":
                   return Common.ReturnValue(Handler.get_goods_info(data));

               case "get_goods_image":
                   return Common.ReturnValue(Handler.get_goods_image(data));
                  
               case "get_card_info":
                   return Common.ReturnValue(Handler.get_card_info(data));
                 
               case "get_card_log":
                   break;
               case "place_an_order":
                   break;
               case "get_order_list":
                   break;
               case "get_order_info":
                   break;
               case "get_order_log":
                   break;
               case "cancel_order":
                   break;
         
           }

           HttpReturn hret = new HttpReturn(false,"服务名称不存在！");
           return Common.ReturnValue(hret);
       }

       private static ReturnModel Validate(ReqModel data)
       {
           RetObj ret = new RetObj();
           if (data == null)
           {
               ret.RetDics["state"] = false;
               ret.RetDics["info"] = "数据有误！";
               return new ReturnModel() { Succeed = false, ReturnObj = Common.ReturnValue(ret) };
           }
           if (string.IsNullOrEmpty(data.service))
           {
               ret.RetDics["state"] = false;
               ret.RetDics["info"] = "接口服务标识有误！";
               return new ReturnModel() { Succeed = false, ReturnObj = Common.ReturnValue(ret) };
           }
           if (!Common.Validate(data))
           {

               ret.RetDics["state"] = false;
               ret.RetDics["info"] = "验证失败！";
               return new ReturnModel() { Succeed = false, ReturnObj = Common.ReturnValue(ret) };
           }
           return new ReturnModel() { Succeed=true};
       }


    }
}
