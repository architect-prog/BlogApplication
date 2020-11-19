using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Services
{
    public class IdentityResultHandler
    {
        private Func<IActionResult> _successAction;
        private Func<IActionResult> _failureAction;
        private IdentityResult _result;

        public IActionResult HandleIdentityResult()
        {
            return _result.Succeeded ? _successAction?.Invoke() : _failureAction?.Invoke();
        }

        public IdentityResultHandler SetSuccessAction(Func<IActionResult> successAction)
        {
            _successAction = null;
            _successAction += successAction;
            return this;
        }

        public IdentityResultHandler SetFailureAction(Func<IActionResult> failureAction)
        {
            _failureAction = null;
            _failureAction += failureAction;
            return this;
        }

        public IdentityResultHandler SetIdentityResult(IdentityResult result)
        {
            _result = result;
            return this;
        }

        public IdentityResultHandler AddModelErrors(ModelStateDictionary modelState)
        {
            foreach (var error in _result.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
            return this;
        }
    }
}
