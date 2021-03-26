using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;

namespace InventoryAPI.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("Inventory/v1/[action]")]

    public class v1Controller : ControllerBase
    {
        // 更新数据，目前主要用于更新册记录的位置信息
        [HttpPost]
        public SetItemsResponse SetItems([FromBody]SetItemsRequest request)
        {
            SetItemsResponse response = new SetItemsResponse();
            response.result = new ApiResult();

            // 检查传入的参数内容是否正确，如果为null或者没有记录，返回普通错误。
            if (request.items == null || request.items.Count == 0)
            {
                response.result.value = -1;
                response.result.errorCode = "commonError";
                response.result.errorInfo = "传入的items参数为null或记录数量为0";
                return response;
            }



            string logText = "";
            foreach (Item item in request.items)
            {

                 logText += "action=" + item.action + ","+ "data=" + item.data + "\r\n";

                if (response.outputItems ==null)
                    response.outputItems = new List<Item>();

                // 把返回的item加到集合里
                Item outputItem = new Item();
                outputItem.result = new ApiResult();
                response.outputItems.Add(outputItem);
            }

            // 写日志
            logText += "共上传" + request.items.Count.ToString() + "条记录";
            Log.Information(logText);


            return response;
        }


    }

    // SetItems请求对象
    public class SetItemsRequest
    {
        // 上传的记录集合
        public List<Item> items { get; set; }
    }

    public class Item
    {
        //要执行的操作，值为new update delete move，默认是update
        public string action { get; set; }

        //风格。常用作附加的特性参数。
        public string style { get; set; }

        // 数据格式，值为json、xml
        public string format { get; set; }

        //数据内容，根据不同的Format，传入不同格式的内容
        public string data { get; set; }

        // 本条记录的处理结果，当用在返回参数时有意义
        public ApiResult result { get; set; }
    }

    // SetItem返回对象
    public class SetItemsResponse
    {
        // 整体的返回结果
        public ApiResult result { get; set; }

        // 每一条记录各自的返回信息，记录数量与传入数量一致。
        public List<Item> outputItems { get; set; }

    }

    // API函数结果
    public class ApiResult
    {
        // -1表示出错，>=0 表示成功
        public long value { get; set; }

        // 错误码，字符串类型，目前值如下
        // noError:成功
        // systemError:系统错误，一般是指严重错误，例如帐户密码不正确，系统内部出现严重错误，导致没法处理记录，此时value=-1。
        // partError:部分错误，在作为整体返回结果时，如果处理的记录部分成功，部分出错，value=-1，errorCode=partError
        // notFound:未找到，根据传入的路径或条码未找到对应记录
        public string errorCode { get; set; }  

        // 错误描述信息
        public string errorInfo { get; set; }
    }



}
