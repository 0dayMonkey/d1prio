﻿using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection;

namespace OpenMarketingApi.Attributes;

/// <summary>
/// Model state validation attribute
/// </summary>
public class ValidateModelStateAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Called before the action method is invoked
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Per https://blog.markvincze.com/how-to-validate-action-parameters-with-dataannotation-attributes/
        var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
        if (descriptor != null)
        {
            foreach (var parameter in descriptor.MethodInfo.GetParameters())
            {
                object args = null;
                if (context.ActionArguments.ContainsKey(parameter.Name))
                {
                    args = context.ActionArguments[parameter.Name];
                }

                ValidateAttributes(parameter, args, context.ModelState);
            }
        }

        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }

    private void ValidateAttributes(ParameterInfo parameter, object args, ModelStateDictionary modelState)
    {
        foreach (var attributeData in parameter.CustomAttributes)
        {
            var attributeInstance = parameter.GetCustomAttribute(attributeData.AttributeType);

            var validationAttribute = attributeInstance as ValidationAttribute;
            if (validationAttribute != null)
            {
                var isValid = validationAttribute.IsValid(args);
                if (!isValid)
                {
                    modelState.AddModelError(parameter.Name, validationAttribute.FormatErrorMessage(parameter.Name));
                }
            }
        }
    }
}
