﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using OrvilleX.ViewModels;
using System.Linq;

namespace Sino.Web.Filters
{
    /// <summary>
    /// 标准输出格式化过滤器
    /// </summary>
    public class StandardResultFilterAttribute : ResultFilterAttribute
    {
        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsUse { get; set; } = true;

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                var use = GetAttribute(context)?.IsUse ?? IsUse;
                if (use)
                {
                    var result = context.Result;
                    if (result is EmptyResult || result is ObjectResult)
                    {
                        context.Result = result is EmptyResult ? new ObjectResult(null) : result;
                        var obj = context.Result as ObjectResult;
                        if (!(obj.Value is BaseResponse))
                        {
                            obj.Value = new BaseResponse
                            {
                                ErrorCode = "0",
                                Success = true,
                                Data = obj.Value
                            };
                        }
                    }
                }
            }
            base.OnResultExecuting(context);
        }

        private StandardResultFilterAttribute GetAttribute(ActionContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                var attribute = descriptor.MethodInfo?.GetCustomAttributes(typeof(StandardResultFilterAttribute), true)?.FirstOrDefault();
                return attribute as StandardResultFilterAttribute;
            }
            return null;
        }
    }
}